using DogGo.Repositories;
using DogGo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DogGo.Models.ViewModels;
using System.Linq;

namespace DogGo.Controllers
{
    public class WalkersController : Controller
    {
        private readonly IWalkerRepository _walkerRepo;
        private readonly IWalkRepository _walkRepo;
        private readonly INeighborhoodRepository _neighborhoodRepo;

        public WalkersController(IWalkerRepository walkerRepo, IWalkRepository walkRepo, INeighborhoodRepository neighborhoodRepo)
        {
            _walkerRepo = walkerRepo;
            _walkRepo = walkRepo;
            _neighborhoodRepo = neighborhoodRepo;
        }

        // GET: WalkersController
        public ActionResult Index()
        {
            List<Walker> walkers = _walkerRepo.GetAllWalkers();
            return View(walkers);
        }

        // GET: WalkersController/Details/5
        public ActionResult Details(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);
            List<Walk> walkerWalks = _walkRepo.GetWalksByWalkerId(walker.Id).OrderBy(walk => walk.Client.Name).ToList();
            Neighborhood _neighborhood = _neighborhoodRepo.GetNeighborhoodById(walker.NeighborhoodId);
            walker.Neighborhood = _neighborhood;

            WalkerDetailsViewModel vm = new WalkerDetailsViewModel() 
            {
                Walker = walker,
                Walks = walkerWalks
            };

            if (walker == null)
            {
                return NotFound();
            }
            else
                return View(vm);
        }

        // GET: WalkersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WalkersController/Create
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

        // GET: WalkersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalkersController/Edit/5
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

        // GET: WalkersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalkersController/Delete/5
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
