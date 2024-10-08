using api.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<Films> Films { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<CommentLikePortfolio> CommentLikePortfolios { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Portfolio>(x => x.HasKey(p => new{p.AppUserId, p.FilmId}));
            builder.Entity<CommentLikePortfolio>(x => x.HasKey(p => new{p.AppUserId, p.CommentId}));


            builder.Entity<Portfolio>()
                .HasOne(u => u.AppUser)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.AppUserId);

            builder.Entity<Portfolio>()
                .HasOne(u => u.Film)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.FilmId);

            builder.Entity<CommentLikePortfolio>()
                .HasOne(u => u.AppUser)
                .WithMany(u => u.CommentLikePortfolios)
                .HasForeignKey(p => p.AppUserId);

            builder.Entity<CommentLikePortfolio>()
                .HasOne(u => u.Comment)
                .WithMany(u => u.CommentLikePortfolios)
                .HasForeignKey(p => p.CommentId);

            List<IdentityRole> roles = new List<IdentityRole>{

                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                 new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}