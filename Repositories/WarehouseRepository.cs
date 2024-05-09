using Microsoft.Data.SqlClient;
using Tutorial6.Models;
using Tutorial6.Models.DTOs;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;

namespace Tutorial6.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly IConfiguration _configuration;

        public WarehouseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Product> FindProductById(int id)
        {
            var query = "SELECT * FROM Product WHERE IdProduct = @Id";

            using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();

            using SqlDataReader dataReader = await command.ExecuteReaderAsync();

            if (await dataReader.ReadAsync())
            {
                return new Product()
                {
                    IdProduct = Convert.ToInt32(dataReader["IdProduct"]),
                    Name = Convert.ToString(dataReader["Name"]),
                    Description = Convert.ToString(dataReader["Description"]),
                    Price = Convert.ToDecimal(dataReader["Price"])
                };
            }

            return null;
        }

        public async Task<Warehouse> FindWarehouseById(int id)
        {
            var query = "SELECT * FROM Warehouse WHERE IdWarehouse = @Id";

            using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();

            using SqlDataReader dataReader = await command.ExecuteReaderAsync();

            if (await dataReader.ReadAsync())
            {
                return new Warehouse()
                {
                    IdWarehouse = Convert.ToInt32(dataReader["IdWarehouse"]),
                    Name = Convert.ToString(dataReader["Name"]),
                    Address = Convert.ToString(dataReader["Address"])
                };
            }

            return null;
        }

        public async Task<Order> FindOrderByProductIdAndAmount(int id, int amount)
        {
            var query = "SELECT * FROM [Order] WHERE IdProduct = @IdProduct AND Amount = @Amount";

            using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@IdProduct", id);
            command.Parameters.AddWithValue("@Amount", amount);

            await connection.OpenAsync();

            using SqlDataReader dataReader = await command.ExecuteReaderAsync();

            if (await dataReader.ReadAsync())
            {
                return new Order()
                {
                    IdOrder = Convert.ToInt32(dataReader["IdOrder"]),
                    IdProduct = Convert.ToInt32(dataReader["IdProduct"]),
                    Amount = Convert.ToInt32(dataReader["Amount"]),
                    CreatedAt = Convert.ToDateTime(dataReader["CreatedAt"]),
                    FulfilledAt = dataReader["FulfilledAt"] as DateTime? // Handle nullable DateTime
                };
            }

            return null;
        }

        public async Task ChangeFulfilledAt(int id, int amount)
        {
            DateTime date = DateTime.Now;
            var query = "UPDATE [Order] SET FulfilledAt = @Date WHERE IdProduct = @IdProduct AND Amount = @Amount";

            using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@IdProduct", id);
            command.Parameters.AddWithValue("@Amount", amount);
            command.Parameters.AddWithValue("@Date", date);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task InsertProductWarehouse(ProductWarehouseReq req)
        {
            var query = "INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt)" +
                        "VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @Date)";

            using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@IdWarehouse", req.IdWarehouse);
            command.Parameters.AddWithValue("@IdProduct", req.IdProduct);
            command.Parameters.AddWithValue("@IdOrder", req.IdOrder);
            command.Parameters.AddWithValue("@Amount", req.Amount);
            command.Parameters.AddWithValue("@Price", req.Price);
            command.Parameters.AddWithValue("@Date", req.CreatedAt);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<int> GetResultPrimaryKey(ProductWarehouseReq req)
        {
            var query = "SELECT IdProductWarehouse FROM Product_Warehouse WHERE IdWarehouse = @IdWarehouse " +
                            "AND IdProduct = @IdProduct AND IdOrder = @IdOrder";

            using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@IdWarehouse", req.IdWarehouse);
            command.Parameters.AddWithValue("@IdProduct", req.IdProduct);
            command.Parameters.AddWithValue("@IdOrder", req.IdOrder);

            await connection.OpenAsync();

            var res = await command.ExecuteScalarAsync();
            int.TryParse(res?.ToString(), out int id);

            return id;
        }
    }
}
