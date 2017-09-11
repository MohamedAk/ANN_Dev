//----------------------------------------------------------------------- 
// <copyright file="EmailMessageType.cs" company="Brandix i3"> 
//     Copyright Brandix i3 2015. All rights reserved. 
// </copyright>
// <summary>
// The EmailMessageType class. target CLR version 4.0.30319.34209.
// Created by : 
// Created date time: 
// </summary>
//-----------------------------------------------------------------------
namespace Anzu.AnnPortal.Identity.Common.Model.Enum
{
    public enum EmailMessageType
    {
        /// <summary>
        /// The user created
        /// </summary>
        UserCreated = 0,
        /// <summary>
        /// The forgot password
        /// </summary>
        ForgotPassword = 1,
        /// <summary>
        /// The reset user
        /// </summary>
        ResetUser = 2,
        /// <summary>
        /// The role changed
        /// </summary>
        RoleChanged = 3,
        /// <summary>
        /// The activate user
        /// </summary>
        ActivateUser = 4,
        /// <summary>
        /// The password expired
        /// </summary>
        PasswordExpired = 5,
        /// <summary>
        /// The selected cube refresh started
        /// </summary>
        SelectedCubeRefreshStarted = 6,
        /// <summary>
        /// All cure refresh started
        /// </summary>
        AllCubeRefreshStarted = 7,
        /// <summary>
        /// The reset user
        /// </summary>
        CubeRefreshCompleted = 8
    }
}
