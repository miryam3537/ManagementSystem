using System.Collections.Generic;
using System.Reflection.Emit;
using SYSTEMTEST.Entities;
using Microsoft.EntityFrameworkCore;

namespace SYSTEMTEST.Data
{
    public class AuctionDbContext : DbContext
    {
        public AuctionDbContext(
            DbContextOptions<AuctionDbContext> options)
            : base(options)
        {
        }

        public DbSet<Auction> Auctions => Set<Auction>();

        public DbSet<Bid> Bids => Set<Bid>();

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auction>()
                .Property(x => x.RowVersion)
                .IsRowVersion();

            modelBuilder.Entity<Bid>()
                .HasOne(x => x.Auction)
                .WithMany(x => x.Bids)
                .HasForeignKey(x => x.AuctionId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
