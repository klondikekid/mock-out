using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace $rootnamespace$
{
    public static class Mocked
    {
        public static string As(
            this MockCategories instance,
            Expression<Func<MockCategories, object>> expression)
        {
            return As(expression);
        }

        public static string As(
            Expression<Func<MockCategories, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(
                    "The expression cannot be null.");
            }

            return As(expression.Body);
        }

        public static string As(
            this MockCategories instance,
            Expression<Action<MockCategories>> expression)
        {
            return As(expression);
        }

        public static string As(
            Expression<Action<MockCategories>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(
                    "The expression cannot be null.");
            }

            return As(expression.Body);
        }

        private static string As(
            Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(
                    "The expression cannot be null.");
            }

            if (expression is MemberExpression)
            {
                // Reference type property or field
                var memberExpression =
                    (MemberExpression)expression;
                return memberExpression.Member.Name;
            }

            if (expression is MethodCallExpression)
            {
                // Reference type method
                var methodCallExpression =
                    (MethodCallExpression)expression;
                return methodCallExpression.Method.Name;
            }

            if (expression is UnaryExpression)
            {
                // Property, field of method returning value type
                var unaryExpression = (UnaryExpression)expression;
                return As(unaryExpression);
            }

            throw new ArgumentException("Invalid expression");
        }

        private static string As(
            UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression)
            {
                var methodExpression =
                    (MethodCallExpression)unaryExpression.Operand;
                return methodExpression.Method.Name;
            }

            return ((MemberExpression)unaryExpression.Operand)
                .Member.Name;
        }
    }
}
