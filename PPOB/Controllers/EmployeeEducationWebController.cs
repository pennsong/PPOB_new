using PPOB.Models;
using PPOB.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using PPOB.Models.Constant;

namespace PPOB.Controllers
{
    public class EmployeeEducationWebController : ApiController
    {
        private PPOBContext db = new PPOBContext();

        // GET api/<controller>/5
        public object Get(int id)
        {
            var result = db.EmployeeEducation.Where(a => a.EmployeeId == id).Select(a => new { a.School, a.Major, a.Degree, a.Start, a.End }).ToList();
            if (result == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                var degrees = Enum.GetValues(typeof(Degree));
                var degreeOption = from object value in degrees
                                   select new
                                   {
                                       Id = (int)value,
                                       Name = value.ToString()
                                   };

                return new
                {
                    EmployeeEducations = result.Select(a => new { a.School, a.Major, a.Degree, Start = a.Start.ToString("yyyy-MM-dd"), End = (a.End.HasValue ? a.End.Value.ToString("yyyy-MM-dd") : null) }),
                    DegreeOption = degreeOption
                };
            }
        }

        public string Post(int id, [FromBody]List<EmployeeEducation> employeeEducations)
        {
            Employee employee = db.Employee.Include(a => a.EmployeeEducations).Where(a => a.Id == id).Single();
            List<EmployeeEducation> nd = db.EmployeeEducation.Where(a => a.EmployeeId == id).ToList();
            foreach (var i in nd)
            {
                db.EmployeeEducation.Remove(i);
            }
            employee.EmployeeEducations = employeeEducations;

            try
            {
                db.SaveChanges();
                return "ok";
            }
            catch (Exception e)
            {
                return "error:" + e.Message;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}