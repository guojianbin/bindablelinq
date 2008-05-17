using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Bindable.Linq.Dependencies.ExpressionAnalysis;

namespace Bindable.Linq.Dependencies.ExpressionAnalysis.Extractors
{
    /// <summary>
    /// Serves as a base class for dependency extractors that create dependencies against properties. 
    /// These dependencies have one thing in common: They only look for MemberAccess expressions, and 
    /// the type of dependency depends on what the root of the expression is.
    /// </summary>
    internal abstract class DependencyExtractor : IDependencyExtractor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyExtractor"/> class.
        /// </summary>
        protected DependencyExtractor()
        {
        }

        /// <summary>
        /// Extracts any dependencies within the specified LINQ expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public IEnumerable<IDependencyDefinition> Extract(Expression expression)
        {
            List<IDependencyDefinition> results = new List<IDependencyDefinition>();

            // Find the root member access expressions
            ExpressionFlattener analyser = new ExpressionFlattener(expression, ExpressionType.MemberAccess);
            IEnumerable<Expression> memberExpressions = analyser.Expressions;

            // Turn each one into the appropriate dependency
            foreach (Expression childExpression in analyser.Expressions)
            {
                bool traverse = false;
                Expression currentExpression = childExpression;
                string propertyPath = null;

                if (childExpression is MemberExpression)
                {
                    MemberExpression childMemberExpression = (MemberExpression) childExpression;
                    propertyPath = childMemberExpression.Member.Name;
                    currentExpression = childMemberExpression.Expression;
                    traverse = true;
                    while (true)
                    {
                        if (currentExpression is MemberExpression)
                        {
                            MemberExpression nextMemberExpression = (MemberExpression) currentExpression;
                            propertyPath = nextMemberExpression.Member.Name + "." + propertyPath;
                            if (nextMemberExpression.Expression != null)
                            {
                                currentExpression = nextMemberExpression.Expression;
                                continue;
                            }
                        }
                        break;
                    }
                }

                if (currentExpression != null)
                {
                    IDependencyDefinition dependency = ExtractFromRoot(currentExpression, propertyPath);
                    if (dependency != null)
                    {
                        results.Add(dependency);
                    }
                    else if (traverse)
                    {
                        results.AddRange(Extract(currentExpression));
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// When overridden in a derived class, extracts the appropriate dependency from the root of the expression.
        /// </summary>
        /// <param name="rootExpression">The root expression.</param>
        /// <param name="propertyPath">The property path.</param>
        /// <returns></returns>
        protected abstract IDependencyDefinition ExtractFromRoot(Expression rootExpression, string propertyPath);
    }
}