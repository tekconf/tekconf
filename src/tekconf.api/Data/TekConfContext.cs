using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using TekConf.Api.Data.Models;

namespace TekConf.Api.Data
{
    public class TekConfContext : DbContext
    {
        private DbContextTransaction _currentTransaction;


        public TekConfContext() : base("TekConfContext")
        {
            
        }
        public TekConfContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public DbSet<Conference> Conferences { get; set; }
        public DbSet<ConferenceInstance> ConferenceInstances { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Presentation> Presentations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Session>().HasRequired(p => p.ConferenceInstance);
            modelBuilder.Entity<ConferenceInstance>().HasRequired(p => p.Conference);
            modelBuilder.Entity<Session>().HasRequired(p => p.Presentation);
            modelBuilder.Entity<Speaker>().HasRequired(p => p.User);
            modelBuilder.Entity<Conference>().HasRequired(p => p.Owner);
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
            //modelBuilder
            //    .Entity<Conference>()
            //    .Property(x => x.Slug)
            //    .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Entity<CourseInstructor>().HasKey(ci => new { ci.CourseID, ci.InstructorID });

            //modelBuilder.Entity<Department>().MapToStoredProcedures();
        }

        public void BeginTransaction()
        {
            try
            {
                if (_currentTransaction != null)
                {
                    return;
                }

                _currentTransaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);
            }
            catch (Exception)
            {
                // todo: log transaction exception
                throw;
            }
        }

        public void CloseTransaction()
        {
            CloseTransaction(exception: null);
        }

        public void CloseTransaction(Exception exception)
        {
            try
            {
                if (_currentTransaction != null && exception != null)
                {
                    // todo: log exception
                    _currentTransaction.Rollback();
                    return;
                }

                SaveChanges();

                if (_currentTransaction != null)
                {
                    _currentTransaction.Commit();
                }
            }
            catch (Exception)
            {
                // todo: log exception
                if (_currentTransaction != null && _currentTransaction.UnderlyingTransaction.Connection != null)
                {
                    _currentTransaction.Rollback();
                }

                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

    }
}