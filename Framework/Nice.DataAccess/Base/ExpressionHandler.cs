using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
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
                    sbSql.Append(expression.Member.Name);
                    parameters.Add(new DataParameter() { ParameterName = string.Format("{0}{1}", DataHelper.GetParameterPrefix(), expression.Member.Name) });
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

    }

    public class DataParameter
    {
        public string ParameterName { get; set; }

        public object Value { get; set; }
    }
}
