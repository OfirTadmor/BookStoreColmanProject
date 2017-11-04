using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BookNet
{
    public static class Utils
    {
        public static bool SaveImage(HttpPostedFileBase file)
        {
            try
            {
                if (!file.ContentType.ToLower().StartsWith("image/"))
                    return false;
                
                string imagesDirectory = HttpContext.Current.Server.MapPath("~/Images");

                if (!Directory.Exists(imagesDirectory))
                {
                    Directory.CreateDirectory(imagesDirectory);
                }

                string fullFilePath = Path.Combine(imagesDirectory + "/" + file.FileName);

                if (!File.Exists(fullFilePath))
                {
                    file.SaveAs(fullFilePath);
                }
            }
            catch (Exception e)
            {
                return false;                
            }

            return true;
        }
    }
}