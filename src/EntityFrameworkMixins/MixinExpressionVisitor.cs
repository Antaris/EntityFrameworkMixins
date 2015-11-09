namespace EntityFrameworkMixins
{
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Microsoft.Data.Entity.Query.ExpressionVisitors;
    using Microsoft.Data.Entity.Query;
    using Microsoft.Data.Entity.Metadata;

    /// <summary>
    /// Replaces calls to <code>Mixin()</code> that form part of queries to the <see cref="EF.Property"/> method.
    /// </summary>
    public class MixinExpressionVisitor : ExpressionVisitorBase
    {
        private readonly IModel _model;

        /// <summary>
        /// Initialises a new instance of <see cref="MixinExpressionVisitor"/>.
        /// </summary>
        /// <param name="model">The entity model.</param>
        public MixinExpressionVisitor(IModel model)
        {
            _model = model;
        }

        /// <summary>
        /// Transforms a call to <code>entity.Mixin&lt;T&gt;().Property</code> to <code>EF.Property&lt;T&gt;(entity, "Property")</code>.
        /// </summary>
        /// <param name="member">The member expression.</param>
        /// <returns>The transformed expression.</returns>
        public Expression TransformMixinMemberExpression(MemberExpression member)
        {
            var method = (MethodCallExpression)member.Expression;
            var target = method.Object;
            string propertyName = $"{method.Type.Name}_{member.Member.Name}";

            return Expression.Call(
                EntityQueryModelVisitor.PropertyMethodInfo.MakeGenericMethod(member.Type),
                target,
                Expression.Constant(propertyName));
        }

        /// <summary>
        /// Transforms a call to <code>entity.Mixin&lt;T&gt;()</code> to <code>new MixinType { Properties, ... }</code>
        /// </summary>
        /// <param name="method">The method call expression.</param>
        /// <returns>The</returns>
        public Expression TransformMixinMethodExpression(MethodCallExpression method)
        {
            var mixinType = method.Type;
            var entityType = method.Object.Type;
            string prefix = $"{mixinType.Name}_";

            // Get the available properties of the mixin.
            var entity = _model.GetEntityType(entityType);
            var properties = entity
                .GetProperties()
                .Where(p => p.Name.StartsWith(prefix))
                .ToArray();

            // Create an object initializer expression.
            var ctor = Expression.New(mixinType);
            var memberBindings = new MemberBinding[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                string propertyName = property.Name.Replace(prefix, "");
                var member = mixinType.GetProperty(propertyName);
                var value = Expression.Call(
                    EntityQueryModelVisitor.PropertyMethodInfo.MakeGenericMethod(member.PropertyType),
                    method.Object,
                    Expression.Constant(property.Name));

                memberBindings[i] = Expression.Bind(member, value);
            }

            return Expression.MemberInit(ctor, memberBindings);
        }

        /// <inheritdoc />
        protected override Expression VisitMember(MemberExpression node)
        {
            var method = node.Expression as MethodCallExpression;
            if (method != null && method.Method.IsGenericMethod && method.Method.Name == "Mixin")
            {
                // Here we are transforming calls like "entity.Mixin<Type>().Property" to "EF.Property<PropertyType>(entity, "FullPropertyName")"
                return TransformMixinMemberExpression(node);
            }

            return base.VisitMember(node);
        }

        /// <inheritdoc />
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var method = node.Method;
            if (method != null && method.IsGenericMethod && method.Name == "Mixin")
            {
                // Here we are transforming calls like "select entity.Mixin<Type>()" to "new Type { PropertyName = EF.Property<PropertyType>(entity, "FullPropertyName") }
                return TransformMixinMethodExpression(node);
            }
            return base.VisitMethodCall(node);
        }
    }
}