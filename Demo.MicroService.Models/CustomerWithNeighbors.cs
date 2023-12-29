namespace Demo.MicroService.Models
{
    public class CustomerWithNeighbors
    {
        public Customer? Customer { get; set; }
        public List<Customer>? HighNeighbors { get; set; }
        public List<Customer>? LowNeighbors { get; set; }
    }
}
