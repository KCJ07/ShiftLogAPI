// Kevin Johanson Web API for test shift logger

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Shiftdb>(opt =>
{
    opt.UseSqlite("shifts.db");
});

var app = builder.Build();

// creates the "master list of shifts" 
// Outdated and not needed
// var shifts = new List<Shift>();

// GET endpoint on parent directory should return list of Shifts 
app.MapGet("/shifts/", async (Shiftdb db) => 
    await db.Shifts.ToListAsync());

// GET endpoint gets specific shift object 
app.MapGet("/shifts/{id}", async Task<Results<Ok<Shift>, NotFound>> (int id, Shiftdb db) =>
{
    var targetShift = await db.Shifts.FindAsync(id);

    // tutorial code with newer syntax 
    /*
    return targetShift == null
        ? TypedResults.NotFound()
        : TypedResults.Ok(targetShift);
    */ 
    if (targetShift == null)
    {
        return TypedResults.NotFound();
    } else
    {
        return TypedResults.Ok(targetShift);
    }
});


// POST endpoint should add a shift
app.MapPost("/shifts", async (Shift shift, Shiftdb db) =>
{
    db.Shifts.Add(shift);
    await db.SaveChangesAsync();

    // might need to add $ here 
    return TypedResults.Created("/shifts/{shift.id}", shift);
});


// Delete Endpoint 
app.MapDelete("/shifts/{id}", async Results<NoContent, NotFound> (int id, Shiftdb db) =>
{
    var targetShift = await db.Shifts.FindAsync(id);

    if (targetShift == null)
    {
        return TypedResults.NotFound();
    } else
    {
        db.Shifts.Remove(targetShift);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
});


app.Run();
