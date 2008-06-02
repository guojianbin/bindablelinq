using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Bindable.Linq.Dependencies.Definitions;

namespace Bindable.Linq.Dependencies.ExpressionAnalysis.Extractors
{
    internal sealed class ExternalDependencyExtractor : DependencyExtractor
    {
        /// <summary>
        /// When overridden in a derived class, extracts the appropriate dependency from the root of the expression.
        /// </summary>
        /// <param name="rootExpression">The root expression.</param>
        /// <param name="propertyPath">The property path.</param>
        /// <returns></returns>
        protected override IDependencyDefinition ExtractFromRoot(Expression rootExpression, string propertyPath)
        {
            IDependencyDefinition result = null;
            if (rootExpression is ConstantExpression)
            {
                ConstantExpression constantExpression = (ConstantExpression) rootExpression;
                if (propertyPath != null || (propertyPath == null && constantExpression.Value is INotifyPropertyChanged))
                {
                    result = new ExternalDependencyDefinition(propertyPath, constantExpression.Value);
                }
            }
            return result;
        }
    }
}