public class OrderProduct
{
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; } = 1; // Esto asignará un valor predeterminado de 1

}
