﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Blog.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    
    public partial class Comment
    {
        public int ID { get; set; }
        public int PostID { get; set; }
        public System.DateTime DateTime { get; set; }

        [Required(ErrorMessage = "لطفا نام کامل خود را وارد کنید")]
        [DisplayName("نام و نام خانوادگی")]
        public string Name { get; set; }

        [Required(ErrorMessage = "لطفا ایمیل خود را وارد کنید")]
        [DisplayName("پست الکترونیکی (پنهان خواهد شد)")]
        [EmailAddress(ErrorMessage = "لطفا از یک ایمیل واقعی استفاده کنید")]
        public string Email { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "لطفا متن پیام خود را وارد کنید")]
        [DisplayName("متن اصلی")]
        public string Body { get; set; }

        public virtual Post Post { get; set; }
    }
}
