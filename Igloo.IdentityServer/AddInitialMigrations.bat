dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Migrations/PersistedGrantDb
dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Migrations/ConfigurationDb
dotnet ef migrations add InitialIdentityServerApplicationDbMigration -c ApplicationDbContext -o Migrations/ApplicationDb