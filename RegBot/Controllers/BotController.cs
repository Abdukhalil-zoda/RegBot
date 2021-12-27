using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using System.Drawing.Imaging;
using System.Drawing;
using RegBot.Services;

namespace RegBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : ControllerBase
    {
        public BotController(TelegramBotClient telegramBot, Data d)
        {
            TelegramBot = telegramBot;
            D = d;
        }

        public TelegramBotClient TelegramBot { get; }
        public Data D { get; }

        /*
[HttpPost]
public async Task<IActionResult> Post(Update update)
{
await TelegramBot.SendTextMessageAsync(update.Message.Chat.Id, "Salom");

return Ok();
}*/
        [HttpPost("Reg/{id}/{answer}")]
        public async Task<IActionResult> PostAsync([FromBody]object json, Guid id, string answer)
        {
            var t = D.GetDict(id);
            if (t == null)
            {
                return Conflict("captcha not found");
            }
            if (t.text.ToLower() == answer.ToLower())
            {
                D.Del(t);
            }
            else
            {
                return Conflict("incorrect captcha");
            }

            await TelegramBot.SendTextMessageAsync(-1001523512708, json.ToString());

            return Ok();
        }
        //private Dictionary<Guid, string> ts = new Dictionary<Guid, string>();

        [HttpGet("cap")]
        public async Task<IActionResult> CreateCaptcha()
        {
            var cap = BotService.GetTuple1();
            var id = Guid.NewGuid();
            D.Add(new Data.Dict
            {
                Id = id,
                text = cap.Item2,
                created = DateTime.Now
            });

            MultipartFormDataContent content = new MultipartFormDataContent();
            FormFileCollection files = new FormFileCollection();


            return Ok(new { cap.Item1, ContentType = "image/png", id = id.ToString() });
        }
    }
}
