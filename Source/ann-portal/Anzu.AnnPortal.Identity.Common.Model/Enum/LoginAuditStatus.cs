//----------------------------------------------------------------------- 
// <copyright file="LoginAuditStatus.cs" company="Brandix i3"> 
//     Copyright Brandix i3 2015. All rights reserved. 
// </copyright>
// <summary>
// The LoginAuditStatus class. target CLR version 4.0.30319.34209.
// Created by : 
// Created date time: 
// </summary>
//-----------------------------------------------------------------------
namespace Anzu.AnnPortal.Identity.Common.Model.Enum
{
    public enum LoginAuditStatus
    {
        /// <summary>
        /// The login success
        /// </summary>
        LoginSuccess = 1,
        /// <summary>
        /// The login failed
        /// </summary>
        LoginFailed = 2,
        /// <summary>
        /// The account locked out
        /// </summary>
        AccountLockedOut = 3,
        /// <summary>
        /// The invalid user name
        /// </summary>
        InvalidUserName = 4,
        /// <summary>
        /// The deactivated user
        /// </summary>
        DeactivatedUser = 5
    }
}
