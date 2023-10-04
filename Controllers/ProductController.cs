using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using MiApi.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;


[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly string _localImagePath;
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _hostingEnvironment;


    public ProductsController(ApplicationContext context, IMapper mapper, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
    {
        _context = context;
        _mapper = mapper;
        _localImagePath = configuration["ImageSettings:LocalPath"];
        _hostingEnvironment = hostingEnvironment;


    }

    // GET: api/Products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
    {
        var products = await _context.Product.Include(p => p.Image).ToListAsync();
        return _mapper.Map<List<ProductDTO>>(products);
    }

    // GET: api/Products/5
[HttpGet("{id}")]
public async Task<ActionResult<ProductDTO>> GetProduct(int id)
{
    var product = await _context.Product.Include(p => p.Image).FirstOrDefaultAsync(p => p.Id == id);

    if (product == null)
    {
        return NotFound();
    }

    return _mapper.Map<ProductDTO>(product);
}

    // POST: api/Products
    [HttpPost]
    public async Task<ActionResult<ProductDTO>> CreateProductCreateProduct([FromForm] CreateProductDTO createProductDto)
    {
        var filePath = Path.Combine(_localImagePath, createProductDto.ImageFile.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await createProductDto.ImageFile.CopyToAsync(stream);
        }

        // 2. Crear un registro en Midier
        var midier = new Midier
        {
            Url = "/images/" + createProductDto.ImageFile.FileName
        };
        _context.Midier.Add(midier);
        await _context.SaveChangesAsync();

        // 3. Obtener el ID del registro Midier
        var midierId = midier.Id;

        // 4. Asignar el ID a Product
        var product = new Product
        {
            Nombre = createProductDto.Nombre,
            Precio = createProductDto.Precio,
            IdImage = midierId
        };
        _context.Product.Add(product);
        await _context.SaveChangesAsync();

        // Convertir el producto a DTO y devolverlo
        var productReadDto = _mapper.Map<ProductDTO>(product);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productReadDto);

    }

    // PUT: api/Products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        _context.Entry(product).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Product.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Product.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProductExists(int id)
    {
        return _context.Product.Any(e => e.Id == id);
    }
}
