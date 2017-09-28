using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using System.ComponentModel;
using Microsoft.Practices.Unity;
using RepositoryComponents.Specifications;
using RepositoryComponents.NamedQuery;

namespace RepositoryComponents.Repository
{
    /// <summary>
    /// Specialized implementation using int as IdT (as it is the most common id-type) 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericRepository<T> : GenericRepository<T, int> where T : class 
    {
        public GenericRepository() : base() { }

        public GenericRepository(IUnityContainer container) : base(container) { }
    }

    /// <summary>
    /// Generic implementation of the CRUD methods using NHibernate
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public class GenericRepository<T, TId> : IGenericRepository<T, TId> where T : class 
    {
        private readonly IUnityContainer _container; 
 
        /// <summary>
        /// Default construtor that set up the repository according to default settings.
        /// </summary>
        public GenericRepository() 
        {
            _container = new UnityContainer();
        }

        /// <summary>
        /// Constructor that can be used to inject dependencies
        /// </summary>
        /// <param name="container"></param>
        public GenericRepository(IUnityContainer container)
        {
            _container = container;
        }
        /// <summary>
        /// The NHibernate session fetched from the NHibernateContext
        /// </summary>
        public virtual ISession Session
        {
            get
            {
                return SessionProvider.GetNewOrCurrentSession();
            }
        }
         
        /// <summary>
        /// Save or insert an entity to the repository
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Save(T entity)
        {
            Session.SaveOrUpdate(entity);
        }

        /// <summary>
        /// Save or insert a list of entities to the repository
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Save(IList<T> entities)
        {
            foreach (T entity in entities)
            {
                Session.SaveOrUpdate(entity);
            }
        }

        /// <summary>
        /// Merge an entity to the repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The merged entity</returns>
        public virtual T Merge(T entity)
        {
            return (T)Session.Merge(entity);
        }

        /// <summary>
        /// Remove one entity from the repository
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Remove(T entity)
        {
            Session.Delete(entity);
        }

        /// <summary>
        /// Remove a list of entities from the repository
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Remove(IList<T> entities)
        {
            foreach (var entity in entities)
            {
                Session.Delete(entity);
            }
        }

        /// <summary>
        /// Synchronize the contents of the firts level cache with the database.
        /// </summary>
        public virtual void Flush()
        {
            Session.Flush();
        }

        /// <summary>
        /// Re-read the state of the entity from the database.
        /// </summary>
        public virtual void Refresh(T entity)
        {
            Session.Refresh(entity);
        }

        /// <summary>
        /// Removes the entity from the session cache.
        /// </summary>
        public virtual void Evict(T entity)
        {
            Session.Evict(entity);
        }


        /// <summary>
        /// Set the read only status of the entities loaded in this session.
        /// </summary>
        /// <param name="readOnly">The boolean value of the read only status</param>
        public virtual void SetSessionReadOnly(bool readOnly)
        {
            Session.DefaultReadOnly = readOnly;
        }

        /// <summary>
        /// Returns the value of the read only status for the current session.
        /// </summary>
        /// <returns>The value of the read only status</returns>
        public virtual bool IsSessionReadOnly { get { return Session.DefaultReadOnly; } }

        /// <summary>
        /// Finds an entity based on its id
        /// Returns null if entity could not be found
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public T Find(TId entityId)
        {
            return Session.Get<T>(entityId);
        }

        /// <summary>
        /// Finds an entity based on its id and locks it until the transaction completes.
        /// Returns null if entity could not be found
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="lockMode"></param>
        /// <returns></returns>
        public T Find(TId entityId, LockMode lockMode)
        {
            return Session.Get<T>(entityId, lockMode);
        }

        /// <summary>
        /// Loads an entity based on its id
        /// Throws an exception if the entity is not found
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public virtual T Load(TId entityId)
        {
            return Session.Load<T>(entityId);
        }

        /// <summary>
        /// Loads an entity based on its id and locks it until the transaction completes.
        /// Throws an exception if the entity is not found
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="lockMode"></param>
        /// <returns></returns>
        public virtual T Load(TId entityId, LockMode lockMode)
        {
            return Session.Load<T>(entityId, lockMode);
        }

        /// <summary>
        /// Looks up and finds the Query Object for a named query.
        /// </summary>
        /// <typeparam name="TNamedQuery">Interface of the query object for the named query</typeparam>
        /// <returns></returns>
        public TNamedQuery GetNamedQuery<TNamedQuery>() where TNamedQuery : INamedQuery
        {
            return _container.Resolve<TNamedQuery>();
        }

        /// <summary>
        /// Generic Find of Type T using Linq.
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> Find()
        {
            return Session.Query<T>();
        }

        /// <summary>
        /// Find all entities for a certain entity type
        /// </summary>
        /// <returns></returns>
        public virtual IList<T> FindAll()
        {
            var spec = new Specification<T>(p => true);
            return Find(spec);
        }

        /// <summary>
        /// Find all entities for a certain entity type
        /// </summary>
        /// <param name="maxNumberOfRecordsToReturn">Max number of records to return</param>
        /// <returns></returns>
        public virtual IList<T> FindAll(int maxNumberOfRecordsToReturn)
        {
            var criteria = Session.CreateCriteria(typeof(T));
            criteria.SetMaxResults(maxNumberOfRecordsToReturn);
            return criteria.List<T>();
        }

        /// <summary>
        /// With a Specification we split the logic of how a selection is made, away from the thing we are selecting.
        /// So, with this method we open the possibility to find a list of objects based on a specific specification.
        /// This method will return a single unique entity.
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        public virtual T FindUnique(AbstractSpecification<T> specification)
        {
            return BuildQuery(specification).Single<T>();
        }

        /// <summary>
        /// With a Specification we split the logic of how a selection is made, away from the thing we are selecting.
        /// So, with this method we open the possibility to find a list of objects based on a specific specification
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        public virtual IList<T> Find(AbstractSpecification<T> specification)
        {
            return BuildQuery(specification).ToList();
        }

        /// <summary>
        /// Finds/Search with filter and paging.
        /// </summary>
        /// <param name="specification">The filter specification.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public virtual IList<T> Find(AbstractSpecification<T> specification, int pageNumber, int pageSize)
        {
            return BuildQuery(specification, pageNumber, pageSize).ToList();
        }

        /// <summary>
        /// Finds/Search with filter and sorting.
        /// </summary>
        /// <param name="specification">The filter specification.</param>
        /// <param name="sortSpecificationList">The sort specification list.</param>
        /// <returns></returns>
        public IList<T> Find(AbstractSpecification<T> specification, IEnumerable<SortSpecification<T>> sortSpecificationList)
        {
            return BuildQuery(specification, sortSpecificationList).ToList();
        }

        /// <summary>
        /// Finds/Search with filter, sort and paging.
        /// </summary>
        /// <param name="specification">The filter specification.</param>
        /// <param name="sortSpecificationList">The sort specification list.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public IList<T> Find(AbstractSpecification<T> specification, IEnumerable<SortSpecification<T>> sortSpecificationList, int pageNumber, int pageSize)
        {
            return BuildQuery(specification, sortSpecificationList, pageNumber, pageSize).ToList();
        }

        /// <summary>
        /// Finds/Search with filter, sort, paging and grouping.
        /// </summary>
        /// <typeparam name="TGroup">The type of the group.</typeparam>
        /// <param name="specification">The filter specification.</param>
        /// <param name="sortSpecificationList">The sort specification list.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="groupSpecification">The group specification.</param>
        /// <returns></returns>
        public IList<TGroup> Find<TGroup>(AbstractSpecification<T> specification, IEnumerable<SortSpecification<T>> sortSpecificationList, int pageNumber, int pageSize, GroupSpecification<T, TGroup> groupSpecification)
        {
            if (groupSpecification == null) return null;
            var data = BuildQuery(specification, sortSpecificationList, pageNumber, pageSize).ToList();
            return groupSpecification.Predicate.Compile().Invoke(data).ToList();
        }
        /// <summary>
        /// Counts and returns the number of entities
        /// </summary>
        /// <returns>The number of entities</returns>
        public int Count()
        {
            return Find().Count();
        }
        /// <summary>
        /// Counts and returns the number of entities that fullfills the specification
        /// </summary>
        /// <param name="filterSpecification"></param>
        /// <returns>The number of entities</returns>
        public int Count(Specification<T> filterSpecification)
        {
            return Find().Count(filterSpecification.Predicate);
        }

        protected virtual IQueryable<T> Query()
        {
            return Session.Query<T>();
        }

        #region Private Helper Methods
        private IQueryable<T> BuildQuery(AbstractSpecification<T> specification)
        {
            IQueryable<T> data = Session.Query<T>();
            data = ApplyFilterSpecification(data, specification);
            return data;
        }

        private IQueryable<T> BuildQuery(AbstractSpecification<T> specification, int pageNumber, int pageSize)
        {
            IQueryable<T> data = Session.Query<T>();
            data = ApplyFilterSpecification(data, specification);
            data = ApplyPaging(data, pageNumber, pageSize);
            return data;
        }

        private IQueryable<T> BuildQuery(AbstractSpecification<T> specification, IEnumerable<SortSpecification<T>> sortSpecificationList)
        {
            IQueryable<T> data = Session.Query<T>();
            data = ApplyFilterSpecification(data, specification);
            data = ApplySortSpecification(data, sortSpecificationList);
            return data;
        }

        protected IQueryable<T> BuildQuery(AbstractSpecification<T> specification, IEnumerable<SortSpecification<T>> sortSpecificationList, int pageNumber, int pageSize)
        {
            IQueryable<T> data = Session.Query<T>();
            data = ApplyFilterSpecification(data, specification);
            data = ApplySortSpecification(data, sortSpecificationList);
            data = ApplyPaging(data, pageNumber, pageSize);
            return data;
        }

        private IQueryable<T> ApplyFilterSpecification(IQueryable<T> data, AbstractSpecification<T> filterSpecification)
        {
            if (filterSpecification != null)
                data = data.Where(filterSpecification.IsSatisfiedBy());
            return data;
        }

        private IQueryable<T> ApplyPaging(IQueryable<T> data, int pageNumber, int pageSize)
        {
            if (pageNumber >= 0 && pageSize > 0)
                data = data.Skip((pageNumber) * pageSize).Take(pageSize);
            return data;
        }

        private IQueryable<T> ApplySortSpecification(IQueryable<T> data, IEnumerable<SortSpecification<T>> sortSpecificationList)
        {
            if (sortSpecificationList != null && data.Count() > 0)
            {
                foreach (var sortSpecification in sortSpecificationList)
                {
                    switch (sortSpecification.Direction)
                    {
                        case ListSortDirection.Ascending:
                            data = data.OrderBy(sortSpecification.Predicate);
                            break;
                        case ListSortDirection.Descending:
                            data = data.OrderByDescending(sortSpecification.Predicate);
                            break;
                    }
                }
            }

            return data;
        }
        #endregion
    }
}
