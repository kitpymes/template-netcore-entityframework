using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kitpymes.Core.EntityFramework.Tests
{
    [TestClass]
    public class EntityFrameworkRepositoryTests
    {
        private readonly FakeContext context;

        private readonly IFakeRepository repository;

        public EntityFrameworkRepositoryTests()
        {
            var services = new ServiceCollection();

            context = services.LoadInMemoryDatabase<FakeContext>();
                        
            repository = new FakeRepository(context);

            SeedDatabase();
        }


        #region GetOne

        [TestMethod]
        public void GetOneWhere()
        {
            Assert.IsNotNull(repository.GetOne(where: x => x.Id == 1));
        }

        [TestMethod]
        public void GetOneAsyncWhere()
        {
            Assert.IsNotNull(repository.GetOneAsync(where: x => x.Id == 1).Result);
        }

        [TestMethod]
        public void GetOneWhereInclude()
        {
            Assert.IsNotNull(repository.GetOne(where: w => w.Id == 1, includes: i => i.FakeEntityChild));
        }

        [TestMethod]
        public void GetOneAsyncWhereInclude()
        {
            Assert.IsNotNull(repository.GetOneAsync(where: w => w.Id == 1, includes: i => i.FakeEntityChild).Result);
        }

        [TestMethod]
        public void GetOnetWhereSelect()
        {
            Assert.IsNotNull(repository.GetOne(where: w => w.Id == 1, select: s => s.FakeEntityChild));
        }

        [TestMethod]
        public void GetOneAsyncWhereSelect()
        {
            Assert.IsNotNull(repository.GetOneAsync(where: w => w.Id == 1, select: s => s.FakeEntityChild).Result);
        }

        [TestMethod]
        public void GetOnetWhereIncludesSelect()
        {
            Assert.IsNotNull(repository.GetOne(where: w => w.Id == 1, select: s => s.FakeEntityChild, includes: i => i.FakeValueObject));
        }

        [TestMethod]
        public void GetOneAsyncWhereIncludesSelect()
        {
            Assert.IsNotNull(repository.GetOneAsync(where: w => w.Id == 1, select: s => s.FakeEntityChild, includes: i => i.FakeValueObject).Result);
        }

        #endregion GetOne

        #region GetAll

        [TestMethod]
        public void GetAll()
        {
            Assert.IsTrue(repository.GetAll().Any());
        }

        [TestMethod]
        public void GetAllAsync()
        {
            Assert.IsNotNull(repository.GetAllAsync().Result);
        }

        [TestMethod]
        public void GetAllWhere()
        {
            Assert.IsTrue(repository.GetAll(where: x => x.Id == 1).Any());
        }

        [TestMethod]
        public void GetAllAsyncWhere()
        {
            Assert.IsTrue(repository.GetAllAsync(where: x => x.Id == 1).Result.Any());
        }

        [TestMethod]
        public void GetAllWhereIncludes()
        {
            Assert.IsTrue(repository.GetAll(where: x => x.Id == 1, includes: i => i.FakeValueObject).Any());
        }

        [TestMethod]
        public void GetAllAsyncWhereIncludes()
        {
            Assert.IsTrue(repository.GetAllAsync(where: x => x.Id == 1, includes: i => i.FakeValueObject).Result.Any());
        }

        [TestMethod]
        public void GetAllWhereSelect()
        {
            Assert.IsNotNull
            (
                repository.GetAll<FakeEntityModel>
                (
                    select: s => new FakeEntityModel { Name = s.Name },

                    includes: i => i.FakeEntityChild
                )
            );
        }

        [TestMethod]
        public void GetAllAsyncWhereSelect()
        {
            Assert.IsNotNull
            (
                repository.GetAllAsync<FakeEntityModel>
                (
                    select: s => new FakeEntityModel { Name = s.Name },

                    includes: i => i.FakeEntityChild
                )
            );
        }

        [TestMethod]
        public void GetAllWhereIncludesSelect()
        {
            Assert.IsNotNull
            (
                repository.GetAll<FakeEntityModel>
                (
                    select: s => new FakeEntityModel { Name = s.Name },

                    where: w => w.Id == 1L,

                    includes: i => i.FakeEntityChild
                )
            );
        }

        [TestMethod]
        public void GetAllAsyncWhereIncludesSelect()
        {
            Assert.IsNotNull
            (
                repository.GetAllAsync<FakeEntityModel>
                (
                    select: s => new FakeEntityModel { Name = s.Name },

                    where: w => w.Id == 1L,

                    includes: i => i.FakeEntityChild
                )
            );
        }

        #endregion GetAll

        #region GetPaged

        [TestMethod]
        public void GetPaged()
        {
            Assert.IsNotNull(repository.GetPaged(nameof(FakeEntity.Name)));
        }

        [TestMethod]
        public void GetPagedAsync()
        {
            Assert.IsNotNull(repository.GetPagedAsync(nameof(FakeEntity.Name)));
        }

        [TestMethod]
        public void GetPagedWhere()
        {
            Assert.IsNotNull(repository.GetPaged(nameof(FakeEntity.Name), where: x => x.Id == 1));
        }

        [TestMethod]
        public void GetPagedAsyncWhere()
        {
            Assert.IsNotNull(repository.GetPagedAsync(nameof(FakeEntity.Name), where: x => x.Id == 1));
        }

        [TestMethod]
        public void GetPagedWhereIncludes()
        {
            Assert.IsNotNull(repository.GetPaged(nameof(FakeEntity.Name), includes: i => i.FakeEntityChild));
        }

        [TestMethod]
        public void GetPagedAsyncWhereIncludes()
        {
            Assert.IsNotNull(repository.GetPagedAsync(nameof(FakeEntity.Name), includes: i => i.FakeEntityChild));
        }

        [TestMethod]
        public void GetPagedWhereSelect()
        {
            Assert.IsNotNull
            (
                repository.GetPaged<FakeEntityModel>
                (
                    nameof(FakeEntity.Name),

                    select: s => new FakeEntityModel { Name = s.Name },

                    includes: i => i.FakeEntityChild
                )
            );
        }

        [TestMethod]
        public void GetPagedAsyncWhereSelect()
        {
            Assert.IsNotNull
            (
                repository.GetPagedAsync<FakeEntityModel>
                (
                    nameof(FakeEntity.Name),

                    select: s => new FakeEntityModel { Name = s.Name },

                    includes: i => i.FakeEntityChild
                )
            );
        }

        [TestMethod]
        public void GetPagedWhereIncludesSelect()
        {
            Assert.IsNotNull
            (
                repository.GetPaged<FakeEntityModel>
                (
                    nameof(FakeEntity.Name),

                    select: s => new FakeEntityModel { Name = s.Name },

                    where: w => w.Id == 1L,

                    includes: i => i.FakeEntityChild
                )
            );
        }

        [TestMethod]
        public void GetPagedAsyncWhereIncludesSelect()
        {
            Assert.IsNotNull
            (
                repository.GetPagedAsync<FakeEntityModel>
                (
                    nameof(FakeEntity.Name),

                    select: s => new FakeEntityModel { Name = s.Name },

                    where: w => w.Id == 1L,

                    includes: i => i.FakeEntityChild
                )
            );
        }

        [TestMethod]
        public void GetPagedSelectOrderByNavigationProperty()
        {
            var result = repository.GetPaged
            (
                $"{nameof(FakeValueObject)}.{nameof(FakeValueObject.Property1)}",

                options: x => 
                {
                    x.Ascending = false;
                },

                select: s => new FakeEntityModel { Name = s.Name }
            );

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetPagedSelectOrderByProperty()
        {
            var result = repository.GetPaged<FakeEntityModel>
            (
                 nameof(FakeEntityModel.Id),

                  options: x =>
                  {
                      x.Ascending = false;
                  },

                select: s => new FakeEntityModel { Name = s.Name }
            );

            Assert.IsNotNull(result);
        }

        #endregion GetPaged

        #region Queryable

        [TestMethod]
        public void QueryableOrderByDescending()
        {
            Assert.IsNotNull(repository.Query.OrderByDescending(o => o.Id).FirstOrDefault());
        }

        #endregion Queryable

        #region Find

        [TestMethod]
        public void Find()
        {
            Assert.IsNotNull(repository.Find(1L));
        }

        [TestMethod]
        public void FindAsync()
        {
            Assert.IsNotNull(repository.FindAsync(1L).Result);
        }

        #endregion Find

        #region Add

        [TestMethod]
        public void Add()
        {
            var entity = CreateEntity();

            repository.Add(entity);
            context.SaveChanges();

            Assert.IsNotNull(repository.Find(entity.Id));
        }

        [TestMethod]
        public void AddAsync()
        {
            var entity = CreateEntity();

            repository.AddAsync(entity);
            context.SaveChanges();

            Assert.IsNotNull(repository.Find(entity.Id));
        }

        [TestMethod]
        public void AddRange()
        {
            var count = repository.Count();

            repository.AddRange(new List<FakeEntity> { CreateEntity() });
            context.SaveChanges();

            Assert.IsTrue(repository.Count() > count);
        }

        [TestMethod]
        public void AddRangeAsync()
        {
            var count = repository.Count();

            repository.AddRangeAsync(new List<FakeEntity> { CreateEntity() });
            context.SaveChanges();

            Assert.IsTrue(repository.Count() > count);
        }

        #endregion Add

        #region Update

        [TestMethod]
        public void Update()
        {
            var entity = new FakeEntity(1L, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), new FakeValueObject());

            repository.Update(1L, entity);

            context.SaveChanges();

            var entityDatabase = repository.Find(1L);

            Assert.AreEqual(entity.Name, entityDatabase.Name);
        }

        [TestMethod]
        public void UpdateSelect()
        {
            var entityDatabase = repository.Find(1L);

            Assert.IsNotNull(entityDatabase);

            var entity = new FakeEntity(1L, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), new FakeValueObject());

            repository.Update(1L, entity);

            context.SaveChanges();

            entityDatabase = repository.Find(1L);

            Assert.AreEqual(entity.Name, entityDatabase.Name);
        }

        [TestMethod]
        public void UpdateAsynchronous()
        {
            var entity = new FakeEntity(1L, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), new FakeValueObject());

            repository.UpdateAsync(1L, entity);

            context.SaveChanges();

            var entityDatabase = repository.Find(1L);

            Assert.AreEqual(entity.Name, entityDatabase.Name);
        }

        [TestMethod]
        public void UpdatePartial()
        {
            var entity = new
            {
                Name = Guid.NewGuid().ToString()
            };

            repository.UpdatePartial(1L, entity);

            context.SaveChanges();

            var entityDatabase = repository.Find(1L);

            Assert.AreEqual(entity.Name, entityDatabase.Name);
            Assert.IsNotNull(entityDatabase.Surname);
        }

        [TestMethod]
        public void UpdateValueObject()
        {
            var entity = new FakeEntity(1L, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), new FakeValueObject(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));

            repository.Update(1L, entity);

            context.SaveChanges();

            var entityDatabase = repository.Find(1L);

            Assert.AreEqual(entity.FakeValueObject.Property1, entityDatabase.FakeValueObject.Property1);
            Assert.AreEqual(entity.FakeValueObject.Property2, entityDatabase.FakeValueObject.Property2);
        }

        #endregion Update

        #region Delete

        [TestMethod]
        public void Delete()
        {
            repository.Delete(70L);
            context.SaveChanges();

            Assert.IsNull(repository.Find(70L));
        }

        [TestMethod]
        public void DeleteAsynchronous()
        {
            repository.DeleteAsync(80L);
            context.SaveChanges();

            Assert.IsNull(repository.Find(80L));
        }

        [TestMethod]
        public void DeleteWhere()
        {
            repository.Delete(w => w.Id == 90L);
            context.SaveChanges();

            Assert.IsNull(repository.Find(90L));
        }

        [TestMethod]
        public void DeleteWhereAsynchronous()
        {
            repository.DeleteAsync(w => w.Id == 100L);
            context.SaveChanges();

            Assert.IsNull(repository.Find(100L));
        }

        #endregion Delete

        #region Any

        [TestMethod]
        public void Any()
        {
            Assert.IsTrue(repository.Any());
        }

        [TestMethod]
        public void AnyAsync()
        {
            Assert.IsTrue(repository.AnyAsync().Result);
        }

        [TestMethod]
        public void AnyWhere()
        {
            Assert.IsTrue(repository.Any(w => w.Id == 1L));
        }

        [TestMethod]
        public void AnyWhereAsync()
        {
            Assert.IsTrue(repository.AnyAsync(w => w.Id == 1L).Result);
        }

        #endregion Any

        #region Count

        [TestMethod]
        public void Count()
        {
            Assert.IsTrue(repository.Count() > 0);
        }

        [TestMethod]
        public void CountAsync()
        {
            Assert.IsTrue(repository.CountAsync().Result > 0);
        }

        [TestMethod]
        public void CountWhere()
        {
            Assert.IsTrue(repository.Count(w => w.Id == 1) == 1L);
        }

        [TestMethod]
        public void CountWhereAsync()
        {
            Assert.IsTrue(repository.CountAsync(w => w.Id == 1L).Result == 1L);
        }

        #endregion Count

        #region Private

        private void SeedDatabase()
        {
            if (context.Set<FakeEntity>().Any()) { return; }

            for (var i = 1L; i <= 100; i++)
            {
                repository.Add(CreateEntity());
            }

            context.SaveChanges();
        }

        private static FakeEntity CreateEntity()
        {
            return new FakeEntity
            (
                $"Name {Guid.NewGuid()}",
                $"Surname {Guid.NewGuid().ToString()}",
                new FakeValueObject("Property", "Property")
            );
        }

        #endregion Private
    }
}