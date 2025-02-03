using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);
//cors (adding the cors definition)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

//DbContext configuration
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"), 
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ToDoDB"))));

//swagger
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

//using in cors
app.UseCors();

// Enable Swagger UI
// if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();  
//}

app.MapGet("/", () => "TodooApi is running!!!");
//Get
app.MapGet("/items", async (ToDoDbContext db)=>{
    var items = await db.Items.ToListAsync();
    return Results.Ok(items);
});

app.MapGet("/items/{id}", async (int id, ToDoDbContext db) =>
    await db.Items.FindAsync(id) is Item item ? Results.Ok(item) : Results.NotFound());
//Post
app.MapPost("/items",async (ToDoDbContext db,Item newItem) =>{
    await db.Items.AddAsync(newItem);
    await db.SaveChangesAsync();
    return Results.Created($"/items/{newItem.Id}", newItem);
});
//Put
app.MapPut("/items/{id}", async (ToDoDbContext db, int id, bool isComplete) =>{
    var item = await db.Items.FindAsync(id);
    if (item == null) return Results.NotFound($"Item with ID {id} not found.");

    //item.Name = updatedItem.Name;
    //item.Name=item.Name;
    item.IsComplete = isComplete;
    await db.SaveChangesAsync();
    return Results.Ok(item);
});
//Delete
app.MapDelete("/items/{id}", async (ToDoDbContext db, int id) => {
    var item = await db.Items.FindAsync(id);
    if (item == null) return Results.NotFound($"Item with ID {id} not found.");

    db.Items.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
