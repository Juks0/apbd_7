using Tutorial6.Models;
using Tutorial6.Models.DTOs;

namespace Tutorial6.Repositories;

public interface IWarehouseRepository
{
    Task<Product> FindProductById(int id);
    Task<Warehouse> FindWarehouseById(int id);
    Task<Order> FindOrderByProductIdAndAmount(int id, int amount);
    Task ChangeFulfilledAt(int id, int amount);
    Task InsertProductWarehouse(ProductWarehouseReq req);
    Task<int> GetResultPrimaryKey(ProductWarehouseReq req);
}