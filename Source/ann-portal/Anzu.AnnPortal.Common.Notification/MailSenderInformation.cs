//----------------------------------------------------------------------- 
// <copyright file="MailSenderInformation.cs" company="Brandix i3"> 
//     Copyright Brandix i3 2015. All rights reserved. 
// </copyright>
// <summary>
// The MailSenderInformation class. target CLR version 4.0.30319.34209.
// Created by : 
// Created date time: 
// </summary>
//-----------------------------------------------------------------------
using System.Web.Configuration;

namespace Anzu.AnnPortal.Common.Notification
{
    public class MailSenderInformation
    {
        /// <summary>
        /// Gets or sets the sender address.
        /// </summary>
        /// <value>
        /// The sender address.
        /// </value>
        public string SenderAddress { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="MailSenderInformation"/> class.
        /// </summary>
        public MailSenderInformation()
        {
            SenderAddress = WebConfigurationManager.AppSettings["ANNEmail"];
        }
    }
}
