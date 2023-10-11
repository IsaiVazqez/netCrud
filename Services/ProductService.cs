
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
        var acceptedExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
        string imageUrl;

        try
        {
            imageUrl = await ImageHelper.SaveImageAsync(createProductDto.ImageFile, _localImagePath, acceptedExtensions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el producto");
            throw;
        }

        var midier = new Midier
        {
            Url = imageUrl
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

    public async Task UpdateProductAsync(int id, UpdateProductDTO updateProductDto)
    {
        var product = await _context.Product.Include(p => p.Image).FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            throw new Exception("Product not found");
        }

        if (updateProductDto.ImageFile != null)
        {
            var imageUrl = await ImageHelper.SaveImageAsync(updateProductDto.ImageFile, _localImagePath, new List<string> { ".jpg", ".jpeg", ".png" });
            product.Image.Url = imageUrl;
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
