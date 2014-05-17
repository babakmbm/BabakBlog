using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class PWDString
    {
        public string pwd(System.DayOfWeek pwd)
        {
            switch (pwd)
            {
                case DayOfWeek.Saturday:
                    return ("شنبه");
                case DayOfWeek.Sunday:
                    return ("یکشنبه");
                case DayOfWeek.Monday:
                    return ("دوشنبه");
                case DayOfWeek.Tuesday:
                    return ("سه شنبه");
                case DayOfWeek.Wednesday:
                    return ("چهارشنبه");
                case DayOfWeek.Thursday:
                    return ("پنجشنبه");
                case DayOfWeek.Friday:
                    return ("جمعه");
                default:
                    return ("ورودی ندارد");
            }

        }
    }
}