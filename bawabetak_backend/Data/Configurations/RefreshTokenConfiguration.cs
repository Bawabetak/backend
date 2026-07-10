
namespace bawabetak_backend.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
           builder.HasIndex(rt => rt.Token).IsUnique();
        }
    }
}
