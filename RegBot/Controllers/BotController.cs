using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace RegBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : ControllerBase
    {
        public BotController(TelegramBotClient telegramBot)
        {
            TelegramBot = telegramBot;
        }

        public TelegramBotClient TelegramBot { get; }
/*
        [HttpPost]
        public async Task<IActionResult> Post(Update update)
        {
            await TelegramBot.SendTextMessageAsync(update.Message.Chat.Id, "Salom");

            return Ok();
        }*/
        [HttpPost("Reg")]
        public async Task<IActionResult> PostAsync(object json)
        {
           
            await TelegramBot.SendTextMessageAsync(-1001523512708,json.ToString());

            return Ok();
        }
    }
}
