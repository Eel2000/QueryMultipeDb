using Microsoft.EntityFrameworkCore;
using MultiDbQuery.Errors;
using MultiDbQuery.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        public static async Task<IEnumerable<TObject>> GetAll<TObject>(TObject @object, TObject entity) where TObject : class
        {
            var list = new List<TObject>();
            var contexts = GetContexts(@object);

            foreach (var item in contexts)
            {
                var repo = new Repository<DbContext>(item);

                var std = await repo.GetAll(entity);

                list.AddRange(std);
            }

            return list;
        }
    }
}
