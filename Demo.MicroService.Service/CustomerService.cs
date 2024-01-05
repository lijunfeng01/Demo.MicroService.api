using Demo.MicroService.Models;
using Demo.MicroService.IService;
using Demo.MicroService.Repository;
using System.Threading;

namespace Demo.MicroService.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerRepository _customerRepository;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 5); // 控制并发访问的信号量

        public CustomerService(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<decimal> UpdateCustomerScoreAsync(long customerId, decimal scoreChange)
        {
            await _semaphore.WaitAsync(); // 等待信号量
            try
            {
                return await _customerRepository.UpdateCustomerScoreAsync(customerId, scoreChange);
            }
            finally
            {
                _semaphore.Release(); // 释放信号量
            }

        }

        public async Task<List<Customer>> GetLeaderboardAsync(int? start, int? end)
        {

            await _semaphore.WaitAsync(); // 等待信号量
            try
            {
                return await _customerRepository.GetLeaderboardAsync(start, end);
            }
            finally
            {
                _semaphore.Release(); // 释放信号量
            }

        }

        /// <summary>
        /// 通过customerid获取客户
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="high"></param>
        /// <param name="low"></param>
        /// <returns></returns>
        public async Task<List<Customer>> GetCustomerByIdAsync(long customerId, int high, int low)
        {
            await _semaphore.WaitAsync(); // 等待信号量
            try
            {
                // 模拟异步并行处理，使用 Task.WhenAll 方法
                var customerTask = Task.Run(() => _customerRepository.GetCustomerByIdAsync(customerId));
                var highTask = Task.Run(() => _customerRepository.GetHighRankCustomersAsync(customerId, high));
                var lowTask = Task.Run(() => _customerRepository.GetLowRankCustomersAsync(customerId, low));

                await Task.WhenAll(customerTask, highTask, lowTask);

                var result = new List<Customer>();
                result.AddRange(highTask.Result);
                result.AddRange(lowTask.Result);

                return result;
            }
            finally
            {
                _semaphore.Release(); // 释放信号量
            }
        }
    }
}