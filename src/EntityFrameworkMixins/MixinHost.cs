namespace EntityFrameworkMixins
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides a base implementation of an entity that supports mixins.
    /// </summary>
    public abstract class MixinHost : ISupportMixins
    {
        private readonly List<Mixin> _mixins = new List<Mixin>();

        /// <inheritdoc />
        void ISupportMixins.AddMixin(Mixin mixin)
        {
            _mixins.Add(mixin);
        }

        /// <inheritdoc />
        public T Mixin<T>() where T : Mixin
        {
            return _mixins.OfType<T>().FirstOrDefault() ?? Activator.CreateInstance<T>();
        }
    }
}