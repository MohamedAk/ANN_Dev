using Anzu.AnnPortal.Business.Core.Core;
using Anzu.AnnPortal.Common.Model.Portal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web;

namespace Anzu.AnnPortal.Web.UI.Handlers
{
    /// <summary>
    /// Summary description for ProfileImageUploadHandler
    /// </summary>
    public class ProfileImageUploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpFileCollection files = context.Request.Files;

            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userName = claims.Where(cl => cl.Type == "UserId").FirstOrDefault();
            PracticeService userService = new PracticeService();

            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFile file = files[i];

                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    fileData = binaryReader.ReadBytes(file.ContentLength);
                }

                string base64 = Convert.ToBase64String(fileData);

                ProfileImageHandlerDTO postData = new ProfileImageHandlerDTO();
                postData.ImageData = base64;
                postData.UserId = userName.Value;
                HttpResponseMessage response;
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var identityDomain = System.Web.Configuration.WebConfigurationManager.AppSettings["identityService"].ToString();

                response = client.PostAsJsonAsync(identityDomain + "api/Account/UpdateProfilePicture", postData).Result;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}