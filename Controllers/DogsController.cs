using DogGo.Models;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DogGo.Controllers
{
    public class DogsController : Controller
    {
        private readonly IDogRepository _dogRepo;

        public DogsController(IDogRepository dogRepo)
        {
            _dogRepo = dogRepo;
        }
        // GET: DogsController
        public ActionResult Index()
        {
            List<Dog> _dogs = _dogRepo.GetAllDogs();
            return View(_dogs);
        }

        // GET: DogsController/Details/5
        public ActionResult Details(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            return View(dog);
        }

        // GET: DogsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DogsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog dog)
        {
            try
            {
                _dogRepo.AddDog(dog);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: DogsController/Edit/5
        public ActionResult Edit(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            if(dog == null)
            {
                return NotFound();
            }
            else
            {
                return View(dog);
            }
        }

        // POST: DogsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Dog dog)
        {
            try
            {
                _dogRepo.UpdateDog(dog);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(dog);
            }
        }

        // GET: DogsController/Delete/5
        public ActionResult Delete(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            if(dog == null)
            {
                return NotFound();
            }
            else
            {
                return View(dog);
            }
        }

        // POST: DogsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Dog dog)
        {
            try
            {
                _dogRepo.DeleteDog(dog.Id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(dog);
            }
        }
    }
}
