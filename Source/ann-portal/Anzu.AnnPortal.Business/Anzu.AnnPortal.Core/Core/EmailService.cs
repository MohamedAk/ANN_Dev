using Anzu.AnnPortal.Common.Notification;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Business.Core.Core
{
    public class EmailService
    {
        private readonly EmailHandler handler;
        private readonly MailSenderInformation mailSenderInfo;

        public EmailService()
        {
            handler = new EmailHandler();
            mailSenderInfo = new MailSenderInformation();
        }

        public string GetEmailBody(EmailMessageType type, string userName = "", List<string> emrList = null)
        {
            var folderPath = AppDomain.CurrentDomain.BaseDirectory + @"\EmailTemplates\";
            var templateHtml = string.Empty;
            switch (type)
            {
                case EmailMessageType.SelectedCubeRefreshStarted:
                    // Read all in html template
                    templateHtml = File.ReadAllText(folderPath + "cube_refresh_manual_start.html");
                    StringBuilder emrListString = new StringBuilder();

                    if (emrList != null)
                    {
                        foreach (var emr in emrList)
                        {
                            emrListString.Append(string.Format("<li>{0}</li>", emr));
                        }
                    }
                    templateHtml = templateHtml.Replace("@emrList", emrListString.ToString());
                    templateHtml = templateHtml.Replace("@userName", userName.Trim());

                    break;
                case EmailMessageType.AllCubeRefreshStarted:
                    templateHtml = File.ReadAllText(folderPath + "cube_refresh_all_start.html");
                    templateHtml = templateHtml.Replace("@userName", userName.Trim());
                    break;
                default:
                    break;
            }

            return templateHtml;
        }

        public string GetEmailSubject(EmailMessageType emailType)
        {
            string title = string.Empty;

            switch (emailType)
            {
                case EmailMessageType.SelectedCubeRefreshStarted:
                    title = "Aesthetic Neural Network System : Cube Refresh";
                    break;

                case EmailMessageType.AllCubeRefreshStarted:
                    title = "Aesthetic Neural Network : Cube Refresh";
                    break;
                default:
                    break;
            }

            return title;
        }

        public bool SendEmail(string to, string subject, string body)
        {
            try
            {
                var mailMessage = new MailMessage(mailSenderInfo.SenderAddress, to);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;

                handler.SendMail(mailMessage);
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public enum EmailMessageType
    {
        /// <summary>
        /// Selected cube refresh started
        /// </summary>
        SelectedCubeRefreshStarted = 1,
        /// <summary>
        /// All cube refres started
        /// </summary>
        AllCubeRefreshStarted = 2
    }
}