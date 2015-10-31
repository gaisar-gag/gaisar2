﻿namespace todo.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Models;

    public class ItemController : Controller
    {
        public ActionResult Index()
        {
            var items = DocumentDBRepository<Item>.GetItems(d => !d.Completed);
            return View(items);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description,Completed")] Item item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<Item>.CreateItemAsync(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,Completed")] Item item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<Item>.UpdateItemAsync(item.Id, item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Item item = DocumentDBRepository<Item>.GetItem(x => x.Id == id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Item item = DocumentDBRepository<Item>.GetItem(x => x.Id == id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed([Bind(Include = "Id")] string id)
        {
            await DocumentDBRepository<Item>.DeleteItemAsync(id);
            return RedirectToAction("Index");
        }

        public ActionResult Details(string id)
        {
            Item item = DocumentDBRepository<Item>.GetItem(x => x.Id == id);
            return View(item);
        }
    }
}