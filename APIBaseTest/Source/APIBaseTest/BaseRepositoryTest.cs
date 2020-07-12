using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Microsoft.EntityFrameworkCore;

namespace APIBaseTest
{
    public abstract class BaseRepositoryTest<TSystem, TContext> : BaseTest<TSystem>
        where TSystem : class
        where TContext : DbContext
    {
        protected BaseRepositoryTest()
        {
            ClearTheDefaultDb();
        }

        protected DbContextOptions<TContext> GivenTheDefaultDbContextOptions()
        {
            var options = new DbContextOptionsBuilder<TContext>()
                .UseInMemoryDatabase("UnitTestDb")
                .Options;

            return options;
        }

        protected TContext GivenTheDefaultDbContext()
        {
            var options = GivenTheDefaultDbContextOptions();

            var contextObject = Activator.CreateInstance(typeof(TContext), options);
            var context = (TContext)contextObject;

            return context;
        }

        protected void ClearTheDefaultDb()
        {
            var context = GivenTheDefaultDbContext();

            context.Database.EnsureDeleted();
        }

        protected void AndIAddTableData<TEntity>(TEntity record) where TEntity : class
        {
            var context = GivenTheDefaultDbContext();

            context.Add(record);
            context.SaveChanges();
        }

        protected void AndIAddRangeTableData<TEntity>(List<TEntity> records) where TEntity : class
        {
            var context = GivenTheDefaultDbContext();

            context.AddRange(records);
            context.SaveChanges();
        }

        protected List<TEntity> AndIGetTableData<TEntity>() where TEntity : class
        {
            var context = GivenTheDefaultDbContext();

            var records = context.Set<TEntity>().ToList();

            return records;
        }

        public new virtual void RegisterBasicDependency(ContainerBuilder builder)
        {
            var options = GivenTheDefaultDbContextOptions();
            builder.RegisterInstance(options);

            base.RegisterBasicDependency(builder);
        }
    }
}
