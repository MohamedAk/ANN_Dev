//----------------------------------------------------------------------- 
// <copyright file="EmailService.cs" company="Brandix i3"> 
//     Copyright Brandix i3 2015. All rights reserved. 
// </copyright>
// <summary>
// The EmailService class. target CLR version 4.0.30319.34209.
// Created by : 
// Created date time: 
// </summary>
//-----------------------------------------------------------------------
using System;
using System.Net.Mail;

namespace Anzu.AnnPortal.Common.Notification
{
    public class EmailHandler
    {
        /// <summary>
        /// The SMTP client service
        /// </summary>
        private SmtpClientService smtpClientService = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailHandler"/> class.
        /// </summary>
        public EmailHandler()
        {
            smtpClientService = new SmtpClientService();
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="mailMessage">The mail message.</param>
        /// <returns></returns>
        public bool SendMail(MailMessage mailMessage)
        {
            bool hasSent = false;

            try
            {
                smtpClientService.Client.Send(mailMessage);
                hasSent = true;
            }
            catch (Exception ex)
            {
                throw;
            }

            return hasSent;
        }
    }
}
