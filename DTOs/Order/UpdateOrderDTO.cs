public class UpdateOrderDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<int> ProductIds { get; set; }
}