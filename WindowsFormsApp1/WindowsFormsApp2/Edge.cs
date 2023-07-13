using System.Collections.Generic;

public class Edge
{
    public int Source { get; set; }
    public int Target { get; set; }
    public string LinkLabel { get; set; }

    // Ağ topolojisi için gerekli veri seti oluşturuluyor
    public List<Edge> CreateEdges()
    {
        return new List<Edge>()
        {
            new Edge { Source = 0, Target = 2, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 0, Target = 4, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 0, Target = 5, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 0, Target = 7, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 1, Target = 8, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 1, Target = 6, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 1, Target = 4, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 1, Target = 5, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 1, Target = 22, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 2, Target = 10, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 2, Target = 3, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 3, Target = 10, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 5, Target = 12, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 5, Target = 8, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 6, Target = 7, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 8, Target = 9, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 9, Target = 12, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 10, Target = 20, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 10, Target = 13, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 10, Target = 14, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 11, Target = 13, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 11, Target = 22, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 11, Target = 21, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 13, Target = 14, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 15, Target = 16, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 15, Target = 22, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 16, Target = 18, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 17, Target = 18, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 18, Target = 19, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 18, Target = 21, LinkLabel = "Leased Wavelength/Managed Service" },
            new Edge { Source = 19, Target = 20, LinkLabel = "Leased Wavelength/Managed Service" }
        };
    }
}
