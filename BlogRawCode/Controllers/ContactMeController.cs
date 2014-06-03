using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.Net.Mail;

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
                var contactMes = model.ContactMes.AsEnumerable();
                return View(contactMes);
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
                    return View("Error");
                }
                
            }
            return View(contact);
        }
        

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
    }
}
