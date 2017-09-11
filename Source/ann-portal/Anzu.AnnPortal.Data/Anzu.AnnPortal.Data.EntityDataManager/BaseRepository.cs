using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Anzu.AnnPortal.Data.Repository;
using System.Linq;
using System.Linq.Dynamic;


namespace Anzu.AnnPortal.Data.EntityDataManager
{ 
    public class BaseRepository : IRepository
    {
        AnnDbContext context;// = new AveraContext();

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository"/> class.
        /// </summary>
        public BaseRepository()
        {
            context = new AnnDbContext();
            //Debug.WriteLine("this.context = new AveraContext(); called by {0}", Thread.CurrentThread.ManagedThreadId);
        }

        /// <summary>
        /// Gets all query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IQueryable<T> GetAllQuery<T>() where T : class
        {
            return context.Set<T>();
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// All Data.
        /// </returns>
        public IList<T> GetAll<T>() where T : class
        {
            return context.Set<T>().ToList();
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// All Data.
        /// </returns>
        public IList<T> GetAll<T>(params string[] includes) where T : class
        {

            var results = context.Set<T>();
            return includes.Aggregate<string, DbSet<T>>(results, (current, inc) => (DbSet<T>)current.Include(inc)).ToList();
        }

        /// <summary>
        /// Inserts the specified instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// Instance.
        /// </returns>
        public T Insert<T>(T instance) where T : class
        {
            instance = UpdateAuditFields(instance);
            context.Set<T>().Add(instance);
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                //Added to tackle entity validation errors.
                throw new ApplicationException("Database update exception", ex);
            }
            return instance;
        }

        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// Instance.
        /// </returns>
        public T Update<T>(T instance, bool commit = true) where T : class
        {

            // This code dynamically find the matching object from the context 
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            ObjectSet<T> set = objectContext.CreateObjectSet<T>();
            IList<string> keyNames = set.EntitySet.ElementType
                                                        .KeyMembers
                                                        .Select(k => k.Name).ToList();

            object[] parameters = new object[keyNames.Count];
            string dynamicPredicate = string.Format("{0} == @0", keyNames[0]);
            parameters[0] = instance.GetType().GetProperty(keyNames[0]).GetValue(instance);

            if (keyNames.Count > 1)
            {
                for (int i = 1; i < keyNames.Count; i++)
                {
                    dynamicPredicate = string.Format("{0} AND {1} == @{2}", dynamicPredicate, keyNames[i], i);
                    parameters[i] = instance.GetType().GetProperty(keyNames[i]).GetValue(instance);
                }
            }

            T currentInstance = context.Set<T>().Where(dynamicPredicate, parameters).FirstOrDefault();
            //if (currentInstance == null)
            //{
            //    string keys = "";
            //    foreach (var item in parameters)
            //    {
            //        keys += item.ToString() + " ";
            //    }

            //    string message = string.Format("Predicate: {0} Keys:{1}",dynamicPredicate,keys);
            //    Debug.WriteLine(message);

            //    throw new ApplicationException(string.Format("Entity {0} not found for update. {1}",typeof(T).FullName,message));
            //}

            // Update the object in current database context            
            instance = UpdateAuditFields(instance);
            context.Entry(currentInstance).CurrentValues.SetValues(instance);
            if (commit)
            {
                context.SaveChanges();
            }
            return currentInstance;

        }

        /// <summary>
        /// Deletes the specified predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        public void Delete<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class
        {

            List<T> instance = context.Set<T>().Where(predicate).ToList();

            if (instance.Any())
            {
                foreach (var item in instance)
                {
                    if (item != null)
                    {
                        context.Set<T>().Remove(item);
                        context.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Finds the specified predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// Predicate.
        /// </returns>
        public IList<T> Find<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class
        {
            return context.Set<T>().Where(predicate).ToList();
        }

        /// <summary>
        /// Finds the specified predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// Predicate.
        /// </returns>
        public T FindOne<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class
        {
            var results = context.Set<T>().Where(predicate).Take(1).ToList();
            return results.Count > 0 ? results[0] : null;
        }

        /// <summary>
        /// Finds the top n.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public IList<T> FindTopN<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate, int count = 0) where T : class
        {
            if (count < 1)
            {
                count = Convert.ToInt32(ConfigurationManager.AppSettings["MaxRecordLimit"]);
            }
            return context.Set<T>().Where(predicate).Take(count).ToList();
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="sorts">The sorts.</param>
        /// <param name="filters">The filters.</param>
        /// <returns>
        /// All Data.
        /// </returns>
        public IEnumerable<T> GetAll<T>(int page, int pageSize, List<string> sorts, List<KeyValuePair<string, string>> filters) where T : class
        { 
            IQueryable<T> results = context.Set<T>().OrderBy("Id ASC");

            if (sorts.Any())
            {
                string orderby = string.Empty;
                foreach (string sort in sorts)
                {
                    orderby += string.Format("{0},", sort);
                }

                orderby = orderby.Substring(0, orderby.Length - 1);

                results = results.OrderBy(orderby);
            }

            if (filters.Any())
            {
                string where = string.Empty;
                object[] parameters = new object[filters.Count];

                for (int i = 0; i < filters.Count; i++)
                {
                    if (i == 0)
                    {
                        where += string.Format("{0}", filters[i].Key);
                    }
                    else
                    {
                        where += string.Format(" OR {0}", filters[i].Key);
                    }

                    parameters[i] = filters[i].Value;
                }

                results = results.Where(where, parameters);
            }

            results = results.Skip((page - 1) * pageSize).Take(pageSize);

            return results.ToList();
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="sorts">The sorts.</param>
        /// <param name="filters">The filters.</param>
        /// <returns>
        /// All Data.
        /// </returns>
        public IEnumerable<T> GetAllToPage<T>(int take, int skip, int page, int pageSize) where T : class
        {

            IQueryable<T> results = context.Set<T>().OrderBy("Id DESC");


            //if (sorts.Any())
            //{
            //    string orderby = string.Empty;
            //    foreach (string sort in sorts)
            //    {
            //        orderby += string.Format("{0},", sort);
            //    }

            //    orderby = orderby.Substring(0, orderby.Length - 1);

            //    results = results.OrderBy(orderby);
            //}


            //results = results.Skip((page - 1) * pageSize).Take(pageSize);
            results = results.Skip(skip).Take(take);

            return results.ToList();
        }


        /// <summary>
        /// Inserts the specified instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">Database update exception</exception>
        public IList<T> Insert<T>(IList<T> instance) where T : class
        {
            if (instance != null)
            {
                foreach (object item in instance)
                {
                    UpdateAuditFields(item);
                }
            }

            context.Set<T>().AddRange(instance);
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                //Added to tackle entity validation errors.
                throw new ApplicationException("Database update exception", ex);
            }
            return instance;
        }

        public IList<T> FastInsert<T>(IList<T> instance) where T : class
        {
            if (instance != null)
            {
                foreach (object item in instance)
                {
                    UpdateAuditFields(item);
                }
            }

            context.Set<T>().AddRange(instance);
            try
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                context.Configuration.ValidateOnSaveEnabled = false;
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                //Added to tackle entity validation errors.
                throw new ApplicationException("Database update exception", ex);
            }
            return instance;
        }

        public void BulkInsert<T>(IList<T> instance) where T : class
        {
            //var options = new BulkInsertOptions
            //{
            //    EnableStreaming = false
            //};
            try
            {
                //context.BulkInsert(instance, options);
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                //Added to tackle entity validation errors.
                throw new ApplicationException("Database update exception", ex);
            }
        }




        public IList<T> Update<T>(IList<T> instance) where T : class
        {
            IList<T> insertedItems = new List<T>();
            int totalCount = instance.Count;
            int count = 0;

            foreach (var item in instance)
            {
                insertedItems.Add(Update(item, count == (totalCount - 1)));
                count++;
            }

            return insertedItems;
        }

        public int ExecuteSqlCommand(string sqlCommand, params object[] parameters)
        {

            return context.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }  

        private T UpdateAuditFields<T>(T instance) where T : class
        {
            try
            {
                // Update BI integration audit properties

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["AutoSetBIIntegrationFields"]))
                {
                    PropertyInfo recordStatus = instance.GetType().GetProperty("RecordStatusId");
                    if (recordStatus != null && recordStatus.GetValue(instance) == null)
                    {
                        recordStatus.SetValue(instance, 1);
                    }

                    PropertyInfo modifiedDate = instance.GetType().GetProperty("ModifiedDate");
                    if (modifiedDate != null)
                    {
                        modifiedDate.SetValue(instance, DateTime.UtcNow);
                    }

                    PropertyInfo createdDate = instance.GetType().GetProperty("CreatedDate");
                    if (createdDate != null && createdDate.GetValue(instance) == null)
                    {
                        createdDate.SetValue(instance, DateTime.UtcNow);
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return instance;
        }


    }
}
