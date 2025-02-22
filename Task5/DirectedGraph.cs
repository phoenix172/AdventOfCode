namespace Task5;

public class DirectedGraph<T>
    where T:IEquatable<T>
{
    private readonly HashSet<Vertex> _vertices;
    private readonly HashSet<Edge> _edges;
    
    public DirectedGraph(params IEnumerable<(T From,T To)> edges)
    {
        _edges = new HashSet<Edge>();
        _vertices = new HashSet<Vertex>();
        foreach (var inputEdge in edges)
        {
            var from = new Vertex(inputEdge.From);
            var to = new Vertex(inputEdge.To);
            _edges.Add(new Edge(from, to));
            _vertices.Add(from);
            _vertices.Add(to);
        }
    }

    public IEnumerable<T> TopologicalSort()
    {
        var outgoingEdges = _edges.ToLookup(x => x.From);
        var incomingEdges = _edges.ToLookup(x => x.To);
        var sourceLookup = _vertices.ToDictionary(x => x, x => outgoingEdges[x].ToHashSet());
        var destinationLookup = _vertices.ToDictionary(x => x, x => incomingEdges[x].ToHashSet());
        
        var sources = destinationLookup.Where(x => x.Value.Count == 0).Select(x=>x.Key);
        PriorityQueue<Vertex, T> unvisited = new(sources.Select(x=> (x, x.Value)) );
        while (unvisited.Count > 0)
        {
            var unvisitedVertex = unvisited.Dequeue();
            yield return unvisitedVertex.Value;
            
            var edgesToRemove = new List<Edge>();
            
            foreach (Edge outgoingEdge in sourceLookup[unvisitedVertex])
            {
                var enteringEdges = destinationLookup[outgoingEdge.To];
                enteringEdges.Remove(outgoingEdge);
                edgesToRemove.Add(outgoingEdge);
                if (enteringEdges.Count == 0)
                    unvisited.Enqueue(outgoingEdge.To, outgoingEdge.To.Value);
            }

            edgesToRemove.ForEach(edge => sourceLookup[unvisitedVertex].Remove(edge));
        }

        if (sourceLookup.Any(x => x.Value.Count > 0) && unvisited.Count == 0)
        {
            throw new InvalidOperationException("Unable to topologically sort graph because a cycle was detected");
        }
    }
    
    private record Vertex(T Value);

    private record Edge(Vertex From, Vertex To);
}