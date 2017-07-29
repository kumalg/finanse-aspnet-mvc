﻿using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Finanse_aspnet_mvc.Models;
using Finanse_aspnet_mvc.Models.Helpers;
using Finanse_aspnet_mvc.Models.Operations;

namespace Finanse_aspnet_mvc.Controllers {
    [Authorize]
    public class OperationsController : Controller {
        private readonly StackMoneyDb _db = new StackMoneyDb();

        // GET: Operations
        public async Task<ActionResult> Index(int lastId = -1, int pageSize = 5) {
            if (!Request.IsAjaxRequest())
                return View();

            var model = await _db.Operations.ToListAsync();

            var items = model
                .OrderByDescending(o => o.Date)
                .SkipWhile(o => lastId != -1 && o.Id != lastId)     // skip while lastId is not equal actual id and there is lastId (!= -1)
                .Skip(lastId == -1 ? 0 : 1)                         // if there is no lastId (== -1) then skip 0
                .Take(pageSize);

            var result = new {
                lastId = items.LastOrDefault()?.Id ?? -1,
                partialView = RenderRazorViewToString("_OperationsList", items)
            };

            return Content(JsonHelper.ToJsonString(result), "application/json"); //PartialView("_OperationsList", items);
        }

        public string RenderRazorViewToString(string viewName, object model) {
            ViewDataDictionary viewData = new ViewDataDictionary {
                Model = model
            };
            using (var sw = new StringWriter()) {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, viewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        // GET: Operations/Details/5
        public async Task<ActionResult> Details(int? id) {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var operation = await _db.Operations.FindAsync(id);
            if (operation == null)
                return HttpNotFound();

            return PartialView("_Details", operation);
        }

        // GET: Operations/Create
        public ActionResult Create() {
            return PartialView("_Create");
        }

        // POST: Operations/Create
        [HttpPost]
        public async Task<ActionResult> Create(Operation operation) {
            if (ModelState.IsValid) {
                _db.Operations.Add(operation);
                await _db.SaveChangesAsync();
                return Json(new { success = true });
            }
            return PartialView("_Create", operation);
        }

        // GET: Operations/Edit/5
        public async Task<ActionResult> Edit(int? id) {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var operation = await _db.Operations.FindAsync(id);
            if (operation == null)
                return HttpNotFound();

            return PartialView("_Edit", operation);
        }

        // POST: Operations/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Operation operation) {
            if (ModelState.IsValid) {
                _db.Entry(operation).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return Json(new { success = true });
            }
            return PartialView("_Edit", operation);
        }

        // GET: Operations/Delete/5
        public async Task<ActionResult> Delete(int? id) {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var operation = await _db.Operations.FindAsync(id);
            if (operation == null)
                return HttpNotFound();

            return PartialView("_Delete", operation);
        }

        // POST: Operations/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id) {
            var operation = await _db.Operations.FindAsync(id);

            if (operation == null)
                return HttpNotFound();

            _db.Operations.Remove(operation);
            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }

        protected override void Dispose(bool disposing) {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
