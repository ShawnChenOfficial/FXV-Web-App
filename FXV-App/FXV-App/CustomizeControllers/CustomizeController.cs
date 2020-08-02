using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FXV.Data;
using FXV.JwtManager;
using FXV.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FXV_App.CustomizeControllers
{
    public class CustomizeController : Controller
    {
        protected UserManager<AppUser> _userManager { get; set; }
        protected ApplicationDbContext _dbContext { get; set; }
        protected IConfiguration _configuration { get; set; }
        protected IHostingEnvironment _hostingEnvironment { get; set; }
        protected JwtHandler _jwtHandler { get; set; }
        protected JwtClaim _claim { get; set; }

        public CustomizeController(IConfiguration configuration, IHttpContextAccessor accessor, UserManager<AppUser> userManager, ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
        {
            this._configuration = configuration;
            this._userManager = userManager;
            this._dbContext = dbContext;
            this._hostingEnvironment = hostingEnvironment;
            this._jwtHandler = new JwtHandler(accessor, userManager, configuration);
        }

        protected bool HasAccessToTeam(int team_id)
        {
            return _dbContext.Team_Membership.Where(w => w.Id == _claim.Uid && w.Role == "Team Manager" && w.Team_ID == team_id).Count() > 0
                    || _dbContext.Organization_Relationship.Where(w => w.Org_ID == _dbContext.Team.Where(ww => ww.Team_ID == team_id).Include(inc => inc.Organization).Select(s => s.Organization).FirstOrDefault().Org_ID && w.Role == "Organization Manager" && w.Id == _claim.Uid).Count() > 0;
        }
        protected bool HasAccessToViewOrgInfo(int org_id)
        {
            return _dbContext.Organization_Relationship.Where(w => w.Id == _claim.Uid && w.Role == "Organization Manager" && w.Org_ID == org_id).Count() > 0 || _dbContext.Team_Membership.Where(w => w.Id == _claim.Uid && w.Role == "Team Manager" && w.Position == "Team Manager").Include(inc => inc.Team).Where(ww => ww.Team.Org_ID == org_id).Count() > 0;
        }
        protected bool IsRelevantTeamManager(int team_id)
        {
            return _dbContext.Team_Membership.Where(w => w.Id == _claim.Uid && w.Role == "Team Manager" && w.Position == "Team Manager" && w.Team_ID == team_id).Count() > 0;
        }
        protected async Task<bool> IsRelevantOrgManager(int org_id)
        {
            return (await _dbContext.Organization_Relationship.Where(w => w.Org_ID == org_id && w.Id == _claim.Uid && w.Role == "Organization Manager").CountAsync()) > 0;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context,
                                         ActionExecutionDelegate next)
        {

            _jwtHandler.SetToken(context.HttpContext.Request.Cookies["access_token"]);//

            var result = await _jwtHandler.RoleIsUpdated();

            if (_jwtHandler.ExpiredInTenMins() || result)
            {
                var claims = _jwtHandler.GetClaims();

                var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:privatekey"]));

                int expMins = Convert.ToInt32(_configuration["jwt:exp"]);

                var access_token = await _jwtHandler.RefreshToken(signInKey, expMins, claims);

                context.HttpContext.Response.Cookies.Append("access_token", access_token, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = true
                });
            }

            _claim = _jwtHandler.GetClaims();

            ViewData["user_img_path"] = _claim.Img_Path;
            ViewData["uid"] = _claim.Uid;

            if (_claim.Role == "Admin")
            {
                ViewData["athletes_num"] = _dbContext.Users.ToList().Count();
            }
            else if (_claim.Role == "Organization")
            {
                var org_ids = _dbContext.Organization_Relationship.Where(w => w.Id == _claim.Uid && w.Role == "Organization Manager").Select(s => s.Org_ID).ToList();

                ViewData["teams_num"] = _dbContext.Team.Include(i => i.Organization).Where(w => org_ids.Contains(w.Organization.Org_ID)).Count();
                ViewData["athletes_num"] = _dbContext.Organization_Relationship.Include(i => i.Organization).Where(w => org_ids.Contains(w.Organization.Org_ID)).Select(s => s.Id).Distinct().Count();
            }
            else if (_claim.Role == "Manager")
            {
                var team_ids = _dbContext.Team_Membership.Where(w => w.Role == "Team Manager" && w.Position == "Team Manager" && w.Id == _claim.Uid).Include(inc => inc.Team).Where(ww => ww.Team.Org_ID != 0).Select(s => s.Team.Team_ID).Distinct().ToList();
                ViewData["teams_num"] = team_ids.Count();
                ViewData["athletes_num"] = _dbContext.Team_Membership.Where(w => team_ids.Contains(w.Team_ID)).Select(s => s.Id).Distinct().Count();
            }
            else if (_claim.Role == "Athlete")
            {
                var best_score = _dbContext.Combine_Result.Where(x => x.Id == _claim.Uid).GroupBy(y => y.Point).Select(z => z.Max(c => c.Point));

                ViewData["best_combine_score"] = (best_score.ToList().Count == 0) ? 0 : best_score.First();
            }

            if (_claim.FirstName.Length >= 14)
            {
                ViewData["FirstName"] = _claim.FirstName.Substring(0, 14) + "...";
                ViewData["LastName"] = "";
            }
            else if ((_claim.FirstName + " " + _claim.FirstName).Length >= 14)
            {
                ViewData["FirstName"] = _claim.FirstName;
                ViewData["LastName"] = "";
            }
            else
            {
                ViewData["FirstName"] = _claim.FirstName;

                ViewData["LastName"] = _claim.LastName;
            }
            await next(); // the actual action

            // logic after the action goes here
        }
    }
}
