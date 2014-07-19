using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PPOB.Models
{
    public class ClientCity
    {
        [DisplayName("系统ID")]
        public int Id { get; set; }

        [DisplayName("客户")]
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        [DisplayName("城市")]
        public int? CityId { get; set; }
        public virtual City City { get; set; }

        [DisplayName("入职资料")]
        public virtual ICollection<ClientDocument> ClientEnterDocuments { get; set; }
    }
}