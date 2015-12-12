using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System;
using System.Runtime.Serialization;
using System.Configuration;
using System.Linq;

namespace Chreytli.Api.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    [DataContract]
    public class ApplicationUser : IdentityUser
    {
        [DataMember]
        public override string Id { get { return base.Id; } set { base.Id = value; } }

        [DataMember]
        public override string UserName { get { return base.UserName; } set { base.UserName = value; } }

        [DataMember]
        public DateTime CreateDate { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DataContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            var context = new ApplicationDbContext();

            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var user = new ApplicationUser
            {
                Email = ConfigurationManager.AppSettings["AdminEmail"],
                UserName = ConfigurationManager.AppSettings["AdminUsername"],
                CreateDate = DateTime.Now
            };

            userManager.Create(user, ConfigurationManager.AppSettings["AdminPassword"]);
            roleManager.Create(new IdentityRole("Admins"));

            var admin = userManager.Users.FirstOrDefault(x => x.UserName == user.UserName);

            if (admin != null)
            {
                userManager.AddToRole(admin.Id, "Admins");
            }

            return context;
        }

        public DbSet<Submission> Submissions { get; set; }

        public DbSet<Favorite> Favorites { get; set; }

        public DbSet<Poll> Polls { get; set; }

        public DbSet<Vote> Votes { get; set; }

        public DbSet<Choice> Choices { get; set; }

        public DbSet<VoteChoice> VoteChoices { get; set; }

        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Properties<int>()
                .Where(x => x.Name == "Id")
                .Configure(x => x.IsKey());

            // Submission /
            modelBuilder.Entity<Submission>()
                .HasRequired(x => x.Author);

            modelBuilder.Entity<Submission>()
                .Ignore(x => x.IsFavorite);

            // Favorite //
            modelBuilder.Entity<Favorite>()
                .HasRequired(x => x.Submission);

            modelBuilder.Entity<Favorite>()
                .HasRequired(x => x.User);

            modelBuilder.Entity<Favorite>()
                .HasKey(x => new { x.UserId, x.SubmissionId });

            modelBuilder.Entity<Favorite>()
                .HasRequired(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Favorite>()
                .HasRequired(x => x.Submission)
                .WithMany()
                .HasForeignKey(x => x.SubmissionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Favorite>()
                .Property(x => x.UserId)
                .HasColumnOrder(0);

            modelBuilder.Entity<Favorite>()
                .Property(x => x.SubmissionId)
                .HasColumnOrder(1);

            // Poll //
            modelBuilder.Entity<Poll>()
                .HasRequired(x => x.Author);

            modelBuilder.Entity<Poll>()
                .HasMany(x => x.Choices)
                .WithRequired(x => x.Poll)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Poll>()
                .Ignore(x => x.IsVoted);

            // Vote //
            modelBuilder.Entity<Vote>()
                .HasMany(x => x.VoteChoices)
                .WithRequired(x => x.Vote)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vote>()
                .HasRequired(x => x.Poll)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vote>()
                .HasRequired(x => x.User);

            // Choice //
            modelBuilder.Entity<Choice>()
                .HasRequired(x => x.Poll);

            // VoteChoice //
            modelBuilder.Entity<VoteChoice>()
                .HasKey(x => new { x.VoteId, x.ChoiceId });

            modelBuilder.Entity<VoteChoice>()
                .HasRequired(x => x.Vote)
                .WithMany(x => x.VoteChoices)
                .HasForeignKey(x => x.VoteId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VoteChoice>()
                .HasRequired(x => x.Choice)
                .WithMany()
                .HasForeignKey(x => x.ChoiceId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VoteChoice>()
                .Property(x => x.VoteId)
                .HasColumnOrder(0);

            modelBuilder.Entity<VoteChoice>()
                .Property(x => x.ChoiceId)
                .HasColumnOrder(1);

            // Event //
            modelBuilder.Entity<Event>()
                .HasRequired(x => x.Author);
        }
    }
}