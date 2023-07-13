public class ACOAlgorithm
{
    private const int AntCount = 10; // Karınca sayısı
    private const double Alpha = 1; // İz feromonuna etki eden faktör
    private const double Beta = 5; // Uzaklığın tersine etki eden faktör
    private const double Evaporation = 0.5; // Feromon buharlaşma oranı
    private const double Q = 100; // Bir adımda bırakılan feromon miktarı
    private const double InitialPheromone = 0.1; // Başlangıçtaki feromon miktarı

    private Random _random;
    private List<Node> _nodes;
    private List<Edge> _edges;
    private double[,] _pheromoneMatrix;
    private double[,] _distanceMatrix;

    public ACOAlgorithm(List<Node> nodes, List<Edge> edges)
    {
        this._nodes = nodes;
        this._edges = edges;
        _random = new Random();
        _pheromoneMatrix = new double[nodes.Count, nodes.Count];
        _distanceMatrix = new double[nodes.Count, nodes.Count];
    }

    public List<int> FindShortestPath()
    {
        InitializePheromone();
        CalculateDistance();

        List<int> bestPath = null;
        double bestDistance = double.MaxValue;

        for (int iteration = 0; iteration < 100; iteration++)
        {
            List<List<int>> antPaths = new List<List<int>>();
            double[] antDistances = new double[AntCount];

            for (int ant = 0; ant < AntCount; ant++)
            {
                List<int> path = ConstructAntPath();
                double distance = CalculatePathDistance(path);

                antPaths.Add(path);
                antDistances[ant] = distance;

                if (distance < bestDistance)
                {
                    bestPath = path;
                    bestDistance = distance;
                }
            }

            UpdatePheromone(antPaths, antDistances);
            EvaporatePheromones();
        }

        return bestPath;
    }

    private void InitializePheromone()
    {
        double initialPheromoneValue = 1.0 / _nodes.Count;

        // Feromon matrisini başlangıç değerleriyle başlatır
        for (int i = 0; i < _nodes.Count; i++)
        {
            for (int j = 0; j < _nodes.Count; j++)
            {
                _pheromoneMatrix[i, j] = initialPheromoneValue;
            }
        }
    }

    private void CalculateDistance()
    {
        // Mesafe matrisini hesaplar
        for (int i = 0; i < _nodes.Count; i++)
        {
            for (int j = 0; j < _nodes.Count; j++)
            {
                Node node1 = _nodes[i];
                Node node2 = _nodes[j];
                double distance = CalculateEuclideanDistance(node1.Longitude, node1.Latitude, node2.Longitude, node2.Latitude);
                _distanceMatrix[i, j] = distance;
            }
        }
    }

    private double CalculateEuclideanDistance(double x1, double y1, double x2, double y2) => Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

    private void EvaporatePheromones()
    {
        // Feromonların buharlaşmasını uygular
        for (int i = 0; i < _nodes.Count; i++)
        {
            for (int j = 0; j < _nodes.Count; j++)
            {
                _pheromoneMatrix[i, j] *= (1 - Evaporation);
            }
        }
    }

    private List<int> ConstructAntPath()
    {
        List<int> path = new List<int>();
        bool[] visited = new bool[_nodes.Count];

        int currentNode = _random.Next(_nodes.Count);
        path.Add(currentNode);
        visited[currentNode] = true;


        while (path.Count < _nodes.Count)
        {

            int nextNode = ChooseNextNode(currentNode, visited);
            
            path.Add(nextNode);
            visited[nextNode] = true;
            currentNode = nextNode;
        }

        return path;
    }
    private int ChooseNextNode(int currentNode, bool[] visited)
    {
        List<double> probabilities = new List<double>();
        double totalProbability = 0;

        // Check if there are any edges matching the source or target node
        bool hasMatchingEdges = _edges.Any(e => e.Source == currentNode || e.Target == currentNode);

        for (int i = 0; i < _nodes.Count; i++)
        {
            if (!visited[i])
            {
                if (hasMatchingEdges && _edges.Any(e => (e.Source == currentNode && e.Target == i) || (e.Source == i && e.Target == currentNode)))
                {
                    // Feromon miktarı ve mesafe değerine dayalı olasılığı hesaplar
                    double pheromone = Math.Pow(_pheromoneMatrix[currentNode, i], Alpha);
                    double distance = Math.Pow(1 / _distanceMatrix[currentNode, i], Beta);
                    double probability = pheromone * distance;
                    probabilities.Add(probability);
                    totalProbability += probability;
                }
                else
                {
                    // Source veya target ile ilişkili olmayan düğümler için rastgele bir olasılık değeri ata
                    double randomProbability = _random.NextDouble();
                    probabilities.Add(randomProbability);
                    totalProbability += randomProbability;
                }
            }
            else
            {
                // Ziyaret edilmiş düğüm için olasılığı sıfır olarak ayarla
                probabilities.Add(0);
            }
        }

        double randomValue = _random.NextDouble();
        double cumulativeProbability = 0;

        for (int i = 0; i < probabilities.Count; i++)
        {
            // Olasılıkları toplam olasılığa bölerek normalleştirir
            probabilities[i] /= totalProbability;
            cumulativeProbability += probabilities[i];

            // Rastgele değeri kümülatif olasılığa göre karşılaştırarak bir sonraki düğümü seçer
            if (randomValue <= cumulativeProbability)
            {
                return i;
            }
        }

        throw new InvalidOperationException("Sonraki düğüm seçimi başarısız oldu.");
    }
    //private int ChooseNextNode(int currentNode, bool[] visited)
    //{
    //    List<double> probabilities = new List<double>();
    //    double totalProbability = 0;

    //    for (int i = 0; i < _nodes.Count; i++)
    //    {
    //        if (!visited[i])
    //        {
    //            // Feromon miktarı ve mesafe değerine dayalı olasılığı hesaplar
    //            double pheromone = Math.Pow(_pheromoneMatrix[currentNode, i], Alpha);
    //            double distance = Math.Pow(1 / _distanceMatrix[currentNode, i], Beta);
    //            double probability = pheromone * distance;
    //            probabilities.Add(probability);
    //            totalProbability += probability;
    //        }
    //        else
    //        {
    //            // Ziyaret edilmiş düğüm için olasılığı sıfır olarak ayarlar
    //            probabilities.Add(0);
    //        }
    //    }

    //    double randomValue = _random.NextDouble();
    //    double cumulativeProbability = 0;

    //    for (int i = 0; i < probabilities.Count; i++)
    //    {
    //        // Olasılıkları toplam olasılığa bölerek normalleştirir
    //        probabilities[i] /= totalProbability;
    //        cumulativeProbability += probabilities[i];

    //        // Rastgele değeri kümülatif olasılığa göre karşılaştırarak bir sonraki düğümü seçer
    //        if (randomValue <= cumulativeProbability)
    //        {
    //            return i;
    //        }
    //    }

    //    throw new InvalidOperationException("Sonraki düğüm seçimi başarısız oldu.");
    //}

    // Yol boyunca her iki düğüm arasındaki mesafeleri toplar
    private double CalculatePathDistance(List<int> path)
    {
        double distance = 0;

        for (int i = 0; i < path.Count - 1; i++)
        {
            int currentNode = path[i];
            int nextNode = path[i + 1];
            distance += _distanceMatrix[currentNode, nextNode];
        }

        return distance;
    }



    // Güncellenen feromon matrisi hesaplanır ve eski değeri ile ağırlıklı olarak karıştırılır.
    private void UpdatePheromone(List<List<int>> antPaths, double[] antDistances)
    {
        for (int i = 0; i < _nodes.Count; i++)
        {
            for (int j = 0; j < _nodes.Count; j++)
            {
                if (i != j)
                {
                    double pheromoneIncrement = 0;

                    for (int ant = 0; ant < AntCount; ant++)
                    {
                        List<int> path = antPaths[ant];

                        if (path.Contains(i) && path.Contains(j))
                        {
                            double delta = Q / antDistances[ant];
                            pheromoneIncrement += delta;
                        }
                    }

                    _pheromoneMatrix[i, j] = (1 - Evaporation) * _pheromoneMatrix[i, j] + pheromoneIncrement;
                }
            }
        }
    }

    // En kısa mesafeyi ve en iyi yolun dizinini döndürür
    private int GetBestPathIndex(double[] antDistances)
    {
        double minDistance = double.MaxValue;
        int bestPathIndex = 0;

        for (int i = 0; i < antDistances.Length; i++)
        {
            if (antDistances[i] < minDistance)
            {
                minDistance = antDistances[i];
                bestPathIndex = i;
            }
        }

        return bestPathIndex;
    }

    private void PrintPath(List<int> path, double distance)
    {
        Console.Write("Path: ");

        foreach (int nodeIndex in path)
        {
            Console.Write($"{_nodes[nodeIndex].Label} -> ");
        }

        Console.WriteLine($"{_nodes[path[0]].Label}");
        Console.WriteLine($"Distance: {distance}");
    }
}