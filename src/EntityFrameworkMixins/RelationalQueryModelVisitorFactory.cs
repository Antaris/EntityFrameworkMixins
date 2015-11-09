namespace EntityFrameworkMixins
{
    using System;
    using Microsoft.Data.Entity.Metadata;
    using Microsoft.Data.Entity.Metadata.Internal;
    using Microsoft.Data.Entity.Query;
    using Microsoft.Data.Entity.Query.ExpressionVisitors;
    using Microsoft.Data.Entity.Query.Internal;
    using Microsoft.Data.Entity.Infrastructure;


    /// <summary>
    /// A factory for creating instances of <see cref="RelationalQueryModelVisitor"/>.
    /// </summary>
    public class RelationalQueryModelVisitorFactory : EntityQueryModelVisitorFactory
    {
        public RelationalQueryModelVisitorFactory(
            IModel model,
            IQueryOptimizer queryOptimizer,
            INavigationRewritingExpressionVisitorFactory navigationRewritingExpressionVisitorFactory,
            ISubQueryMemberPushDownExpressionVisitor subQueryMemberPushDownExpressionVisitor,
            IQuerySourceTracingExpressionVisitorFactory querySourceTracingExpressionVisitorFactory,
            IEntityResultFindingExpressionVisitorFactory entityResultFindingExpressionVisitorFactory,
            ITaskBlockingExpressionVisitor taskBlockingExpressionVisitor,
            IMemberAccessBindingExpressionVisitorFactory memberAccessBindingExpressionVisitorFactory,
            IOrderingExpressionVisitorFactory orderingExpressionVisitorFactory,
            IProjectionExpressionVisitorFactory projectionExpressionVisitorFactory,
            IEntityQueryableExpressionVisitorFactory entityQueryableExpressionVisitorFactory,
            IQueryAnnotationExtractor queryAnnotationExtractor,
            IResultOperatorHandler resultOperatorHandler,
            IEntityMaterializerSource entityMaterializerSource,
            IExpressionPrinter expressionPrinter,
            IRelationalAnnotationProvider relationalAnnotationProvider,
            IIncludeExpressionVisitorFactory includeExpressionVisitorFactory,
            ISqlTranslatingExpressionVisitorFactory sqlTranslatingExpressionVisitorFactory,
            ICompositePredicateExpressionVisitorFactory compositePredicateExpressionVisitorFactory,
            IQueryFlatteningExpressionVisitorFactory queryFlatteningExpressionVisitorFactory,
            IShapedQueryFindingExpressionVisitorFactory shapedQueryFindingExpressionVisitorFactory,
            IDbContextOptions contextOptions)
            : base(
                model,
                queryOptimizer,
                navigationRewritingExpressionVisitorFactory,
                subQueryMemberPushDownExpressionVisitor,
                querySourceTracingExpressionVisitorFactory,
                entityResultFindingExpressionVisitorFactory,
                taskBlockingExpressionVisitor,
                memberAccessBindingExpressionVisitorFactory,
                orderingExpressionVisitorFactory,
                projectionExpressionVisitorFactory,
                entityQueryableExpressionVisitorFactory,
                queryAnnotationExtractor,
                resultOperatorHandler,
                entityMaterializerSource,
                expressionPrinter)
        {
            RelationalAnnotationProvider = relationalAnnotationProvider;
            IncludeExpressionVisitorFactory = includeExpressionVisitorFactory;
            SqlTranslatingExpressionVisitorFactory = sqlTranslatingExpressionVisitorFactory;
            CompositePredicateExpressionVisitorFactory = compositePredicateExpressionVisitorFactory;
            QueryFlatteningExpressionVisitorFactory = queryFlatteningExpressionVisitorFactory;
            ShapedQueryFindingExpressionVisitorFactory = shapedQueryFindingExpressionVisitorFactory;
            ContextOptions = contextOptions;
        }

        protected virtual IRelationalAnnotationProvider RelationalAnnotationProvider { get; }
        protected virtual IIncludeExpressionVisitorFactory IncludeExpressionVisitorFactory { get; }
        protected virtual ISqlTranslatingExpressionVisitorFactory SqlTranslatingExpressionVisitorFactory { get; }
        protected virtual ICompositePredicateExpressionVisitorFactory CompositePredicateExpressionVisitorFactory { get; }
        protected virtual IQueryFlatteningExpressionVisitorFactory QueryFlatteningExpressionVisitorFactory { get; }
        protected virtual IShapedQueryFindingExpressionVisitorFactory ShapedQueryFindingExpressionVisitorFactory { get; }
        protected virtual IDbContextOptions ContextOptions { get; }

        /// <inheritdoc />
        public override EntityQueryModelVisitor Create(
            QueryCompilationContext queryCompilationContext,
            EntityQueryModelVisitor parentEntityQueryModelVisitor)
            => new RelationalQueryModelVisitor(
                Model,
                QueryOptimizer,
                NavigationRewritingExpressionVisitorFactory,
                SubQueryMemberPushDownExpressionVisitor,
                QuerySourceTracingExpressionVisitorFactory,
                EntityResultFindingExpressionVisitorFactory,
                TaskBlockingExpressionVisitor,
                MemberAccessBindingExpressionVisitorFactory,
                OrderingExpressionVisitorFactory,
                ProjectionExpressionVisitorFactory,
                EntityQueryableExpressionVisitorFactory,
                QueryAnnotationExtractor,
                ResultOperatorHandler,
                EntityMaterializerSource,
                ExpressionPrinter,
                RelationalAnnotationProvider,
                IncludeExpressionVisitorFactory,
                SqlTranslatingExpressionVisitorFactory,
                CompositePredicateExpressionVisitorFactory,
                QueryFlatteningExpressionVisitorFactory,
                ShapedQueryFindingExpressionVisitorFactory,
                ContextOptions,
                (RelationalQueryCompilationContext)queryCompilationContext,
                (RelationalQueryModelVisitor)parentEntityQueryModelVisitor);
    }
}
