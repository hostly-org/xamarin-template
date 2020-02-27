using System;
using System.Collections.Generic;
using System.Text;

namespace Example.Mobile.EntityFrameworkCore.DbContexts
{
    public interface IDbContextFactory
    {
        ExampleMobileDbContext Create();
    }
}
