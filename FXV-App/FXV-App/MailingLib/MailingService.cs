using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace FXV.MailingLib
{
    public class MailingService
    {
        public enum MailingType { PwdReset, Activate }

        private string emailFromAddress { get; set; }
        private string emailPassword { get; set; }
        private string emailToAddress { get; set; }
        private string emailSubject = "FXV activate your account";

        private string emailBody = "This is a confirmation email sent by FXV.\nPlease click the following link to activate your account.\n http://localhost:5000/Activate/SignUpActivate?";

        private string emailBodyForPwdReset = "This is a confirmation email sent by FXV.\nPlease click the following link to activate your account.\n http://localhost:5000/Password/Reset?";

        private string token { get; set; }
        public string Error { get; set; }


        public MailingService(string _emailFromAddress, string _emailPassword, string _emailToAddress, string token)
        {
            this.emailFromAddress = _emailFromAddress;
            this.emailPassword = _emailPassword;
            this.emailToAddress = _emailToAddress;
            this.token = token;
        }

        public bool Sending(MailingType mailingType)
        {
            if (mailingType == MailingType.PwdReset)
            {
                try
                {
                    MailMessage myMail = new MailMessage();
                    myMail.From = new MailAddress(emailFromAddress);
                    myMail.To.Add(new MailAddress(emailToAddress));
                    myMail.Subject = emailSubject;
                    myMail.SubjectEncoding = Encoding.UTF8;

                    myMail.Body = emailBodyForPwdReset + token;
                    myMail.BodyEncoding = Encoding.UTF8;
                    myMail.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;                    //Gmail smtp port
                    smtp.Credentials = new NetworkCredential(emailFromAddress, emailPassword);
                    smtp.EnableSsl = true;              //Gmail SSL
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network; //Gmail: declear sending through network

                    smtp.Send(myMail);
                    return true;
                }
                catch (Exception err)
                {
                    Error = err.Message + "\n" + err.Source;
                    return false;
                }
            }
            else if(mailingType == MailingType.Activate)
            {
                try
                {
                    MailMessage myMail = new MailMessage();
                    myMail.From = new MailAddress(emailFromAddress);
                    myMail.To.Add(new MailAddress(emailToAddress));
                    myMail.Subject = emailSubject;
                    myMail.SubjectEncoding = Encoding.UTF8;

                    myMail.Body = emailBody + token;
                    myMail.BodyEncoding = Encoding.UTF8;
                    myMail.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;                    //Gmail smtp port
                    smtp.Credentials = new NetworkCredential(emailFromAddress, emailPassword);
                    smtp.EnableSsl = true;              //Gmail SSL
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network; //Gmail: declear sending through network

                    smtp.Send(myMail);
                    return true;
                }
                catch (Exception err)
                {
                    Error = err.Message + "\n" + err.Source;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
