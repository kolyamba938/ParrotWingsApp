using Microsoft.EntityFrameworkCore;
using ParrotWingsApp.Data.Model;

namespace ParrotWingsApp.Data.DataLayer
{
    internal class ParrotWingsContext : DbContext
    {
        private void InitConnection()
        {
            Database.EnsureCreated();
        }


        public ParrotWingsContext(DbContextOptions<ParrotWingsContext> options)
            : base(options)
        {
            InitConnection();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(p => p.IncomeTransactions)
                .WithOne(t => t.Recipient);

            modelBuilder.Entity<User>()
                .HasMany(p => p.OutcomeTransactions)
                .WithOne(t => t.Sender);

        }


        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }


    }
}

