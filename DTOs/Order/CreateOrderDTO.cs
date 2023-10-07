public class CreateOrderDTO
{
    public int UserId { get; set; }
    public List<ProductOrderDTO> Products { get; set; }
}