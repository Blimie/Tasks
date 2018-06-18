using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tasks.Data;

namespace Tasks
{
    public class TasksHub : Hub
    {
        TasksRepository taskRepo = new TasksRepository(Properties.Settings.Default.ConStr);
        UsersRepository userRepo = new UsersRepository(Properties.Settings.Default.ConStr);
       
        public void AddTask(Task task)
        {
            taskRepo.AddTask(task);
            Clients.All.newTaskAdded(task);
        }
        public void SetUserForTask(int id)
        {
            string email = Context.User.Identity.Name;
            var user = userRepo.GetByEmail(email);
            taskRepo.SetUserForTask(id, user.Id);
            Task task = taskRepo.GetById(id);
            Clients.All.userWasSet(new { UserName = task.User.Name, Id = task.Id, UserId = task.UserId });
        }
        public void SetTaskCompleted(int id)
        {
            taskRepo.SetTaskCompleted(id);
            Clients.All.taskCompleted(id);
        }         
    }
}