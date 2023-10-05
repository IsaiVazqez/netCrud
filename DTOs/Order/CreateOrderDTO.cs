public class CreateOrderDTO
{
    public int UserId { get; set; }
    public List<int> ProductIds { get; set; } // Lista de IDs de productos para la orden.
}