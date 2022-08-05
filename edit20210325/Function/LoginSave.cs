using edit20210325.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace edit20210325.Function
{
    public class LoginSave
    {
        private MarkGmailData gmailData;
        private Claim[] claimsSave;


        public LoginSave( MarkGmailData gmailData,string LoginStatus)
        {
            this.gmailData = gmailData;
            claimsSave = new[] {
                new Claim(ClaimTypes.Name, gmailData.Name),
                new Claim(ClaimTypes.Uri, gmailData.Info),
                new Claim(ClaimTypes.Email, gmailData.Gmail),
                new Claim(ClaimTypes.Actor, LoginStatus)
            };
        }

        public ClaimsPrincipal getClaimsPrincipal()
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claimsSave, CookieAuthenticationDefaults.AuthenticationScheme);//Scheme必填
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);

            return principal;
        }
    }
}
