using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.IO;

namespace Blog.Controllers
{
    public class UploadController : Controller
    {
        public bool IsAdmin { get { return Session["IsAdmin"] != null && (bool)Session["IsAdmin"]; } }

        [HttpGet]
        public ActionResult UploadFiles()
        {
            if (IsAdmin)
            {
                ViewBag.IsAdmin = IsAdmin;
                return View();
            }
            else
            {
                return View("NotAdmin");
            }
        }

        [HttpPost]
        public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> Fs, string pa)
        {
            if (IsAdmin)
            {
                if (!string.IsNullOrWhiteSpace(pa) || pa=="مسیر دلخواه را از موارد زیر انتخاب کنید")
                {
                    ViewBag.IsAdmin = IsAdmin;
                    Files e = new Files()
                    {
                        UploadingFiles = Fs
                    };

                    foreach (var f in e.UploadingFiles)
                    {
                        try
                        {
                            if (f.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(f.FileName);
                                var path = Path.Combine(Server.MapPath(pa), fileName);
                                f.SaveAs(path);
                            }
                        }
                        catch
                        {
                            ViewBag.Message = "آپلود فایلها با مشکل مواجه شد، لطفا مجددا تلاش کنید.";
                            ViewBag.Err1 = "تذکرات:";
                            ViewBag.Err2 = "1- تمامی فایلها باید حاوی اطلاعات باشد.";
                            ViewBag.Err3 = "2- حجم تمامی فایلها نباید بیشتر از 2GB باشد.";
                            ViewBag.Err4 = "3- ممکن است فایلی وارد نشده باشد.";
                            return View("Error");
                        }
                    }

                    return RedirectToAction("UpLoadFiles");
                }
                else
                {
                    ViewBag.Message = "آپلود فایلها با مشکل مواجه شد، لطفا مجددا تلاش کنید.";
                    ViewBag.Err1 = "تذکرات:";
                    ViewBag.Err2 = "هیچ مسیری وارد نشده است.";
                    return View("Error");
                }
            }
            else
            {
                return View("NotAdmin");
            }
            
        }

    }
}
