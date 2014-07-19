using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PPOB.Models
{
    public class ClientDocument
    {
        [DisplayName("系统ID")]
        public int Id { get; set; }

        [DisplayName("客户")]
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        [Required]
        [DisplayName("名称")]
        [MaxLength(50)]
        public string Name { get; set; }

        [DisplayName("入职")]
        public virtual ICollection<ClientCity> EnterClientCities { get; set; }
    }
}