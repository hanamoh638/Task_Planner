using Concepts_OOP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concepts_OOP
{
    public class Task
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTimeRetrieval DueDate { get; set; }
        public int Priority { get; set; }
        public string Status { get; set; }

        public Task(int id, string description, DateTimeRetrieval dueDate, int priority, string status)
        {
            Id = id;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
            Status = status;
        }

        public void Update(string status, int priority)
        {
            Status = status;
            Priority = priority;
        }

    }


}