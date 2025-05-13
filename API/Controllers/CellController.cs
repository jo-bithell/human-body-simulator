using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CellController : ControllerBase
    {
        private readonly IConnectionMultiplexer _redis;

        public CellController(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> GetAsync(string key)
        {
            var db = _redis.GetDatabase();
            var value = await db.StringGetAsync(key);

            if (value.IsNullOrEmpty)
            {
                return NotFound(new { Message = "Key not found in Redis cache." });
            }

            return Ok(new { Key = key, Value = value });
        }
    }
}
