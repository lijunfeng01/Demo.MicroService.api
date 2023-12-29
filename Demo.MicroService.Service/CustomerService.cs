using Demo.MicroService.Models;
using Dome.MicroService.IService;
using Dome.MicroService.Repository;

namespace Dome.MicroService.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerRepository _customerRepository;

        public CustomerService(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<decimal> UpdateCustomerScoreAsync(long customerId, decimal scoreChange)
        {
            return await _customerRepository.UpdateCustomerScoreAsync(customerId, scoreChange);
        }

        public async Task<List<Customer>> GetLeaderboardAsync(int? start, int? end)
        {
            return await _customerRepository.GetLeaderboardAsync(start, end);
        }

        public async Task<CustomerWithNeighbors> GetCustomerByIdAsync(long customerId, int high, int low)
        {    
            //改用加缓存处理方法
            //return await _customerRepository.GetCustomerByIdAsync(customerId, high, low);
            return await _customerRepository.GetCustomerWithNeighborsByIdAsync(customerId, high, low); 
        }
    }
}