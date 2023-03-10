using NUnit.Framework.Constraints;
using System;
using System.Linq.Expressions;

namespace Reflection.Differentiation
{
   public static class Algebra
   {
        public static Expression<Func<double, double>> Differentiate(Expression<Func<double, double>> expression)
        {
            return Expression.Lambda<Func<double, double>>(SelectBody(expression.Body), expression.Parameters);
        }

        private static Expression SelectBody(Expression expression)
        {
            if(expression is ConstantExpression)
                return Expression.Constant(0d);

            if(expression is ParameterExpression)
                return Expression.Constant(1d);

            if(expression is BinaryExpression binaryExpression)
            {
                var leftOperator = binaryExpression.Left;
                var rightOperator = binaryExpression.Right;

                if(expression.NodeType == ExpressionType.Add)
                    return Expression.Add(SelectBody(leftOperator), SelectBody(rightOperator));
                else if(expression.NodeType == ExpressionType.Multiply)
                    return Expression.Add(
                        Expression.Multiply(SelectBody(leftOperator), rightOperator),
                        Expression.Multiply(SelectBody(rightOperator), leftOperator));
            }

            if(expression is MethodCallExpression methodCallExpression) 
            {
                var method = expression;
                var values = methodCallExpression.Arguments[0];

                if (methodCallExpression.Method.Name == "Sin")
                    method = Expression.Call(
                        typeof(Math).GetMethod("Cos", new[] { typeof(double) }), 
                        values);
                else if (methodCallExpression.Method.Name == "Cos")
                    method = Expression.Negate(
                        Expression.Call(
                            typeof(Math).GetMethod("Sin", new[] { typeof(double) }), 
                            values));
                else
                    throw new ArgumentException($"Method {methodCallExpression.Method.Name} is not valid");
                return Expression.Multiply(method, SelectBody(values));
            }

            throw new ArgumentException($"ToString is not valid");
        }
   }
}
