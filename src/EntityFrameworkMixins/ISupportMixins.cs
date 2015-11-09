namespace EntityFrameworkMixins
{
    /// <summary>
    /// Defines the required contract for implementing a mixin host.
    /// </summary>
    public interface ISupportMixins
    {
        /// <summary>
        /// Adds a mixin to the host entity.
        /// </summary>
        /// <param name="mixin">The mixin instance.</param>
        void AddMixin(Mixin mixin);

        /// <summary>
        /// Gets the mixin of the given type.
        /// </summary>
        /// <typeparam name="T">The mixin type.</typeparam>
        /// <returns>The mixin instance.</returns>
        T Mixin<T>() where T : Mixin;
    }
}