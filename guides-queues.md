---
title: Queues
layout: api 
---
# Queues

When managing the messages for queues you need to enter the relevant queue uri.  To facilitate this process you can configure a list of queue uris for use by using the queue management module:

![Queues Image]({{ site.baseurl }}/assets/images/queues.png "Queues")

In order to store the queue uris we need **somewhere** to store them.  This is configured by using a repository.  There are two implementations of the `IQueueRepository` interface:

- `SqlQueueRepository`
- `XmlQueueRepository`

### SqlQueueRepository

The `SqlQueueRepository` requires a Sql Server connection string named *SqlQueueRepository* that connects to a database with a structure defined by the sql script `shuttle.sql` that you will find under the *database* folder.

The full application configuration file would be the following:

``` xml
<?xml version="1.0"?>
<configuration>
	<appSettings>
		<add key="QueueRepositoryType" value="SqlQueueRepository"/>
	</appSettings>

	<connectionStrings>
		<clear/>
		<add name="SqlQueueRepository" connectionString="Data Source=.\sqlexpress; Initial Catalog=shuttle;Integrated Security=SSPI;" providerName="System.Data.SqlClient"/>
	</connectionStrings>
</configuration>
```

### XmlQueueRepository

The `XmlQueueRepository` persists the queue uris to an xml file specified in the application configuration file.

The full application configuration file would be the following:

``` xml
<?xml version="1.0"?>
<configuration>
	<appSettings>
		<add key="QueueRepositoryType" value="XmlQueueRepository"/>
		<add key="XmlQueueRepositoryPath" value="stores\queue.store"/>
	</appSettings>
</configuration>
```
