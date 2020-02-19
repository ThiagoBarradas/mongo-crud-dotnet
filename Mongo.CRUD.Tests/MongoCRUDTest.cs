//using Mongo.CRUD.Attributes;
//using Mongo.CRUD.Builders;
//using Mongo.CRUD.Models;
//using Mongo.CRUD.Tests.Fakes;
//using MongoDB.Bson.Serialization;
//using MongoDB.Bson.Serialization.Attributes;
//using MongoDB.Driver;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Xunit;

//namespace Mongo.CRUD.Tests
//{
//    public static class MongoCRUDTest
//    {
//        [Fact]
//        public static void RegisterDefaultConventionPack_Should_Register_Custom_ConventionPack()
//        {
//            // arrange & act (register)
//            MongoCRUD.RegisterDefaultConventionPack(t => t == typeof(ConventionPackTestClass));
//            var serializerRegistry = BsonSerializer.SerializerRegistry;
//            var documentSerializer = serializerRegistry.GetSerializer<ConventionPackTestClass>();
//            var builder = FilterBuilder.GetFilterBuilder<ConventionPackTestClass>();
//            var myTest = new ConventionPackTestClass
//            {
//                MyGuid = Guid.NewGuid(),
//                MyTest = ConventionPackTestEnum.Test2
//            };

//            FilterDefinition<ConventionPackTestClass> filter1 = builder.Eq(r => r.MyGuid, myTest.MyGuid);
//            FilterDefinition<ConventionPackTestClass> filter2 = builder.Eq(r => r.MyTest, myTest.MyTest);

//            // act (using)
//            var rendered1 = filter1?.Render(documentSerializer, serializerRegistry);
//            var rendered2 = filter2?.Render(documentSerializer, serializerRegistry);

//            // assert
//            Assert.NotNull(rendered1);
//            Assert.Single(rendered1);
//            Assert.Equal("myGuid", rendered1?.FirstOrDefault().Name);
//            Assert.True(rendered1?.FirstOrDefault().Value.IsString);

//            Assert.NotNull(rendered2);
//            Assert.Single(rendered2);
//            Assert.Equal("myTest", rendered2?.FirstOrDefault().Name);
//            Assert.Equal(myTest.MyTest.ToString(), rendered2?.FirstOrDefault().Value);

//            // Cleanup
//            MongoCRUD.UnregisterDefaultConventionPack();
//        } 

//        [Fact]
//        public static void Contructor_Empty_Should_Return_An_Instance_Without_MongoClient_Instance()
//        {
//            // arrange & act
//            var mongoCRUD = new MongoCRUD<GenericTestWithIdProperty>();

//            // assert
//            Assert.Null(mongoCRUD.MongoClient);
//        }

//        [Fact]
//        public static void Contructor_With_MongoClient_Should_Throw_Exception_When_MongoClient_Is_Null()
//        {
//            // arrange
//            IMongoClient client = null;
//            string database = "MyDB";

//            // act
//            Exception ex =
//                Assert.Throws<ArgumentNullException>(() =>
//                new MongoCRUD<GenericTestWithIdProperty>(client, database));


//            // assert
//            Assert.Equal("Value cannot be null.\r\nParameter name: mongoClient", ex.Message);
//        }

//        [Fact]
//        public static void Contructor_With_MongoClient_Should_Throw_Exception_When_Database_Is_Null()
//        {
//            // arrange
//            IMongoClient client = new MongoClient();
//            string database = null;

//            // act
//            Exception ex =
//                Assert.Throws<ArgumentNullException>(() =>
//                new MongoCRUD<GenericTestWithIdProperty>(client, database));


//            // assert
//            Assert.Equal("Value cannot be null.\r\nParameter name: database", ex.Message);
//        }

//        [Fact]
//        public static void Contructor_With_MongoClient_Should_Throw_Exception_When_Database_Is_Empty_String()
//        {
//            // arrange
//            IMongoClient client = new MongoClient();
//            string database = "";

//            // act
//            Exception ex =
//                Assert.Throws<ArgumentNullException>(() =>
//                new MongoCRUD<GenericTestWithIdProperty>(client, database));


//            // assert
//            Assert.Equal("Value cannot be null.\r\nParameter name: database", ex.Message);
//        }

//        [Fact]
//        public static void Contructor_With_MongoClient_Should_Returns_Instance()
//        {
//            // arrange
//            IMongoClient client = new MongoClient();
//            string database = "MyDB";

//            // act
//            var mongoCRUD = new MongoCRUD<GenericTestWithIdProperty>(client, database);

//            // assert
//            Assert.NotNull(mongoCRUD);
//            Assert.NotNull(mongoCRUD.MongoClient);
//            Assert.Null(mongoCRUD.Configuration);
//        }

//        [Fact]
//        public static void Contructor_With_MongoConfiguration_Should_Throw_Exception_When_MongoConfiguration_Is_Null()
//        {
//            // arrange
//            MongoConfiguration config = null;

//            // act
//            Exception ex =
//                Assert.Throws<ArgumentNullException>(() =>
//                new MongoCRUD<GenericTestWithIdProperty>(config));

//            // assert
//            Assert.Equal("Value cannot be null.\r\nParameter name: configuration", ex.Message);
//        }

//        [Fact]
//        public static void Contructor_With_MongoConfiguration_Should_Throw_Exception_When_Database_Is_Null()
//        {
//            // arrange
//            MongoConfiguration config = new MongoConfiguration
//            {
//                ConnectionString = "mongodb://localhost",
//                Database = null,
//            };

//            // act
//            Exception ex =
//                Assert.Throws<ArgumentNullException>(() =>
//                new MongoCRUD<GenericTestWithIdProperty>(config));

//            // assert
//            Assert.Equal("Value cannot be null.\r\nParameter name: database", ex.Message);
//        }

//        [Fact]
//        public static void Contructor_With_MongoConfiguration_Should_Throw_Exception_When_Database_Is_Empty_String()
//        {
//            // arrange
//            MongoConfiguration config = new MongoConfiguration
//            {
//                ConnectionString = "mongodb://localhost",
//                Database = ""
//            };

//            // act
//            Exception ex =
//                Assert.Throws<ArgumentNullException>(() =>
//                new MongoCRUD<GenericTestWithIdProperty>(config));

//            // assert
//            Assert.Equal("Value cannot be null.\r\nParameter name: database", ex.Message);
//        }

//        [Fact]
//        public static void Contructor_With_MongoConfiguration_Should_Throw_Exception_When_ConnectionString_Is_Null()
//        {
//            // arrange
//            MongoConfiguration config = new MongoConfiguration
//            {
//                ConnectionString = null,
//                Database = "MyDB"
//            };

//            // act
//            Exception ex =
//                Assert.Throws<ArgumentNullException>(() =>
//                new MongoCRUD<GenericTestWithIdProperty>(config));

//            // assert
//            Assert.Equal("Value cannot be null.\r\nParameter name: connectionString", ex.Message);
//        }

//        [Fact]
//        public static void Contructor_With_MongoConfiguration_Should_Throw_Exception_When_ConnectionString_Is_Invalid()
//        {
//            // arrange
//            MongoConfiguration config = new MongoConfiguration
//            {
//                ConnectionString = "some text",
//                Database = "MyDB"
//            };

//            // act
//            Exception ex =
//                Assert.Throws<MongoConfigurationException>(() =>
//                new MongoCRUD<GenericTestWithIdProperty>(config));

//            // assert
//            Assert.Equal("The connection string 'some text' is not valid.", ex.Message);
//        }

//        [Fact]
//        public static void Contructor_With_MongoConfiguration_Should_Returns_Instance()
//        {
//            // arrange
//            MongoConfiguration config = new MongoConfiguration
//            {
//                ConnectionString = "mongodb://localhost",
//                Database = "MyDB"
//            };

//            // act
//            var mongoCRUD = new MongoCRUD<GenericTestWithIdProperty>(config);

//            // assert
//            Assert.NotNull(mongoCRUD);
//            Assert.NotNull(mongoCRUD.MongoClient);
//            Assert.NotNull(mongoCRUD.Configuration);
//        }

//        [Fact]
//        public static void Contructor_With_MongoConfiguration_Splitted_Data_Should_Returns_Instance()
//        {
//            // arrange
//            MongoConfiguration config = new MongoConfiguration
//            {
//                ConnectionString = "mongodb://localhost",
//                Database = "MyDB"
//            };

//            // act
//            var mongoCRUD = new MongoCRUD<GenericTestWithIdProperty>(config.ConnectionString, config.Database);

//            // assert
//            Assert.NotNull(mongoCRUD);
//            Assert.NotNull(mongoCRUD.MongoClient);
//            Assert.NotNull(mongoCRUD.Configuration);
//        }

//        [Fact]
//        public static void Create_Should_Works()
//        {
//            // arrange
//            var mongoClient = FakeMongoClient.GetMongoClientMock<GenericTestWithIdProperty>(true).Object;
//            var mongoCRUD = new MongoCRUD<GenericTestWithIdProperty>(mongoClient, "SomeDB");
//            var document = new GenericTestWithIdProperty
//            {
//                Id = "123",
//                SomeTest = "test"
//            };

//            // act
//            mongoCRUD.Create(document);

//            // assert
//            // if no exception, create has success
//        }

//        [Fact]
//        public static void Create_Should_Throws_Exception_When_Document_Is_Null()
//        {
//            // arrange
//            var mongoClient = FakeMongoClient.GetMongoClientMock<GenericTestWithIdProperty>(true).Object;
//            var mongoCRUD = new MongoCRUD<GenericTestWithIdProperty>(mongoClient, "SomeDB");
//            GenericTestWithIdProperty document = null;

//            // act
//            Exception ex =
//                Assert.Throws<ArgumentNullException>(() => mongoCRUD.Create(document));

//            // assert
//            Assert.Equal("Value cannot be null.\r\nParameter name: document", ex.Message);
//        }

//        [Fact]
//        public static void Create_Many_Should_Works()
//        {
//            // arrange
//            var mongoClient = FakeMongoClient.GetMongoClientMock<GenericTestWithIdProperty>(true).Object;
//            var mongoCRUD = new MongoCRUD<GenericTestWithIdProperty>(mongoClient, "SomeDB");

//            var document1 = new GenericTestWithIdProperty
//            {
//                Id = "123",
//                SomeTest = "test"
//            };

//            var document2 = new GenericTestWithIdProperty
//            {
//                Id = "124",
//                SomeTest = "test2"
//            };

//            var documents = new List<GenericTestWithIdProperty> { document1, document2 };

//            // act
//            mongoCRUD.Create(documents);

//            // assert
//            // if no exception, create has success
//        }

//        [Fact]
//        public static void Create_Many_Should_Throws_Exception_When_Document_Is_Null()
//        {
//            // arrange
//            var mongoClient = FakeMongoClient.GetMongoClientMock<GenericTestWithIdProperty>(true).Object;
//            var mongoCRUD = new MongoCRUD<GenericTestWithIdProperty>(mongoClient, "SomeDB");
//            List<GenericTestWithIdProperty> documents = null;

//            // act
//            Exception ex =
//                Assert.Throws<ArgumentNullException>(() => mongoCRUD.Create(documents));

//            // assert
//            Assert.Equal("Value cannot be null.\r\nParameter name: documents", ex.Message);
//        }

//        [Fact]
//        public static void Update_Should_Works()
//        {
//            // arrange
//            var mongoClient = FakeMongoClient.GetMongoClientMock<GenericTestWithIdProperty>(true).Object;
//            var mongoCRUD = new MongoCRUD<GenericTestWithIdProperty>(mongoClient, "SomeDB");
//            var document = new GenericTestWithIdProperty
//            {
//                Id = "123",
//                SomeTest = "test"
//            };

//            // act
//            var isAcknowledged = mongoCRUD.Update(document);

//            // assert
//            Assert.True(isAcknowledged);
//        }

//        [Fact]
//        public static void Update_Should_Throws_Exception_When_Document_Is_Null()
//        {
//            // arrange
//            var mongoClient = FakeMongoClient.GetMongoClientMock<GenericTestWithIdProperty>(true).Object;
//            var mongoCRUD = new MongoCRUD<GenericTestWithIdProperty>(mongoClient, "SomeDB");
//            GenericTestWithIdProperty document = null;

//            // act
//            Exception ex =
//                Assert.Throws<ArgumentNullException>(() => mongoCRUD.Update(document));

//            // assert
//            Assert.Equal("Value cannot be null.\r\nParameter name: document", ex.Message);
//        }

//        [Fact]
//        public static void UpdateByQuery_Should_Works_With_Filter()
//        {
//            // arrange
//            var mongoClient = FakeMongoClient.GetMongoClientMock<GenericTestWithIdProperty>(true).Object;
//            var mongoCRUD = new MongoCRUD<GenericTestWithIdProperty>(mongoClient, "SomeDB");
//            var document = new
//            {
//                Id = "123",
//                SomeTest = "test"
//            };
//            var filters = FilterBuilder.GetFilterBuilder<GenericTestWithIdProperty>().Eq(r => r.SomeTest, "test");

//            // act
//            var isAcknowledged = mongoCRUD.UpdateByQuery(filters, document);

//            // assert
//            Assert.True(isAcknowledged);
//        }

//        [Fact]
//        public static void UpdateByQuery_Should_Works_With_Null_Filter()
//        {
//            // arrange
//            var mongoClient = FakeMongoClient.GetMongoClientMock<GenericTestWithIdProperty>(true).Object;
//            var mongoCRUD = new MongoCRUD<GenericTestWithIdProperty>(mongoClient, "SomeDB");
//            var document = new
//            {
//                Id = "123",
//                SomeTest = "test"
//            };

//            // act
//            var isAcknowledged = mongoCRUD.UpdateByQuery(null, document);

//            // assert
//            Assert.True(isAcknowledged);
//        }
//    }
//        //        [Fact]
//        //        public static void UpdateByQuery_Should_Throws_Exception_When_PartialDocument_Is_Null()
//        //        {
//        //            // arrange
//        //            var mongoClient = FakeMongoClient.GetMongoClientMock<GenericTestWithIdProperty>(true).Object;
//        //            var mongoCRUD = new MongoCRUD<GenericTestWithIdProperty>(mongoClient, "SomeDB");

//        //            // act
//        //            Exception ex =
//        //                Assert.Throws<ArgumentNullException>(() => mongoCRUD.UpdateByQuery(null, null));

//        //            // assert
//        //            Assert.Equal("Value cannot be null.\r\nParameter name: partialDocument", ex.Message);
//        //        }

//        //        [Fact]
//        //        public static void Upsert_Should_Works()
//        //        {
//        //            // arrange
//        //            var mongoClient = FakeMongoClient.GetMongoClientMock<GenericTestWithBsonIdAnnotation>(true).Object;
//        //            var mongoCRUD = new MongoCRUD<GenericTestWithBsonIdAnnotation>(mongoClient, "SomeDB");
//        //            var document = new GenericTestWithBsonIdAnnotation
//        //            {
//        //                CustomId = "123",
//        //                SomeTest = "test"
//        //            };

//        //            // act
//        //            var isAcknowledged = mongoCRUD.Upsert(document);

//        //            // assert
//        //            Assert.True(isAcknowledged);
//        //        }

//        //        [Fact]
//        //        public static void Upsert_Should_Throws_Exception_When_Document_Is_Null()
//        //        {
//        //            // arrange
//        //            var mongoClient = FakeMongoClient.GetMongoClientMock<GenericTestWithBsonIdAnnotation>(true).Object;
//        //            var mongoCRUD = new MongoCRUD<GenericTestWithBsonIdAnnotation>(mongoClient, "SomeDB");
//        //            GenericTestWithBsonIdAnnotation document = null;

//        //            // act
//        //            Exception ex =
//        //                Assert.Throws<ArgumentNullException>(() => mongoCRUD.Upsert(document));

//        //            // assert
//        //            Assert.Equal("Value cannot be null.\r\nParameter name: document", ex.Message);
//        //        }

//        //        [Fact]
//        //        public static void Delete_Should_Works()
//        //        {
//        //            // arrange
//        //            var mongoClient = FakeMongoClient.GetMongoClientMock<GenericTestWithIdProperty>(true).Object;
//        //            var mongoCRUD = new MongoCRUD<GenericTestWithIdProperty>(mongoClient, "SomeDB");
//        //            var id = "123";

//        //            // act
//        //            var isAcknowledged = mongoCRUD.Delete(id);

//        //            // assert
//        //            Assert.True(isAcknowledged);
//        //        }

//        //        [Fact]
//        //        public static void Delete_Should_Throws_Exception_When_Id_Is_Null()
//        //        {
//        //            // arrange
//        //            var mongoClient = FakeMongoClient.GetMongoClientMock<GenericTestWithIdProperty>(true).Object;
//        //            var mongoCRUD = new MongoCRUD<GenericTestWithIdProperty>(mongoClient, "SomeDB");
//        //            string id = null;

//        //            // act
//        //            Exception ex =
//        //                Assert.Throws<ArgumentNullException>(() => mongoCRUD.Delete(id));

//        //            // assert
//        //            Assert.Equal("Value cannot be null.\r\nParameter name: id", ex.Message);
//        //        }

//        //        [Fact]
//        //        public static void DeleteByQuery_Should_Works_With_Filter()
//        //        {
//        //            // arrange
//        //            var mongoClient = FakeMongoClient.GetMongoClientMock<GenericTestWithIdProperty>(true).Object;
//        //            var mongoCRUD = new MongoCRUD<GenericTestWithIdProperty>(mongoClient, "SomeDB");
//        //            var filters = FilterBuilder.GetFilterBuilder<GenericTestWithIdProperty>().Eq(r => r.SomeTest, "test");

//        //            // act
//        //            var isAcknowledged = mongoCRUD.DeleteByQuery(filters);

//        //            // assert
//        //            Assert.True(isAcknowledged);
//        //        }

//        //        [Fact]
//        //        public static void DeleteByQuery_Should_Works_With_Null_Filter()
//        //        {
//        //            // arrange
//        //            var mongoClient = FakeMongoClient.GetMongoClientMock<GenericTestWithIdProperty>(true).Object;
//        //            var mongoCRUD = new MongoCRUD<GenericTestWithIdProperty>(mongoClient, "SomeDB");
//        //            FilterDefinition<GenericTestWithIdProperty> filters = null;

//        //            // act
//        //            var isAcknowledged = mongoCRUD.DeleteByQuery(filters);

//        //            // assert
//        //            Assert.True(isAcknowledged);
//        //        }

//        //        [Fact]
//        //        public static void Get_Should_Throws_Exception_When_Id_Is_Null()
//        //        {
//        //            // arrange
//        //            var mongoClient = FakeMongoClient.GetMongoClientMock<GenericTestWithIdProperty>(true).Object;
//        //            var mongoCRUD = new MongoCRUD<GenericTestWithIdProperty>(mongoClient, "SomeDB");

//        //            // act
//        //            Exception ex =
//        //                Assert.Throws<ArgumentNullException>(() => mongoCRUD.Get(null));

//        //            // assert
//        //            Assert.Equal("Value cannot be null.\r\nParameter name: id", ex.Message);
//        //        }

//        //        [Fact]
//        //        public static void GetDocumentId_Should_Throws_Exception_When_Object_Not_Have_Id_Property_Or_BsonId_Annotation()
//        //        {
//        //            // arrange
//        //            var mongoClient = FakeMongoClient.GetMongoClientMock<GenericTestWithoutIdPropertyOrBsonIdAnnotation>(true).Object;
//        //            var mongoCRUD = new MongoCRUD<GenericTestWithoutIdPropertyOrBsonIdAnnotation>(mongoClient, "SomeDB");
//        //            var document = new GenericTestWithoutIdPropertyOrBsonIdAnnotation
//        //            {
//        //                CustomId = "123",
//        //                SomeTest = "some test"
//        //            };

//        //            // act
//        //            Exception ex =
//        //                Assert.Throws<InvalidOperationException>(() => mongoCRUD.Update(document));

//        //            // assert
//        //            Assert.Equal("Model must have a property called 'Id' or a property with BsonId attribute", ex.Message);
//        //        }
//        //    }

//        public enum ConventionPackTestEnum
//    {
//        Test1 = 0,
//        Test2 = 1
//        }

//    public class ConventionPackTestClass
//    {
//        public ConventionPackTestEnum MyTest { get; set; }

//        public Guid MyGuid { get; set; }
//    }

//    public class GenericTestWithIdProperty
//    {
//        public string Id { get; set; }

//        public string SomeTest { get; set; }
//    }

//    public class GenericTestWithBsonIdAnnotation
//    {
//        [BsonId]
//        public string CustomId { get; set; }

//        public string SomeTest { get; set; }
//    }

//    [CollectionName("CustomCollectionName")]
//    public class GenericTestWithoutIdPropertyOrBsonIdAnnotation
//    {
//        public string CustomId { get; set; }

//        public string SomeTest { get; set; }
//    }
//}
