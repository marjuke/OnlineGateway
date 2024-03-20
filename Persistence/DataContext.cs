using Domain.DatabaseC;
using Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext:IdentityDbContext<AppUser> 
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Stan> stan { get; set; }
        public DbSet<GatewayInfo> GatewayInfo { get; set; }
        public DbSet<ChannelList> ChannelList { get; set; } 
        public DbSet<RoutingServerList> RoutingServerList { get; set; } 
        public DbSet<ChannelCode> ChannelCode { get; set; }
        public DbSet<GatewayCheckInfo> GatewayCheckInfo { get; set; }
        //public DbSet<IPListInfo> IPListInfo { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<GatewayInfo>()
            .Property(e => e.ServerReqDate)
            .HasDefaultValueSql("GetDate()");

            modelBuilder.Entity<GatewayCheckInfo>()
            .Property(e => e.ServerReqDate)
            .HasDefaultValueSql("GetDate()");

            modelBuilder.Entity<GatewayInfo>()
          .HasKey(m => new { m.StanId, m.ServerReqDate });
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Stan>()
        .ToTable(tb => tb.HasTrigger("Trigger_Stan_update"));
            modelBuilder.Entity<GatewayInfo>()
        .ToTable(tb => tb.HasTrigger("Trigger_GatewayInfos_Insert"));


            modelBuilder.Entity<GatewayCheckInfo>()
          .HasKey(m => new { m.StanId, m.ServerReqDate });
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GatewayCheckInfo>()
        .ToTable(tb => tb.HasTrigger("Trigger_GatewayCheckInfo_Insert"));
        }
    }
}
