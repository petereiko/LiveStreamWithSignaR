using LiveStreamWithSignaR;
using LiveStreamWithSignaR.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSignalR();


builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins("https://localhost:44334").AllowAnyHeader().AllowCredentials())
);

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll",
//        policy =>
//        {
//            policy.WithOrigins("https://localhost:44334").AllowAnyOrigin()
//                   .AllowAnyHeader()
//                   .AllowAnyMethod()
//                   .AllowCredentials(); // Needed for SignalR
//        });
//});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ICameraService, CameraService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.MapHub<VideoHub>("/videoHub");
app.MapHub<LiveStreamHub>("/livestreamhub");

app.Run();
