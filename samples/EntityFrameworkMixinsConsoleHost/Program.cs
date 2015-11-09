namespace EntityFrameworkMixinsConsoleHost
{
    using System;
    using System.Linq;
    using Microsoft.Data.Entity;
    using Microsoft.Framework.DependencyInjection;
    using EntityFrameworkMixins;

    public class Program
    {
        public void Main(string[] args)
        {
            var services = new ServiceCollection();

            services
                .AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<UserDbContext>()
                .AddEntityFrameworkMixins();

            var provider = services.BuildServiceProvider();
            var context = provider.GetRequiredService<UserDbContext>();

            var users = (
                from u in context.Users
                where u.Mixin<Author>().GooglePlusProfile != null
                select u //u.Mixin<Author>()
            ).ToList();

            
            var user = users.FirstOrDefault();
            if (user != null)
            {
                var author = user.Mixin<Author>();

                // You can make changes here:
                author.IsAwesome = true;

                // Save changes
                context.SaveChanges();
            }
        }
    }

    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("" /* Connection String */);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entity = modelBuilder.Entity<User>();

            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).IsRequired();
            entity.Property(u => u.Name).IsRequired().HasMaxLength(20);

            var mixin = entity.Mixin<Author>();
            mixin.Property(a => a.GooglePlusProfile).HasMaxLength(20);
            mixin.Property(a => a.IsAwesome).IsRequired();
        }
    }

    public class User : MixinHost
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Author : Mixin
    {
        public string GooglePlusProfile
        {
            get { return GetValue<string>(nameof(GooglePlusProfile)); }
            set { SetValue<string>(nameof(GooglePlusProfile), value); }
        }
        
        public bool IsAwesome
        {
            get { return GetValue<bool>(nameof(IsAwesome)); }
            set { SetValue<bool>(nameof(IsAwesome), value); }
        }
    }
}
