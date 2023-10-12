using SardCoreAPI.Models.Units;
using System.Diagnostics;

namespace SardCoreAPI.Utility.Units
{
    public static class UnitConverter
    {
        public static UnitConversionResult Convert(UnitConversionRequest request, List<Unit> units)
        {
            GraphEdge[] edges = BuildGraph(units);
            GetShortestPath(edges, (int)request.UnitFrom.Id, (int)request.UnitTo.Id);
            return null;
        }

        private static GraphEdge[] BuildGraph(List<Unit> units)
        {
            var edges = new List<GraphEdge>();
            foreach (var unit in units)
            {
                if (unit.ParentId != null && unit.AmountPerParent != null && unit.Id != null)
                {
                    edges.Add(new GraphEdge((int)unit.ParentId, (int)unit.Id, (double)unit.AmountPerParent));
                }
            }
            return edges.ToArray();
        }

        private static List<GraphEdge> GetShortestPath(GraphEdge[] graph, int root, int target)
        {
            Dictionary<int, bool> visited = new Dictionary<int, bool>();

            Queue<int> queue = new Queue<int>();
            visited.Add(root, true);
            queue.Enqueue(root);

            while (queue.Any())
            {
                int current = queue.First();
                queue.Dequeue();

                int[] neighbors = GetNeighbors(graph, current);
                foreach (int neighbor in neighbors)
                {
                    bool visitedValue;
                    if (!(visited.TryGetValue(neighbor, out visitedValue) && visitedValue))
                    {
                        Debug.WriteLine("Hello");
                        visited.Add(neighbor, true);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return null;
        }

        private static int[] GetNeighbors(GraphEdge[] graph, int root)
        {
            List<int> neighbors = new List<int>();
            
            foreach (var edge in graph) 
            {
                if (edge.ParentId == root)
                {
                    neighbors.Add(edge.ChildId);
                }
                if (edge.ChildId == root)
                {
                    neighbors.Add(edge.ParentId);
                }
            }

            return neighbors.ToArray();
        }
    }
}
