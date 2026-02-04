var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// GELÝÞTÝRME KONTROLÜNÜ KALDIR - HER ZAMAN AÇIK OLSUN
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce API v1");
    options.RoutePrefix = "swagger"; // http://localhost:5050/swagger
    // Veya ana sayfada: options.RoutePrefix = string.Empty;
});

// HTTPS yönlendirmesini geçici olarak KAPAT
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.Run();