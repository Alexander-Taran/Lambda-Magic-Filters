using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Filtering
{
    public class FilterDefinitionForCollectionPropertyValue<FilteredClass, CollectionProperty, MemberValue>:IFilter<FilteredClass>
    {
        public Expression<Func<FilteredClass, IEnumerable<CollectionProperty>>> CollectionSelector { get; set; }
        public Expression<Func<CollectionProperty, MemberValue>> CollectionPropertySelector { get; set; }



        public Expression<Func<FilteredClass, bool>> GetFilterPredicateFor(FilterOperations operation, string value)
        {




            var getExpressionBody = CollectionPropertySelector.Body as MemberExpression;
            if (getExpressionBody == null)
            {
                throw new Exception("getExpressionBody is not MemberExpression: " + CollectionPropertySelector.Body);
            }

            var propertyParameter = CollectionPropertySelector.Parameters[0];
            var collectionParameter = CollectionSelector.Parameters[0];
            var left = CollectionPropertySelector.Body;
            var right = Expression.Constant(value);

            var innerLambda = Expression.Equal(left, right);

            var innerFunction = Expression.Lambda<Func<CollectionProperty, bool>>(innerLambda, propertyParameter);

            var method = typeof(Enumerable).GetMethods().Where(m => m.Name == "Any" && m.GetParameters().Length == 2).Single().MakeGenericMethod(typeof(CollectionProperty));

      
            var outerLambda = Expression.Call(method, Expression.Property(collectionParameter, (CollectionSelector.Body as MemberExpression).Member as System.Reflection.PropertyInfo), innerFunction);
      

            var result = Expression.Lambda<Func<FilteredClass, bool>>(outerLambda, collectionParameter);

            return result;

           

        }

    }
}
