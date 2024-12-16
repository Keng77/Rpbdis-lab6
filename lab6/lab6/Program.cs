using lab6.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Получаем строку подключения
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DBConnection' not found.");

// Добавляем DbContext для работы с базой данных
builder.Services.AddDbContext<InspectionsDbContext>(options =>
    options.UseSqlServer(connectionString));

// Добавляем сервисы для работы с контроллерами и представлениями
builder.Services.AddControllersWithViews(); // Для работы с контроллерами и представлениями

// Настройка Swagger с включением аннотаций
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations(); // Включаем аннотации Swagger
});

var app = builder.Build();

// Конфигурация HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Генерация Swagger JSON
    app.UseSwaggerUI(); // Интерфейс для взаимодействия с API через Swagger
}

app.UseAuthorization();

// Маршруты для контроллеров
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inspections}/{action=index}/{id?}"); // Указываем, что по умолчанию будет вызываться метод Index в контроллере Inspections

app.Run();
