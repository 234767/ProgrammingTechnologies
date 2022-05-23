using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Database.Repositories
{
    internal abstract class RepositoryBase<TAbstraction, TDto, TRecord> : IRepository<TAbstraction> where TRecord : TAbstraction where TDto : class, TAbstraction
    {
        protected readonly LibraryDataContext dbContext;
        protected readonly DbSet<TDto> dbSet;

        protected RepositoryBase( DbSet<TDto> dbSet, LibraryDataContext dbContext )
        {
            this.dbContext = dbContext;
            this.dbSet = dbSet;
        }

        [return: NotNullIfNotNull("src")]
        protected abstract TDto? MapToDto( TAbstraction? src );

        [return: NotNullIfNotNull("src")]
        protected abstract TRecord? MapToResult( TAbstraction? src );

        protected async Task SaveChanges()
        {
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    await ex.Entries.Single().ReloadAsync();
                }
            } while (saveFailed);
        }

        protected virtual IQueryable<TDto> LoadRelations(IQueryable<TDto> data)
        {
            return data;
        }

        public virtual async Task CreateAsync( TAbstraction user )
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            string id = user switch
            {
                IUser u   => u.Id,
                IBook b   => b.Id,
                ILease l  => l.Id,
                IReturn r => r.Id,
                _         => throw new InvalidOperationException( $"Cannot have repository of type {user.GetType()}" )
            };

            TDto? existingUser = await dbSet.FindAsync( id );
            if ( existingUser is not null )
            {
                throw new InvalidOperationException( "Entity with such Id already exists" );
            }

            TDto dto = MapToDto( user );
            await dbSet.AddAsync( dto );
            await SaveChanges();
        }

        public virtual async Task<TAbstraction?> GetAsync( string id )
        {
            return MapToResult(await dbSet.FindAsync( id ));
        }

        public virtual async Task<IEnumerable<TAbstraction>> WhereAsync( Expression<Func<TAbstraction, bool>> predicate )
        {
            IEnumerable<TAbstraction> result;
            try
            {
                result = await LoadRelations(dbSet).AsNoTracking().Where(predicate).ToListAsync();
            }
            catch ( InvalidOperationException )
            {
                // Could not convert Linq to SQL
                result = ( await LoadRelations(dbSet).ToListAsync() ).Where( predicate.Compile() );
            }

            return (IEnumerable<TAbstraction>)result.Select( MapToResult );
        }

        public abstract Task UpdateAsync( TAbstraction item );

        public virtual async Task DeleteAsync( string id )
        {
            TDto? entityToRemove = await dbSet.FindAsync( id );
            if ( entityToRemove is not null )
            {
                dbSet.Remove(entityToRemove);
                await SaveChanges();
                await OnSuccessfulDelete();
            }
        }

        protected virtual Task OnSuccessfulDelete()
        {
            return Task.CompletedTask;
        }

        public virtual async Task<IEnumerable<TAbstraction>> GetAllAsync()
        {
            var results = await LoadRelations(dbSet).ToListAsync();
            return (IEnumerable<TAbstraction>)results.Select(MapToResult).ToList();
        }
    }
}
