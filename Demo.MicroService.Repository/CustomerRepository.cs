using Demo.MicroService.Common;
using Demo.MicroService.Models;
using System.Collections.Concurrent;
using static System.Formats.Asn1.AsnWriter;

namespace Demo.MicroService.Repository
{
    /// <summary>
    /// 仓储
    /// </summary>
    public class CustomerRepository
    {
        //private readonly MemoryCache _cache;//添加本地缓存（弃用）
        //使用SortedSet和SortedDictionary
        private Dictionary<long, Customer> _customerDictionary;
        private SortedSet<Customer> _sortedCustomersSet;

        public CustomerRepository()
        {
            //_cache = new MemoryCache(new MemoryCacheOptions());（弃用）
            // 添加默认初始数据到_customerDictionary
            _customerDictionary = new Dictionary<long, Customer>
           {
              { 15514665, new Customer { CustomerId = 15514665, Score = 124, Rank = 1 }},
              { 81546541, new Customer { CustomerId = 81546541, Score = 113, Rank = 2 }},
              { 1745431, new Customer { CustomerId = 1745431, Score = 100, Rank = 3 }},
              { 76786448, new Customer { CustomerId = 76786448, Score = 100, Rank = 4 }},
              { 254814111, new Customer { CustomerId = 254814111, Score = 96, Rank = 5 }},
              { 53274324, new Customer { CustomerId = 53274324, Score = 95, Rank = 6 }},
              { 6144320, new Customer { CustomerId = 6144320, Score = 93, Rank = 7 }},
              { 8009471, new Customer { CustomerId = 8009471, Score = 93, Rank = 8 }},
              { 11028481, new Customer { CustomerId = 11028481, Score = 93, Rank = 9 }},
              { 38819, new Customer { CustomerId = 38819, Score = 92, Rank = 10 }},

          };
            _sortedCustomersSet = new SortedSet<Customer>(new CustomerComparer())
        {
            _customerDictionary[15514665],
            _customerDictionary[81546541],
            _customerDictionary[1745431],
            _customerDictionary[76786448],
            _customerDictionary[254814111],
            _customerDictionary[53274324],
            _customerDictionary[6144320],
            _customerDictionary[8009471],
            _customerDictionary[11028481],
            _customerDictionary[38819],


        };
            //插入列表测试数据        
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
        }

        #region 加入缓存处理方法（弃用）
        ///// <summary>
        ///// 通过customerId异步获取有邻居的客户
        ///// </summary>
        ///// <param name="customerId"></param>
        ///// <param name="high"></param>
        ///// <param name="low"></param>
        ///// <returns></returns>
        //public async Task<CustomerWithNeighbors> GetCustomerWithNeighborsByIdAsync(long customerId, int high, int low)
        //{
        //    var customer = await GetCustomerByIdAsync(customerId);
        //    var highNeighbors = await GetHighRankNeighborsAsync(customer.Rank, high);
        //    var lowNeighbors = await GetLowRankNeighborsAsync(customer.Rank, low);

        //    return new CustomerWithNeighbors
        //    {
        //        Customer = customer,
        //        HighNeighbors = highNeighbors,
        //        LowNeighbors = lowNeighbors
        //    };
        //}

        ///// <summary>
        ///// 通过customerid获取客户，加入本地缓存处理
        ///// </summary>
        ///// <param name="customerId"></param>
        ///// <returns></returns>
        //private async Task<Customer> GetCustomerByIdAsync(long customerId)
        //{
        //    // 从缓存中获取客户信息
        //    if (_cache.TryGetValue(customerId, out Customer customer))
        //    {
        //        return customer;
        //    }

        //    // 如果缓存中不存在，从数据库或其他持久性存储中获取客户信息
        //    customer = await RetrieveCustomerFromDatabaseAsync(customerId);

        //    // 将客户信息存入缓存
        //    _cache.Set(customerId, customer, TimeSpan.FromMinutes(10)); // 设置缓存过期时间

        //    return customer;
        //}

        ///// <summary>
        ///// 获取客户高排名邻居列表，加入本地缓存处理
        ///// </summary>
        ///// <param name="rank"></param>
        ///// <param name="count"></param>
        ///// <returns></returns>
        //private async Task<List<Customer>> GetHighRankNeighborsAsync(int rank, int count)
        //{
        //    // 从缓存中获取高排名邻居列表
        //    if (_cache.TryGetValue($"HighRankNeighbors_{rank}_{count}", out List<Customer> neighbors))
        //    {
        //        return neighbors;
        //    }

        //    // 如果缓存中不存在，从数据库或其他持久性存储中获取高排名邻居列表
        //    neighbors = await RetrieveHighRankNeighborsFromDatabaseAsync(rank, count);

        //    // 将高排名邻居列表存入缓存
        //    _cache.Set($"HighRankNeighbors_{rank}_{count}", neighbors, TimeSpan.FromMinutes(10)); // 设置缓存过期时间

        //    return neighbors;
        //}

        //// <summary>
        ///// 获取客户低排名邻居列表，加入本地缓存处理
        ///// </summary>
        ///// <param name="rank"></param>
        ///// <param name="count"></param>
        ///// <returns></returns>
        //private async Task<List<Customer>> GetLowRankNeighborsAsync(int rank, int count)
        //{
        //    // 从缓存中获取低排名邻居列表
        //    if (_cache.TryGetValue($"LowRankNeighbors_{rank}_{count}", out List<Customer> neighbors))
        //    {
        //        return neighbors;
        //    }

        //    // 如果缓存中不存在，从数据库或其他持久性存储中获取低排名邻居列表
        //    neighbors = await RetrieveLowRankNeighborsFromDatabaseAsync(rank, count);

        //    // 将低排名邻居列表存入缓存
        //    _cache.Set($"LowRankNeighbors_{rank}_{count}", neighbors, TimeSpan.FromMinutes(10));
        //    return neighbors;
        //}

        ///// <summary>
        ///// 从数据库（内存）中检索客户
        ///// </summary>
        ///// <param name="customerId"></param>
        ///// <returns></returns>
        //private async Task<Customer> RetrieveCustomerFromDatabaseAsync(long customerId)
        //{
        //    // 从数据库中获取客户信息的逻辑
        //    // 模拟从数据库中获取客户信息
        //    await Task.Delay(100); // 模拟数据库访问延迟
        //    var customer = _customers.Find(c => c.CustomerId == customerId);
        //    if (customer == null)
        //    {
        //        return null;
        //    }
        //    return customer;
        //}

        ///// <summary>
        ///// 从数据库(内存)中获取高排名邻居列表
        ///// </summary>
        ///// <param name="rank"></param>
        ///// <param name="count"></param>
        ///// <returns></returns>
        //private async Task<List<Customer>> RetrieveHighRankNeighborsFromDatabaseAsync(int rank, int count)
        //{
        //    // 从数据库中获取高排名邻居列表的逻辑

        //    // 模拟从数据库中获取高排名邻居列表
        //    await Task.Delay(100); // 模拟数据库访问延迟
        //    var neighbors = _customers.Where(c => c.Rank < rank).OrderByDescending(c => c.Rank).Take(count).ToList();// 示例高排名邻居列表
        //    return neighbors;
        //}

        ///// <summary>
        ///// 从数据库（内存）中获取低排名邻居列表
        ///// </summary>
        ///// <param name="rank"></param>
        ///// <param name="count"></param>
        ///// <returns></returns>
        //private async Task<List<Customer>> RetrieveLowRankNeighborsFromDatabaseAsync(int rank, int count)
        //{
        //    // 从数据库中获取低排名邻居列表的逻辑

        //    // 模拟从数据库中获取低排名邻居列表
        //    await Task.Delay(100); // 模拟数据库访问延迟
        //    var neighbors = _customers.Where(c => c.Rank > rank).OrderBy(c => c.Rank).Take(count).ToList();
        //    return neighbors;
        //}


        #endregion

        public async Task<Customer> GetCustomerByIdAsync(long customerId)
        {
            // 模拟异步操作，例如更新数据库中的数据
            await Task.Delay(100); // 正常业务替换为实际的异步操作
            // 实际数据库操作
            if (_customerDictionary.TryGetValue(customerId, out Customer customer))
            {
                return customer;
            }
            return null;
        }

        /// <summary>
        /// 获取排名高的客户信息
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<List<Customer>> GetHighRankCustomersAsync(long customerId, int count)
        {
            Customer customer = await GetCustomerByIdAsync(customerId);
            if (customer == null)
            {
                return null;
            }

            int index = _sortedCustomersSet.GetViewBetween(customer, _sortedCustomersSet.Max).Count - 1;
            int start = Math.Max(0, index - count + 1);
            int end = index + 1;

            return _sortedCustomersSet.Skip(start).Take(end - start).ToList();
        }

        /// <summary>
        /// 获取排名低的客户信息
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<List<Customer>> GetLowRankCustomersAsync(long customerId, int count)
        {
            Customer customer = await GetCustomerByIdAsync(customerId);
            if (customer == null)
            {
                return null;
            }

            int index = _sortedCustomersSet.GetViewBetween(_sortedCustomersSet.Min, customer).Count;
            int start = index;
            int end = Math.Min(_sortedCustomersSet.Count, index + count);

            return _sortedCustomersSet.Skip(start).Take(end - start).ToList();
        }

        /// <summary>
        /// 更新客户排名榜
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="scoreChange"></param>
        /// <returns></returns>
        public async Task<decimal> UpdateCustomerScoreAsync(long customerId, decimal newScore)
        {
            //模拟异步操作，例如更新数据库中的数据
            await Task.Delay(100); // 正常业务替换为实际的异步操作
            if (!_customerDictionary.ContainsKey(customerId))
            {
                // 客户ID不存在，将新客户添加到排行榜中
                var newCustomer = new Customer { CustomerId = customerId, Score = newScore, Rank = 0 };
                _customerDictionary.Add(customerId, newCustomer);
                _sortedCustomersSet.Add(newCustomer);
            }
            else
            {
                // 客户ID已经存在，更新其分数
                var existingCustomer = _customerDictionary[customerId];
                _sortedCustomersSet.Remove(existingCustomer); // 从有序集合中移除
                existingCustomer.Score += newScore;
                _sortedCustomersSet.Add(existingCustomer); // 将更新后的客户信息添加回有序集合
            }

            UpdateRanks(); // 更新排名

            return _customerDictionary[customerId].Score;
        }

        /// <summary>
        /// 获取排行榜
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public async Task<List<Customer>> GetLeaderboardAsync(int? start, int? end)
        {
            // 模拟异步操作，例如更新数据库中的数据
            await Task.Delay(100); // 正常业务替换为实际的异步操作
            var leaderboard = _sortedCustomersSet
               .Where(c => (!start.HasValue || c.Rank >= start) && (!end.HasValue || c.Rank <= end))
               .ToList();
            return leaderboard;
        }

        /// <summary>
        /// 更新排名
        /// </summary>
        private void UpdateRanks()
        {
            int rank = 1;
            foreach (var customer in _sortedCustomersSet)
            {
                customer.Rank = rank++;
            }
        }

    }
}