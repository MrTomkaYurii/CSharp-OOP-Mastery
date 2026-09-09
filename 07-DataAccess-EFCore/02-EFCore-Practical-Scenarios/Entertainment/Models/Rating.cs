namespace Entertainment.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int ProductionId { get; set; }
        public Production Production { get; set; }
        public string Source { get; set; }
        public int Stars { get; set; }
    }
    
}