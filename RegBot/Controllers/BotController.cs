using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using System.Drawing.Imaging;
using System.Drawing;

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
        public async Task<IActionResult> PostAsync(object json, Guid id, string answer)
        {
            var t = D.GetDict(id);
            if (t == null)
            {
                return Conflict("captcha not found");
            }
            if (t.text.ToLower() == answer.ToLower() )
            {
                D.Del(t);
            }
            else
            {
                return Conflict("incorrect captcha");
            }

            await TelegramBot.SendTextMessageAsync(-1001523512708,json.ToString());

            return Ok();
        }
        //private Dictionary<Guid, string> ts = new Dictionary<Guid, string>();

        [HttpGet("cap")]
        public async Task<IActionResult> CreateCaptcha()
        {
            var cap = CreateCap(200, 50);
            var id = Guid.NewGuid();
            D.Add(new Data.Dict
            {
                Id = id,
                text = cap.Item2,
                created = DateTime.Now
            });
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                cap.Item1.Save(stream,  ImageFormat.Png);
                bytes = stream.ToArray();
            }

            MultipartFormDataContent content = new MultipartFormDataContent();
                FormFileCollection files = new FormFileCollection();
            

            return Ok(new{ bytes, ContentType = "image/png", id = id.ToString()});
        }

        private Tuple< Bitmap, string> CreateCap(int Width, int Height)
        {
            Random rnd = new Random();

            //Создадим изображение
            Bitmap result = new Bitmap(Width, Height);

            //Вычислим позицию текста
            int Xpos = rnd.Next(0, Width - 50);
            int Ypos = rnd.Next(15, Height - 15);

            //Добавим различные цвета
            Brush[] colors = { Brushes.Black,
                     Brushes.Red,
                     Brushes.RoyalBlue,
                     Brushes.Green };

            //Укажем где рисовать
            Graphics g = Graphics.FromImage((Image)result);

            //Пусть фон картинки будет серым
            g.Clear(Color.Gray);

            //Сгенерируем текст
            var text = String.Empty;
            string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
            for (int i = 0; i < 5; ++i)
                text += ALF[rnd.Next(ALF.Length)];

            //Нарисуем сгенирируемый текст
            g.DrawString(text,
                         new Font("Arial", 15),
                         colors[rnd.Next(colors.Length)],
                         new PointF(Xpos, Ypos));

            //Добавим немного помех
            /////Линии из углов
            g.DrawLine(Pens.Black,
                       new Point(0, 0),
                       new Point(Width - 1, Height - 1));
            g.DrawLine(Pens.Black,
                       new Point(0, Height - 1),
                       new Point(Width - 1, 0));
            ////Белые точки
            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (rnd.Next() % 20 == 0)
                        result.SetPixel(i, j, Color.White);
            
            return new Tuple<Bitmap, string>( result, text);
        }

    }
}
