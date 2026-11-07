using System.Collections.Generic;

namespace Entertainment.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }




        public List<Character> Characters { get; set; } = new List<Character>();
    }
    
}