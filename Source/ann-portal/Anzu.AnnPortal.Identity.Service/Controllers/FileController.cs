using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anzu.AnnPortal.Identity.Service.Controllers
{
    public class FileController : Controller
    {
        /// <summary>
        /// Saves the specified logofile.
        /// </summary>
        /// <param name="Logofile">The logofile.</param>
        /// <returns></returns>
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Save(IEnumerable<System.Web.HttpPostedFileBase> files)
        {
            string base64 = null;

            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    byte[] fileData = null;
                    using (var binaryReader = new BinaryReader(file.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(file.ContentLength);
                    }

                    base64 = Convert.ToBase64String(fileData);

                    //base64 = new ImageHelper().GetBase64ofTumbnailImage(file);
                }
            }

            // Return an empty string to signify success
            return Json(new { data = base64 }, "text/plain");
        }

        //<summary>
        //Deletes the site logo.
        //</summary>
        //<param name="id">The identifier.</param>
        //<returns></returns>
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult DeleteSiteLogo(string[] fileNames)
        {

            return Content("");
        }

    }
}