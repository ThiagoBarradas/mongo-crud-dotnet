using Mongo.CRUD.Builders;
using Mongo.CRUD.Enums;
using Mongo.CRUD.Models;
using Mongo.CRUD.Tests.Fakes;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Linq;
using Xunit;

namespace Mongo.CRUD.Tests.Builders
{
    public class FilterBuilderTest
    {
        [Fact]
        public void WithSorting_By_Asc()
        {
            // arrange
            SearchOptions options = new SearchOptions()
            {
                PageNumber = 1,
                PageSize = 10,
                SortField = "MyCustomField",
                SortMode = SortMode.Asc
            };

            FakeFindFluent<SomeClass> findFluent = new FakeFindFluent<SomeClass>();

            // act
            var result = findFluent.WithSorting(options);
            var resultAsFakeType = (FakeFindFluent<SomeClass>) result;

            // assert
            Assert.NotNull(result);
            Assert.Single(resultAsFakeType.SortValues);
            Assert.Equal("MyCustomField", resultAsFakeType.SortValues.First().Key);
            Assert.Equal(SortMode.Asc, resultAsFakeType.SortValues.First().Value);
        }

        [Fact]
        public void WithSorting_By_Desc()
        {
            // arrange
            SearchOptions options = new SearchOptions()
            {
                PageNumber = 1,
                PageSize = 10,
                SortField = "MyCustomField2",
                SortMode = SortMode.Desc
            };

            FakeFindFluent<SomeClass> findFluent = new FakeFindFluent<SomeClass>();

            // act
            var result = findFluent.WithSorting(options);
            var resultAsFakeType = (FakeFindFluent<SomeClass>)result;

            // assert
            Assert.NotNull(result);
            Assert.Single(resultAsFakeType.SortValues);
            Assert.Equal("MyCustomField2", resultAsFakeType.SortValues.First().Key);
            Assert.Equal(SortMode.Desc, resultAsFakeType.SortValues.First().Value);
        }

        [Fact]
        public void WithSorting_With_Empty_Field()
        {
            // arrange
            SearchOptions options = new SearchOptions()
            {
                PageNumber = 1,
                PageSize = 10,
                SortField = "",
                SortMode = SortMode.Asc
            };

            FakeFindFluent<SomeClass> findFluent = new FakeFindFluent<SomeClass>();

            // act
            var result = findFluent.WithSorting(options);
            var resultAsFakeType = (FakeFindFluent<SomeClass>)result;

            // assert
            Assert.NotNull(result);
            Assert.Null(resultAsFakeType.SortValues);
        }

        [Fact]
        public void WithPaging_Calculated()
        {
            // arrange
            SearchOptions options = new SearchOptions()
            {
                PageNumber = 4,
                PageSize = 6,
                SortField = "MyCustomField2",
                SortMode = SortMode.Desc
            };

            FakeFindFluent<SomeClass> findFluent = new FakeFindFluent<SomeClass>();

            // act
            var result = findFluent.WithPaging(options);
            var resultAsFakeType = (FakeFindFluent<SomeClass>)result;

            // assert
            Assert.NotNull(result);
            Assert.NotNull(resultAsFakeType.LimitValue);
            Assert.NotNull(resultAsFakeType.SkipValue);
            Assert.Equal(18, resultAsFakeType.SkipValue);
            Assert.Equal(6, resultAsFakeType.LimitValue);
        }

        [Fact]
        public void Join_Should_Return_Null_When_Both_Filters_Are_Null()
        {
            // arrange
            FilterDefinition<SomeClass> filter1 = null;
            FilterDefinition<SomeClass> filter2 = null;

            // act
            var result = filter1.Join(filter2);

            // assert
            Assert.Null(result);
        }

        [Fact]
        public void Join_Should_Return_First_Filter_When_Second_Filter_Is_Null()
        {
            // arrange
            var builder = FilterBuilder.GetFilterBuilder<SomeClass>();
            var serializerRegistry = BsonSerializer.SerializerRegistry;
            var documentSerializer = serializerRegistry.GetSerializer<SomeClass>();

            FilterDefinition<SomeClass> filter1 = builder.Eq(r => r.OtherProperty, "somevalue");
            FilterDefinition<SomeClass> filter2 = null;

            // act
            var result = filter1.Join(filter2);

            // assert
            var rendered = result?.Render(documentSerializer, serializerRegistry);
            Assert.NotNull(result);
            Assert.NotNull(rendered);
            Assert.Single(rendered);
            Assert.Equal("OtherProperty", rendered.First().Name);
            Assert.Equal("somevalue", rendered.First().Value);
        }

        [Fact]
        public void Join_Should_Return_Second_Filter_When_First_Filter_Is_Null()
        {
            // arrange
            var builder = FilterBuilder.GetFilterBuilder<SomeClass>();
            var serializerRegistry = BsonSerializer.SerializerRegistry;
            var documentSerializer = serializerRegistry.GetSerializer<SomeClass>();

            FilterDefinition<SomeClass> filter1 = null;
            FilterDefinition<SomeClass> filter2 = builder.Eq(r => r.SomeProperty, "value");

            // act
            var result = filter1.Join(filter2);

            // assert
            var rendered = result?.Render(documentSerializer, serializerRegistry);
            Assert.NotNull(result);
            Assert.NotNull(rendered);
            Assert.Single(rendered);
            Assert.Equal("SomeProperty", rendered.First().Name);
            Assert.Equal("value", rendered.First().Value);
        }

        [Fact]
        public void Join_Should_Return_And_Operation_With_Two_Filters()
        {
            // arrange
            var builder = FilterBuilder.GetFilterBuilder<SomeClass>();
            var serializerRegistry = BsonSerializer.SerializerRegistry;
            var documentSerializer = serializerRegistry.GetSerializer<SomeClass>();

            FilterDefinition<SomeClass> filter1 = builder.Eq(r => r.OtherProperty, "somevalue");
            FilterDefinition<SomeClass> filter2 = builder.Eq(r => r.SomeProperty, "value");

            // act
            var result = filter1.Join(filter2);

            // assert
            var rendered = result?.Render(documentSerializer, serializerRegistry);
            var renderedString = rendered.ToString();
            Assert.NotNull(result);
            Assert.NotNull(rendered);
            Assert.Equal(2, rendered.Count());
            Assert.Equal(
                "{ \"OtherProperty\" : \"somevalue\", \"SomeProperty\" : \"value\" }", 
                renderedString);
            Assert.Equal("somevalue", rendered.First().Value);
            Assert.Equal("SomeProperty", rendered.Last().Name);
            Assert.Equal("value", rendered.Last().Value);
        }

        [Fact]
        public void Join_Should_Return_Or_Operation_With_Two_Filters()
        {
            // arrange
            var builder = FilterBuilder.GetFilterBuilder<SomeClass>();
            var serializerRegistry = BsonSerializer.SerializerRegistry;
            var documentSerializer = serializerRegistry.GetSerializer<SomeClass>();

            FilterDefinition<SomeClass> filter1 = builder.Eq(r => r.OtherProperty, "somevalue");
            FilterDefinition<SomeClass> filter2 = builder.Eq(r => r.SomeProperty, "value");

            // act
            var result = filter1.Join(filter2, Operator.Or);

            // assert
            var rendered = result?.Render(documentSerializer, serializerRegistry);
            var renderedString = rendered.ToString();
            Assert.NotNull(result);
            Assert.NotNull(rendered);
            Assert.Single(rendered);
            Assert.Equal(
                "{ \"$or\" : [{ \"OtherProperty\" : \"somevalue\" }, { \"SomeProperty\" : \"value\" }] }", 
                renderedString);
        }     
    }

    public class SomeClass 
    {
        public string SomeProperty { get; set; }

        public string OtherProperty { get; set; }
    }
}
