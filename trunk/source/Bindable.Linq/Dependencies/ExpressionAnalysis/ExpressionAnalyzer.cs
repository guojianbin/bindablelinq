using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Bindable.Linq.Helpers;
using Bindable.Linq.Dependencies.ExpressionAnalysis;
using Bindable.Linq.Dependencies.ExpressionAnalysis.Extractors;

namespace Bindable.Linq.Dependencies.ExpressionAnalysis
{
    /// <summary>
    /// A factory for extracting dependencies from LINQ expression trees.
    /// </summary>
    public sealed class ExpressionAnalyzer : IExpressionAnalyzer
    {
        private IDependencyExtractor[] _extractors;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionAnalyzer"/> class.
        /// </summary>
        /// <param name="extractors">The extractors.</param>
        public ExpressionAnalyzer(params IDependencyExtractor[] extractors)
        {
            _extractors = extractors;
        }

        /// <summary>
        /// Extracts the dependencies.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="itemParameter">The item parameter.</param>
        public IEnumerable<IDependencyDefinition> DiscoverDependencies(Expression expression, ParameterExpression itemParameter)
        {
            return _extractors.Select(extractor => extractor.Extract(expression))
                              .UnionAll()
                              .Distinct(DependencyComparer.Instance);
        }
    }
}