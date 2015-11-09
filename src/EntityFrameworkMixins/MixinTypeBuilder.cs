namespace EntityFrameworkMixins
{
    using System;
    using System.Linq.Expressions;
    using Microsoft.Data.Entity.Metadata.Builders;

    /// <summary>
    /// Provides support for building model information related to mixin properties.
    /// </summary>
    /// <typeparam name="TMixin">The mixin type.</typeparam>
    public class MixinTypeBuilder<TMixin> where TMixin : Mixin
    {
        private readonly EntityTypeBuilder _entityTypeBuilder;
        private readonly string _propertyPrefix = $"{typeof(TMixin).Name}_";

        /// <summary>
        /// Initialises a new instance of <see cref="MixinTypeBuilder{TMixin}"/>.
        /// </summary>
        /// <param name="entityTypeBuilder">The parent entity type builder.</param>
        internal MixinTypeBuilder(EntityTypeBuilder entityTypeBuilder)
        {
            _entityTypeBuilder = entityTypeBuilder;

            _entityTypeBuilder.Annotation("MixinType", typeof(TMixin));
        }

        /// <summary>
        /// Returns a property builder for the given mixin property.
        /// </summary>
        /// <typeparam name="TProperty">The property type.</typeparam>
        /// <param name="propertyExpression">The property selection expression.</param>
        /// <returns>The property builder.</returns>
        public PropertyBuilder<TProperty> Property<TProperty>(Expression<Func<TMixin, TProperty>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            string propertyName = propertyExpression.GetPropertyAccess().Name;
            string propertyKey = $"{_propertyPrefix}{propertyName}";

            var builder = _entityTypeBuilder.Property<TProperty>(propertyKey);

            return builder;
        }
    }
}