using Jobsity.Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jobsity.Chat.Data.Mapping
{
    public class MessageMap : IEntityTypeConfiguration<MessageEntity>
    {
        public void Configure(EntityTypeBuilder<MessageEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Message)
                .IsRequired();

            builder.HasOne(a => a.User).WithMany(a => a.Messages);
        }
    }
}
