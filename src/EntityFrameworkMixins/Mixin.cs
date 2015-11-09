namespace EntityFrameworkMixins
{
    using System;
    using System.Collections.Concurrent;
    using Microsoft.Data.Entity.ChangeTracking;

    /// <summary>
    /// Provides a base implementation of a mixin.
    /// </summary>
    public abstract class Mixin
    {
        private readonly ConcurrentDictionary<string, object> _untrackedValues = new ConcurrentDictionary<string, object>();
        private readonly string _mixinTypePrefix;
        private Func<string, PropertyEntry> _propertyEntryResolver;

        /// <summary>
        /// Initialises a new instance of <see cref="Mixin"/>.
        /// </summary>
        protected Mixin()
        {
            _mixinTypePrefix = GetType().Name;
        }

        /// <summary>
        /// Sets the property entry resolver for the current mixin.
        /// </summary>
        /// <param name="propertyEntryResolver">The resolver instance.</param>
        internal void SetPropertyEntryResolver(Func<string, PropertyEntry> propertyEntryResolver)
        {
            if (propertyEntryResolver == null)
            {
                throw new ArgumentNullException(nameof(propertyEntryResolver));
            }

            _propertyEntryResolver = propertyEntryResolver;
        }

        /// <summary>
        /// Gets the property key used to access a shadow property.
        /// </summary>
        /// <param name="name">The name of the mixin property.</param>
        /// <returns>The property key.</returns>
        protected string GetKey(string name)
        {
            return $"{_mixinTypePrefix}_{name}";
        }

        /// <summary>
        /// Gets the value for the property with the given name.
        /// </summary>
        /// <typeparam name="T">The property type.</typeparam>
        /// <param name="name">The property name.</param>
        /// <returns>The property instance.</returns>
        protected T GetValue<T>(string name)
        {
            if (_propertyEntryResolver != null)
            {
                var prop = _propertyEntryResolver(GetKey(name));
                if (prop != null)
                {
                    return (T)prop.CurrentValue;
                }
            }

            return (T)_untrackedValues.GetOrAdd(name, k => default(T));
        }

        /// <summary>
        /// Sets the value for the property with the given name.
        /// </summary>
        /// <typeparam name="T">The property type.</typeparam>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value of the property.</param>
        protected void SetValue<T>(string name, T value)
        {
            if (_propertyEntryResolver != null)
            {
                var prop = _propertyEntryResolver(GetKey(name));
                if (prop != null)
                {
                    prop.CurrentValue = value;
                    return;
                }
            }

            _untrackedValues.AddOrUpdate(name, value, (k, e) => value);
        }
    }
}