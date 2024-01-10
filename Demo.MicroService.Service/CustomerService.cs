using Demo.MicroService.IService;
using Demo.MicroService.Models;
using Demo.MicroService.Repository;

namespace Demo.MicroService.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerRepository _customerRepository;
        //初始信号量为1，最大信号量为5。这表示最多只有一个线程可以访问共享资源，但最多可以有5个线程等待访问
        //private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 5); // 控制并发访问的信号量

        //读写分离
        private readonly SemaphoreSlim _readSemaphore = new SemaphoreSlim(5); // 控制读操作的并发数量
        private readonly SemaphoreSlim _writeSemaphore = new SemaphoreSlim(1); // 控制写操作的并发数量

        public CustomerService(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<decimal> UpdateCustomerScoreAsync(long customerId, decimal scoreChange)
        {
            await _writeSemaphore.WaitAsync(); // 等待信号量
            try
            {
                return await _customerRepository.UpdateCustomerScoreAsync(customerId, scoreChange);
            }
            finally
            {
                _writeSemaphore.Release(); // 释放信号量
            }

        }

        public async Task<List<Customer>> GetLeaderboardAsync(int? start, int? end)
        {

            await _readSemaphore.WaitAsync(); // 等待信号量
            try
            {
                return await _customerRepository.GetLeaderboardAsync(start, end);
            }
            finally
            {
                _readSemaphore.Release(); // 释放信号量
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
            await _readSemaphore.WaitAsync(); // 等待信号量
            try
            {
                // 模拟异步并行处理，使用 Task.WhenAll 方法
                var customerTask = Task.Run(() => _customerRepository.GetCustomerByIdAsync(customerId));
                var highTask = Task.Run(() => _customerRepository.GetHighRankCustomersAsync(customerId, high));
                var lowTask = Task.Run(() => _customerRepository.GetLowRankCustomersAsync(customerId, low));

                await Task.WhenAll(customerTask, highTask, lowTask);

                var result = new List<Customer>();
                result.Add(customerTask.Result);
                result.AddRange(highTask.Result);
                result.AddRange(lowTask.Result);

                return result.OrderBy(c=>c.Rank).ToList();
            }
            finally
            {
                _readSemaphore.Release(); // 释放信号量
            }
        }
    }
}