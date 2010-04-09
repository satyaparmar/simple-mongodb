using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Pls.SimpleMongoDb
{
    public static class TypeExtensions
    {
        public static MemberInfo DecodeMemberAccessExpression<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> expression) {
            if (expression.Body.NodeType != ExpressionType.MemberAccess) {
                if ((expression.Body.NodeType == ExpressionType.Convert) && (expression.Body.Type == typeof(object))) {
                    return ((MemberExpression)((UnaryExpression)expression.Body).Operand).Member;
                }
                throw new Exception(string.Format("Invalid expression type: Expected ExpressionType.MemberAccess, Found {0}",
                                                  expression.Body.NodeType));
            }
            return ((MemberExpression)expression.Body).Member;
        }
    }
}