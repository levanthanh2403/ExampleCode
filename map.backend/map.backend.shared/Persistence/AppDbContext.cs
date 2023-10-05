using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using map.backend.shared.Entities.Auth;

namespace map.backend.shared.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var mapper = new Npgsql.NameTranslation.NpgsqlSnakeCaseNameTranslator();
            var types = modelBuilder.Model.GetEntityTypes().ToList();

            //modelBuilder.HasSequence<int>("seq_gdpl_tree")
            //    .StartsAt(10000)
            //    .HasMin(10000)
            //    .HasMax(99999)
            //    .IncrementsBy(1)
            //    .IsCyclic(false);

            //modelBuilder.Entity<extb_data_import>()
            //            .Property(e => e.description)
            //            .HasColumnType("text");
            //modelBuilder.Entity<extb_log>()
            //            .Property(e => e.description)
            //            .HasColumnType("text");

            modelBuilder.Entity<tb_user>()
                .Property(e => e.limit)
                .HasDefaultValue(5);
            modelBuilder.Entity<tb_user>()
                .Property(e => e.status)
                .HasDefaultValue("O");
            modelBuilder.Entity<tb_user>()
                .Property(e => e.img)
                .HasColumnType("text");
        }
        public DbSet<tb_user> tb_user { get; set; }
    }
}
