using System;
using System.Collections.Generic;
using System.Linq; 
using System.Linq.Expressions; 

namespace Anzu.AnnPortal.Data.Repository
{
    /// <summary>
    /// Interface IRepository.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Gets all query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IQueryable<T> GetAllQuery<T>() where T : class;

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>All Data.</returns>
        IList<T> GetAll<T>() where T : class;

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="includes">The includes.</param>
        /// <returns>All Data.</returns>
        IList<T> GetAll<T>(params string[] includes) where T : class;

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="sorts">The sorts.</param>
        /// <param name="filters">The filters.</param>
        /// <returns>All Data.</returns>
        IEnumerable<T> GetAll<T>(int page, int pageSize, List<string> sorts, List<KeyValuePair<string, string>> filters) where T : class;

        /// <summary>
        /// Inserts the specified instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>Instance.</returns>
        T Insert<T>(T instance) where T : class;

        /// <summary>
        /// Inserts the specified instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        IList<T> Insert<T>(IList<T> instance) where T : class;

        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>Instance.</returns>
        T Update<T>(T instance, bool commit = true) where T : class;

        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        IList<T> Update<T>(IList<T> instance) where T : class;

        /// <summary>
        /// Deletes the specified predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        void Delete<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// Finds the specified predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Predicate.</returns>
        IList<T> Find<T>(Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// Finds the specified predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Predicate.</returns>
        T FindOne<T>(Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// Finds the top n.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        IList<T> FindTopN<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate, int count = 0) where T : class;

        /// <summary>
        /// Gets all for given page.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="sorts">The sorts.</param>
        /// <param name="filters">The filters.</param>
        /// <returns>All Data.</returns>
        IEnumerable<T> GetAllToPage<T>(int take, int skip, int page, int pageSize) where T : class;

        int ExecuteSqlCommand(string sqlCommand, params object[] parameters);


        /// <summary>
        /// Bulks insert.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        void BulkInsert<T>(IList<T> instance) where T : class;

        /// <summary>
        /// Fasts the insert.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        IList<T> FastInsert<T>(IList<T> instance) where T : class;

        /// <summary>
        /// Takes the specified no of item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="noOfItem">The no of item.</param>
        /// <param name="isDesc">if set to <c>true</c> [is desc].</param>
        /// <returns></returns>
        IEnumerable<T> Take<T>(int noOfItem, bool isDesc) where T : class;
    }
}
