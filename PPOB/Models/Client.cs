using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PPOB.Models
{
    public class Client
    {
        [DisplayName("系统ID")]
        public int Id { get; set; }

        [Required]
        [DisplayName("名称")]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}