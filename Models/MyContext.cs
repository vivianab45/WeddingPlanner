#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;
namespace WeddingPlanner.Models;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions options) : base(options) {}

    //tables
    public DbSet<User> Users {get; set;}
    public DbSet<Wedding> Weddings {get;set;}
    public DbSet<UserWeddingRSVP>  UserWeddingRSVPs {get;set;}
}