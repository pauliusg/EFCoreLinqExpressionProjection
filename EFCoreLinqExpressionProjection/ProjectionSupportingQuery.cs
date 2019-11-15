using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EFCoreLinqExpressionProjection
{
    /// <summary>
    /// An IQueryable wrapper that allows us to visit the query's expression tree just before LINQ to SQL gets to it.
    /// This is based on the excellent work of Tomas Petricek: http://tomasp.net/blog/linq-expand.aspx
    /// </summary>
    public class ProjectionSupportingQuery<T> : IQueryable<T>, IOrderedQueryable<T>, IOrderedQueryable, IAsyncEnumerable<T>
    {
        ProjectionSupportingQueryProvider<T> _provider;
		IQueryable<T> _inner;

		internal IQueryable<T> InnerQuery { get { return _inner; } }			// Original query, that we're wrapping

        internal ProjectionSupportingQuery(IQueryable<T> inner)
		{
			_inner = inner;
            _provider = new ProjectionSupportingQueryProvider<T>(this);
		}

		Expression IQueryable.Expression { get { return _inner.Expression; } }
		Type IQueryable.ElementType { get { return typeof (T); } }
		IQueryProvider IQueryable.Provider { get { return _provider; } }
		public IEnumerator<T> GetEnumerator () { return _inner.GetEnumerator (); }
		IEnumerator IEnumerable.GetEnumerator () { return _inner.GetEnumerator (); }
        IAsyncEnumerator<T> IAsyncEnumerable<T>.GetEnumerator()
        {
            return new AsyncEnumerator<T>(_inner.AsEnumerable().GetEnumerator());
        }
        public override string ToString () { return _inner.ToString (); }
    }
}
