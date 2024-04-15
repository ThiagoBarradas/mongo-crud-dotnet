using Mongo.CRUD.Helpers;
using MongoDB.Bson.Serialization.Attributes;
using System;
using Xunit;

namespace Mongo.CRUD.Tests.Helpers
{
    public static class PropertyHelperTest
    {
        [Fact]
        public static void GetIndividualPropertyDetailsByAttribute_Should_Get_Value()
        {
            // arrange
            MyExampleWithAnnotation obj = new MyExampleWithAnnotation
            {
                UniqueId = Guid.Parse("5be8f701-8358-4ea9-b2d1-5006d516da21"),
                SomeString = "Test"
            };

            // act
            var result = PropertyHelper
                .GetIndividualPropertyDetailsByAttribute<MyExampleWithAnnotation, BsonIdAttribute>(obj);

            // assert
            Assert.NotNull(result);
            Assert.Equal(obj.UniqueId, result.Value);
            Assert.Single(result.Attributes);
            Assert.Equal(nameof(MyExampleWithAnnotation.UniqueId), result.PropertyInfo.Name);
        }

        [Fact]
        public static void GetIndividualPropertyDetailsByAttribute_Should_Get_Null()
        {
            // arrange
            MyExampleWithoutAnnotation obj = new MyExampleWithoutAnnotation
            {
                UniqueId = Guid.Parse("5be8f701-8358-4ea9-b2d1-5006d516da21"),
                SomeString = "Test"
            };

            // act
            var result = PropertyHelper
                .GetIndividualPropertyDetailsByAttribute<MyExampleWithoutAnnotation, BsonIdAttribute>(obj);

            // assert
            Assert.Null(result);
        }

        [Fact]
        public static void GetIndividualPropertyDetailsByAttribute_Should_Throw_Exception_When_Obj_Is_Null()
        {
            // arrange
            MyExampleWithoutAnnotation obj = null;

            // act
            Exception ex =
                Assert.Throws<ArgumentNullException>(() => PropertyHelper
                    .GetIndividualPropertyDetailsByAttribute<MyExampleWithoutAnnotation, BsonIdAttribute>(obj));

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'obj')", ex.Message.Replace("\r", ""));
        }

        [Theory]
        [InlineData(nameof(MyExampleWithIdProperty.Id))]
        [InlineData("id")]
        public static void GetIndividualPropertyDetailsByPropertyName_Should_Get_Value(string propertyName)
        {
            // arrange
            MyExampleWithIdProperty obj = new MyExampleWithIdProperty
            {
                Id = Guid.Parse("5be8f701-8358-4ea9-b2d1-5006d516da21"),
                SomeString = "Test"
            };

            // act
            var result = PropertyHelper
                .GetIndividualPropertyDetailsByPropertyName(obj, propertyName);

            // assert
            Assert.NotNull(result);
            Assert.Equal(obj.Id, result.Value);
            Assert.Equal(nameof(MyExampleWithIdProperty.Id), result.PropertyInfo.Name);
        }

        [Fact]
        public static void GetIndividualPropertyDetailsByPropertyName_Should_Get_Null()
        {
            // arrange
            MyExampleWithIdProperty obj = new MyExampleWithIdProperty
            {
                Id = Guid.Parse("5be8f701-8358-4ea9-b2d1-5006d516da21"),
                SomeString = "Test"
            };

            // act
            var result = PropertyHelper
                .GetIndividualPropertyDetailsByPropertyName(obj, "InvalidProperty");

            // assert
            Assert.Null(result);
        }

        [Fact]
        public static void GetIndividualPropertyDetailsByPropertyName_Should_Throw_Exception_When_Obj_Is_Null()
        {
            // arrange
            MyExampleWithIdProperty obj = null;

            // act
            Exception ex =
                Assert.Throws<ArgumentNullException>(() => PropertyHelper
                    .GetIndividualPropertyDetailsByPropertyName(obj, "UniqueId"));

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'obj')", ex.Message.Replace("\r", ""));
        }

        [Fact]
        public static void GetIndividualPropertyDetailsByPropertyName_Should_Throw_Exception_When_PropertyName_Is_Null()
        {
            // arrange
            MyExampleWithIdProperty obj = new MyExampleWithIdProperty
            {
                Id = Guid.Parse("5be8f701-8358-4ea9-b2d1-5006d516da21"),
                SomeString = "Test"
            };

            // act
            Exception ex =
                Assert.Throws<ArgumentNullException>(() => PropertyHelper
                    .GetIndividualPropertyDetailsByPropertyName(obj, null));

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'propertyName')", ex.Message.Replace("\r", ""));
        }

        [Fact]
        public static void GetIndividualPropertyDetailsByPropertyName_Should_Throw_Exception_When_PropertyName_Is_Empty_String()
        {
            // arrange
            MyExampleWithIdProperty obj = new MyExampleWithIdProperty
            {
                Id = Guid.Parse("5be8f701-8358-4ea9-b2d1-5006d516da21"),
                SomeString = "Test"
            };

            // act
            Exception ex =
                Assert.Throws<ArgumentNullException>(() => PropertyHelper
                    .GetIndividualPropertyDetailsByPropertyName(obj, ""));

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'propertyName')", ex.Message.Replace("\r", ""));
        }
    }

    public class MyExampleWithAnnotation
    {
        [BsonId]
        public Guid UniqueId { get; set; }

        public string SomeString { get; set; }
    }

    public class MyExampleWithoutAnnotation
    {
        public Guid UniqueId { get; set; }

        public string SomeString { get; set; }
    }

    public class MyExampleWithIdProperty
    {
        public Guid Id { get; set; }

        public string SomeString { get; set; }
    }
}
