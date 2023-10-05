public class OrderReadDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<ProductDTO> Products { get; set; }
}
