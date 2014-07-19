using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PPOB.Models.ViewModel
{
    public class Sms
    {
        [Required]
        public string Account { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [DisplayName("手机")]
        public string Phone { get; set; }

        [Required]
        [DisplayName("内容")]
        public string Content { get; set; }

    }
}