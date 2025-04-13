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
        public async Task<IActionResult> Get(string key)
        {
            var db = _redis.GetDatabase();
            var value = await db.StringGetAsync(key);

            if (value.IsNullOrEmpty)
            {
                return NotFound(new { Message = "Key not found in Redis cache." });
            }

            return Ok(new { Key = key, Value = value.ToString() });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] KeyValuePair<string, string> data)
        {
            var db = _redis.GetDatabase();
            bool isSet = await db.StringSetAsync(data.Key, data.Value);

            if (!isSet)
            {
                return StatusCode(500, new { Message = "Failed to set key-value in Redis cache." });
            }

            return Ok(new { Message = "Key-value pair added to Redis cache.", Data = data });
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete(string key)
        {
            var db = _redis.GetDatabase();
            bool isDeleted = await db.KeyDeleteAsync(key);

            if (!isDeleted)
            {
                return NotFound(new { Message = "Key not found in Redis cache." });
            }

            return Ok(new { Message = "Key deleted from Redis cache.", Key = key });
        }
    }
}
