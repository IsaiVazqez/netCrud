// services/ProductService.cs

using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class ProductService
{
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly string _localImagePath;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly ILogger<ProductService> _logger;

    public ProductService(ApplicationContext context, IMapper mapper, IConfiguration configuration,
                          IWebHostEnvironment hostingEnvironment, ILogger<ProductService> logger)
    {
        _context = context;
        _mapper = mapper;
        _localImagePath = configuration["ImageSettings:LocalPath"];
        _hostingEnvironment = hostingEnvironment;
        _logger = logger;
    }

    public async Task<List<ProductDTO>> GetAllProductsAsync()
    {
        var products = await _context.Product.Include(p => p.Image).ToListAsync();
        return _mapper.Map<List<ProductDTO>>(products);
    }

    public async Task<ProductDTO> GetProductByIdAsync(int id)
    {
        var product = await _context.Product.Include(p => p.Image).FirstOrDefaultAsync(p => p.Id == id);
        return _mapper.Map<ProductDTO>(product);
    }
    public async Task<ProductDTO> CreateProductAsync(CreateProductDTO createProductDto)
    {
        BaseResponse baseResponse = new BaseResponse();
        var acceptedExtensions = new List<string> { ".jpg", ".jpeg", ".png" };

        // Validar que el archivo no es nulo
        if (createProductDto.ImageFile == null)
        {
            baseResponse.ErrorInformation = new ErrorInformation()
            {
                ErrorDescription = "No hay ningún archivo que guardar en el servidor. Seleccione un archivo he intente nuevamente"
            };
            throw new Exception(baseResponse.ErrorInformation.ErrorDescription);
        }

        string fileExtension = Path.GetExtension(createProductDto.ImageFile.FileName);

        // Validar que la extensión del archivo sea aceptada
        if (!acceptedExtensions.Contains(fileExtension.ToLower()))
        {
            baseResponse.ErrorInformation = new ErrorInformation()
            {
                ErrorDescription = "El archivo es de una extensión no permitida por el sistema. Archivos permitidos: " + string.Join(", ", acceptedExtensions)
            };
            throw new Exception(baseResponse.ErrorInformation.ErrorDescription);
        }

        var filePath = Path.Combine(_localImagePath, createProductDto.ImageFile.FileName);

        try
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await createProductDto.ImageFile.CopyToAsync(stream);
            }

            var midier = new Midier
            {
                Url = "/images/" + createProductDto.ImageFile.FileName
            };

            _context.Midier.Add(midier);
            await _context.SaveChangesAsync();

            var product = new Product
            {
                Nombre = createProductDto.Nombre,
                Precio = createProductDto.Precio,
                IdImage = midier.Id
            };
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductDTO>(product);
        }
        catch (DirectoryNotFoundException)
        {
            throw new Exception("El directorio donde se almacenan los archivos no existe. Por favor contante con el administrador de la aplicación para resolver el problema.");
        }
        catch (PathTooLongException)
        {
            throw new Exception("No es posible guardar el archivo debido a que contiene un nombre muy largo (máximo 219 caracteres). Cambie el nombre del archivo he intente nuevamente.");
        }
        catch (Exception)
        {
            throw new Exception("Ocurrió un error al momento de guardar el archivo. La operación no fue realizada. Recargue la página he intente nuevamente.");
        }
    }


    public async Task UpdateProductAsync(int id, UpdateProductDTO updateProductDto)
    {
        var product = await _context.Product.Include(p => p.Image).FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            throw new Exception("Product not found");
        }

        if (updateProductDto.ImageFile != null)
        {
            var filePath = Path.Combine(_localImagePath, updateProductDto.ImageFile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await updateProductDto.ImageFile.CopyToAsync(stream);
            }

            product.Image.Url = "/images/" + updateProductDto.ImageFile.FileName;
        }

        product.Nombre = updateProductDto.Nombre;
        product.Precio = updateProductDto.Precio;

        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _context.Product.Include(p => p.Image).FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            throw new Exception("Product not found");
        }

        if (!string.IsNullOrWhiteSpace(product.Image?.Url) && !string.IsNullOrWhiteSpace(_hostingEnvironment.WebRootPath))
        {
            var imagePath = Path.Combine(_localImagePath, product.Image.Url.TrimStart('/'));
            _logger.LogInformation("Ruta del archivo a eliminar: " + imagePath);

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        _context.Midier.Remove(product.Image);
        _context.Product.Remove(product);
        await _context.SaveChangesAsync();
    }
}
