using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Filtering
{
   
    public class FilterDefinitionForPropertyValue<O,T>:FilterDefinition, IFilter<O>
    {

        public Expression<Func<O, T>> FieldSelector { get; set; }

        public Expression<Func<O, bool>> GetFilterPredicateFor(FilterOperations operation, string value)
        {
     
            T typedValue;
            Type type = typeof(T);
            bool nullable = Nullable.GetUnderlyingType(type) != null;
            object conversionResult;

            if (nullable)
            {
                type = Nullable.GetUnderlyingType(type);
                
                if (value == "NULL")
                {
                    conversionResult = null;
                }
                else
                {
                    if (type.IsEnum)
                    {
                        conversionResult = (T)Enum.Parse(type, value);
                    }
                    else
                    {
                        conversionResult = Convert.ChangeType(value, type);
                    }
                }

            }
            else
            {
                if (type.IsEnum)
                {
                    conversionResult = (T)Enum.Parse(type, value);
                }
                else
                {
                    conversionResult = Convert.ChangeType(value, type);
                }
            }


            typedValue = (T)conversionResult;


            var predicate = GetFilterPredicate(FieldSelector, operation, typedValue);
            return predicate;
        
        }

        private Expression<Func<O, bool>> GetFilterPredicate(Expression<Func<O, T>> selector, FilterOperations operand, T value)
        {
            var getExpressionBody = selector.Body as MemberExpression;
            if (getExpressionBody == null)
            {
                throw new Exception("getExpressionBody is not MemberExpression: " + selector.Body);
            }

            var parameter = selector.Parameters[0];
            var left = selector.Body;
            var right = Expression.Constant(value);
            var correctValue = Expression.Convert(right, typeof(T));
            var binaryExpression = Expression.MakeBinary(operand.ToExpressionType(), left, correctValue);
            return Expression.Lambda<Func<O, bool>>(binaryExpression, parameter);
        }
    }
}
