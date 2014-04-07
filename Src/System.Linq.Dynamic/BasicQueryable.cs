﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace System.Linq.Dynamic
{
    /// <summary>
    /// Provides a set of static (Shared in Visual Basic) methods for querying data 
    /// structures that implement <see cref="IQueryable"/>. It adds basic methods to <see cref="IQueryable"/> that would
    /// normally require <see cref="IQueryable{T}"/>.
    /// </summary>
    public static class BasicQueryable
    {
        /// <summary>
        /// Returns a specified number of contiguous elements from the start of a sequence.
        /// </summary>
        /// <param name="source">The sequence to return elements from.</param>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>A <see cref="IQueryable"/> that contains the specified number of elements from the start of source.</returns>
        public static IQueryable Take(this IQueryable source, int count)
        {
            Validate.Argument(source, "source").IsNotNull().Check()
                    .Argument(count, "count").IsInRange(x => x > 0).Check();

            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable), "Take",
                    new Type[] { source.ElementType },
                    source.Expression, Expression.Constant(count)));
        }

        /// <summary>
        /// Bypasses a specified number of elements in a sequence and then returns the remaining elements.
        /// </summary>
        /// <param name="source">A <see cref="IQueryable"/> to return elements from.</param>
        /// <param name="count">The number of elements to skip before returning the remaining elements.</param>
        /// <returns>A <see cref="IQueryable"/> that contains elements that occur after the specified index in the input sequence.</returns>
        public static IQueryable Skip(this IQueryable source, int count)
        {
            Validate.Argument(source, "source").IsNotNull().Check()
                    .Argument(count, "count").IsInRange(x => x >= 0).Check();

            //no need to skip if count is zero
            if (count == 0) return source;

            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable), "Skip",
                    new Type[] { source.ElementType },
                    source.Expression, Expression.Constant(count)));
        }

        /// <summary>
        /// Determines whether a sequence contains any elements.
        /// </summary>
        /// <param name="source">A sequence to check for being empty.</param>
        /// <returns>true if the source sequence contains any elements; otherwise, false.</returns>
        public static bool Any(this IQueryable source)
        {
            Validate.Argument(source, "source").IsNotNull().Check();

            return (bool)source.Provider.Execute(
                Expression.Call(
                    typeof(Queryable), "Any",
                    new Type[] { source.ElementType }, source.Expression));
        }

        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>
        /// <param name="source">The <see cref="IQueryable"/> that contains the elements to be counted.</param>
        /// <returns>The number of elements in the input sequence.</returns>
        public static int Count(this IQueryable source)
        {
            Validate.Argument(source, "source").IsNotNull().Check();

            return (int)source.Provider.Execute(
                Expression.Call(
                    typeof(Queryable), "Count",
                    new Type[] { source.ElementType }, source.Expression));
        }

        /// <summary>
        /// Returns the only element of a sequence, and throws an exception if there
        /// is not exactly one element in the sequence.
        /// </summary>
        /// <param name="source">A <see cref="IQueryable"/> to return the single element of.</param>
        /// <returns>The single element of the input sequence.</returns>
        public static object Single(this IQueryable source)
        {
            Validate.Argument(source, "source").IsNotNull().Check();

            return source.Provider.Execute(Expression.Call(
                typeof(Queryable), "Single",
                new Type[] { source.ElementType }, source.Expression));
        }

        /// <summary>
        /// Returns the only element of a sequence, or a default value if the sequence
        /// is empty; this method throws an exception if there is more than one element
        /// in the sequence.
        /// </summary>
        /// <param name="source">A <see cref="IQueryable"/> to return the single element of.</param>
        /// <returns>The single element of the input sequence, or default(TSource) if the sequence contains no elements.</returns>
        public static object SingleOrDefault(this IQueryable source)
        {
            Validate.Argument(source, "source").IsNotNull().Check();

            return source.Provider.Execute(Expression.Call(
                typeof(Queryable), "SingleOrDefault",
                new Type[] { source.ElementType }, source.Expression));
        }

        /// <summary>
        /// Returns the first element of a sequence.
        /// </summary>
        /// <param name="source">The <see cref="IQueryable"/> to return the first element of.</param>
        /// <returns>The first element in source.</returns>
        public static object First(this IQueryable source)
        {
            Validate.Argument(source, "source").IsNotNull().Check();

            return source.Provider.Execute(Expression.Call(
                typeof(Queryable), "First",
                new Type[] { source.ElementType }, source.Expression));
        }

        /// <summary>
        /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
        /// </summary>
        /// <param name="source">The <see cref="IQueryable"/> to return the first element of.</param>
        /// <returns>default(TSource) if source is empty; otherwise, the first element in source.</returns>
        public static object FirstOrDefault(this IQueryable source)
        {
            Validate.Argument(source, "source").IsNotNull().Check();

            return source.Provider.Execute(Expression.Call(
                typeof(Queryable), "FirstOrDefault",
                new Type[] { source.ElementType }, source.Expression));
        }

        /// <summary>
        /// Inverts the order of the elements in a sequence.
        /// </summary>
        /// <param name="source">A sequence of values to reverse.</param>
        /// <returns>A <see cref="IQueryable"/> whose elements correspond to those of the input sequence in reverse order.</returns>
        public static IQueryable Reverse(this IQueryable source)
        {
            Validate.Argument(source, "source").IsNotNull().Check();

            return source.Provider.CreateQuery(Expression.Call(
                typeof(Queryable), "Reverse",
                new Type[] { source.ElementType }, source.Expression));
        }

        #region As Dynamics
#if !NET35

        /// <summary>
        /// Returns the only element of a sequence as a dynamic object, and throws an exception if there
        /// is not exactly one element in the sequence.
        /// </summary>
        /// <param name="source">A <see cref="IQueryable"/> to return the single element of.</param>
        /// <returns>The single element of the input sequence.</returns>
        public static dynamic SingleDynamic(this IQueryable source)
        {
            Validate.Argument(source, "source").IsNotNull().Check();

            return (dynamic)Single(source);
        }

        /// <summary>
        /// Returns the only element of a sequence as a dynamic object, or a default value if the sequence
        /// is empty; this method throws an exception if there is more than one element
        /// in the sequence.
        /// </summary>
        /// <param name="source">A <see cref="IQueryable"/> to return the single element of.</param>
        /// <returns>The single element of the input sequence, or default(TSource) if the sequence contains no elements.</returns>
        public static dynamic SingleOrDefaultDynamic(this IQueryable source)
        {
            Validate.Argument(source, "source").IsNotNull().Check();

            return (dynamic)SingleOrDefault(source);
        }

        /// <summary>
        /// Returns the first element of a sequence as a dynamic object.
        /// </summary>
        /// <param name="source">The <see cref="IQueryable"/> to return the first element of.</param>
        /// <returns>The first element in source.</returns>
        public static dynamic FirstDynamic(this IQueryable source)
        {
            Validate.Argument(source, "source").IsNotNull().Check();

            return (dynamic)First(source);
        }

        /// <summary>
        /// Returns the first element of a sequence as a dynamic object, or a default value if the sequence contains no elements.
        /// </summary>
        /// <param name="source">The <see cref="IQueryable"/> to return the first element of.</param>
        /// <returns>default(TSource) if source is empty; otherwise, the first element in source.</returns>
        public static dynamic FirstOrDefaultDynamic(this IQueryable source)
        {
            Validate.Argument(source, "source").IsNotNull().Check();

            return (dynamic)FirstOrDefault(source);
        }

        /// <summary>
        /// Converts the result of a <see cref="IQueryable"/> to a <see cref="IEnumerable{T}"/> of dynamic objects.
        /// </summary>
        /// <param name="source">The <see cref="IQueryable"/> to convert.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> of dynamic objects.</returns>
        public static IEnumerable<dynamic> AsEnumerableDynamic(this IQueryable source)
        {
            Validate.Argument(source, "source").IsNotNull().Check();

            foreach (var row in source) yield return row;
        }


#endif
        #endregion

    }
}