using Demo.MicroService.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Demo.MicroService.Repository
{
    /// <summary>
    /// 仓储
    /// </summary>
    public class CustomerRepository
    {
        //使用缓存来存储排行榜数据，以减少对数据库或其他持久性存储的频繁访问
        private readonly List<Customer> _customers;
        private readonly MemoryCache _cache;//添加本地缓存

        public CustomerRepository()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _customers = new List<Customer>() {
                 //获取列表测试数据
                 new Customer { CustomerId = 15514665, Score = 124, Rank = 1 },
                 new Customer { CustomerId = 81546541, Score = 113, Rank = 2 },
                 new Customer { CustomerId = 1745431, Score = 100, Rank = 3 },
                 new Customer { CustomerId = 8009471, Score = 93, Rank = 8 },
                 new Customer { CustomerId = 11028481, Score = 93, Rank = 9 },
                 new Customer { CustomerId = 38819, Score = 92, Rank = 10 },
                 new Customer { CustomerId = 76786448, Score = 100, Rank = 4 },
                 new Customer { CustomerId = 254814111, Score = 96, Rank = 5 },
                 new Customer { CustomerId = 53274324, Score = 95, Rank = 6 },
                 new Customer { CustomerId = 6144320, Score = 93, Rank = 7 },


                 //3.2测试数据
                 //new Customer { CustomerId = 76786448, Score = 78, Rank = 41 },
                 //new Customer { CustomerId = 254814111, Score = 65, Rank = 42 },
                 //new Customer { CustomerId = 53274324, Score = 64, Rank = 43 },
                 //new Customer { CustomerId = 6144320, Score = 32, Rank = 44 },

                  //3.3测试数据                                             
                 //new Customer { CustomerId = 7786448, Score = 313, Rank = 89 },
                 //new Customer { CustomerId = 54814111, Score = 301, Rank = 90 },
                 //new Customer { CustomerId = 7777777, Score = 298, Rank = 91 },
                 //new Customer { CustomerId = 96144320, Score = 298, Rank = 92 },
                 //new Customer { CustomerId = 16144320, Score = 270, Rank = 93 },
                 //new Customer { CustomerId = 2000437, Score = 239, Rank = 94 },
            };
        }

        #region 加入缓存处理方法
        /// <summary>
        /// 通过customerId异步获取有邻居的客户
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="high"></param>
        /// <param name="low"></param>
        /// <returns></returns>
        public async Task<CustomerWithNeighbors> GetCustomerWithNeighborsByIdAsync(long customerId, int high, int low)
        {
            var customer = await GetCustomerByIdAsync(customerId);
            var highNeighbors = await GetHighRankNeighborsAsync(customer.Rank, high);
            var lowNeighbors = await GetLowRankNeighborsAsync(customer.Rank, low);

            return new CustomerWithNeighbors
            {
                Customer = customer,
                HighNeighbors = highNeighbors,
                LowNeighbors = lowNeighbors
            };
        }

        /// <summary>
        /// 通过customerid获取客户，加入本地缓存处理
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        private async Task<Customer> GetCustomerByIdAsync(long customerId)
        {
            // 从缓存中获取客户信息
            if (_cache.TryGetValue(customerId, out Customer customer))
            {
                return customer;
            }

            // 如果缓存中不存在，从数据库或其他持久性存储中获取客户信息
            customer = await RetrieveCustomerFromDatabaseAsync(customerId);

            // 将客户信息存入缓存
            _cache.Set(customerId, customer, TimeSpan.FromMinutes(10)); // 设置缓存过期时间

            return customer;
        }

        /// <summary>
        /// 获取客户高排名邻居列表，加入本地缓存处理
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private async Task<List<Customer>> GetHighRankNeighborsAsync(int rank, int count)
        {
            // 从缓存中获取高排名邻居列表
            if (_cache.TryGetValue($"HighRankNeighbors_{rank}_{count}", out List<Customer> neighbors))
            {
                return neighbors;
            }

            // 如果缓存中不存在，从数据库或其他持久性存储中获取高排名邻居列表
            neighbors = await RetrieveHighRankNeighborsFromDatabaseAsync(rank, count);

            // 将高排名邻居列表存入缓存
            _cache.Set($"HighRankNeighbors_{rank}_{count}", neighbors, TimeSpan.FromMinutes(10)); // 设置缓存过期时间

            return neighbors;
        }

        // <summary>
        /// 获取客户低排名邻居列表，加入本地缓存处理
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private async Task<List<Customer>> GetLowRankNeighborsAsync(int rank, int count)
        {
            // 从缓存中获取低排名邻居列表
            if (_cache.TryGetValue($"LowRankNeighbors_{rank}_{count}", out List<Customer> neighbors))
            {
                return neighbors;
            }

            // 如果缓存中不存在，从数据库或其他持久性存储中获取低排名邻居列表
            neighbors = await RetrieveLowRankNeighborsFromDatabaseAsync(rank, count);

            // 将低排名邻居列表存入缓存
            _cache.Set($"LowRankNeighbors_{rank}_{count}", neighbors, TimeSpan.FromMinutes(10));
            return neighbors;
        }

        /// <summary>
        /// 从数据库（内存）中检索客户
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        private async Task<Customer> RetrieveCustomerFromDatabaseAsync(long customerId)
        {
            // 从数据库中获取客户信息的逻辑
            // 模拟从数据库中获取客户信息
            await Task.Delay(100); // 模拟数据库访问延迟
            var customer = _customers.Find(c => c.CustomerId == customerId);
            if (customer == null)
            {
                return null;
            }
            return customer;
        }

        /// <summary>
        /// 从数据库(内存)中获取高排名邻居列表
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private async Task<List<Customer>> RetrieveHighRankNeighborsFromDatabaseAsync(int rank, int count)
        {
            // 从数据库中获取高排名邻居列表的逻辑

            // 模拟从数据库中获取高排名邻居列表
            await Task.Delay(100); // 模拟数据库访问延迟
            var neighbors = _customers.Where(c => c.Rank < rank).OrderByDescending(c => c.Rank).Take(count).ToList();// 示例高排名邻居列表
            return neighbors;
        }

        /// <summary>
        /// 从数据库（内存）中获取低排名邻居列表
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private async Task<List<Customer>> RetrieveLowRankNeighborsFromDatabaseAsync(int rank, int count)
        {
            // 从数据库中获取低排名邻居列表的逻辑

            // 模拟从数据库中获取低排名邻居列表
            await Task.Delay(100); // 模拟数据库访问延迟
            var neighbors = _customers.Where(c => c.Rank > rank).OrderBy(c => c.Rank).Take(count).ToList();
            return neighbors;
        }


        #endregion

        #region 未加入缓存处理方法
        /// <summary>
        /// 通过customerid获取客户
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="high"></param>
        /// <param name="low"></param>
        /// <returns></returns>
        public async Task<CustomerWithNeighbors> GetCustomerByIdAsync(long customerId, int high, int low)
        {
            //模拟异步操作，例如更新数据库中的数据
            await Task.Delay(100); //正常业务替换为实际的异步操作
            var customer = _customers.Find(c => c.CustomerId == customerId);
            if (customer == null)
            {
                return null;
            }

            //高分数相邻数据
            var highNeighbors = _customers.Where(c => c.Rank < customer.Rank).OrderByDescending(c => c.Rank).Take(high).ToList();
            //低分数相邻数据
            var lowNeighbors = _customers.Where(c => c.Rank > customer.Rank).OrderBy(c => c.Rank).Take(low).ToList();

            return new CustomerWithNeighbors
            {
                Customer = customer,
                HighNeighbors = highNeighbors,
                LowNeighbors = lowNeighbors
            };
        }
        #endregion

        /// <summary>
        /// 更新客户排名榜
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="scoreChange"></param>
        /// <returns></returns>
        public async Task<decimal> UpdateCustomerScoreAsync(long customerId, decimal scoreChange)
        {
            //模拟异步操作，例如更新数据库中的数据
            await Task.Delay(100); //正常业务替换为实际的异步操作
            var existingCustomer = _customers.Find(c => c.CustomerId == customerId);
            if (existingCustomer == null)
            {
                var newCustomer = new Customer { CustomerId = customerId, Score = scoreChange };
                _customers.Add(newCustomer);
            }
            else
            {
                existingCustomer.Score += scoreChange;
            }

            UpdateRanks();
            return existingCustomer?.Score ?? scoreChange;
        }

        /// <summary>
        /// 获取排行榜
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public async Task<List<Customer>> GetLeaderboardAsync(int? start, int? end)
        {
            //模拟异步操作，例如更新数据库中的数据
            await Task.Delay(100); //正常业务替换为实际的异步操作
            if (start.HasValue && end.HasValue)
            {
                return _customers.OrderBy(c => c.Rank).Skip(start.Value - 1).Take(end.Value - start.Value + 1).ToList();
            }
            else
            {
                return _customers.OrderBy(c => c.Rank).ToList();
            }
        }

        /// <summary>
        /// 更新排名
        /// </summary>
        private void UpdateRanks()
        {
            //实际业务数据库操作更新排名
            var rankedCustomers = _customers.OrderByDescending(c => c.Score).ToList();
            for (int i = 0; i < rankedCustomers.Count; i++)
            {
                rankedCustomers[i].Rank = i + 1;
            }
        }
    }
}