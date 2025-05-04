using MilligramServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MilligramServer.Database.Context;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UsersRoles => Set<UserRole>();
    public DbSet<Chat> Chats => Set<Chat>();
    public DbSet<UserChat> UsersChats => Set<UserChat>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<Message> Messages => Set<Message>();

    private readonly ILogger<ApplicationContext>? _logger;

    public ApplicationContext(ILogger<ApplicationContext>? logger = null)
    {
        _logger = logger;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MilligramDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        optionsBuilder.LogTo(LogMessage, LogLevel.Information);
    }

    private void LogMessage(string message)
    {
        _logger?.LogInformation(message);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region User

        modelBuilder.Entity<User>()
            .Property(u => u.Login)
            .HasMaxLength(50);

        modelBuilder.Entity<User>()
            .ToTable(t => t.HasCheckConstraint(
                "CK_Users_Login",
                "LEN(Login) > 0"));

        modelBuilder.Entity<User>()
            .HasIndex(u => u.NormalizedLogin)
            .IsUnique();

        modelBuilder.Entity<User>()
            .ToTable(t => t.HasCheckConstraint(
                "CK_Users_NormalizedLogin",
                "LEN(NormalizedLogin) > 0"));

        modelBuilder.Entity<User>()
            .Property(u => u.Name)
            .HasMaxLength(50);

        modelBuilder.Entity<User>()
            .ToTable(t => t.HasCheckConstraint(
                "CK_Users_Name",
                "LEN(Name) > 0"));

        modelBuilder.Entity<User>()
             .Property(u => u.PasswordHash)
             .HasAnnotation("MinLength", 8);

        modelBuilder.Entity<User>()
            .ToTable(t => t.HasCheckConstraint(
                "CK_Users_PasswordHash",
                "LEN(PasswordHash) >= 8"
            ));

        modelBuilder.Entity<User>()
            .HasQueryFilter(u => !u.IsDeleted);

        #endregion

        #region Role

        modelBuilder.Entity<Role>()
            .Property(r => r.Name)
            .HasMaxLength(50);

        modelBuilder.Entity<Role>()
            .ToTable(t => t.HasCheckConstraint(
                "CK_Roles_Name",
                "LEN(Name) > 0"));

        modelBuilder.Entity<Role>()
            .HasIndex(r => r.NormalizedName)
            .IsUnique();

        modelBuilder.Entity<Role>()
            .ToTable(t => t.HasCheckConstraint(
                "CK_Roles_NormalizedName",
                "LEN(NormalizedName) > 0"));

        modelBuilder.Entity<Role>()
            .HasQueryFilter(r => !r.IsDeleted);

        #endregion

        #region UserRole

        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UsersRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UsersRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region Chat

        modelBuilder.Entity<Chat>()
            .HasOne(c => c.OwnerUser)
            .WithMany(u => u.ChatsOwner)
            .HasForeignKey(c => c.OwnerUserId);

        modelBuilder.Entity<Chat>()
           .Property(c => c.Name)
           .HasMaxLength(50);

        modelBuilder.Entity<Chat>()
            .ToTable(t => t.HasCheckConstraint(
                "CK_Chats_Name",
                "LEN(Name) > 0"));

        modelBuilder.Entity<Chat>()
            .HasQueryFilter(c => !c.IsDeleted);

        #endregion

        #region UserChat

        modelBuilder.Entity<UserChat>()
           .HasKey(uc => new { uc.UserId, uc.ChatId });

        modelBuilder.Entity<UserChat>()
            .HasOne(uc => uc.User)
            .WithMany(u => u.UsersChats)
            .HasForeignKey(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserChat>()
            .HasOne(uc => uc.Chat)
            .WithMany(c => c.UsersChats)
            .HasForeignKey(uc => uc.ChatId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region Contact

        modelBuilder.Entity<Contact>()
            .HasOne(c => c.OwnerUser)
            .WithMany(u => u.OwnerUserForContacts)
            .HasForeignKey(c => c.OwnerUserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Contact>()
            .HasOne(c => c.AddedUser)
            .WithMany(u => u.AddedUserForContacts)
            .HasForeignKey(c => c.AddedUserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Contact>()
            .Property(c => c.Name)
            .HasMaxLength(50);

        modelBuilder.Entity<Contact>()
            .ToTable(t => t.HasCheckConstraint(
                "CK_Contacts_Name",
                "Name IS NULL OR LEN(Name) > 0"));

        modelBuilder.Entity<Contact>()
            .HasQueryFilter(c => !c.IsDeleted);

        #endregion

        #region Message

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ChatId);

        modelBuilder.Entity<Message>()
            .Property(m => m.CreationTime)
            .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<Message>()
            .Property(m => m.LastChangeTime)
            .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<Message>()
            .ToTable(t => t.HasCheckConstraint(
                "CK_Messages_Text",
                "Text IS NULL OR LEN(Text) > 0"));

        modelBuilder.Entity<Message>()
            .ToTable(t => t.HasCheckConstraint(
                "CK_Messages_FilePath",
                "FilePath IS NULL OR LEN(FilePath) > 0"));

        modelBuilder.Entity<Message>()
            .HasQueryFilter(m => !m.IsDeleted);

        #endregion
    }
}