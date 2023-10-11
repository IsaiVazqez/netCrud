public static class ImageHelper
{
    public static async Task<string> SaveImageAsync(IFormFile imageFile, string localImagePath, List<string> acceptedExtensions)
    {
        if (imageFile == null)
        {
            throw new Exception("No hay ningún archivo que guardar en el servidor. Seleccione un archivo he intente nuevamente.");
        }

        string fileExtension = Path.GetExtension(imageFile.FileName);
        if (!acceptedExtensions.Contains(fileExtension.ToLower()))
        {
            throw new Exception("El archivo es de una extensión no permitida por el sistema. Archivos permitidos: " + string.Join(", ", acceptedExtensions));
        }

        var filePath = Path.Combine(localImagePath, imageFile.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        return "/images/" + imageFile.FileName;
    }
}
