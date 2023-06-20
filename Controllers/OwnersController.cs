using DogGo.Repositories;
using DogGo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

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
        public ActionResult Create(Owner owner)
        {
            try
            {
                _ownerRepo.AddOwner(owner);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(owner);
            }
        }

        // GET: Owners/Edit/5
        public ActionResult Edit(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);

            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // POST: Owners/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Owner owner)
        {
            try
            {
                _ownerRepo.UpdateOwner(owner);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(owner);
            }
        }

        // GET: Owner/Delete/5
        public ActionResult Delete(int id)
        {
            Owner _owner = _ownerRepo.GetOwnerById(id);
            return View(_owner);
        }

        // POST: Owner/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Owner owner)
        {
            try
            {
                _ownerRepo.DeleteOwner(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(owner);
            }
        }
    }
}
