using Anzu.AnnPortal.Identity.Data.Model.Models;
using System;
using System.Net.Mail;
using System.Text;
using System.Web.Configuration;
using Anzu.AnnPortal.Identity.Common.Model.Enum;
using Notification = Anzu.AnnPortal.Common.Notification;
using Anzu.AnnPortal.Identity.Core.Encryption;
using System.Collections.Generic;
// using Anzu.AnnPortal.Web.UI.Encryption;

namespace Anzu.AnnPortal.Identity.Core
{
    /// <summary>
    /// Email service class
    /// </summary>
    public class EmailService
    {
        private string appUrl;
        private string identityUrl;
        private SmtpClient client;
        private Notification.MailSenderInformation mailSenderInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        public EmailService()
        {
            //Notification.

            //client = new SmtpClient();

            //client.Port = 25;
            //client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            //client.PickupDirectoryLocation = @"C:\inetpub\mailroot\Pickup";
            //client.UseDefaultCredentials = false;
            //client.Host = @"localhost";

            client = new Notification.SmtpClientService().Client;

            mailSenderInfo = new Notification.MailSenderInformation();

            appUrl = WebConfigurationManager.AppSettings["eEDApp"];
            identityUrl = WebConfigurationManager.AppSettings["identityService"];
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="emailType">Type of the email.</param>
        /// <param name="reciever">The reciever.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="tempPassword">The temporary password.</param>
        /// <returns></returns>
        public bool SendEmail(EmailMessageType emailType, ApplicationUser reciever, ApplicationUser sender = null, string tempPassword = null, int days = 0, string prevRole = null, string curRole = null)
        {
            bool isSuccess = false;

            try
            {
                MailMessage mail = new MailMessage(mailSenderInfo.SenderAddress, reciever.Email);
                mail.Subject = EmailSubject(emailType);
                mail.Body = EmailBody(emailType, reciever, sender, tempPassword, days, prevRole, curRole);
                mail.IsBodyHtml = true;

                client.Send(mail);
                isSuccess = true;
            }
            catch (Exception ex)
            {
            }

            return isSuccess;
        }

        /// <summary>
        /// Emails the body.
        /// </summary>
        /// <param name="emailType">Type of the email.</param>
        /// <param name="reciever">The reciever.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="tempPassword">The temporary password.</param>
        /// <returns></returns>
        private string EmailBody(EmailMessageType emailType, ApplicationUser reciever, ApplicationUser sender = null, string tempPassword = null, int days = 0, string prevRole = null, string curRole = null)
        {
            StringBuilder body = new StringBuilder();

            string lineBreak = "<br/><br/>";

            body.Append("<html><body>");
            body.Append("<strong>***Please do not reply to this message***<strong>");
            body.Append(lineBreak);
            body.Append(string.Format("<div style='font-weight:normal;'>Hi, "));
            body.Append(lineBreak);

            var tempLink = string.Empty;

            switch (emailType)
            {
                case EmailMessageType.ForgotPassword:
                    body.Append(string.Format("( {0} )", sender.UserName));
                    body.Append(" has requested to reset login credentials in the Aesthetic Neural Network System");
                    break;

                case EmailMessageType.ResetUser:
                    body.Append("Your Credentials for The Aesthetic Neural Network(ANN) need to be reset.");
                    body.Append(lineBreak);
                    body.Append("Please use the following link to login and reset your new password.");
                    body.Append(lineBreak);
                    body.Append("Your User Name will  be your email address : " + reciever.Email);
                    body.Append(lineBreak);

                    // body.Append(string.Format("User ID: {0}", reciever.UserName));
                    // body.Append(lineBreak);
                    // body.Append(string.Format("Temporary Password: {0}", tempPassword));
                    tempLink = string.Format("<a href='{0}/Login/FirstTimeLogin?p={1}&u={2}' target='_blank'>Click here</a>", identityUrl, EncryptionManager.Encrypt(tempPassword), reciever.UserName);
                    body.Append(lineBreak);

                    body.Append(lineBreak);

                    body.Append(@"Link: " + tempLink);
                    break;

                case EmailMessageType.RoleChanged:
                    body.Append("Your access level for Aesthetic Neural Network(ANN) has changed from " + prevRole + " to " + curRole + ".");
                    body.Append(lineBreak);
                    body.Append("Please logout and login again for the updates to effect.");
                    body.Append("Your User Name will  be your email address : " + reciever.Email);
                    body.Append(lineBreak);
                    tempLink = string.Format("<a href='{0}/Login/' target='_blank'>Click here</a>", identityUrl);

                    body.Append(lineBreak);

                    body.Append(@"Link: " + tempLink);
                    break;

                case EmailMessageType.UserCreated:
                    body.Append("You have been added as a new user in the Aesthetic Neural Network System.");
                    body.Append(lineBreak);
                    body.Append("Please use the following link to log into the system and set your own password.");
                    body.Append(lineBreak);
                    // body.Append(string.Format("User ID: {0}", reciever.UserName));
                    // body.Append(lineBreak);
                    // body.Append(string.Format("Temporary Password: {0}", tempPassword));
                    // body.Append(lineBreak);
                    tempLink = string.Format("<a href='{0}/Login/FirstTimeLogin?p={1}&u={2}' target='_blank'>Click here</a>", identityUrl, EncryptionManager.Encrypt(tempPassword), reciever.UserName);

                    body.Append(lineBreak);

                    body.Append(@"Link: " + tempLink);
                    break;

                case EmailMessageType.ActivateUser:
                    body.Append("Your access to Aesthetic Neural Network (ANN) is now active.");
                    body.Append(lineBreak);
                    body.Append("Your User Name will  be your email address : " + reciever.Email);
                    body.Append(lineBreak);
                    body.Append("Please use the following link to login and set your own password.");
                    body.Append(lineBreak);
                    // body.Append(string.Format("User ID: {0}", reciever.UserName));
                    // body.Append(lineBreak);
                    // body.Append(string.Format("Temporary Password: {0}", tempPassword));
                    // body.Append(lineBreak);
                    tempLink = string.Format("<a href='{0}/Login/FirstTimeLogin?p={1}&u={2}' target='_blank'>Click here</a>", identityUrl, EncryptionManager.Encrypt(tempPassword), reciever.UserName);

                    body.Append(lineBreak);

                    body.Append(@"Link: " + tempLink);
                    break;

                case EmailMessageType.PasswordExpired:
                    body.Append(String.Format("This is an auto-generated email to remind you that your Password for account - ( {0} ) will expire in {1} Day(s)", reciever.UserName, days));
                    body.Append(lineBreak);
                    body.Append("If expired, you will need to reset your password using 'Forgot Password' function to access the system again.");

                    body.Append(lineBreak);

                    body.Append(@"Link: " + tempLink);
                    break;
            }

            body.Append("</div></body></html>");

            return body.ToString();
        }

        /// <summary>
        /// Emails the subject.
        /// </summary>
        /// <param name="emailType">Type of the email.</param>
        /// <returns></returns>
        private string EmailSubject(EmailMessageType emailType)
        {
            string title = string.Empty;

            switch (emailType)
            {
                case EmailMessageType.ForgotPassword:
                    title = "Aesthetic Neural Network System : Reset User Credential";
                    break;

                case EmailMessageType.ResetUser:
                    title = "Aesthetic Neural Network : Credentials Reset";
                    break;

                case EmailMessageType.RoleChanged:
                    title = "Aesthetic Neural Network : Access Level Change";
                    break;

                case EmailMessageType.UserCreated:
                    title = "Aesthetic Neural Network System : You Have Been Added as a User in the System";
                    break;

                case EmailMessageType.ActivateUser:
                    title = "Aesthetic Neural Network : User Activation";
                    break;

                case EmailMessageType.PasswordExpired:
                    title = "Aesthetic Neural Network System : Your Password is about to expire";
                    break;
            }

            return title;
        }

    }

}

