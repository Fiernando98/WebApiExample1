namespace WebApplication1.Models {
    public class Food {
        public long ID { get; set; }
        public Restaurant? Restaurant { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Calories { get; set; }
    }
}
