using Microsoft.EntityFrameworkCore;

class Shiftdb : DbContext
{
    public Shiftdb(DbContextOptions<Shiftdb> options) : base(options) { }

    public DbSet<Shift> Shifts => Set<Shift>();
}