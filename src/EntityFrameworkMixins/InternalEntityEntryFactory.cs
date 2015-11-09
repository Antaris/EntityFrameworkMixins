namespace EntityFrameworkMixins
{
    using System;
    using System.Linq;
    using Microsoft.Data.Entity.ChangeTracking;
    using Microsoft.Data.Entity.ChangeTracking.Internal;
    using Microsoft.Data.Entity.Metadata;
    using Microsoft.Data.Entity.Storage;
    using EFInternalEntityEntryFactory = Microsoft.Data.Entity.ChangeTracking.Internal.InternalEntityEntryFactory;

    /// <summary>
    /// Provides services for creating internal entity entries for change tracking.
    /// </summary>
    public class InternalEntityEntryFactory : EFInternalEntityEntryFactory
    {
        /// <summary>
        /// Initialises a new instance of <see cref="InternalEntityEntryFactory"/>
        /// </summary>
        /// <param name="metadataServices">The metadata services.</param>
        public InternalEntityEntryFactory(IEntityEntryMetadataServices metadataServices) : base(metadataServices)
        {
        }

        /// <inheritdoc />
        public override InternalEntityEntry Create(IStateManager stateManager, IEntityType entityType, object entity)
        {
            var entry = base.Create(stateManager, entityType, entity);

            BindMixins(entry, entityType, entity);

            return entry;
        }

        /// <inheritdoc />
        public override InternalEntityEntry Create(IStateManager stateManager, IEntityType entityType, object entity, ValueBuffer valueBuffer)
        {
            var entry = base.Create(stateManager, entityType, entity, valueBuffer);

            BindMixins(entry, entityType, entity);

            return entry;
        }

        /// <summary>
        /// Binds the mixins to the entity through change tracking.
        /// </summary>
        /// <param name="entry">The entity entry.</param>
        /// <param name="entityType">The entity type.</param>
        /// <param name="entity">The entity instance.</param>
        private void BindMixins(InternalEntityEntry entry, IEntityType entityType, object entity)
        {
            var mixinHost = entity as ISupportMixins;
            if (mixinHost != null)
            {
                var mixinTypes = entityType
                    .Annotations
                    .Where(a => a.Name == "MixinType")
                    .Select(a => (Type)a.Value)
                    .Distinct()
                    .ToArray();

                foreach (var mixinType in mixinTypes)
                {
                    // Create the mixin.
                    var mixin = (Mixin)Activator.CreateInstance(mixinType);

                    // Set the resolver.
                    mixin.SetPropertyEntryResolver(p => new PropertyEntry(entry, p));

                    // Assign to the host entity.
                    mixinHost.AddMixin(mixin);
                }
            }
        }
    }
}
