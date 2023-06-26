using DogGo.Repositories;
using DogGo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DogGo.Models.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace DogGo.Controllers
{
    public class WalkersController : Controller
    {
        private readonly IWalkerRepository _walkerRepo;
        private readonly IWalkRepository _walkRepo;
        private readonly INeighborhoodRepository _neighborhoodRepo;
        private readonly IOwnerRepository _ownerRepo;
        private readonly IDogRepository _dogRepo;

        public WalkersController(IWalkerRepository walkerRepo, IWalkRepository walkRepo, INeighborhoodRepository neighborhoodRepo, IOwnerRepository ownerRepo)
        {
            _walkerRepo = walkerRepo;
            _walkRepo = walkRepo;
            _neighborhoodRepo = neighborhoodRepo;
            _ownerRepo = ownerRepo;
        }

        // GET: WalkersController
        public ActionResult Index()
        {
            if(!(GetCurrentUserId() == -1))
            {
                Owner owner = _ownerRepo.GetOwnerById(GetCurrentUserId());
                List<Walker> walkers = _walkerRepo.GetAllWalkers().Where(walker => walker.NeighborhoodId == owner.NeighborhoodId).ToList();
                return View(walkers);
            }
            else
            {
                List<Walker> walkers = _walkerRepo.GetAllWalkers();
                return View(walkers);
            }
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
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                return int.Parse(id);
            }
            catch
            {
                return -1;
            }
        }
    }
}
