using System;
using System.Linq.Expressions;

namespace Filtering
{
    interface IFilter<ClassToFilter>
    {
        Expression<Func<ClassToFilter, bool>> GetFilterPredicateFor(FilterOperations operation, string value);
    }
}
