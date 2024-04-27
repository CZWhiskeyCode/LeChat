using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Collections.Generic;
using System.Linq;

namespace Database;
public class Program
{
    static void Main()
    {
        using (var db = new ApplicationContextFactory().CreateDbContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
    public static void AddUser(User user)
    {
        using (var db = new ApplicationContextFactory().CreateDbContext())
        {
            db.Users.Add(user);
            db.SaveChanges();
        }
    }
    public static User FindUser(string login, string password)
    {
        using (var db = new ApplicationContextFactory().CreateDbContext())
        {
            return db.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
        }
    }
}
public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public Status Status { get; set; }

    public List<Message> SentMessages { get; set; } = new();
    public List<Message> ReceivedMessages { get; set; } = new();
    public List<Blacklist> Blacklists { get; set; } = new();
    public List<Contact> Contacts { get; set; } = new();

}
public class Message
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int RecipientId { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }

    public User Sender { get; set; }
    public User Recipient { get; set; }
}

public class Blacklist
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BlockedUserId { get; set; }

    public User User { get; set; }
    public User BlockedUser { get; set; }
}

public class Contact
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ContactUserId { get; set; }

    public User User { get; set; }
    public User ContactUser { get; set; }
}

public enum Status
{
    Online,
    Offline,
    Away,
    DontDisturb
}
public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Blacklist> Blacklists { get; set; } 
    public DbSet<Contact> Contacts { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(u => u.SentMessages)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Recipient)
            .WithMany(u => u.ReceivedMessages)
            .HasForeignKey(m => m.RecipientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Blacklist>()
            .HasOne(bl => bl.User)
            .WithMany(u => u.Blacklists)
            .HasForeignKey(bl => bl.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Blacklist>()
            .HasOne(bl => bl.BlockedUser)
            .WithMany()
            .HasForeignKey(bl => bl.BlockedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Contact>()
           .HasOne(c => c.User)
           .WithMany(u => u.Contacts)
           .HasForeignKey(c => c.UserId)
           .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Contact>()
            .HasOne(c => c.ContactUser)
            .WithMany()
            .HasForeignKey(c => c.ContactUserId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}
public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    private static IConfigurationRoot config;
    static ApplicationContextFactory()
    {
        ConfigurationBuilder builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");
        config = builder.Build();
    }
    public ApplicationContext CreateDbContext(string[]? args = null)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        return new ApplicationContext(optionsBuilder.Options);
    }
}

