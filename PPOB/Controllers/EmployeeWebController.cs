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
    public class EmployeeWebController : ApiController
    {
        private PPOBContext db = new PPOBContext();
        // GET api/<controller>
        public IEnumerable<Employee> Get()
        {
            return db.Employee.ToList();
        }

        // GET api/<controller>/5
        public object Get(int id)
        {
            var result = db.Employee.Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                //var employeeOBStatuses = Enum.GetValues(typeof(EmployeeOBStatus));
                //var employeeOBStatusOption = from object value in employeeOBStatuses
                //                             select new
                //                             {
                //                                 Id = (int)value,
                //                                 Name = value.ToString()
                //                             };

                var sexes = Enum.GetValues(typeof(Sex));
                var sexOption = from object value in sexes
                                select new
                                {
                                    Id = (int)value,
                                    Name = value.ToString()
                                };

                var documentTypes = Enum.GetValues(typeof(DocumentType));
                var documentTypeOption = from object value in documentTypes
                                         select new
                                         {
                                             Id = (int)value,
                                             Name = value.ToString()
                                         };

                var marriages = Enum.GetValues(typeof(Marriage));
                var marriageOption = from object value in marriages
                                     select new
                                     {
                                         Id = (int)value,
                                         Name = value.ToString()
                                     };

                var degrees = Enum.GetValues(typeof(Degree));
                var degreeOption = from object value in degrees
                                   select new
                                   {
                                       Id = (int)value,
                                       Name = value.ToString()
                                   };

                var hukouTypes = Enum.GetValues(typeof(HukouType));
                var hukouTypeOption = from object value in hukouTypes
                                      select new
                                      {
                                          Id = (int)value,
                                          Name = value.ToString()
                                      };

                var cp = db.ClientCity.Where(a => a.ClientId == result.ClientId && ((a.CityId == null && result.CityId == null) || a.CityId == result.CityId)).SingleOrDefault();
                var eDoc = result.EmployeeEnterDocs;
                var docItems = (
                                from b in cp.ClientEnterDocuments
                                join c in eDoc on b.Id equals c.ClientDocumentId into b_c
                                from bc in b_c.DefaultIfEmpty()
                                select new { Code = b.Id, Name = b.Name, WXPath = (bc == null ? null : bc.WXPath), LocalPath = (bc == null ? null : bc.LocalPath) }).ToList();

                return new
                {
                    result.Id,
                    result.Name,
                    ClientName = result.Client.Name,
                    result.Mobile,
                    CityName = result.City.Name,
                    EmployeeOBStatus = Enum.GetName(typeof(EmployeeOBStatus), result.EmployeeOBStatus),
                    result.EnglishName,
                    Sex = (result.Sex == null ? null : Enum.GetName(typeof(Sex), result.Sex)),
                    DocumentType = (result.DocumentType == null ? null : Enum.GetName(typeof(DocumentType), result.DocumentType)),
                    result.DocumentNumber,
                    Birthday = (result.Birthday.HasValue ? result.Birthday.Value.ToString("yyyy-MM-dd") : null),
                    Marriage = (result.Marriage == null ? null : Enum.GetName(typeof(Marriage), result.Marriage)),
                    result.Nation,
                    Yhy = result.Yhy.HasValue ? result.Yhy.Value ? "是" : "否" : null,
                    Ysy = result.Ysy.HasValue ? result.Ysy.Value ? "是" : "否" : null,
                    result.FixPhone,
                    result.Email,
                    Degree = (result.Degree == null ? null : Enum.GetName(typeof(Degree), result.Degree)),
                    HukouType = (result.HukouType == null ? null : Enum.GetName(typeof(HukouType), result.HukouType)),
                    result.HujiAddress,
                    result.HujiZipCode,
                    result.Address,
                    result.Phone,
                    result.ZipCode,
                    result.EmergencyContactPerson,
                    result.EmergencyContactPhone,
                    EverPension = result.EverPension.HasValue ? result.EverPension.Value ? "是" : "否" : null,
                    EverAccumulation = result.EverAccumulation.HasValue ? result.EverAccumulation.Value ? "是" : "否" : null,
                    result.EnterDate,
                    EmployeeEducations = result.EmployeeEducations.Select(a => new { a.School, a.Major, Degree = Enum.GetName(typeof(Degree), a.Degree), Start = a.Start.ToString("yyyy-MM-dd"), End = (a.End.HasValue ? a.End.Value.ToString("yyyy-MM-dd") : null) }),
                    DocItems = docItems,
                    //EmployeeOBStatusOption = employeeOBStatusOption,
                    SexOption = sexOption,
                    DocumentTypeOption = documentTypeOption,
                    MarriageOption = marriageOption,
                    DegreeOption = degreeOption,
                    HukouTypeOption = hukouTypeOption
                };
            }
        }

        //// POST api/<controller>
        //public void Post(int id, [FromBody]List<EmployeeEducation> ees)
        //{
        //    Employee employee = db.Employee.Include(a => a.EmployeeEducations).Where(a => a.Id == id).Single();
        //    List<EmployeeEducation> nd = db.EmployeeEducation.Where(a => a.EmployeeId == id).ToList();
        //    foreach (var i in nd)
        //    {
        //        db.EmployeeEducation.Remove(i);
        //    }
        //    employee.EmployeeEducations = ees;
        //    db.SaveChanges();
        //}

        // PUT api/<controller>/5
        public string Post(int id, [FromBody]Employee employee)
        {
            var result = db.Employee.Find(id);
            result.Mobile = employee.Mobile;
            result.EnglishName = employee.EnglishName;
            result.Sex = employee.Sex;
            result.DocumentType = employee.DocumentType;
            result.DocumentNumber = employee.DocumentNumber;
            result.Birthday = employee.Birthday;
            result.Marriage = employee.Marriage;
            result.Nation = employee.Nation;
            result.Yhy = employee.Yhy;
            result.Ysy = employee.Ysy;
            result.FixPhone = employee.FixPhone;
            result.Email = employee.Email;
            result.Degree = employee.Degree;
            result.HukouType = employee.HukouType;
            result.HujiAddress = employee.HujiAddress;
            result.HujiZipCode = employee.HujiZipCode;
            result.Address = employee.Address;
            result.Phone = employee.Phone;
            result.ZipCode = employee.ZipCode;
            result.EmergencyContactPerson = employee.EmergencyContactPerson;
            result.EmergencyContactPhone = employee.EmergencyContactPhone;
            result.EverPension = employee.EverPension;
            result.EverAccumulation = employee.EverAccumulation;
            result.EnterDate = employee.EnterDate;

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

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
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