using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

public class ACOAlgorithm
{
    private const int AntCount = 10; // Karınca sayısı
    private const double Alpha = 1; // İz feromonuna etki eden faktör
    private const double Beta = 10; // Uzaklığın tersine etki eden faktör
    private const double Evaporation = 0.5; // Feromon buharlaşma oranı
    private const double Q = 10; // Bir adımda bırakılan feromon miktarı
    private const double InitialPheromone = 0.1; // Başlangıçtaki feromon miktarı
    public double BestDistance { get; set; }
    private int _width { get; set; }
    private int _height { get; set; }
    public PaintEventArgs _drawer { get; set; }
    private List<int> _bestPath { get; set; }

    private Random _random;
    private List<Node> _nodes = new Node().CreateNodes();
    private List<Edge> _edges = new Edge().CreateEdges();
    private double[,] _pheromoneMatrix;
    private double[,] _distanceMatrix;

    public ACOAlgorithm(int width, int height , PaintEventArgs paintEventArgs)
    {
        this._drawer = paintEventArgs;
        this._width = width;
        this._height = height;
        _random = new Random();
        _pheromoneMatrix = new double[_nodes.Count, _nodes.Count];
        _distanceMatrix = new double[_nodes.Count, _nodes.Count];
    }

    public List<int> FindShortestPath()
    {
        InitializePheromone(); //noktalar arasına ilk feromon değerini atıyoruz
        CalculateDistance(); //ilk yolu hesapla

        List<int> bestPath = null;
        double bestDistance = double.MaxValue;

        for (int iteration = 0; iteration < 100; iteration++)
        {
            List<List<int>> antPaths = new List<List<int>>();
            double[] antDistances = new double[AntCount];

            for (int ant = 0; ant < AntCount; ant++)
            {
                List<int> path = ConstructAntPath(); //yeni yol oluşturdu
                
                double distance = CalculatePathDistance(path); // yol maaliyeti hesaplandı

                antPaths.Add(path); //bulunan yol gidilebilecek yollara eklendi
                antDistances[ant] = distance; // bulunan maaliyet, maaliyet listesine eklendi

                if (distance < bestDistance) //daha az bir maaliyet bulursak en iyi yol ve optimum maaliyeti değiştiriyoruz
                {
                    bestPath = path;
                    bestDistance = distance;
                }
            }
            _bestPath = bestPath;
            DrawPath(false); //çizim yapılıyor
            UpdatePheromone(antPaths, antDistances); //feromon miktarları güncelleniyor
            EvaporatePheromones(); //feromonların buharlaşmasını sağlar
        }
        BestDistance = bestDistance;
        DrawPath(true);
        return bestPath;
    }

    private void DrawPath(bool isLast)
    {
        Graphics graphics = this._drawer.Graphics;
        Pen pen = new Pen(Color.Red, 3);
        pen.DashStyle = isLast ? System.Drawing.Drawing2D.DashStyle.Solid : System.Drawing.Drawing2D.DashStyle.DashDot;
        Brush blackBrush = Brushes.Black;

        List<PointF> path = new List<PointF>(); 
        foreach (int nodeId in _bestPath)
        {
            path.Add(new PointF(((float)_nodes[nodeId].Latitude), (float)_nodes[nodeId].Longitude));
        }

        for (int i = 0; i < _bestPath.Count - 1; i++)
        {
            
            PointF startPoint = path[i];
            PointF endPoint = path[i + 1];

            PointF startPixel = ConvertToPixelCoordinates(startPoint);
            PointF endPixel = ConvertToPixelCoordinates(endPoint);

            // İki nokta arasında bir çizgi çizin
            if (i != path.Count - 2)
            {
                graphics.DrawLine(pen, startPixel, endPixel);
            }

            // Label'ı çizmek için X ve Y koordinatlarını alın
            float labelX = startPixel.X;
            float labelY = startPixel.Y;

            // Label'ı çiz
            if (i == 0 || i == path.Count - 2)
            {
                string label = _nodes[i].Label;
                Font font = new Font(FontFamily.GenericSansSerif, 12);
                graphics.DrawString(label, font, Brushes.Black, labelX + 10, labelY - 10);
            }
            int radius = 8; // Siyah nokta çapı
            float centerX = labelX - radius / 2;
            float centerY = labelY - radius / 2;
            graphics.FillEllipse(blackBrush, centerX, centerY, radius, radius);

            Thread.Sleep(10);
        }
        if(!isLast) this._drawer.Graphics.Clear(Color.White);

    }

    private PointF ConvertToPixelCoordinates(PointF point)
    {
        int mapWidth = _width;   // Harita genişliği (piksel cinsinden)
        int mapHeight = _height;  // Harita yüksekliği (piksel cinsinden)

        float latitude = point.X;
        float longitude = point.Y;

        // Ölçek faktörlerini belirleyin ve artırın
        float scaleX = 1.4f * (mapWidth / 360f);
        float scaleY = 1.4f * (mapHeight / 180f);

        // Form üzerindeki piksel koordinatlarını hesaplayın
        float x = (longitude + 180) * scaleX;
        float y = (-1 * latitude + 90) * scaleY;

        return new PointF(x, y);
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

    //düğümler arasında rastgele bir yol oluşturur
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

        for (int i = 0; i < _nodes.Count; i++)
        {
            if (!visited[i])
            {
                // Her düğüm için feromon miktarı ve mesafe değerine dayalı olasılığı hesaplar
                double pheromone = Math.Pow(_pheromoneMatrix[currentNode, i], Alpha);
                double distance = Math.Pow(1 / _distanceMatrix[currentNode, i], Beta);
                double probability = pheromone * distance;
                probabilities.Add(probability);
                totalProbability += probability;
            }
            else
            {
                // Ziyaret edilmiş düğüm için olasılığı sıfır olarak ayarlar
                probabilities.Add(0);
            }
        }

        double randomValue = _random.NextDouble();
        double cumulativeProbability = 0;

        for (int i = 0; i < probabilities.Count; i++)
        {
            // Olasılıkları toplam olasılığa bölerek normalleştiriyoruz
            probabilities[i] /= totalProbability;
            cumulativeProbability += probabilities[i];

            // Rastgele değeri kümülatif olasılığa göre karşılaştırıp bir sonraki düğümü seçiyoruz
            if (randomValue <= cumulativeProbability)
            {
                return i;
            }
        }

        throw new InvalidOperationException("Sonraki düğüm seçimi başarısız oldu.");
    }

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
                if (i != j) //aynı düğümler ise kontrol etme
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
                    //Feromon buharlaşma oranına göre azalt, keşfedilen yollardan gelen yeni feromon mıktarını ekle
                    _pheromoneMatrix[i, j] = (1 - Evaporation) * _pheromoneMatrix[i, j] + pheromoneIncrement;
                }
            }
        }
    }
}