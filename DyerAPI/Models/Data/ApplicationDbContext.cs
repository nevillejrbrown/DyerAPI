using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace DyerAPI.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Celeb>()
        //        .HasOne(p => p.Game)
        //        .WithMany(b => b.Celebs);
        //}

        //public ApplicationDbContext()
        //{
        //    options.UseSqlServer(Configuration.GetConnectionString("ApplicationDbContext"))
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Test");
        //    optionsBuilder.UseSqlServer("Server=(localdb)\\Training;Database=DyerDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        //}

        public DbSet<Celeb> Celeb { get; set; }
        public DbSet<Game> Game { get; set; }


    }
}
