using Microsoft.EntityFrameworkCore;

namespace Keepass.DBLib;

public class KeepassDbContext : DbContext
{
    #region Properties

    public DbSet<AppUser> AppUsers { get; set; } = default!;
    public DbSet<Vault> Vaults { get; set; } = default!;
    public DbSet<VaultEntry> VaultEntries { get; set; } = default!;

    #endregion

    #region Constructors

    public KeepassDbContext() : base()
    {

    }

    public KeepassDbContext(DbContextOptions<KeepassDbContext> options) : base(options)
    {

    }

    #endregion

    #region Methods

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured == false)
        {
            optionsBuilder.UseSqlite("Data Source=C:\\_workdir\\keepass.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasMany(e => e.SharedVaults).WithMany(e => e.SharedUsers);
        });

        modelBuilder.Entity<Vault>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasMany(e => e.Entries).WithOne(e => e.Vault).HasForeignKey(e => e.VaultId);
            entity.HasOne(e => e.Creator).WithMany(e => e.Vaults).HasForeignKey(e => e.AppUserId);

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);

            entity.HasIndex(e => new { e.AppUserId, e.Name }).IsUnique();
        });

        modelBuilder.Entity<VaultEntry>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Username).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
        });

    }

    #endregion
}
