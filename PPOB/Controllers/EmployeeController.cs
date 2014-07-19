using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PPOB.Models;
using PPOB.Models.ViewModel;

namespace PPOB.Controllers
{
    public class EmployeeController : Controller
    {
        private PPOBContext db = new PPOBContext();

        // GET: /Employee/
        public ActionResult Index()
        {
            var employee = db.Employee.Include(e => e.City).Include(e => e.Client);
            return View(employee.ToList());
        }

        // GET: /Employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: /Employee/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.City, "Id", "Name");
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name");
            return View();
        }

        // POST: /Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ClientId,Mobile,RandomCode,CityId,EmployeeOBStatus,OpenId,EnglishName,Sex,DocumentType,DocumentNumber,Birthday,Marriage,Nation,Yhy,Ysy,FixPhone,Email,Degree,HukouType,HujiAddress,HujiZipCode,Address,Phone,ZipCode,EmergencyContactPerson,EmergencyContactPhone,EverPension,EverAccumulation,EnterDate,Code")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employee.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.City, "Id", "Name", employee.CityId);
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", employee.ClientId);
            return View(employee);
        }

        // GET: /Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.City, "Id", "Name", employee.CityId);
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", employee.ClientId);
            return View(employee);
        }

        // POST: /Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ClientId,Mobile,RandomCode,CityId,EmployeeOBStatus,OpenId,EnglishName,Sex,DocumentType,DocumentNumber,Birthday,Marriage,Nation,Yhy,Ysy,FixPhone,Email,Degree,HukouType,HujiAddress,HujiZipCode,Address,Phone,ZipCode,EmergencyContactPerson,EmergencyContactPhone,EverPension,EverAccumulation,EnterDate,Code")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.City, "Id", "Name", employee.CityId);
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", employee.ClientId);
            return View(employee);
        }

        // GET: /Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: /Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employee.Find(id);
            db.Employee.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SendSms(int id)
        {
            Employee employee = db.Employee.Find(id);
            return null;
            if (employee == null)
            {
                return null;
            }
            Sms sms = new Sms { Account = "88010108", Password = "cbff36039c3d0212b3e34c23dcde1456", Phone = employee.Mobile, Content = employee.RandomCode };
            return View(sms);
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
