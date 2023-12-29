namespace Demo.MicroService.Models
{
    /// <summary>
    /// 客户排名表
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// 客户id
        /// </summary>
        public long CustomerId { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public decimal Score { get; set; }
        /// <summary>
        /// 排名
        /// </summary>
        public int Rank { get; set; }
    }
}