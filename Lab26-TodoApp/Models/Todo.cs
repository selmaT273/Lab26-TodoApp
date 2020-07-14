using System;
using System.ComponentModel.DataAnnotations;

namespace Lab26_TodoApp.Models
{
    public class Todo
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime DueDate { get; set; }

        public string Assignee { get; set; }

        public int Difficulty { get; set; }

        public string CreatedByUserId { get; set; }

        public DateTime CreatedByTimestamp { get; set; }

        public string ModifiedByUserId { get; set; }

        public DateTime? ModifiedByTimestamp { get; set; }
    }

    public class TodoDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }
    }
}
