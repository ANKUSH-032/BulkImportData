


using BluckImport.Core.Interface;
using BluckImport.Infrastructure;
using BulkImport.Core.Interface;
using BulkImport.Infrastructure;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

LicenseContext context = LicenseContext.NonCommercial; // Set the LicenseContext

ExcelPackage.LicenseContext = context;
// Add services to the container.
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAllOrigins",
//        builder =>
//        {
//            builder.WithOrigins("http://localhost:4200/")
//                   .AllowAnyHeader()
//                   .AllowAnyMethod();
//        });
//});
string MyAllowSpecificOrigins = "http://localhost:7003/api";
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policyBuilder => policyBuilder
            .WithOrigins(MyAllowSpecificOrigins, "http://localhost:4200")
            // Specify the client app's origin
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            ); // Enable credentials
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBulkImportRepository, BulkImportRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowSpecificOrigin");

app.Run();
