using Demo.MicroService.Models;

namespace Demo.MicroService.IService
{
    public interface ICustomerService
    {
        Task<decimal> UpdateCustomerScoreAsync(long customerId, decimal scoreChange);
        Task<CustomerWithNeighbors> GetCustomerByIdAsync(long customerId, int high, int low);
        Task<List<Customer>> GetLeaderboardAsync(int? start, int? end);
    }
}