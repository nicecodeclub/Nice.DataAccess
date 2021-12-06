using Nice.DataAccess.Attributes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Nice.DataAccess
{
    public class ExpressionHandler
    {
        private readonly StringBuilder sbSql = null;
        private readonly IList<DataParameter> parameters = null;
        private int index = 0;
        protected readonly DataHelper DataHelper = null;
        public ExpressionHandler(DataHelper DataHelper, StringBuilder sbSql, IList<DataParameter> parameters)
        {
            this.sbSql = sbSql;
            this.parameters = parameters;
            this.DataHelper = DataHelper;
        }

        public void Execute<T>(Expression<Func<T, T>> expression)
        {
            Execute(expression.Body);
        }
        public void Execute<T>(Expression<Func<T, bool>> expression)
        {
            Execute(expression.Body);
        }

        public void Execute(Expression expression)
        {
            if (expression is BinaryExpression)
            {
                Execute(expression as BinaryExpression);
            }
            else if (expression is MemberExpression)
            {
                Execute(expression as MemberExpression);
            }
            else if (expression is ConstantExpression)
            {
                Execute(expression as ConstantExpression);
            }
            else if (expression is MemberInitExpression)
            {
                Execute(expression as MemberInitExpression);
            }
            else if (expression is MethodCallExpression)
            {
                Execute(expression as MethodCallExpression);
            }
            else if (expression is UnaryExpression)
            {
                Execute(expression as UnaryExpression);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        public void Execute(BinaryExpression expression)
        {
            Execute(expression.Left);
            Operate(expression.NodeType);
            Execute(expression.Right);
        }
        public void Operate(ExpressionType type)
        {
            if (type == ExpressionType.Equal) sbSql.Append("=");
            else if (type == ExpressionType.GreaterThan) sbSql.Append(">");
            else if (type == ExpressionType.GreaterThanOrEqual) sbSql.Append(">=");
            else if (type == ExpressionType.LessThan) sbSql.Append("<");
            else if (type == ExpressionType.LessThanOrEqual) sbSql.Append("<=");
            else if (type == ExpressionType.OrElse) sbSql.Append(" OR ");
            else if (type == ExpressionType.Or) sbSql.Append("|");
            else if (type == ExpressionType.AndAlso) sbSql.Append(" AND ");
            else if (type == ExpressionType.And) sbSql.Append("&");
            else if (type == ExpressionType.NotEqual) sbSql.Append("<>");
            else if (type == ExpressionType.Add) sbSql.Append("+");
            else if (type == ExpressionType.Subtract) sbSql.Append("-");
            else if (type == ExpressionType.Multiply) sbSql.Append("*");
            else if (type == ExpressionType.Divide) sbSql.Append("/");
            else if (type == ExpressionType.Modulo) sbSql.Append("%");
            else
            {
                throw new NotImplementedException();
            }
        }

        public void Execute(MemberExpression expression)
        {
            if (expression.Expression == null)
            {
                throw new NotImplementedException();
            }
            else
            {
                if (expression.Expression is ParameterExpression)
                {
                    string columnName = null;
                    ColumnAttribute attr = CustomAttributeExtensions.GetCustomAttribute<ColumnAttribute>(expression.Member);
                    if (attr == null || string.IsNullOrEmpty(attr.Name))
                    {
                        columnName = expression.Member.Name;
                    }
                    else
                    {
                        columnName = attr.Name;
                    }
                    sbSql.Append(columnName);
                    parameters.Add(new DataParameter() { ParameterName = string.Format("{0}{1}_{2}", DataHelper.GetParameterPrefix(), expression.Member.Name, parameters.Count) });
                }
                else
                {
                    UnaryExpression exprConvert = Expression.Convert(expression, typeof(object));
                    //sb.Append(Expression.Lambda<Func<object>>(exprConvert).Compile().Invoke());
                    CreateParameter(Expression.Lambda<Func<object>>(exprConvert).Compile().Invoke());
                }
            }
        }
        public void Execute(ConstantExpression expression)
        {
            //sb.Append(expression.Value);
            CreateParameter(expression.Value);
        }

        public void Execute(UnaryExpression expression)
        {
            Execute(expression.Operand);
        }

        public void Execute(MemberInitExpression expression)
        {

        }
        public void Execute(MethodCallExpression expression)
        {
            //sb.Append(Expression.Lambda(expression).Compile().DynamicInvoke());
            CreateParameter(Expression.Lambda(expression).Compile().DynamicInvoke());
        }

        private void CreateParameter(object value)
        {
            sbSql.Append(parameters[index].ParameterName);
            parameters[index].Value = value;
            index++;
        }

        public static string GetPropertyName<TEntity, TReturn>(Expression<Func<TEntity, TReturn>> expression)
        {
            MemberExpression memberExpress = expression.Body as MemberExpression;
            if (memberExpress != null)
            {
                return memberExpress.Member.Name;
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            MemberExpression memberExpress = null;
            if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpress = expression.Body as MemberExpression;
            }
            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                memberExpress = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }
            if (memberExpress != null)
            {
                return memberExpress.Member.Name;
            }
            else
            {
                return string.Empty;
            }
        }

        public static IList<string> GetPropertyNames<T>(params Expression<Func<T, object>>[] expressions)
        {
            IList<string> propertyNames = new List<string>();
            MemberExpression memberExpress = null;
            if (expressions == null) return propertyNames;
            foreach (Expression<Func<T, object>> expression in expressions)
            {
                if (expression.Body.NodeType == ExpressionType.MemberAccess)
                {
                    memberExpress = expression.Body as MemberExpression;
                }
                if (expression.Body.NodeType == ExpressionType.Convert)
                {
                    memberExpress = ((UnaryExpression)expression.Body).Operand as MemberExpression;
                }
                if (memberExpress != null)
                {
                    propertyNames.Add(memberExpress.Member.Name);
                }
                else
                {
                    propertyNames.Add(string.Empty);
                }
            }
            return propertyNames;
        }
    }

    public class DataParameter
    {
        public string ParameterName { get; set; }

        public object Value { get; set; }
    }
}
