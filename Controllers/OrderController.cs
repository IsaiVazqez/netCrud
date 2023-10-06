using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;

    public OrdersController(ApplicationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderReadDTO>>> GetOrders()
    {
    var orders = await _context.Order
                                .Include(o => o.User) 
                               .Include(o => o.OrderProducts)
                               .ThenInclude(op => op.Product)
                               .ThenInclude(p => p.Image)
                               .ToListAsync();
    return _mapper.Map<List<OrderReadDTO>>(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderReadDTO>> GetOrder(int id)
    {
        var order = await _context.Order
                                  .Include(o => o.OrderProducts)
                                  .ThenInclude(op => op.Product)
                                  .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return _mapper.Map<OrderReadDTO>(order);
    }

    [HttpPost]
    public async Task<ActionResult<OrderReadDTO>> CreateOrder(CreateOrderDTO createOrderDto)
    {
        var order = _mapper.Map<Order>(createOrderDto);
        _context.Order.Add(order);
        await _context.SaveChangesAsync();

        foreach (var productId in createOrderDto.ProductIds)
        {
            _context.OrderProduct.Add(new OrderProduct { OrderId = order.Id, ProductId = productId });
        }
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, _mapper.Map<OrderReadDTO>(order));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        var order = await _context.Order.FindAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        // Eliminar registros relacionados en OrderProduct
        var orderProducts = _context.OrderProduct.Where(op => op.OrderId == id);
        _context.OrderProduct.RemoveRange(orderProducts);

        // Eliminar el Order
        _context.Order.Remove(order);
        await _context.SaveChangesAsync();

        return NoContent(); // Retorna un 204 No Content para indicar que la operación fue exitosa pero no hay contenido para devolver.
    }

}
