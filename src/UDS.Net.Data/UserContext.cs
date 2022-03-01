using System;
using Microsoft.EntityFrameworkCore;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data

{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
        public DbSet<UserPreference> UserPreferences { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserPreference>().Property(x => x.Preference).HasConversion(
                    x => x.ToString(),
                    x => (UserPreferenceOptions)Enum.Parse(typeof(UserPreferenceOptions), x)
                );
        }
    }
}

