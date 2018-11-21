﻿using BedeSlots.Data.Models;
using BedeSlots.Data.Models.Contracts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new UserConfiguration());
            
            modelBuilder
               .Entity<BankCard>()
               .HasOne(c => c.Type)
               .WithMany(t => t.Cards)
               .HasForeignKey(u => u.TypeId)
              .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder
               .Entity<BankCard>()
               .HasOne(c => c.User)
               .WithMany(u => u.Cards)
               .HasForeignKey(u => u.UserId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
              .Entity<Transaction>()
              .HasOne(t => t.User)
              .WithMany(u => u.Transactions)
              .HasForeignKey(u => u.UserId);

            var transactionTypeConverter = new EnumToStringConverter<TransactionType>();

            modelBuilder
                .Entity<Transaction>()
                .Property(t => t.Type)
                .HasConversion(transactionTypeConverter);

            var gameTypeConverter = new EnumToStringConverter<GameType>();

            modelBuilder
                .Entity<Transaction>()
                .Property(t => t.GameType)
                .HasConversion(gameTypeConverter);

            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        //TODO: Override SaveChangesAsync
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

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CardType>().HasData(new CardType { Id = 1, Name = "Visa" });
            modelBuilder.Entity<CardType>().HasData(new CardType { Id = 2, Name = "MasterCard" });
            modelBuilder.Entity<CardType>().HasData(new CardType { Id = 3, Name = "American Express" });
        }
    }
}

