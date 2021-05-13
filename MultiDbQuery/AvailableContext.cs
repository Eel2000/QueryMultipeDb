using Microsoft.EntityFrameworkCore;
using MultiDbQuery.Errors;
using MultiDbQuery.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace MultiDbQuery
{
    public static class AvailableContext
    {
        public static int propertiesNumber { get; set; }
        private static PropertyInfo[] propertyInfo { get; set; }
        private static List<DbContext> _dbContexts { get; set; } = new List<DbContext>();
        public static IEnumerable<DbContext> GetContexts<TObject>(TObject @object)
        {
            propertyInfo = @object.GetType().GetProperties();
            propertiesNumber = propertyInfo.Length;

            if (propertiesNumber < 0)
                throw new ReadPropertiesException();

            foreach (var item in propertyInfo)
            {
                if (item.Name.Contains("Context") || item.Name.Contains("db") ||
                    item.Name.Contains("context"))
                {
                    var context = @object.GetType()
                        .GetProperties()
                        .Where(x => x.Name == item.Name)
                        .FirstOrDefault();

                    var c = (Object)context?.GetValue(@object);

                    _dbContexts.Add((DbContext)c);
                }
            }
            return _dbContexts;
        }

        public static async Task<IEnumerable<TEntity>> Find<TObject,TEntity>(TObject @object, TEntity entity,Expression<Func<TEntity,bool>> expression) where TObject : class where TEntity:class,new()
        {
            var contexts = GetContexts(@object).Distinct();
            List<TEntity> entities = new List<TEntity>();

            foreach (var item in contexts)
            {
                var repo = item.Set<TEntity>().Where(expression);
                entities.AddRange(repo);
            }
            return entities;
        }

        public static async Task<IEnumerable<TEntity>> GetAll<TObject, TEntity>(TObject @object, TEntity entity) where TObject : class where TEntity : class, new()
        {
            var contexts = GetContexts(@object).Distinct();
            List<TEntity> entities = new List<TEntity>();

            foreach (var item in contexts)
            {
                var repo = item.Set<TEntity>().ToHashSet();
                entities.AddRange(repo);
            }
            return entities;
        }

        public static async Task<IEnumerable<TEntity>> FirstOrDefaultAsync<TObject,TEntity>(TObject @object,TEntity entity, Expression<Func<TEntity,bool>> expression)
            where TObject:class where TEntity : class, new()
        {
            var contexts = GetContexts(@object).Distinct();

            List<TEntity> entities = new List<TEntity>();

            foreach (var context in contexts)
            {
                var std = await context.Set<TEntity>().Where(expression).FirstOrDefaultAsync();
                entities.Add(std);
            }

            return entities;
        }
    }
}
