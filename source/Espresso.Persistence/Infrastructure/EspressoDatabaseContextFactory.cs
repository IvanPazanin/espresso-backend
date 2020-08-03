﻿using Espresso.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Espresso.Persistence.Infrastructure
{
    public class EspressoDatabaseContextFactory : DesignTimeDatabaseContextFactoryBase<ApplicationDatabaseContext>
    {
        public EspressoDatabaseContextFactory()
            : base() { }

        protected override ApplicationDatabaseContext CreateNewInstance(DbContextOptions<ApplicationDatabaseContext> options)
        {
            return new ApplicationDatabaseContext(options);
        }
    }
}
