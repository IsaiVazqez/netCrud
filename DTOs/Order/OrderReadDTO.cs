public class OrderReadDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } 

    public List<ProductDTO> Products { get; set; }
}
