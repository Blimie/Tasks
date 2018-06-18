using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Data
{
    public class TasksRepository
    {
        private string _connectionString;

        public TasksRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddTask(Task task)
        {
            using (var context = new TasksDataContext(_connectionString))
            {
                context.Tasks.InsertOnSubmit(task);
                context.SubmitChanges();
            }
        }
        public IEnumerable<Task> GetIncompletedTasks()
        {
            using (var context = new TasksDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Task>(t => t.User);
                context.LoadOptions = loadOptions;
                return context.Tasks.Where(t => !t.Completed).ToList();
            }
        }
        public void SetUserForTask(int taskId, int userId)
        {
            using (var context = new TasksDataContext(_connectionString))
            {
                context.ExecuteCommand("UPDATE Tasks SET UserId = {1} where Id = {0}", taskId, userId);
            }
        }
        public void SetTaskCompleted(int taskId)
        {
            using (var context = new TasksDataContext(_connectionString))
            {
                context.ExecuteCommand("UPDATE Tasks SET Completed = 1 where Id = {0}", taskId);
            }
        }
        public Task GetById(int id)
        {
            using (var context = new TasksDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Task>(t => t.User);
                context.LoadOptions = loadOptions;
                return context.Tasks.First(i => i.Id == id);
            }
        }
    }
}
