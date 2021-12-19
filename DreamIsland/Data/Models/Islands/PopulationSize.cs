namespace DreamIsland.Data.Models.Islands
{
    using System.Collections.Generic;

    public class PopulationSize
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<Island> Islands { get; set; }
    }
}
