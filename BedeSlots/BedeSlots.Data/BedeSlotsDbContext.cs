using BedeSlots.Data.Models;
using BedeSlots.Data.Models.Contracts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BedeSlots.Data
{
    public class BedeSlotsDbContext : IdentityDbContext<User>
    {
        public BedeSlotsDbContext(DbContextOptions<BedeSlotsDbContext> options) : base(options)
        {
        }

        public DbSet<BankCard> BankCards { get; set; }

        public DbSet<CardType> CardTypes { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder
               .Entity<User>()
               .HasOne(u => u.Currency)
               .WithMany(c => c.Users)
               .HasForeignKey(u => u.CurrencyId);

            modelBuilder
               .Entity<BankCard>()
               .HasOne(c => c.Type)
               .WithMany(t => t.Cards)
               .HasForeignKey(u => u.TypeId);

            modelBuilder
               .Entity<BankCard>()
               .HasOne(c => c.User)
               .WithMany(u => u.Cards)
               .HasForeignKey(u => u.UserId);

            modelBuilder
              .Entity<Transaction>()
              .HasOne(t => t.User)
              .WithMany(u => u.Transactions)
              .HasForeignKey(u => u.UserId);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges();
        }

        private void ApplyAuditInfoRules()
        {
            var newlyCreatedEntities = this.ChangeTracker.Entries()
                .Where(e => e.Entity is IAuditable && ((e.State == EntityState.Added) || (e.State == EntityState.Modified)));

            foreach (var entry in newlyCreatedEntities)
            {
                var entity = (IAuditable)entry.Entity;

                if (entry.State == EntityState.Added && entity.CreatedOn == null)
                {
                    entity.CreatedOn = DateTime.Now;
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        }
    }
}
