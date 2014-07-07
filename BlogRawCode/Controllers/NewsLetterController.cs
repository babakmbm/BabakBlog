using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.Net.Mail;

namespace Blog.Controllers
{
    public class NewsLetterController : Controller
    {
        private BlogModel model = new BlogModel();
        public bool IsAdmin { get { return Session["IsAdmin"] != null && (bool)Session["IsAdmin"]; } }

        [ChildActionOnly]
        public ActionResult EmailCount()
        {
            ViewBag.IsAdmin = IsAdmin;
            ViewData["EmailCount"] = model.EmailBanks.Count();
            return PartialView("NewsLetterEmailSummery");
        }

        public ActionResult NewsLetterIndex()
        {
            ViewBag.IsAdmin = IsAdmin;
            if (IsAdmin)
            {
                var emails = model.EmailBanks.AsEnumerable();
                return View(emails);
            }
            else
            {
                return View("NotAdmin");
            }
        }

        [HttpPost]
        public ActionResult EnterEmail(string e)
        {
            if (!string.IsNullOrWhiteSpace(e))
            {
                var existCheck = (from EM in model.EmailBanks
                                 where EM.Email == e
                                 select EM).ToList();
                if (existCheck.Count <= 0)
                {
                    EmailBank email = new EmailBank()
                    {
                        Email = e
                    };
                    try
                    {
                        model.EmailBanks.Add(email);
                        model.SaveChanges();
                        ViewBag.Message = "ایمیل زیر با موفقیت در خبر نامه تک لیست ثبت شد:";
                        ViewBag.extrainfo = email.Email;
                        return View("Success");
                    }
                    catch
                    {
                        ViewBag.Message = "متاسفانه مشکلی در انجام عملیات پیش آمده است. لطفا مجددا تلاش کنید.";
                        return View("Error");
                    }
                }
                else
                {
                    ViewBag.Message = "شما قبلا با همین ایمیل، در خبرنامه ثبت نام کرده اید:";
                    ViewBag.extraInfo = e;
                    return View("Error");
                }
            }
            else
            {
                ViewBag.Message = "هیچ ایمیلی وارد نشده است.";
                return View("Error");
            }

        }

        public ActionResult DeleteEmail(int id)
        {
            ViewBag.IsAdmin = IsAdmin;
            if (IsAdmin)
            {
                var email = model.EmailBanks.Where(x => x.ID == id).First();
                model.EmailBanks.Remove(email);
                model.SaveChanges();
                ViewBag.Message = "ایمیل مورد نظر حذف شد.";
                return View("Success");
            }
            return View("NotAdmin");
        }

    }
}
