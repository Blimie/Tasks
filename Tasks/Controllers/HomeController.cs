using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Tasks.Data;
using Tasks.Models;

namespace Tasks.Controllers
{
    public class HomeController : Controller
    {
        UsersRepository usersRepo = new UsersRepository(Properties.Settings.Default.ConStr);
        TasksRepository tasksRepo = new TasksRepository(Properties.Settings.Default.ConStr);

        [Authorize]
        public ActionResult Index()
        {            
            return View(new IndexViewModel
            {
                Tasks = tasksRepo.GetIncompletedTasks(),
                CurrentUser = usersRepo.GetByEmail(User.Identity.Name)
            });
        }
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(User user, string password)
        {
            usersRepo.AddUser(user, password);
            return RedirectToAction("Index");
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            User user = usersRepo.Login(email, password);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            FormsAuthentication.SetAuthCookie(email, true);
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}