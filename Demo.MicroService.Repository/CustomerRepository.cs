using Demo.MicroService.Common;
using Demo.MicroService.Models;

namespace Demo.MicroService.Repository
{
    /// <summary>
    /// 仓储
    /// </summary>
    public class CustomerRepository
    {
        //使用SortedDictionary
        private SortedDictionary<Customer, bool> _customerDictionary;

        public CustomerRepository()
        {
            _customerDictionary = new SortedDictionary<Customer, bool>(new CustomerComparer())
           {
               { new Customer { CustomerId = 15514665, Score = 124, Rank = 1 }, true },
               { new Customer { CustomerId = 81546541, Score = 113, Rank = 2 }, true },
               { new Customer { CustomerId = 53274324, Score = 95, Rank = 6 }, true },
               { new Customer { CustomerId = 6144320, Score = 93, Rank = 7 }, true },
               { new Customer { CustomerId = 8009471, Score = 93, Rank = 8 }, true },
               { new Customer { CustomerId = 11028481, Score = 93, Rank = 9 }, true },
               { new Customer { CustomerId = 1745431, Score = 100, Rank = 3 }, true },
               { new Customer { CustomerId = 76786448, Score = 100, Rank = 4 }, true },
               { new Customer { CustomerId = 254814111, Score = 96, Rank = 5 }, true },
               
               { new Customer { CustomerId = 38819, Score = 92, Rank = 10 }, true },
            };        
        }
        /// <summary>
        /// 通过customerId获取指定Customer对象
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<Customer> GetCustomerByIdAsync(long customerId)
        {
            //模拟异步操作，例如更新数据库
            await Task.Delay(100); //替换为实际的异步操作
            //实际的数据库操作
            foreach (var pair in _customerDictionary)
            {
                if (pair.Key.CustomerId == customerId)
                {
                    return pair.Key;
                }
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
            Customer targetCustomer = await GetCustomerByIdAsync(customerId);

            if (targetCustomer == null)
            {
                return null;
            }

            // 获取排名比目标客户高的所有客户
            var highRankCustomers = _customerDictionary.Keys
                .Where(c => c.Rank < targetCustomer.Rank) // 所有等级较高的客户
                .OrderByDescending(c => c.Rank) // 按等级升序排列的
                .Take(count) 
                .ToList();

            return highRankCustomers;
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
            Customer targetCustomer = await GetCustomerByIdAsync(customerId);

            if (targetCustomer == null)
            {
                return null;
            }

            //获取所有排名比目标客户低的客户
            var lowRankCustomers = _customerDictionary.Keys
                .Where(c => c.Rank > targetCustomer.Rank) 
                .OrderBy(c => c.Rank) 
                .Take(count) 
                .ToList();

            return lowRankCustomers;
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
            Customer customerToUpdate = await GetCustomerByIdAsync(customerId);
            if (customerToUpdate == null)
            {
                // CustomerID不存在添加数据
                customerToUpdate = new Customer { CustomerId = customerId, Score = newScore };
                _customerDictionary.Add(customerToUpdate, true);
            }
            else
            {
                // CustomerID已经存在，更新它的分数
                _customerDictionary.Remove(customerToUpdate);
                customerToUpdate.Score += newScore;
                _customerDictionary.Add(customerToUpdate, true); //更新客户信息
            }

            UpdateRanks(); // 修改排名

            return customerToUpdate.Score; // 返回跟新后的数据
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
            var leaderboard = _customerDictionary.Keys
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
            foreach (var customer in _customerDictionary.Keys.OrderByDescending(c => c.Score))
            {
                customer.Rank = rank++;
            }
        }

    }
}