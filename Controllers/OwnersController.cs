using DogGo.Repositories;
using DogGo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DogGo.Controllers
{
    public class OwnersController : Controller
    {
        private readonly IOwnerRepository _ownerRepo;

        public OwnersController(IOwnerRepository ownerRepo)
        {
            _ownerRepo = ownerRepo;
        }
        // GET: Owner
        public ActionResult Index()
        {
            List<Owner> _owners = _ownerRepo.GetAllOwners();
            return View(_owners);
        }

        // GET: Owner/Details/5
        public ActionResult Details(int id)
        {
            Owner _owner = _ownerRepo.GetOwnerById(id);
            return View(_owner);
        }

        // GET: Owner/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Owner/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Owner/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Owner/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Owner/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Owner/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
