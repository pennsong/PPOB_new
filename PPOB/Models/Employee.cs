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
    public class Employee
    {
        [DisplayName("系统ID")]
        public int Id { get; set; }

        [Required]
        [DisplayName("中文名")]
        [MaxLength(50)]
        public string Name { get; set; }

        [DisplayName("客户")]
        public int ClientId { get; set; }

        public virtual Client Client { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("手机号")]
        public string Mobile { get; set; }

        [Required]
        [DisplayName("识别码")]
        [MaxLength(10)]
        public string RandomCode { get; set; }

        [DisplayName("社保城市")]
        public int? CityId { get; set; }
        public virtual City City { get; set; }

        [DisplayName("入职状态")]
        public EmployeeOBStatus EmployeeOBStatus { get; set; }

        [MaxLength(200)]
        [DisplayName("OpenId")]
        public string OpenId { get; set; }

        [DisplayName("英文名")]
        [MaxLength(50)]
        public string EnglishName { get; set; }

        [DisplayName("性别")]
        public Sex? Sex { get; set; }

        [DisplayName("证件类型")]
        public DocumentType? DocumentType { get; set; }

        [DisplayName("证件号码")]
        [MaxLength(50)]
        public string DocumentNumber { get; set; }

        [DisplayName("出生日期")]
        [Column(TypeName = "Date")]
        public DateTime? Birthday { get; set; }

        [DisplayName("婚姻状况")]
        public Marriage? Marriage { get; set; }

        [DisplayName("民族")]
        [MaxLength(50)]
        public string Nation { get; set; }

        [DisplayName("是否已孕")]
        public bool? Yhy { get; set; }

        [DisplayName("是否已育")]
        public bool? Ysy { get; set; }

        [MaxLength(50)]
        [DisplayName("固定电话")]
        public string FixPhone { get; set; }

        [MaxLength(50)]
        [DisplayName("邮箱")]
        public string Email { get; set; }

        [DisplayName("文化程度")]
        public Degree? Degree { get; set; }

        [DisplayName("户籍性质")]
        public HukouType? HukouType { get; set; }

        [MaxLength(100)]
        [DisplayName("户籍地址")]
        public string HujiAddress { get; set; }

        [MaxLength(50)]
        [DisplayName("户籍地邮编")]
        public string HujiZipCode { get; set; }

        [MaxLength(100)]
        [DisplayName("联系地址")]
        public string Address { get; set; }

        [MaxLength(50)]
        [DisplayName("联系电话")]
        public string Phone { get; set; }

        [MaxLength(50)]
        [DisplayName("联系地址邮编")]
        public string ZipCode { get; set; }

        [MaxLength(50)]
        [DisplayName("紧急联系人")]
        public string EmergencyContactPerson { get; set; }

        [MaxLength(50)]
        [DisplayName("紧急联系人方式")]
        public string EmergencyContactPhone { get; set; }

        [DisplayName("是否缴纳过社保")]
        public bool? EverPension { get; set; }

        [DisplayName("是否缴纳过公积金")]
        public bool? EverAccumulation { get; set; }

        [DisplayName("开始工作时间")]
        [Column(TypeName = "Date")]
        public DateTime? EnterDate { get; set; }

        [DisplayName("待输入")]
        public string Code { get; set; }

        [DisplayName("教育经历")]
        public virtual ICollection<EmployeeEducation> EmployeeEducations { get; set; }

        [DisplayName("入职资料")]
        public virtual ICollection<EmployeeEnterDoc> EmployeeEnterDocs { get; set; }
    }
}
