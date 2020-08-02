using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FXV.Data;
using FXV.JwtManager;
using FXV.Models;
using FXV.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FXV_App.Controllers
{
    public class PublicLeaderboardController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public PublicLeaderboardController(IHttpContextAccessor accessor, UserManager<AppUser> userManager, ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index(int event_id)
        {
            TempData["Event_ID"] = event_id;

            TempData["Event_Name"] = dbContext.Event.Find(event_id).Name;

            return View();
        }

        public string GetFundersLogo(int event_id)
        {
            List<string> IMG_URLS = new List<string>();

            IMG_URLS.Add("../adImg/FXVLogo.png");
            IMG_URLS.Add("../adImg/SilverbackEventsLogo_white.png");
            IMG_URLS.Add("../adImg/GreenbackLogo_white.png");
            IMG_URLS.Add("../adImg/AperitivoLogo_white.png");
            IMG_URLS.Add("../adImg/Advert.png");

            return JsonConvert.SerializeObject(IMG_URLS);
        }

        public string StartEvent(int event_id, int last_Id)
        {
            if (dbContext.TempEventTable.Where(x=>x.Event_ID == event_id).ToList().Count == 0)
            {
                var attendee = dbContext.Event_Assigned_Attendee.Where(x => x.E_ID == event_id && x.Id > last_Id).ToList();

                PublicLeaderboardProfile publicLeaderboardProfile = new PublicLeaderboardProfile();

                if (attendee.Count == 0)
                {
                    var firstAttendee = dbContext.Event_Assigned_Attendee.Where(x => x.E_ID == event_id).First();
                    var achievements = dbContext.AthleteAchievement.Where(x => x.Id == firstAttendee.Id).Select(y => y.Achievement).ToList();
                    var user_detials = dbContext.Users.Find(firstAttendee.Id);
                    publicLeaderboardProfile.Attendee_ID = firstAttendee.Id;
                    publicLeaderboardProfile.Attendee_FirstName = user_detials.FirstName;
                    publicLeaderboardProfile.Attendee_LastName = user_detials.LastName;
                    publicLeaderboardProfile.Event_Name = dbContext.Event.Find(event_id).Name;
                    publicLeaderboardProfile.Achievement = achievements;

                    return JsonConvert.SerializeObject(publicLeaderboardProfile);

                }
                else
                {
                    var achievements = dbContext.AthleteAchievement.Where(x => x.Id == attendee.First().Id).Select(y => y.Achievement).ToList();
                    var user_detials = dbContext.Users.Find(attendee.First().Id);
                    publicLeaderboardProfile.Attendee_ID = attendee.First().Id;
                    publicLeaderboardProfile.Attendee_FirstName = user_detials.FirstName;
                    publicLeaderboardProfile.Attendee_LastName = user_detials.LastName;
                    publicLeaderboardProfile.Event_Name = dbContext.Event.Find(event_id).Name;
                    publicLeaderboardProfile.Achievement = achievements;

                    return JsonConvert.SerializeObject(publicLeaderboardProfile);
                }
            }
            else
            {
                return JsonConvert.SerializeObject("Ready to display leaderboard");
            }

        }

        public string GetOverallResultLength(int event_id)
        {

            var length = dbContext.Event_Result.Where(x => x.E_ID == event_id).ToList().Count;

            return JsonConvert.SerializeObject(length);
        }

        public string GetResult(int event_id, int num)
        {

            var all_attendees = from t1 in dbContext.Event_Assigned_Attendee.Where(x => x.E_ID == event_id)
                                join t2 in dbContext.Users on t1.Id equals t2.Id
                                select t2;

            var event_Result = from t3 in dbContext.Event_Result
                               join t4 in all_attendees on t3.Id equals t4.Id
                               select t3;

            var all_attendees_with_result = from t5 in all_attendees
                                            join t6 in event_Result on t5.Id equals t6.Id
                                            select new PublicLeaderboardResult
                                            {
                                                UID = t5.Id,
                                                FullName = ((t5.FirstName + " " + t5.LastName).Length > 16) ? (t5.FirstName + " " + t5.LastName).Substring(0, 13) + "..." : t5.FirstName + " " + t5.LastName,
                                                Point = t6.Final_Point
                                            };


            var list = all_attendees_with_result.OrderByDescending(x => x.Point).Skip((num -1) * 4).Take(4).ToList();

            return JsonConvert.SerializeObject(list);
        }

        public string GetLastOrNext(int event_id)
        {
            var temInfo = dbContext.TempEventTable.Where(x => x.Event_ID == event_id).First();

            var user = dbContext.Users.Find(temInfo.Athlete_ID);

            var athlete_detail = new PublicLeaderboardProfile()
            {
                Event_Name = dbContext.Event.Find(event_id).Name,
                Attendee_FirstName = user.FirstName,
                Attendee_LastName = user.LastName,
                Achievement = dbContext.AthleteAchievement.Where(x => x.Id == user.Id).Select(y => y.Achievement).ToList(),
                IsFinish = temInfo.IsFinished
            };

            return JsonConvert.SerializeObject(athlete_detail);
        }
    }
}
