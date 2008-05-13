using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Bindable.Linq.Dependencies.Definitions;

namespace Bindable.Linq.Dependencies.ExpressionAnalysis.Extractors
{
    internal sealed class ItemDependencyExtractor : DependencyExtractor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemPropertyDependencyExtractor"/> class.
        /// </summary>
        /// <param name="itemParameterExpression">The item parameter expression.</param>
        public ItemDependencyExtractor()
        {
            
        }

        /// <summary>
        /// When overridden in a derived class, extracts the appropriate dependency from the root of the expression.
        /// </summary>
        /// <param name="rootExpression">The root expression.</param>
        /// <param name="propertyPath">The property path.</param>
        /// <returns></returns>
        protected override IDependencyDefinition ExtractFromRoot(Expression rootExpression, string propertyPath)
        {
            IDependencyDefinition result = null;
            if (rootExpression is ParameterExpression)
            {
                ParameterExpression parameterExpression = (ParameterExpression) rootExpression;
                result = new ItemDependencyDefinition(propertyPath, parameterExpression.Name);
            }
            return result;
        }
    }
}