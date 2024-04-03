using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data
{
    public class WebAppContext : DbContext
    {
        public DbSet<WebApp.Models.Artist> Artist { get; set; } = default!;
        public DbSet<WebApp.Models.Song> Song { get; set; } = default!;
        public DbSet<WebApp.Models.Chart> Chart { get; set; } = default!;

        public WebAppContext(DbContextOptions<WebAppContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WebApp.Models.Chart>()
                .HasMany(chart => chart.Songs)
                .WithMany(song => song.Charts)
                .UsingEntity<WebApp.Models.ChartSong>(
                    joinTable => joinTable.HasOne(chart => chart.Song).WithMany(song => song.ChartSongs).HasForeignKey(chart => chart.SongId),
                    joinTable => joinTable.HasOne(song => song.Chart).WithMany(chart => chart.ChartSongs).HasForeignKey(song => song.ChartId),
                    joinTable => joinTable.HasKey(j => new { j.ChartId, j.SongId })
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
