using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Iso.Data.DbContexts;

public class GameDbContextFactory: IDesignTimeDbContextFactory<GameDbContext>
{
    public GameDbContext CreateDbContext(string[] args)
    {
        string basePath = Path.Combine(Directory.GetCurrentDirectory(), "../API");
        
        IConfigurationRoot configurationRoot = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .Build();
        
        DbContextOptionsBuilder<GameDbContext> optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
        optionsBuilder.UseNpgsql(configurationRoot.GetConnectionString("DbConnectionString"));
        
        return new GameDbContext(optionsBuilder.Options);
    }
}