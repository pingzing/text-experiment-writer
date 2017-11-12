using System.Collections.Generic;

namespace TextGameTool.Models
{
    public class Dialogue
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public IEnumerable<string> DirectedNeighborRelativePaths { get; set; }
        public IEnumerable<string> UndirectedNeighborRelativePaths { get; set; }
        public string RelativeDialoguePath { get; set; }
    }
}
