public class OrderReadDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }  // Nueva propiedad para almacenar el nombre del usuario

    public List<ProductDTO> Products { get; set; }
}
