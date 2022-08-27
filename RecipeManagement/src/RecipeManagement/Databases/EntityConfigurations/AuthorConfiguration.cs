namespace RecipeManagement.Databases.EntityConfigurations;

using RecipeManagement.Domain.Authors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    /// <summary>
    /// The database configuration for Authors. 
    /// </summary>
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        // example for a simple 1:1 value object
        // builder.Property(x => x.Percent)
        //     .HasConversion(x => x.Value, x => new Percent(x))
        //     .HasColumnName("percent");
        
        // example for a more complex value object
        // builder.OwnsOne(x => x.PhysicalAddress, opts =>
        // {
        //     opts.Property(x => x.Line1).HasColumnName("physical_address_line1");
        //     opts.Property(x => x.Line2).HasColumnName("physical_address_line2");
        //     opts.Property(x => x.City).HasColumnName("physical_address_city");
        //     opts.Property(x => x.State).HasColumnName("physical_address_state");
        //     opts.Property(x => x.PostalCode).HasColumnName("physical_address_postal_code")
        //         .HasConversion(x => x.Value, x => new PostalCode(x));
        //     opts.Property(x => x.Country).HasColumnName("physical_address_country");
        // }).Navigation(x => x.PhysicalAddress);
    }
}