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
        var orders = await _context.Order.Include(o => o.OrderProducts).ThenInclude(op => op.Product).ToListAsync();
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
}
