[![Codacy Badge](https://api.codacy.com/project/badge/Grade/d1d4c2ef4ee5433ea91f874483b55119)](https://www.codacy.com/app/ThiagoBarradas/mongo-crud-dotnet?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=ThiagoBarradas/mongo-crud-dotnet&amp;utm_campaign=Badge_Grade)
[![Build status](https://ci.appveyor.com/api/projects/status/3ytedkys4s6h83k0/branch/master?svg=true)](https://ci.appveyor.com/project/ThiagoBarradas/mongo-crud-dotnet/branch/master)
[![codecov](https://codecov.io/gh/ThiagoBarradas/mongo-crud-dotnet/branch/master/graph/badge.svg)](https://codecov.io/gh/ThiagoBarradas/mongo-crud-dotnet)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Mongo.CRUD.svg)](https://www.nuget.org/packages/Mongo.CRUD/)
[![NuGet Version](https://img.shields.io/nuget/v/Mongo.CRUD.svg)](https://www.nuget.org/packages/Mongo.CRUD/)

# Mongo.CRUD

MongoCRUD is a high level library to make easy basic operations like create, update, update partial by query, upsert, delete, delete by query, get, search with paging and sorting, and filter buiders.

# Sample

Sample Entity Class
```c#

public class MyEntity
{
	[BsonId]
	public string MyId { get; set; }

	public string SomeProperty { get; set; }
}

```

Using MongoCRUD
```c#

var document = new MyEntity()
{
	MyId = "123",
	SomeProperty = "Something"
};

IMongoCRUD<MyEntity> client = new MongoCRUD<MyEntity>("mongodb://localhost", "MyDatabase");

client.Create(document);

document.SomeProperty = "Something2";
client.Update(document);

client.Delete(document);

```

## Install via NuGet

```
PM> Install-Package Mongo.CRUD
```

## How to use

:construction:

## How can I contribute?
Please, refer to [CONTRIBUTING](.github/CONTRIBUTING.md)

## Found something strange or need a new feature?
Open a new Issue following our issue template [ISSUE_TEMPLATE](.github/ISSUE_TEMPLATE.md)

## Changelog
See in [nuget version history](https://www.nuget.org/packages/Mongo.CRUD)

## Did you like it? Please, make a donate :)

if you liked this project, please make a contribution and help to keep this and other initiatives, send me some Satochis.

BTC Wallet: `1G535x1rYdMo9CNdTGK3eG6XJddBHdaqfX`

![1G535x1rYdMo9CNdTGK3eG6XJddBHdaqfX](https://i.imgur.com/mN7ueoE.png)
