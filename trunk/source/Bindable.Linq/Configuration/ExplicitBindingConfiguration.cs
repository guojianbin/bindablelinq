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
    internal sealed class ExplicitBindingConfiguration : IBindingConfiguration
    {
        private IExpressionAnalyzer _expressionAnalyzer;
        private IPathNavigator _pathNavigator;

        public ExplicitBindingConfiguration()
        {
            _expressionAnalyzer = new ExpressionAnalyzer();
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
