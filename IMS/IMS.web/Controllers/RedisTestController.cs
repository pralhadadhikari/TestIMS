using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace IMS.web.Controllers
{
    public class RedisTestController : Controller
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisTestController(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        [HttpGet]
        public async Task<IActionResult> Set()
        {
            var database = _redis.GetDatabase();

            await database.StringSetAsync("testims:name", "Pralhad");

            return Ok("Value stored in Redis.");
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var database = _redis.GetDatabase();

            var value = await database.StringGetAsync("testims:name");

            return Ok(value.ToString());
        }


        [HttpGet]
        public async Task<IActionResult> SetOtp()
        {
            var database = _redis.GetDatabase();

            var otp = "123456";

            await database.StringSetAsync("testims:otp", otp, TimeSpan.FromSeconds(60));

            return Ok("OTP stored. It will expire after 60 seconds.");
        }
    }
}
