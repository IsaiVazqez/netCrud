public class ProductDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public decimal Precio { get; set; }
    public int IdImage { get; set; }
    public MidierDTO Image { get; set; }
}