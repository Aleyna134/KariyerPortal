using KariyerPortal.Models;
using KariyerPortal.Models.Entity;
using KariyerPortal.Models.Identity;
using KariyerPortal.Models.Profile;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;


namespace KariyerPortal.Context;

public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }

    public DbSet<UserDetail> UserDetails { get; set; }
    public DbSet<UserJobExperience> UserJobExperiences { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Certificate> Certificates { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Education> Educations { get; set; }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserDetail>()
            .HasKey(ud => ud.AppUserId);

        builder.Entity<UserDetail>()
            .HasOne(ud => ud.AppUser)
            .WithOne(u => u.UserDetail)
            .HasForeignKey<UserDetail>(ud => ud.AppUserId);

        builder.Entity<UserJobExperience>()
            .HasOne(uje => uje.User)
            .WithMany(u => u.UserJobExperiences)
            .HasForeignKey(uje => uje.UserId);

        builder.Entity<JobApplication>()
        .HasOne(a => a.User)
        .WithMany() // kullanýcý birden fazla baþvuru yapabilir
        .HasForeignKey(a => a.UserId);
    }


}