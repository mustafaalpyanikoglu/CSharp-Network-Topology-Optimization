using System.Collections.Generic;

public class Node
{
    public int Id { get; set; }
    public string Label { get; set; }
    public string Country { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }

    // Ağ topolojisi için gerekli veri seti oluşturuluyor
    public List<Node> CreateNodes()
    {
        return new List<Node>
        {
            new Node { Id = 0, Label = "Washington CDC", Country = "United States", Longitude = -77.03637, Latitude = 38.89511 },
            new Node { Id = 1, Label = "New York", Country = "United States", Longitude = -74.00597, Latitude = 40.71427 },
            new Node { Id = 2, Label = "Atlanta", Country = "United States", Longitude = -84.38798, Latitude = 33.749 },
            new Node { Id = 3, Label = "Miami", Country = "United States", Longitude = -80.19366, Latitude = 25.77427 },
            new Node { Id = 4, Label = "Boston", Country = "United States", Longitude = -71.05977, Latitude = 42.35843 },
            new Node { Id = 5, Label = "London", Country = "United Kingdom", Longitude = -0.12574, Latitude = 51.50853 },
            new Node { Id = 6, Label = "Philadelphia", Country = "United States", Longitude = -75.16379, Latitude = 39.95234 },
            new Node { Id = 7, Label = "Baltimore", Country = "United States", Longitude = -76.61219, Latitude = 39.29038 },
            new Node { Id = 8, Label = "Amsterdam", Country = "Netherlands", Longitude = 4.88969, Latitude = 52.37403 },
            new Node { Id = 9, Label = "Frankfurt", Country = "Germany", Longitude = 8.68333, Latitude = 50.11667 },
            new Node { Id = 10, Label = "Houston", Country = "United States", Longitude = -95.36327, Latitude = 29.76328 },
            new Node { Id = 11, Label = "None"},
            new Node { Id = 12, Label = "Paris", Country = "France", Longitude = 2.3488, Latitude = 48.85341 },
            new Node { Id = 13, Label = "Dallas", Country = "United States", Longitude = -96.80667, Latitude = 32.78306 },
            new Node { Id = 14, Label = "Austin", Country = "United States", Longitude = -97.74306, Latitude = 30.26715 },
            new Node { Id = 15, Label = "Seattle", Country = "United States", Longitude = -122.33207, Latitude = 47.60621 },
            new Node { Id = 16, Label = "Portland", Country = "United States", Longitude = -122.67621, Latitude = 45.52345 },
            new Node { Id = 17, Label = "Tokyo", Country = "Japan", Longitude = 139.5813, Latitude = 35.61488 },
            new Node { Id = 18, Label = "San Francisco", Country = "United States", Longitude = -122.41942, Latitude = 37.77493 },
            new Node { Id = 19, Label = "Los Angeles", Country = "United States", Longitude = -118.24368, Latitude = 34.05223 },
            new Node { Id = 20, Label = "Phoenix", Country = "United States", Longitude = -112.07404, Latitude = 33.44838 },
            new Node { Id = 21, Label = "Denver", Country = "United States", Longitude = -104.9847, Latitude = 39.73915 },
            new Node { Id = 22, Label = "Chicago", Country = "United States", Longitude = -87.65005, Latitude = 41.85003 }
        };

    }
  
}
