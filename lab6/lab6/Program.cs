using lab6.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// �������� ������ �����������
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DBConnection' not found.");

// ��������� DbContext ��� ������ � ����� ������
builder.Services.AddDbContext<InspectionsDbContext>(options =>
    options.UseSqlServer(connectionString));

// ��������� ������� ��� ������ � ������������� � ���������������
builder.Services.AddControllersWithViews(); // ��� ������ � ������������� � ���������������

// ��������� Swagger � ���������� ���������
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations(); // �������� ��������� Swagger
});

var app = builder.Build();

// ������������ HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // ��������� Swagger JSON
    app.UseSwaggerUI(); // ��������� ��� �������������� � API ����� Swagger
}

app.UseAuthorization();

// �������� ��� ������������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inspections}/{action=index}/{id?}"); // ���������, ��� �� ��������� ����� ���������� ����� Index � ����������� Inspections

app.Run();
