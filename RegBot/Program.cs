using RegBot;
using Telegram.Bot;
var base64 = "MTc3Nzc5NjY5NTpBQUVGQkVrS0RSR0ZLNXNlYmJhWWRKWHd0NlB1RFBkdWp0WQ==";
var Bot = new TelegramBotClient(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(base64)));
//Bot.SetWebhookAsync("https://dk-reg.herokuapp.com/api/bot");
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSingleton<TelegramBotClient>(Bot);
builder.Services.AddSingleton<Data>();
builder.Services.AddCors();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(opt =>
    opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
