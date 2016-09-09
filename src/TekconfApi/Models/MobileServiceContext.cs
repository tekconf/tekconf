using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using TekconfApi.DataObjects;

namespace TekconfApi.Models
{
    public class MobileServiceContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to alter your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
        //
        // To enable Entity Framework migrations in the cloud, please ensure that the 
        // service name, set by the 'MS_MobileServiceName' AppSettings in the local 
        // Web.config, is the same as the service name when hosted in Azure.

        private const string connectionStringName = "Name=MS_TableConnectionString";

        public MobileServiceContext() : base(connectionStringName)
        {
        }

        public DbSet<Conference> Conferences { get; set; }
        //public DbSet<ConferenceInstance> ConferenceInstances { get; set; }
        //public DbSet<Speaker> Speakers { get; set; }
        //public DbSet<Session> Sessions { get; set; }
        //public DbSet<Presentation> Presentations { get; set; }
        //public DbSet<User> Users { get; set; }
        //public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Session>().HasRequired(p => p.ConferenceInstance);
            modelBuilder.Entity<ConferenceInstance>().HasRequired(p => p.Conference);
            modelBuilder.Entity<Session>().HasRequired(p => p.Presentation);
            modelBuilder.Entity<Speaker>().HasRequired(p => p.User);
            //modelBuilder.Entity<Conference>().HasRequired(p => p.Owner);
            modelBuilder.Entity<Tag>().HasRequired(p => p.ConferenceInstance);

            modelBuilder.Entity<Session>()
                .HasMany(p => p.Speakers)
                .WithMany(t => t.Sessions)
                .Map(mc =>
                {
                    mc.ToTable("SessionSpeakers");
                    mc.MapLeftKey("SessionId");
                    mc.MapRightKey("SpeakerId");

                });

            modelBuilder.Entity<Session>()
                .HasMany(p => p.Tags)
                .WithMany(t => t.Sessions)
                .Map(mc =>
                {
                    mc.ToTable("SessionTags");
                    mc.MapLeftKey("SessionId");
                    mc.MapRightKey("TagId");

                });

            modelBuilder.Entity<User>()
                    .HasMany(p => p.Presentations)
                    .WithMany(t => t.Owners)
                    .Map(mc =>
                    {
                        mc.ToTable("UserPresentations");
                        mc.MapLeftKey("UserId");
                        mc.MapRightKey("PresentationId");
                    });
        }
    }
}
