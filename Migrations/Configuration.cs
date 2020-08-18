namespace BugTracker.Migrations
{
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
               new RoleStore<IdentityRole>(context));

            // See if a role is present in the DB
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "ProjectManager"});
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            // Creates a users
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            // Seed new users roles
            if (!context.Users.Any(u => u.Email == "peterbguthrie@gmail.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "peterbguthrie@gmail.com",
                    UserName = "peterbguthrie@gmail.com",
                    FirstName = "Peter",
                    LastName = "Guthrie"
                }, "Abc&123!");
            }

            //Grab the ID
            var userId = userManager.FindByEmail("peterbguthrie@gmail.com").Id;
            // Now that I have created a user I want to assign this person to a role
            userManager.AddToRole(userId, "Admin");


            // Adding Drew as a Project Manager
            if (!context.Users.Any(u => u.Email == "arussell@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "arussell@coderfoundry.com",
                    UserName = "arussell@coderfoundry.com",
                    FirstName = "Andrew",
                    LastName = "Russell"
                }, "Abc&123!");
            }
            userId = userManager.FindByEmail("arussell@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Project Manager");

            // Seeding a Developer
            if (!context.Users.Any(u => u.Email == "davethedev@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "davethedev@coderfoundry.com",
                    UserName = "davethedev@coderfoundry.com",
                    FirstName = "Dave",
                    LastName = "Dev"
                }, "Abc&123!");
            }
            userId = userManager.FindByEmail("davethedev@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Developer");

            // Seeding a Submitter
            if (!context.Users.Any(u => u.Email == "hillary@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "hillary@coderfoundry.com",
                    UserName = "hillary@coderfoundry.com",
                    FirstName = "Hillary's",
                    LastName = "Clinton"
                }, "Abc&123!");
            }
            userId = userManager.FindByEmail("hillary@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Submitter");

            context.SaveChanges();
            #region TicketType Seed
            context.TicketTypes.AddOrUpdate(
                tt => tt.Name,
                new TicketType() { Name = "Software" },
                new TicketType() { Name = "Hardware" },
                new TicketType() { Name = "UI" },
                new TicketType() { Name = "Defect" },
                new TicketType() { Name = "Feature Request" },
                new TicketType() { Name = "Other" }
                );
            #endregion

            #region TicketPriority Seed
            context.TicketPriorities.AddOrUpdate(
                tp => tp.Name,
                new TicketPriority() { Name = "Low" },
                new TicketPriority() { Name = "Medium" },
                new TicketPriority() { Name = "High" },
                new TicketPriority() { Name = "On Hold" }
                );
            #endregion

            #region TicketStatus Seed
            context.TicketStatuses.AddOrUpdate(
                ts => ts.Name,
                new TicketStatus() { Name = "Open" },
                new TicketStatus() { Name = "Assigned" },
                new TicketStatus() { Name = "Resolved" },
                new TicketStatus() { Name = "Reopened" },
                new TicketStatus() { Name = "Reopened" }
                );
            #endregion
            #region Project Seed
            context.Projects.AddOrUpdate(
                p => p.Name,
                new Project() { Name = "Seed 1", Created = DateTime.Now.AddDays(-60), IsArchived = true },
                new Project() { Name = "Seed 2", Created = DateTime.Now.AddDays(-45), IsArchived = false },
                new Project() { Name = "Seed 3", Created = DateTime.Now.AddDays(-30), IsArchived = false },
                new Project() { Name = "Seed 4", Created = DateTime.Now.AddDays(-15), IsArchived = false },
                new Project() { Name = "Seed 5", Created = DateTime.Now.AddDays(-7), IsArchived = false }
                );
            #endregion
            context.SaveChanges();
        }
    }
}
