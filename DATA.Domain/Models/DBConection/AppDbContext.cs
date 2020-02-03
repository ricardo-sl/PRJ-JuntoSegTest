using System;
using System.Linq;
using DATA.Domain.Models;
using Microsoft.EntityFrameworkCore;  

namespace DATA.Domain.Models
{
    public partial class AppDbContext : DbContext
    { 
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
         
        public DbSet<User> Users { get; set; }  
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<User>().HasAlternateKey(u => u.Email); // Esta linha implica campo como nao-nulo e unico (FK), alem de indexamento, o que facilita nas consultas
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            //O correto era a utilizacao do json com ambientes separados dev-tst-prod (claro, sendo um singleton)
             //   optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0");  
        } 

        //Override do metodo para salvar modelos, para abstrair o preenchimento das infos da BaseEntity
        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).ModificadoEm = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CriadoEm = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }
    }
}