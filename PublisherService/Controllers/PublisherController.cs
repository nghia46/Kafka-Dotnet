using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace PublisherService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController(ProducerConfig config) : ControllerBase
    {
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] string message)
        {
            try
            {
                using var producer = new ProducerBuilder<string, string>(config).Build();
                var result = await producer.ProduceAsync("test-topic", new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = message
                });
                return Ok(new { Status = "Sent", Message = message, Partition = result.Partition, Offset = result.Offset });

            }
            catch (Exception e)
            {
                return BadRequest(new { Status = "Error", Message = e.Message });
            }
        }
    }
}
