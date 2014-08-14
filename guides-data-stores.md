---
title: Data Stores
layout: api 
---
# Data Stores

When managing the subscriptions need access to a data store.  To facilitate this process you can configure a list of data store for use through the data store management module:

![Data Store Image]({{ site.baseurl }}/assets/images/queues.png "Queues")

In order to store the data store entries we need **somewhere** to store them.  This is configured by using a repository.  There are two implementations of the `IDataStoreRepository` interface:

- `XmlDataStoreRepository`
- `SqlDataStoreRepository`

### SqlDataStoreRepository

The `SqlDataStoreRepository` requires a Sql Server connection string named *SqlDataStoreRepository* that connects to a database with a structure defined by the sql script `shuttle.sql` that you will find under the *database* folder.

The full application configuration file would be the following:

``` xml
<?xml version="1.0"?>
<configuration>
	<appSettings>
		<add key="QueueRepositoryType" value="SqlDataStoreRepository"/>
	</appSettings>

	<connectionStrings>
		<clear/>
		<add name="SqlDataStoreRepository" connectionString="Data Source=.\sqlexpress; Initial Catalog=shuttle;Integrated Security=SSPI;" providerName="System.Data.SqlClient"/>
	</connectionStrings>
</configuration>
```

### XmlDataStoreRepository

The `XmlDataStoreRepository` persists the data store connection information to an xml file specified in the application configuration file.

The full application configuration file would be the following:

``` xml
<?xml version="1.0"?>
<configuration>
	<appSettings>
		<add key="DataStoreRepositoryType" value="XmlDataStoreRepository"/>
		<add key="XmlDataStoreRepositoryPath" value="stores\datastore.store"/>
	</appSettings>
</configuration>
```
