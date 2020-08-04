using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace FXV.MailingLib
{
    public class MailingService
    {
        public enum MailingType { PwdReset, Activate }
        private string emailToAddress { get; set; }

        private string smtpServer { get; set; }
        private int smtpPort { get; set; }
        private string smtpUserName { get; set; }
        private string smtpPassword { get; set; }
        private string gmailClientID { get; set; }
        private string gmailClientSecret { get; set; }

        private string emailSubjectForActivate = "FXV: Reset Your Password";
        private string linkForActivate = "https://localhost:5001/Activate/SignUpActivate?";  //local test
        //private string linkForActivate = "https://www.fxv.co.nz/Activate/SignUpActivate?";  //Release


        private string emailSubjectForResetPwd = "FXV: Reset Your Password";
        private string linkForPwdReset = "https://localhost:5001/Password/Reset?";  //local test
        //private string linkForPwdReset = "https://www.fxv.co.nz/Password/Reset?";  //Release


        private string token { get; set; }
        public string Error { get; set; }


        public MailingService(IConfiguration configuration, string _emailToAddress, string token)
        {
            this.emailToAddress = _emailToAddress;
            this.smtpServer = configuration["MaillingSettings:SmtpServer"];
            this.smtpPort = int.Parse(configuration["MaillingSettings:SmtpPort"].ToString());
            this.smtpUserName = configuration["MaillingSettings:SmtpUserName"];
            this.smtpPassword = configuration["MaillingSettings:SmtpPassword"];
            this.gmailClientID = configuration["MaillingSettings:GMailClientID"];
            this.gmailClientSecret = configuration["MaillingSettings:GMailClientSecret"];
            this.token = token;
        }

        public async System.Threading.Tasks.Task<bool> Sending(MailingType mailingType)
        {
            try
            {
                var myMail = new MimeMessage();
                myMail.From.Add(MailboxAddress.Parse(smtpUserName));
                myMail.To.Add(MailboxAddress.Parse(emailToAddress));

                if (mailingType == MailingType.PwdReset)
                {
                    myMail.Subject = emailSubjectForResetPwd;
                    myMail.Body = new TextPart(TextFormat.Html)
                    {
                        Text = "<p>Hi</p>"
                                + "<br />"
                                + "<p>This is a confirmation email sent by FXV.</p>"
                                + "<br />"
                                + "<p>Please click the following link to activate your account.</p>"
                                + "<a href=" + linkForPwdReset + token + ">Here</p>"
                    };
                }
                else if (mailingType == MailingType.Activate)
                {
                    myMail.Subject = emailSubjectForActivate;
                    myMail.Body = new TextPart(TextFormat.Html)
                    {
                        Text = "<p>Hi</p>"
                                + "<br />"
                                + "<p>This is a confirmation email sent by FXV.</p>"
                                + "<br />"
                                + "<p>Please click the following link to activate your account.</p>"
                                + "<a href=" + linkForActivate + token + ">Here</p>"
                    };

                }

                using (var client = new SmtpClient())
                {
                    client.Connect(smtpServer, smtpPort, SecureSocketOptions.StartTls);

                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(smtpUserName, smtpPassword);

                    await client.SendAsync(myMail);
                    client.Disconnect(true);
                }
                return true;
            }
            catch (Exception err)
            {
                Error = err.Message + "\n" + err.Source;
                return false;
            }
        }
    }
}
