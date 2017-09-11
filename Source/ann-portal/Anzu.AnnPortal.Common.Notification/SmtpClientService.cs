//----------------------------------------------------------------------- 
// <copyright file="SmtpClientService.cs" company="Brandix i3"> 
//     Copyright Brandix i3 2015. All rights reserved. 
// </copyright>
// <summary>
// The SmtpClientService class. target CLR version 4.0.30319.34209.
// Created by : 
// Created date time: 
// </summary>
//-----------------------------------------------------------------------
using System;
using System.Net.Mail;
using System.Web.Configuration;

namespace Anzu.AnnPortal.Common.Notification
{
    public class SmtpClientService
    {
        /// <summary>
        /// The client
        /// </summary>
        private SmtpClient client;

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public SmtpClient Client
        {
            get { return client; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpClientService"/> class.
        /// </summary>
        public SmtpClientService()
        {
            client = new SmtpClient();

            try
            {
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                client.PickupDirectoryLocation = WebConfigurationManager.AppSettings["PickupDirectoryLocation"];
                client.UseDefaultCredentials = false;
                client.Host = WebConfigurationManager.AppSettings["SmtpHost"];
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
