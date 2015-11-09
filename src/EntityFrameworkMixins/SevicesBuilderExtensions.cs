namespace EntityFrameworkMixins
{
    using Microsoft.Data.Entity.ChangeTracking.Internal;
    using Microsoft.Data.Entity.Infrastructure;
    using Microsoft.Data.Entity.Query;
    using Microsoft.Framework.DependencyInjection;
    using Microsoft.Framework.DependencyInjection.Extensions;

    /// <summary>
    /// Provides extension methods for service collections.
    /// </summary>
    public static class SevicesBuilderExtensions
    {
        /// <summary>
        /// Adds support for mixins for EF relational entities.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <returns>The services collection.</returns>
        public static IServiceCollection AddEntityFrameworkMixins(this IServiceCollection services)
        {
            if (services != null)
            {
                services.Replace(ServiceDescriptor.Scoped<IInternalEntityEntryFactory, InternalEntityEntryFactory>());
                services.Replace(ServiceDescriptor.Scoped<IEntityQueryModelVisitorFactory, RelationalQueryModelVisitorFactory>());
            }

            return services;
        }

        /// <summary>
        /// Adds support for mixins for EF relational entities.
        /// </summary>
        /// <param name="builder">The entity framework services builder.</param>
        /// <returns>The entity framework services builder.</returns>
        public static EntityFrameworkServicesBuilder AddEntityFrameworkMixins(this EntityFrameworkServicesBuilder builder)
        {
            var services = builder.GetService();

            services.AddEntityFrameworkMixins();

            return builder;
        }
    }
}