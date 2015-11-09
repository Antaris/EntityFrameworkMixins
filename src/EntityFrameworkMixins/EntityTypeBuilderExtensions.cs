namespace EntityFrameworkMixins
{
    using Microsoft.Data.Entity.Metadata.Builders;

    /// <summary>
    /// Provides extension methods for <see cref="EntityTypeBuilder"/> instances.
    /// </summary>
    public static class EntityTypeBuilderExtensions
    {
        /// <summary>
        /// Returns an mixin type builder attached to the given entity type builder.
        /// </summary>
        /// <typeparam name="TMixin">The mixin type.</typeparam>
        /// <param name="entityTypeBuilder">The entity type builder.</param>
        /// <returns>The mixin type builder.</returns>
        public static MixinTypeBuilder<TMixin> Mixin<TMixin>(this EntityTypeBuilder entityTypeBuilder) where TMixin : Mixin
        {
            return new MixinTypeBuilder<TMixin>(entityTypeBuilder);
        }
    }
}