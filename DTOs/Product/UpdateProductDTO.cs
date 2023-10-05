public class UpdateProductDTO
{
    public string Nombre { get; set; }
    public decimal Precio { get; set; }
    public IFormFile ImageFile { get; set; }
}