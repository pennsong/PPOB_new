using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PPOB.Models.ViewModel
{
    public class EmployeeSummary
    {
        public virtual Employee Employee { get; set; }

        public virtual ICollection<EmployeeEducation> EmployeeEducations { get; set; }

        public virtual ICollection<EmployeeEnterDoc> EmployeeEnterDocs { get; set; }

    }
}