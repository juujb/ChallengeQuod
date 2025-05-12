using Quod.API;
using Quod.Infra.Mongo;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureContainer(builder.Configuration);

builder.Services
    .Configure<DefaultMongoDbSettings>(
    builder.Configuration.GetSection("MongoDb"));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AntiFraudAPI v1");
        c.RoutePrefix = string.Empty;
    });
}

app.Run();
