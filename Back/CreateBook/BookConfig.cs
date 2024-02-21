using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Syki.Back.CreateBook;

public class BookConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> book)
    {
        book.ToTable("books");

        book.HasKey(b => b.Id);
        book.Property(b => b.Id).ValueGeneratedNever();

        book.Property(b => b.Title);
    }
}
