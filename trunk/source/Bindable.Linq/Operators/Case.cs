using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Bindable.Linq.Operators
{
    public static class Case
    {
        //public static ISwitchCase<TInput, TReturn> When<TInput, TReturn>(Expression<Func<TInput, bool>> condition, Expression<Func<TInput, TReturn>> result)
        //{
        //    return new SwitchCase<TInput, TReturn>(condition, result, false);
        //}

        //public static ISwitchCase<TInput, TReturn> When<TInput, TReturn>(TInput condition, Expression<Func<TInput, TReturn>> result)
        //{
        //    return new SwitchCase<TInput, TReturn>(o => object.Equals(o, condition), result, false);
        //}

        public static ISwitchCase<TInput, TReturn> When<TInput, TReturn>(Expression<Func<TInput, bool>> condition, TReturn result)
        {
            return new SwitchCase<TInput, TReturn>(condition, o => result, false);
        }

        //public static ISwitchCase<TInput, TReturn> When<TInput, TReturn>(TInput condition, TReturn result)
        //{
        //    return new SwitchCase<TInput, TReturn>(o => object.Equals(o, condition), o => result, false);
        //}

        public static ISwitchCase<TInput, TReturn> Default<TInput, TReturn>(Expression<Func<TInput, TReturn>> result)
        {
            return new SwitchCase<TInput, TReturn>(o => true, result, true);
        }

        public static ISwitchCase<TInput, TReturn> Default<TInput, TReturn>(TReturn result)
        {
            return new SwitchCase<TInput, TReturn>(o => true, o => result, true);
        }
    }
}
