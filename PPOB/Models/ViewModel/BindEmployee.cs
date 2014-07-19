using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PPOB.Models.ViewModel
{
    public class BindEmployee
    {
        [DisplayName("手机号")]
        [RegularExpression("^([0-9]{11})$", ErrorMessage = "请输入正确手机号")]
        public string Mobile { get; set; }

        [DisplayName("认证码")]
        [RegularExpression("^([0-9]{4})$", ErrorMessage = "请输入正确验证码")]
        public string Code { get; set; }
    }
}