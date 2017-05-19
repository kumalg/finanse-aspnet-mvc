﻿using System.Linq;
using System.Web.Mvc;
using Finanse_aspnet_mvc.Models;
using Finanse_aspnet_mvc.Models.Operations;

namespace Finanse_aspnet_mvc.Controllers
{
    public class OperationsController : Controller
    {
        StackMoneyDb _db = new StackMoneyDb();
        // GET: NewOperation
        public ActionResult Index()
        {
            return View(_db.Operations.ToList());
        }

        // GET: NewOperation/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NewOperation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NewOperation/Create
        [HttpPost]
        public ActionResult Create(Operation operation)
        {
            try
            {
                // TODO: Add insert logic here
                _db.Operations.Add(operation);
                _db.SaveChanges();

                return RedirectToAction("Index","Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: NewOperation/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NewOperation/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: NewOperation/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NewOperation/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}