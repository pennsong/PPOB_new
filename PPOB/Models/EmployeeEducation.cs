using PPOB.Models.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PPOB.Models
{
    public class EmployeeEducation
    {
        [DisplayName("系统ID")]
        public int Id { get; set; }

        [DisplayName("员工")]
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [Required]
        [DisplayName("学校")]
        [MaxLength(50)]
        public string School { get; set; }

        [Required]
        [DisplayName("专业")]
        [MaxLength(50)]
        public string Major { get; set; }

        [DisplayName("学历")]
        public Degree Degree { get; set; }

        [DisplayName("开始")]
        [Column(TypeName = "Date")]
        public DateTime Start { get; set; }

        [DisplayName("结束")]
        [Column(TypeName = "Date")]
        public DateTime? End { get; set; }
    }
}