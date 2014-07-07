using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.Net.Mail;
using System.IO;

namespace Blog.Controllers
{
    public class ContactMeController : Controller
    {
        private BlogModel model = new BlogModel();
        public bool IsAdmin { get { return Session["IsAdmin"] != null && (bool)Session["IsAdmin"]; } }

        [ChildActionOnly]
        public ActionResult ContactCount()
        {
            ViewBag.IsAdmin = IsAdmin;
            ViewData["ContactCount"] = model.ContactMes.Count();
            return PartialView("ContactSummary");
        }

        public ActionResult ContactIndex()
        {
            ViewBag.IsAdmin = IsAdmin;
            if (IsAdmin)
            {
                var contactMe = model.ContactMes.AsEnumerable();
                return View(contactMe);
            }
            else
            {
                return View("NotAdmin");
            }
        }

        public ActionResult ContactDetails(int id)
        {
            ViewBag.IsAdmin = IsAdmin;
            if (IsAdmin)
            {
                var contact = model.ContactMes.Where(x => x.ID == id).First();
                return View(contact);
            }
            else
            {
                return View("NotAdmin");
            }
        }

        [HttpGet]
        public ActionResult CreateContact()
        {
            ViewBag.IsAdmin = IsAdmin;
            ContactMe contact = new ContactMe();
            contact.DateTime = DateTime.Now;
            return View(contact);
        } 

        
        [HttpPost]
        public ActionResult CreateContact(ContactMe contact)
        {
            ViewBag.IsAdmin = IsAdmin;
            if (ModelState.IsValid)
            {
                try
                {
                    MailMessage msg = new MailMessage();
                    model.ContactMes.Add(contact);
                    model.SaveChanges();
                    ViewBag.Message = "پیام شما با موفقیت ارسال شد. پاسخ شما در اسرع وقت به پست الکترونیکی وارد شده ارسال خواهد شد.";
                    return View("Success");
                }
                catch
                {
                    ViewBag.Message = "متاسفانه مشکلی در انجام عملیات پیش آمده است. لطفا مجددا تلاش کنید.";
                    return View("Error");
                }
                
            }
            return View(contact);
        }

        //[HttpPost]
        //public ActionResult ContactModal(string name, string sub, string email, string body)
        //{
        //    if (!string.IsNullOrWhiteSpace(name)
        //        && !string.IsNullOrWhiteSpace(email)
        //        && !string.IsNullOrWhiteSpace(body)
        //       )
        //    {
        //        ContactMe contact = new ContactMe()
        //        {
        //            Name=name,
        //            Subject=sub,
        //            Email=email,
        //            Body=body,
        //            DateTime = DateTime.Now
        //        };
        //        try
        //        {
        //            model.ContactMes.Add(contact);
        //            model.SaveChanges();
        //            ViewBag.Message = "پیام شما با موفقیت دریافت شد:";
                    
        //            return View("Success");
        //        }
        //        catch
        //        {
        //            ViewBag.Message = "متاسفانه مشکلی در انجام عملیات پیش آمده است. لطفا مجددا تلاش کنید.";
        //            return View("Error");
        //        }

        //    }
        //    else
        //    {
        //        ViewBag.Message = "متاسفانه مشکلی در انجام عملیات پیش آمده است. لطفا مجددا تلاش کنید.";
        //        if (string.IsNullOrWhiteSpace(name))
        //        {
        //            ViewBag.err1 = "لطفا نام و نام خانوادگی خود را وارد کنید";
        //        }
        //        if (string.IsNullOrWhiteSpace(email))
        //        {
        //            ViewBag.err2 = "لطفا پست الکترونیکی خود را وارد کنید";
        //        }
        //        if (string.IsNullOrWhiteSpace(body))
        //        {
        //            ViewBag.err3 = "لطفا متن پیام خود را وارد کنید";
        //        }
        //        return View("Error");
        //    }
                
        //}
        

        public ActionResult ContactDelete(int id)
        {
            ViewBag.IsAdmin = IsAdmin;
            if (IsAdmin)
            {
                var contact = model.ContactMes.Where(x => x.ID == id).First();
                model.ContactMes.Remove(contact);
                model.SaveChanges();
                ViewBag.Message = "پیام مورد نظر با موفقیت حذف شد.";
                return View("Success");
            }
            return View("NotAdmin");
        }

        public ActionResult AboutMe()
        {
            ViewBag.IsAdmin = IsAdmin;
            return View();
        }
    }
}
