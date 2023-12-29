namespace Demo.MicroService.Models
{
    public class UpdateCustomerScoreRequest
    {
        public long CustomerId { get; set; }
        public decimal ScoreChange { get; set; }
    }

    public class GetCustomersByRankRequest
    {
        public int Start { get; set; }
        public int End { get; set; }
    }

    public class GetCustomerByIdRequest
    {
        public long CustomerId { get; set; }
        public int High { get; set; }
        public int Low { get; set; }
    }
}
