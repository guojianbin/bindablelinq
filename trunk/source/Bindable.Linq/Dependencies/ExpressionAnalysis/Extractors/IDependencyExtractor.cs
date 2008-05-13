using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Bindable.Linq.Dependencies;

namespace Bindable.Linq.Dependencies.ExpressionAnalysis.Extractors
{
    /// <summary>
    /// Implemented by objects which analyse expressions and extract dependencies.
    /// </summary>
    public interface IDependencyExtractor
    {
        /// <summary>
        /// Extracts any dependencies within the specified LINQ expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        IEnumerable<IDependencyDefinition> Extract(Expression expression);
    }
}