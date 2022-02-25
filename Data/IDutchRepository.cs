using DutchTreat.Data.Entities;
using System.Collections.Generic;

namespace DutchTreat.Data
{
    public interface IDutchRepository
    {
        IEnumerable<Product> GetALLProduct();
        IEnumerable<Product> GetProductsByCategory(string Category);


        bool SaveAll();
        IEnumerable<Order> GetAllOrders(bool includeItems);
        IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems);

        Order GetOrderById(string username, int id);
        void AddEntity(object model);
    }
}