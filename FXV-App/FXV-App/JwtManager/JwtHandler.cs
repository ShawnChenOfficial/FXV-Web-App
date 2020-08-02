using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using FXV.Models;
using JWT;
using JWT.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FXV.JwtManager
{
    //constructor of jwt claims in jwt payload
    public class JwtClaim
    {
        public int Uid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IP { get; set; }
        public string Role { get; set; }
        public int Permission_ID { get; set; }
        public string Name { get; set; }
        public string Img_Path { get; set; }
    }

    public class JwtHandler
    {
        private string Access_token { get; set; }
        private readonly IHttpContextAccessor accessor;
        private readonly UserManager<AppUser> userManager;
        private readonly IConfiguration configuration;

        public JwtHandler(IHttpContextAccessor accessor, UserManager<AppUser> userManager,IConfiguration configuration)
        {
            this.accessor = accessor;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        //generate a new token
        public async Task<string> GenerateToken(SymmetricSecurityKey key, int exp, AppUser user)
        {
            var signInKey = key;

            var role = await userManager.GetRolesAsync(user);

            //just in case of this token only for confirmaion email
            var ip = (exp == 0) ? "192.0.0.0" : accessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (role.Count == 0 && exp == 0)
            {
                role.Add("Athlete");
            }

            var token = new JwtSecurityToken(
                issuer: "localhost",
                audience: "localhost",
                claims: new[]
                {
                        new Claim ("Ip",ip),
                        new Claim ("FirstName",user.FirstName),
                        new Claim ("LastName",user.LastName),
                        new Claim ("Uid",user.Id.ToString()),
                        new Claim ("Role",role[0]),
                        new Claim ("Permission",user.P_ID.ToString()),
                        new Claim ("Name",user.Email),
                        new Claim ("Img Path",(user.Profile_Img_Path == null || user.Profile_Img_Path == "" || user.Profile_Img_Path == "") ? "/sources/userProfileImg/user-profile-null.png" : user.Profile_Img_Path)
                },
                expires: DateTime.UtcNow.AddMinutes(exp),
                signingCredentials: new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //generate a new token for pwd reset
        public async Task<string> GenerateToken(SymmetricSecurityKey key, AppUser user)
        {
            var signInKey = key;

            var role = await userManager.GetRolesAsync(user);

            var ip = accessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (role.Count == 0)
            {
                role.Add("Athlete");
            }


            var token = new JwtSecurityToken(
                issuer: "localhost",
                audience: "localhost",
                claims: new[]
                {
                        new Claim ("Ip",ip),
                        new Claim ("FirstName",user.FirstName),
                        new Claim ("LastName",user.LastName),
                        new Claim ("Uid",user.Id.ToString()),
                        new Claim ("Role",role[0]),
                        new Claim ("Permission",user.P_ID.ToString()),
                        new Claim ("Name",user.Email),
                        new Claim ("Img Path",(user.Profile_Img_Path == null || user.Profile_Img_Path == "" || user.Profile_Img_Path == "") ? "/sources/userProfileImg/user-profile-null.png" : user.Profile_Img_Path)
                },
                signingCredentials: new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // set the token from cookie on client side 
        public void SetToken(string access_token)
        {
            Access_token = access_token;
        }
        //check if the token will expire within 10 mins
        public bool ExpiredInTenMins()
        {
            long num = new DateTimeOffset(DateTime.UtcNow.AddMinutes(10)).ToUnixTimeSeconds();
            bool result = new JwtSecurityTokenHandler().ReadJwtToken(Access_token).Payload.Exp <= num;
            if (new JwtSecurityTokenHandler().ReadJwtToken(Access_token).Payload.Exp == 0)
            {
                return false;
            }
            return result;
        }
        // check if the user role has been updated by admin
        public async Task<bool> RoleIsUpdated()
        {
            var user = await userManager.FindByIdAsync(new JwtSecurityTokenHandler().ReadJwtToken(Access_token).Payload.Claims.First(x => x.Type == "Uid").Value.ToString());
            var roleInSystem = (await userManager.GetRolesAsync(user))[0].ToString();
            var role = new JwtSecurityTokenHandler().ReadJwtToken(Access_token).Payload.Claims.First(x => x.Type == "Role").Value.ToString();
            return roleInSystem != role;
        }
        //return the claims information in payload
        public JwtClaim GetClaims()
        {
            var jwtClaim = new JwtClaim();
            var claims = new JwtSecurityTokenHandler().ReadJwtToken(Access_token).Claims;

            jwtClaim.IP = claims.First(claim => claim.Type == "Ip").Value;
            jwtClaim.Uid = int.Parse(claims.First(claim => claim.Type == "Uid").Value);
            jwtClaim.FirstName = claims.First(claim => claim.Type == "FirstName").Value;
            jwtClaim.LastName = claims.First(claim => claim.Type == "LastName").Value;
            jwtClaim.Permission_ID = int.Parse(claims.First(claim => claim.Type == "Permission").Value);
            jwtClaim.Name = claims.First(claim => claim.Type == "Name").Value;
            jwtClaim.Role = claims.First(claim => claim.Type == "Role").Value;
            jwtClaim.Img_Path = claims.First(claim => claim.Type == "Img Path").Value;

            return jwtClaim;
        }
        // return a refreshed token based on information
        public async Task<string> RefreshToken(SymmetricSecurityKey key, int exp, JwtClaim claims)
        {
            var user = await userManager.FindByIdAsync(claims.Uid.ToString());
            var role = await userManager.GetRolesAsync(user);

            var token = new JwtSecurityToken(
                issuer: "localhost",
                audience: "localhost",
                claims: new[]
                {
                        new Claim ("Ip",accessor.HttpContext.Connection.RemoteIpAddress.ToString()),
                        new Claim ("FirstName",user.FirstName),
                        new Claim ("LastName",user.LastName),
                        new Claim ("Uid",user.Id.ToString()),
                        new Claim ("Role",role[0]),
                        new Claim ("Permission",user.P_ID.ToString()),
                        new Claim ("Name",user.Email),
                        new Claim ("Img Path",(user.Profile_Img_Path == null || user.Profile_Img_Path == "") ? "/sources/userProfileImg/user-profile-null.png" : user.Profile_Img_Path)
                },
                expires: DateTime.UtcNow.AddMinutes(exp),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
            string access_token = new JwtSecurityTokenHandler().WriteToken(token);
            return access_token;
        }

        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            return true;
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = "Sample",
                ValidAudience = "Sample",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:privatekey"])) // The same key as the one that generate the token
            };
        }

    }
}
