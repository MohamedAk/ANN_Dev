using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Anzu.AnnPortal.Identity.Service.IdentityExtentions
{
    public class CustomizePasswordValidation : IIdentityValidator<string>
    {
        private int _lengthRequired;
        private bool _isChangePW;
        private int _maxLength;

        public CustomizePasswordValidation(bool isChangePW = false)
        {
            _lengthRequired = 8;
            _isChangePW = isChangePW;
            _maxLength = 20;
        }

        public Task<IdentityResult> ValidateAsync(string Item)
        {
            if (String.IsNullOrEmpty(Item) || Item.Length < _lengthRequired || Item.Length > _maxLength)
            {
                return Task.FromResult(IdentityResult.Failed(String.Format("Password length should be {0} - {1} characters", _lengthRequired, _maxLength)));
            }

            bool hasUpperCaseLetter = false;
            bool hasLowerCaseLetter = false;
            bool hasNumber = false;
            bool hasNonAlphaNumeric = false;
            bool notContainUserId = true;

            if (_isChangePW)
            {
                string currentUser = HttpContext.Current.User.Identity.Name;
                if (Regex.IsMatch(Item, currentUser))
                {
                    notContainUserId = false;
                }
            }

            string upperCasePattern = @"[A-Z]";

            if (Regex.IsMatch(Item, upperCasePattern))
            {
                hasUpperCaseLetter = true;
            }

            string lowerCasePattern = @"[a-z]";

            if (Regex.IsMatch(Item, lowerCasePattern))
            {
                hasLowerCaseLetter = true;
            }

            string numericPattern = @"[0-9]";

            if (Regex.IsMatch(Item, numericPattern))
            {
                hasNumber = true;
            }

            string nonAlphaNumericPattern = @"[^a-zA-Z\d\s]";

            if (Regex.IsMatch(Item, nonAlphaNumericPattern))
            {
                hasNonAlphaNumeric = true;
            }

            bool[] results = new bool[4];
            results[0] = hasUpperCaseLetter;
            results[1] = hasLowerCaseLetter;
            results[2] = hasNumber;
            results[3] = hasNonAlphaNumeric;

            List<string> errors=new List<string>();
            int resultTrueCount = results.Where(b => b == true).Count();

            if (resultTrueCount >= 3 && notContainUserId)
            {
                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                //if(!notContainUserId)
                //{
                //    errors.Add("The Password cannot contain User ID");
                //}
                //if (!(resultTrueCount >= 3) || !notContainUserId)
                //{
                    errors.Add("Password should be at least 8 characters in length and should at least have 3 of the following characters: UPPERCASE LETTERS, lowercase letters, numbers, non-alphanumeric characters: !@#$%^&*()-_=+[]{}\\|’”;:,<.>/? and should not contain the USER ID");
                //}
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            
        }
    }
}