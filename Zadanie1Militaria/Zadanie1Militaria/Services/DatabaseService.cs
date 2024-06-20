using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Zadanie1Militaria.Models;

namespace Zadanie1Militaria.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    return await connection.QueryAsync<Order>("SELECT * FROM OrderTable");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving orders: {ex.Message}");
                return new List<Order>();
            }
        }

        public async Task SaveBillingEntriesAsync(IEnumerable<BillingEntry> entries)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    foreach (var entry in entries)
                    {
                        await connection.ExecuteAsync(
                            "INSERT INTO BillingEntries (OrderId, EntryType, Amount, EntryDate) VALUES (@OrderId, @EntryType, @Amount, @EntryDate)",
                            entry);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving billing entries: {ex.Message}");
            }
        }
    }
}