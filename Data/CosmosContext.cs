using Cosmos.Sts.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity;

namespace Cosmos.Sts.Data
{
    // Info : Adding in STS again so that STS is independent of any app references
    public class CosmosContext : IdentityDbContext<CosmosUser, CosmosRole, Guid>
    {
        public CosmosContext(DbContextOptions<CosmosContext> options) : base(options)
        {
        }
        public virtual DbSet<CosmosRole> Role { get; set; }
        public virtual DbSet<CosmosUser> User { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<CosmosUser>().ToTable("User", "Security");
            // Change the ApolloRole entity to point to the Role Table instead of the default
            builder.Entity<CosmosRole>().ToTable("Role", "Security");

            // The below AspIdentity Tables are not used , however they need to be created
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("AspNetUserClaims", "Security");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("AspNetUserRoles", "Security");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AspNetRoleClaims", "Security");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("AspNetUserLogins", "Security");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("AspNetUserTokens", "Security");
            // ~The below AspIdentity Tables are not used , however they need to be created

        }
    }
}
