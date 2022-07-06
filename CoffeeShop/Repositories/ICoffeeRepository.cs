using CoffeeShop.Models;
using System.Collections.Generic;

namespace CoffeeShop.Repositories
{
    public interface ICoffeeRepository
    {
        Coffee Get(int id);
        List<Coffee> GetAll();
        void Update(Coffee variety);
        void Add(Coffee variety);
        void Delete(int id);
    }
}