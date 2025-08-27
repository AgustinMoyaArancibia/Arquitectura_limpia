using Users.Application.Interfaces;
using Users.Application.Services;
using Users.Infrastructure;
using FluentValidation;
using FluentValidation.AspNetCore;
using Users.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

// ?? Inyección de dependencias de Application
builder.Services.AddScoped<IUserService, UserService>();

// ?? Inyección de dependencias de Infrastructure (repos + DbContext)
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("Default"));

// ?? Controllers y validación con FluentValidation
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();

// ?? CORS (útil si después Angular consume esta API)
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("Dev", p => p.AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowAnyOrigin());
});

// ?? Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ?? Swagger solo en Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ?? Middleware
app.UseHttpsRedirection();
app.UseCors("Dev");      // habilitar CORS
app.UseAuthorization();
app.MapControllers();

app.Run();
