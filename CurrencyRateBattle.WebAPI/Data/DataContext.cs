using Microsoft.EntityFrameworkCore;

using CurrencyRateBattle.WebAPI.Data.Entities;

namespace CurrencyRateBattle.WebAPI.Data;

public class DataContext : DbContext
{

    public DbSet<Currency> Currencies { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<RoomUser> RoomUsers { get; set; } = null!;
    public DbSet<Timing> Timings { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseNpgsql("server=localhost; Port=5600; database=Currency_Rate_Battle; Username=postgres; password=big_money");
    //}

}
