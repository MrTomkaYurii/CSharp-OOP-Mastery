namespace Entertainment.Models
{
    public class Character
    {
        public int Id { get; set; }
        public int ProductionId { get; set; }
        public Production Production { get; set; }
        public string Name { get; set; }
        public Actor Actor { get; set; }
        public int ActorId { get; set; }
    }
    
}