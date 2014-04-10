# ClickOnceDM

ClickOnceDM is a mass email sending system. This program is written by C# on Microsoft.NET framework. It supports history, HTML preview and multiple SMTP server binding for fast sending. This program also supports retrieving a recipients list on remote databases (MySQL, SQL Server, etc..). Currently only support data source from MySQL and manual text.

## Requirements

### Microsoft Enterprise Library

[Microsoft Enterprise Library 5.0 Download](http://www.microsoft.com/en-us/download/details.aspx?id=15104)

### System.Data.SQLite

[System.Data.SQLite Download](http://system.data.sqlite.org/index.html/doc/trunk/www/downloads.wiki)

or Use NuGet Package.

## Composition

This project has two sub-projects. These two projects based on Microsoft .NET Framework and written using C#. It is ClickOnceDMAdmin and ClickOnceDMService. ClickOnceDMAdmin is web application that can manage mail sending, log and status. ClickOnceDMService is service application that can install in Windows OS. You can install this service using Installutil.exe. (see more http://msdn.microsoft.com/en-us/library/50614e95(v=vs.110).aspx)

## Configuration

### App.config in ClickOnceDMService

#### AppSettings

```
<appSettings>
  <add key="SMTPServer" value="smtp.server.com:25"/>
  <add key="Workspace" value="C:\Workspace\"/>
  <add key="BlockCount" value="200"/>
  <add key="BlockSleep" value="3000"/>
  <add key="StatisticsDatabase" value="C:\ClickOnceDM\db\Statistics.db" />
</appSettings>
```

* SMTPServer: SMTP Server and Post list. It can be multiple SMTP Servers separated by semicolon(;).
* Workspace: Workspace is managed directory by this application. It can be contained queue file, success and error logs.
* BlockCount: Sending count per one program loop.
* BlockSleep: Delay time after sending. (Millisecond)

### Web.config in ClickOnceDMAdmin

#### Plugins

```
<plugins>
  <add name="None" source="Liternal" value="" />
  <add name="Test User" source="Liternal" value="user@domain.com" />
  <add name="Total User" source="Member" value="select userName as name, userEmail as address from Member'" />
</plugins>
```

#### AppSettings

```
<appSettings>
  <add key="Workspace" value="C:\Workspace\" />
  <add key="Database" value="C:\ClickOnceDM\db\ClickOnceDM.db" />
  <add key="StatisticsDatabase" value="C:\ClickOnceDM\db\Statistics.db" />
</appSettings>
```

* Workspace: Workspace is managed directory by this application. It can be contained queue file, success and error logs. It must be same with App.config setting.
* Database: SQLite log database file path.

#### ConnectionStrings

ConnectionString is related to Plugin section. The source value of Plugin section reference this setting.

```
<connectionStrings>
  <add name="Member"
       providerName="System.Data.SqlClient"
       connectionString="Data Source=db;Initial Catalog=db;Integrated Security=False;User Id=user;Password=passwd;MultipleActiveResultSets=True" />
  <add name="Liternal"
       providerName="System.String[]"
       connectionString="NODATA" />
</connectionStrings>
```

## License

ClickOnceDM is available under the terms of the [MIT License](https://github.com/jongha/clickoncedm/blob/master/LICENSE).
