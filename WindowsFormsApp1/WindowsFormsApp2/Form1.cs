using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        ACOAlgorithm algorithm;
        private List<PointF> _points;
        public Form1()
        {
            _points = new List<PointF>();

            this.Paint += MainForm_Paint;
            
            InitializeComponent();
        }
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            algorithm = new ACOAlgorithm(this.Width, this.Height, e);

            List<int> shortestPath = algorithm.FindShortestPath();
            label1.Text = $"Maaliyet: {algorithm.BestDistance}";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

}
