
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DoctorApi.Models;

namespace DoctorApi.Data
{
	public class DatabaseContext : DbContext
	{
        protected readonly IConfiguration Configuration;

        public DatabaseContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<DoctorModel> Doctors { get; set; }
    }
}