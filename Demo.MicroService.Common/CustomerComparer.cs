using Demo.MicroService.Models;
using System.Collections.Concurrent;

namespace Demo.MicroService.Common
{
    public class CustomerComparer : IComparer<Customer>
    {
        public int Compare(Customer x, Customer y)
        {
            // 在这里实现你自定义的比较逻辑
            // 按分数降序排列
            if (x.Score == y.Score)
            {
                return x.CustomerId.CompareTo(y.CustomerId); // 分数相同时按照ID升序排列
            }
            return y.Score.CompareTo(x.Score);
        }
    }
}