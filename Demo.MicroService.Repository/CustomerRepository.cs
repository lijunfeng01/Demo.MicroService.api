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
        //使用SortedSet和SortedDictionary
        private Dictionary<long, Customer> _customerDictionary;
        private SortedSet<Customer> _sortedCustomersSet;

        public CustomerRepository()
        {
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