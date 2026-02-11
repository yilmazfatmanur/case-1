using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuratins;
using System.IO;
using FileOrganizer.Models;
using ECommerceAPI.Models;

namespace FileOrganizer.Data
{
	public class AppDbContext : DbContext
	{
		//constructor
		public AppDbContext(DbContextOptions<AppDbContext>options)
			:base (options) { }

		//empty constructot (for migration)
		public AppDbContext()
			{ 
			}

		//DbSet-tables
		public DbSet<FileLog> FileLog { get; set; }

		//connection string
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if(!optionsBuilder.IsConfigured)
			{
				var configuration = new ConfigurationBuilder()
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsetting.json", optional: false , reloaadOnChange: true)
					.Build();

				var connectionString = configuration.GetConnectionString("DefaultConnection");
				optionsBuilder.UseSqlServer(connectionString);
			}
		}

		//table configurations
		protected  override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<FileLogs>(entity =>
			{
				entity.ToTable("FileLog");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.FileName).HasMaxLength(225).IsRequired();
				entity.Property(e => e.OriginalPath).HasMaxLength(500);
				entity.Property(e => e.NewPath).HasMaxLength(500);
				entity.Property(e => e.Extension).HasMaxLength(10);
				entity.Property(e => e.Category).HasMaxLength(50);
				entity.Property(e => e.MoveDate).HasDefaultValueSql("GETDATE()");
			});

		}
	}
}




