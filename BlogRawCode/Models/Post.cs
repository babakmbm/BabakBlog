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
    
    public partial class Post
    {
        
        public Post()
        {
            this.Comments = new HashSet<Comment>();
            this.Tags = new HashSet<Tag>();
        }
    
        public int ID { get; set; }

        [Required]
        public System.DateTime DateTime { get; set; }

        [Required]
        public System.DateTime DateModified { get; set; } /* THE PROBLEM SOLVED IN: http://stackoverflow.com/questions/5502872/efcode-first-property-null-problem */

        [Required(ErrorMessage="عنوان پست را وارد کنید")]
        [Display(Name="عنوان")]
        public string Title { get; set; }
        
        [AllowHtml]
        [Required(ErrorMessage = "متن پست را وارد کنید")]
        [DisplayName("متن پست")]
        public string Body { get; set; }
        
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
