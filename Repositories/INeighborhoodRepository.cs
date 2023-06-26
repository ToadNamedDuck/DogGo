using DogGo.Models;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public interface INeighborhoodRepository
    {
        public List<Neighborhood> GetAll();
        public Neighborhood GetNeighborhoodById(int id);
    }
}