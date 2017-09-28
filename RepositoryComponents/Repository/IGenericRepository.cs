using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using RepositoryComponents.NamedQuery;
using RepositoryComponents.Specifications;

namespace RepositoryComponents.Repository
{
    /// <summary>
    /// Interface of Repository implementing 
    /// CRUD methods (Create, Retreive, Update, Delete)
    /// </summary>
    /// <typeparam name="T">type to be persisted</typeparam>
    /// <typeparam name="TId">type of the identifer property of <see cref="T"/></typeparam>
    public interface IGenericRepository<T, TId> : INamedQueryFactory where T : class
    {
        /// <summary>
        /// The NHibernate session fetched from the NHibernateContext
        /// </summary>
        ISession Session { get; }
        /// <summary>
        /// Save or insert an entity to the repository
        /// </summary>
        /// <param name="entity"></param>
        void Save(T entity);
        /// <summary>
        /// Save or insert a list of entities to the repository
        /// </summary>
        /// <param name="entities"></param>
        void Save(IList<T> entities);
        /// <summary>
        /// Merge an entity to the repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The merged entity</returns>
        T Merge(T entity);
        /// <summary>
        /// Remove one entity from the repository
        /// </summary>
        /// <param name="entity"></param>
        void Remove(T entity);
        /// <summary>
        /// Remove a list of entities from the repository
        /// </summary>
        /// <param name="entities"></param>
        void Remove(IList<T> entities);

        /// <summary>
        /// Synchronize the contents of the firts level cache with the database.
        /// </summary>
        void Flush();
        /// <summary>
        /// Re-read the state of the entity from the database.
        /// </summary>
        void Refresh(T entity);
        /// <summary>
        /// Removes the entity from the session cache.
        /// </summary>
        void Evict(T entity);

        /// <summary>
        /// Set the read only status of the entities loaded in this session.
        /// </summary>
        /// <param name="readOnly">The boolean value of the read only status</param>
        void SetSessionReadOnly(bool readOnly);
        /// <summary>
        /// Returns the value of the read only status for the current session.
        /// </summary>
        /// <returns>The value of the read only status</returns>
        bool IsSessionReadOnly { get; }

        /// <summary>
        /// Finds an entity based on its id
        /// Returns null if entity could not be found
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        T Find(TId entityId);
        /// <summary>
        /// Finds an entity based on its id and locks it until the transaction completes.
        /// Returns null if entity could not be found
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="lockMode"></param>
        /// <returns></returns>
        T Find(TId entityId, LockMode lockMode);
        /// <summary>
        /// Loads an entity based on its id
        /// Throws an exception if the entity is not found
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        T Load(TId entityId);
        /// <summary>
        /// Loads an entity based on its id and locks it until the transaction completes.
        /// Throws an exception if the entity is not found
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="lockMode"></param>
        /// <returns></returns>
        T Load(TId entityId, LockMode lockMode);

        /// <summary>
        /// Generic Find of Type T using Linq.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> Find();
        /// <summary>
        /// Find all entities for a certain entity type
        /// </summary>
        /// <returns></returns>
        IList<T> FindAll();
        /// <summary>
        /// Find all entities for a certain entity type
        /// </summary>
        /// <param name="maxNumberOfRecordsToReturn">Max number of records to return</param>
        /// <returns></returns>
        IList<T> FindAll(int maxNumberOfRecordsToReturn);
        /// <summary>
        /// With a Specification we split the logic of how a selection is made, away from the thing we are selecting.
        /// So, with this method we open the possibility to find a list of objects based on a specific specification.
        /// This method will return a single unique entity.
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        T FindUnique(AbstractSpecification<T> specification);
        /// <summary>
        /// With a Specification we split the logic of how a selection is made, away from the thing we are selecting.
        /// So, with this method we open the possibility to find a list of objects based on a specific specification
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        IList<T> Find(AbstractSpecification<T> specification);
        /// <summary>
        /// Finds/Search with filter and paging.
        /// </summary>
        /// <param name="specification">The filter specification.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        IList<T> Find(AbstractSpecification<T> specification, int pageNumber, int pageSize);
        /// <summary>
        /// Finds/Search with filter and sorting.
        /// </summary>
        /// <param name="specification">The filter specification.</param>
        /// <param name="sortSpecificationList">The sort specification list.</param>
        /// <returns></returns>
        IList<T> Find(AbstractSpecification<T> specification, IEnumerable<SortSpecification<T>> sortSpecificationList);
        /// <summary>
        /// Finds/Search with filter, sort and paging.
        /// </summary>
        /// <param name="specification">The filter specification.</param>
        /// <param name="sortSpecificationList">The sort specification list.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        IList<T> Find(AbstractSpecification<T> specification, IEnumerable<SortSpecification<T>> sortSpecificationList, int pageNumber, int pageSize);
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
        IList<TGroup> Find<TGroup>(AbstractSpecification<T> specification, IEnumerable<SortSpecification<T>> sortSpecificationList, int pageNumber, int pageSize, GroupSpecification<T, TGroup> groupSpecification);
        /// <summary>
        /// Counts and returns the number of entities
        /// </summary>
        /// <returns>The number of entities</returns>
        int Count();
        /// <summary>
        /// Counts and returns the number of entities that fullfills the specification
        /// </summary>
        /// <param name="filterSpecification"></param>
        /// <returns>The number of entities</returns>
        int Count(Specification<T> filterSpecification);
    }
    /// <summary>
    /// Specialized interface using int as IdT (as it is the most common id-type) 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericRepository<T> : IGenericRepository<T, int> where T : class { };
}
