namespace Calendar.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ReminderTime { get; set; }
        public bool IsNotified { get; set; }
    }
}
