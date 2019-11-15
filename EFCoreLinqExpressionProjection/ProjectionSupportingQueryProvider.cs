using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace EFCoreLinqExpressionProjection
{
    internal class ProjectionSupportingQueryProvider<T> : IQueryProvider, IAsyncQueryProvider
    {
        private readonly ProjectionSupportingQuery<T> _query;

        internal ProjectionSupportingQueryProvider(ProjectionSupportingQuery<T> query)
        {
            _query = query;
        }

        // The following four methods first call ExpressionExpander to visit the expression tree, then call
        // upon the inner query to do the remaining work.

        IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
        {
            return new ProjectionSupportingQuery<TElement>(_query.InnerQuery.Provider.CreateQuery<TElement>(expression.ExpandExpressionsForProjection()));
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            return _query.InnerQuery.Provider.CreateQuery(expression.ExpandExpressionsForProjection());
        }

        object IQueryProvider.Execute(Expression expression)
        {
            return _query.InnerQuery.Provider.Execute(expression.ExpandExpressionsForProjection());
        }

        TResult IQueryProvider.Execute<TResult>(Expression expression)
        {
            return _query.InnerQuery.Provider.Execute<TResult>(expression.ExpandExpressionsForProjection());
        }
        
        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            var result = new ProjectionSupportingQuery<TResult>(_query.InnerQuery.Provider.CreateQuery<TResult>(expression.ExpandExpressionsForProjection()));
            return result;
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            var result = _query.InnerQuery.Provider.Execute<TResult>(expression.ExpandExpressionsForProjection());
            return Task.FromResult(result);
        }
    }
}
