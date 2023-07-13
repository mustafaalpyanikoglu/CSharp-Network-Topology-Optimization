List<Node> nodes = new Node().CreateNodes();
List<Edge> edges = new Edge().CreateEdges();

ACOAlgorithm acoAlgorithm = new ACOAlgorithm(nodes, edges);
List<int> shortestPath = acoAlgorithm.FindShortestPath();


foreach (int nodeId in shortestPath)
{
    Console.Write($"{nodes[nodeId].Label} -> ");
}