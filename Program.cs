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
app.MapGet("/shifts/", () => shifts);

// GET endpoint gets specific shift object 
app.MapGet("/shifts/{id}", Results<Ok<Shift>, NotFound> (int id) =>
{
    var targetShift = shifts.SingleOrDefault(t => id == t.id);

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
app.MapPost("/shifts", (Shift shift) =>
{
    shifts.Add(shift);

    // might need to add $ here 
    return TypedResults.Created("/shifts/{id}", shift);
});


// Delete Endpoint 
app.MapPost("/shifts/{id}", (int id) =>
{
    shifts.RemoveAll(t => id == t.id);
    return TypedResults.NoContent();
});

app.Run();
