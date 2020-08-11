using BugTracker.Helper;
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
        // GET: Assignments
        [Authorize(Roles = "Admin")]
        public ActionResult ManageRoles()
        {
            // Use my ViewBag to hold a multi select list of Users in the system
            // New MultiSelectList(db.Users.ToList())
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");


            // Use my ViewBag to hold a select list Roles
            ViewBag.RoleName = new SelectList(db.Roles, "Name", "Name");
            return View(db.Users.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(List<string> userIds, string roleName)
        {
            if (userIds == null)
                return RedirectToAction("RolesIndex");

            foreach(var userId in userIds)
            {
                foreach (var role in roleHelper.ListUserRoles(userId).ToList())
                {
                    roleHelper.RemoveUserFromRole(userId, role);
                }

                if(!string.IsNullOrEmpty(roleName))
                {
                    roleHelper.AddUserToRole(userId, roleName);
                }
            }


            return RedirectToAction("ManageRoles");
        }



        public ActionResult ManageUserRoles()
        {
            return View();
        }
    }
}