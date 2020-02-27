# Migrations

**Migrations are automatically run before the app starts up**  
(See `Startup.cs`, and the corrosponding `SeedDatabase` extension method for the implementation)

**The app will only start once the migrations are complete.**

1. [Adding new migrations using the package manager console](Addingnewmigrationsusingthepackagemanagerconsole)
    * 1.1. [ExampleMobileDbContext](#ExampleMobileDbContext-package-manager)
2. [Adding new migrations using the command line](#Addingnewmigrationsusingthecommandline)
    * 2.1. [ExampleMobileDbContext](#ExampleMobileDbContext-cli)

##  1. <a name='Addingnewmigrationsusingthepackagemanagerconsole'></a>Adding new migrations using the package manager console
Ensure that you have the `Example.Mobile.Migrations` project as your start up project otherwise it will fail to create migrations.
####  1.1. <a name='ExampleMobileDbContext-package-manager'></a>ExampleMobileDbContext

```
Add-Migration <MigrationName> -c ExampleMobileDbContext -p Example.Mobile.EntityFrameworkCore -o Migrations/ExampleMobile
```
##  2. <a name='Addingnewmigrationsusingthecommandline'></a>Adding new migrations using the command line
Ensure that you have the `Example.Mobile.Migrations` project as your start up project otherwise it will fail to create migrations.

####  2.1. <a name='ExampleMobileDbContext-cli'></a>ExampleMobileDbContext

```
dotnet ef migrations add <MigrationName> -c ExampleMobileDbContext -p Example.Mobile.EntityFrameworkCore -o Migrations/ExampleMobile
```
