namespace People365ToDoList.Models
{
    public class ToDoList
    {
        public int id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int Priority { get; set; }
        public bool isDeleted { get; set; }
    }
}
