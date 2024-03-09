using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SardCoreAPI.Models.Units
{
    public class GraphEdge
    {
        public int ParentId { get; set; }
        public int ChildId { get; set; }
        public double ConversionAmount { get; set; }

        public GraphEdge(int ParentId, int ChildId, double ConversionAmount) { 
        
        }
    }
}
