using Microsoft.EntityFrameworkCore;

namespace LemonPlatform.Core.Databases.Quartz
{
    public readonly struct EntityTypeConfigurationContext
    {
        public EntityTypeConfigurationContext(ModelBuilder modelBuilder)
        {
            ModelBuilder = modelBuilder;
        }

        public ModelBuilder ModelBuilder { get; }
    }
}