using DogGo.Models;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public interface IWalkRepository
    {
        public List<Walk> GetAllWalks();

        public List<Walk> GetWalksByWalkerId(int id);
        public void AddWalk(Walk walk);
    }
}
