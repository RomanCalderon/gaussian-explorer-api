using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Microsoft.Extensions.Logging;

namespace Persistence.Utility;

public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger _logger;

    public DatabaseInitializer(ApplicationDbContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        if ((await _context.Database.GetPendingMigrationsAsync()).Any())
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (Exception ex) when (ex.Message.Contains("already exists"))
            {
                _logger.LogInformation("Database already exists. Continuing...");
            }
        }
    }
}
