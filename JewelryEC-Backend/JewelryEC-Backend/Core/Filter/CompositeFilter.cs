using JewelryEC_Backend.Models.Products;
using System.Linq.Expressions;

namespace JewelryEC_Backend.Core.Filter
{
    public class CompositeFilter<T>
    {
        private static Expression BuildFilterExpression(Filter filter, ParameterExpression parameter)
        {
            if (filter.Value == null || string.IsNullOrWhiteSpace(filter.Value.ToString()))
                return null;

            var property = Expression.Property(parameter, filter.Field);
            var constant = Expression.Constant(Convert.ChangeType(filter.Value, Nullable.GetUnderlyingType(property.Type) ?? property.Type));

            if (property.Type == typeof(string))
            {
                switch (filter.Operator.ToLower())
                {
                    case "contains":
                        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        return Expression.Call(property, containsMethod, constant);
                    case "startswith":
                        var startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string), typeof(StringComparison) });

                        // Convert the constant value to lowercase for case-insensitive comparison
                        var constantLower = Expression.Call(constant, typeof(string).GetMethod("ToLower", Type.EmptyTypes));

                        return Expression.Call(property, startsWithMethod, constantLower, Expression.Constant(StringComparison.OrdinalIgnoreCase));
                    // Add more string operators as needed...
                    default:
                        throw new ArgumentException($"Unsupported operator: {filter.Operator}");
                }
            }
            else
            {
                // Check if the property is nullable
                if (Nullable.GetUnderlyingType(property.Type) != null)
                {
                    // Handle nullable types
                    var nullableProperty = Expression.Convert(property, Nullable.GetUnderlyingType(property.Type));
                    var comparison = Expression.GreaterThan(nullableProperty, (Expression)constant);
                    return comparison;
                }
                else
                {
                    // Handle non-nullable types
                    switch (filter.Operator.ToLower())
                    {
                        case "eq":
                            return Expression.Equal(property, constant);
                        case "neq":
                            return Expression.NotEqual(property, constant);
                        case "lt":
                            return Expression.LessThan(property, constant);
                        case "lte":
                            return Expression.LessThanOrEqual(property, constant);
                        case "gt":
                            return Expression.GreaterThan(property, constant);
                        case "gte":
                            return Expression.GreaterThanOrEqual(property, constant);
                        // Add more numeric operators as needed...
                        default:
                            throw new ArgumentException($"Unsupported operator: {filter.Operator}");
                    }
                }
            }
        }

        //public static Expression BuildFilterExpression(Filter filter, ParameterExpression parameter)
        //{
        //    if (filter.Filters != null && filter.Filters.Any())
        //    {
        //        if (filter.Logic?.ToLower() == "and")
        //        {
        //            var andFilters = filter.Filters.Select(f => BuildFilterExpression(f, parameter));
        //            return andFilters.Aggregate(Expression.AndAlso);
        //        }
        //        else if (filter.Logic?.ToLower() == "or")
        //        {
        //            var orFilters = filter.Filters.Select(f => BuildFilterExpression(f, parameter));
        //            return orFilters.Aggregate(Expression.OrElse);
        //        }
        //    }

        //    if (filter.Value == null || string.IsNullOrWhiteSpace(filter.Value.ToString()))
        //        return null;

        //    var property = Expression.Property(parameter, filter.Field);
        //    var constant = Expression.Constant(filter.Value);

        //    switch (filter.Operator.ToLower())
        //    {
        //        case "eq":
        //            return Expression.Equal(property, constant);
        //        case "neq":
        //            return Expression.NotEqual(property, constant);
        //        case "lt":
        //            return Expression.LessThan(property, constant);
        //        case "lte":
        //            return Expression.LessThanOrEqual(property, constant);
        //        case "gt":
        //            return Expression.GreaterThan(property, constant);
        //        case "gte":
        //            return Expression.GreaterThanOrEqual(property, constant);
        //        case "contains":
        //            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        //            return Expression.Call(property, containsMethod, constant);
        //        case "startswith":
        //            var startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string), typeof(StringComparison) });

        //            // Convert the constant value to lowercase for case-insensitive comparison
        //            var constantLower = Expression.Call(constant, typeof(string).GetMethod("ToLower", Type.EmptyTypes));

        //            return Expression.Call(property, startsWithMethod, constantLower, Expression.Constant(StringComparison.OrdinalIgnoreCase));

        //        // Add more operators as needed...
        //        default:
        //            throw new ArgumentException($"Unsupported operator: {filter.Operator}");
        //    }
        //}
        public static Expression<Func<T, bool>> GetAndFilterExpression(List<Filter> filters)
        {
            if (filters == null || !filters.Any())
                return null;

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression andExpression = null;

            foreach (var filter in filters)
            {
                var filterExpression = BuildFilterExpression(filter, parameter);
                if (filterExpression != null)
                {
                    if (andExpression == null)
                    {
                        andExpression = filterExpression;
                    }
                    else
                    {
                        andExpression = Expression.AndAlso(andExpression, filterExpression);
                    }
                }
            }

            if (andExpression == null)
            {
                // Return default expression that always evaluates to false
                andExpression = Expression.Constant(false);
            }

            return Expression.Lambda<Func<T, bool>>(andExpression, parameter);
        }
        public static Expression<Func<T, bool>> GetOrFilterExpression(List<Filter> filters)
        {
            if (filters == null || !filters.Any())
                return null;

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression orExpression = null;

            foreach (var filter in filters)
            {
                var filterExpression = BuildFilterExpression(filter, parameter);
                if (filterExpression != null)
                {
                    if (orExpression == null)
                    {
                        orExpression = filterExpression;
                    }
                    else
                    {
                        orExpression = Expression.OrElse(orExpression, filterExpression);
                    }
                }
            }

            if (orExpression == null)
            {
                // Return default expression that always evaluates to false
                orExpression = Expression.Constant(false);
                orExpression = Expression.Constant(false);
            }

            return Expression.Lambda<Func<T, bool>>(orExpression, parameter);
        }
        public static Expression<Func<T, bool>>? ApplyFilter(RootFilter filter)
        {
            if (filter == null || filter.Filters == null || !filter.Filters.Any())
                return null;

            Expression<Func<T, bool>> compositeFilterExpression = null;

            if (filter.Logic?.ToLower() == "and")
            {
                compositeFilterExpression = GetAndFilterExpression(filter.Filters);
            }
            else if (filter.Logic?.ToLower() == "or")
            {
                compositeFilterExpression = GetOrFilterExpression(filter.Filters);
            }
            return compositeFilterExpression;
        }
    }

}
