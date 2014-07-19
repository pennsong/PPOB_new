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
    public class EmployeeEnterDoc
    {
        [DisplayName("系统ID")]
        public int Id { get; set; }

        [DisplayName("员工")]
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [DisplayName("资料")]
        public int ClientDocumentId { get; set; }
        public virtual ClientDocument ClientDocument { get; set; }

        [Required]
        [DisplayName("微信路径")]
        public string WXPath { get; set; }

        [DisplayName("本地路径")]
        public string LocalPath { get; set; }

        [DisplayName("收到实物")]
        public bool Received { get; set; }
        
    }
}