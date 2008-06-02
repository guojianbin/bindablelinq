using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq.Dependencies.ExpressionAnalysis;
using Bindable.Linq.Dependencies.ExpressionAnalysis.Extractors;
using Bindable.Linq.Dependencies.PathNavigation;
using Bindable.Linq.Dependencies.PathNavigation.TokenFactories;

namespace Bindable.Linq.Configuration
{
    internal sealed class DefaultBindingConfiguration : IBindingConfiguration
    {
        private IExpressionAnalyzer _expressionAnalyzer;
        private IPathNavigator _pathNavigator;

        public DefaultBindingConfiguration()
        {
            _expressionAnalyzer = new ExpressionAnalyzer(
                new ItemDependencyExtractor(),
                new ExternalDependencyExtractor(),
                new StaticDependencyExtractor()
                );
            _pathNavigator = new PathNavigator(
                new WpfMemberTokenFactory(),
                new SilverlightMemberTokenFactory(),
                new WindowsFormsMemberTokenFactory(),
                new ClrMemberTokenFactory()
                );
        }

        public IExpressionAnalyzer CreateExpressionAnalyzer()
        {
            return _expressionAnalyzer;
        }

        public IPathNavigator CreatePathNavigator()
        {
            return _pathNavigator;
        }
    }
}
