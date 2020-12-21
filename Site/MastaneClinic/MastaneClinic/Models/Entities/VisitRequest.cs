using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class VisitRequest:BaseEntity
    {
        [Display(Name="نام و نام خانوادگی")]
        public string FullName { get; set; }
        [Display(Name="شماره تماس")]
        public string CellNumber { get; set; }
        [Display(Name="تاریخ و زمان مراجعه")]
        public string DateAndTime { get; set; }
        [Display(Name="نوع خدمت")]
        public Guid? ServiceId { get; set; }
        public virtual Service Service { get; set; }
    }
}