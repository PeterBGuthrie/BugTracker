using BugTracker.Helpers;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class AssignmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectHelper projectHelper = new ProjectHelper();

        #region Role Assignments
        // GET: Assignments
        [Authorize(Roles = "Admin")]
        public ActionResult ManageRoles()
        {
            // Use my ViewBag to hold a multi select list of Users in the system
            // New MultiSelectList(the data itself, How we are passing the data, What we display )
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");

            // Use my ViewBag to hold a select list of roles
            ViewBag.RoleName = new SelectList(db.Roles.Where(r => r.Name != "Admin"), "Name", "Name");
            return View(db.Users.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(List<string> userIds, String roleName)
        {
            if (userIds == null)
            {
                return RedirectToAction("RolesIndex");
            }

            foreach (var userId in userIds)
            {
                foreach (var role in roleHelper.ListUserRoles(userId).ToList())
                {
                    roleHelper.RemoveUserFromRole(userId, role);
                }

                if (!string.IsNullOrEmpty(roleName))
                {
                    roleHelper.AddUserToRole(userId, roleName);
                }

            }
            return RedirectToAction("ManageRoles");
        }


        // GET: Assignments
        public ActionResult ManageUserRoles()
        {
            return View();
        }

        public ActionResult RolesIndex()
        {
            return View();
        }

        #endregion

        #region Projects Assignments
        public ActionResult ManageProjectUsers()
        {
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");
            ViewBag.ProjectIds = new MultiSelectList(db.Projects, "Id", "Name");

            return View(db.Users.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjectUsers(List<string> userIds, List<int> projectIds)
        {
            if(userIds == null || projectIds == null)
            {
            return RedirectToAction("ManageProjectUsers");
            }

            foreach(var userId in userIds)
            {
                foreach(var projectId in projectIds)
                {
                    projectHelper.AddUserToProject(userId, projectId);
                }
            }

            return RedirectToAction("ManageProjectUsers");
        }
        #endregion
    }
}