using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace TriangleVisualizer
{
    public partial class Form1 : Form
    {

        bool leftMouseDown = false;
        bool middleMouseDown = false;
        Point startPoint = Point.Empty;

        float selectTolerance = 5;
        TrianglePoint selected = null;
        Triangle triangle;
        float[] penDashes = new float[] { 10, 5 };
        Pen trianglePen = new Pen(Color.Red, 2);

        List<VisualizerGroup> visualizers = new List<VisualizerGroup>();
        public Form1()
        {
            InitializeComponent();

            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            triangle = new Triangle(new TrianglePoint(100, 100), new TrianglePoint(100, 300), new TrianglePoint(200, 200));

            accordion1.CheckBoxFactory = new Opulos.Core.UI.CheckBoxHeaderFactory();
            splitContainer1.SplitterDistance =
                splitContainer1.Width - splitContainer1.Panel2MinSize - splitContainer1.SplitterWidth;
            accordion1.CheckBoxMargin = new System.Windows.Forms.Padding(0);
            SetUpVisualizers();
            SetUpCheckBoxes();
            ;
        }

        private void SetUpVisualizers()
        {
            VisualizerGroup incircle = new VisualizerGroup("Upisana kruznica");
            incircle.Add("Kruznica", new IncircleVisualizer(Pens.Orange));
            incircle.Add("Centar", new IncenterVisualizer(Brushes.Orange));
            visualizers.Add(incircle);

            VisualizerGroup eulerLine = new VisualizerGroup("Ojelrova prava");
            eulerLine.Add("Prava", new EulerLineVisualizer(Pens.Purple));
            visualizers.Add(eulerLine);

            VisualizerGroup centroid = new VisualizerGroup("Teziste i medijane");
            centroid.Add("Medijane", new MediansVisualizer(Pens.Brown));
            centroid.Add("Teziste", new CentroidVisualizer(Brushes.Brown));
            visualizers.Add(centroid);

            VisualizerGroup circumcircle = new VisualizerGroup("Opisana kruznica");
            circumcircle.Add("Kruznica", new CircumcircleVisualizer(Pens.Green));
            circumcircle.Add("Centar", new CircumcenterVisualizer(Brushes.Green));
            visualizers.Add(circumcircle);

            VisualizerGroup altitudes = new VisualizerGroup("Visine i ortocentar");
            altitudes.Add("Visine", new AltitudeVisualizer(Pens.Black, new Pen(Color.Black) { DashPattern = penDashes}));
            altitudes.Add("Ortocentar", new OrthocenterVisualizer(Brushes.Black));
            visualizers.Add(altitudes);

            VisualizerGroup bisectors = new VisualizerGroup("Simetrale");
            bisectors.Add("Unutrasnjih uglova", new AngleBisectorVisualizer(new Pen(Color.Orange) { DashPattern = penDashes }));
            bisectors.Add("Stranica", new SideBisectorVisualizer(new Pen(Color.Green) { DashPattern = penDashes }));
            bisectors.Add("Spoljasnjih uglova", new ExternalAngleBisectorVisualizer(new Pen(Color.DarkBlue) { DashPattern = penDashes }));
            visualizers.Add(bisectors);

            VisualizerGroup excircles = new VisualizerGroup("Pripisane kruznice");
            excircles.Add("Kruznice", new ExcirclesVisualiser(Pens.DarkBlue));
            excircles.Add("Centri", new ExcircleCentersVisualizer(Brushes.DarkBlue));
            excircles.Add("Tangente", new ExcircleTangentVisualizer(new Pen(Color.DarkRed) { DashPattern = penDashes }));
            visualizers.Add(excircles);

            VisualizerGroup ninePointCircle = new VisualizerGroup("Ojelrova kruznica");
            ninePointCircle.Add("Kruznica", new NinePointCircleVisualizer(Pens.DarkMagenta));
            ninePointCircle.Add("Kruznica", new NinePointCenterVisualizer(Brushes.DarkMagenta));
            visualizers.Add(ninePointCircle);
            
        }

        private void SetUpCheckBoxes()
        {
            int controlNumber = 0;
            foreach(VisualizerGroup group in visualizers)
            {
                int controlNumberCopy = controlNumber;
                FlowLayoutPanel panel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.TopDown,
                    Padding = new Padding { Left = 5 },
                    AutoSize = true
                };

                int numberOfControls = group.Count();

                foreach(VisualizerData data in group)
                {
                    CheckBox check = new CheckBox { Text = data.Name, Visible = numberOfControls > 1 ? true : false};
                    check.CheckedChanged += (s, e) =>
                        {
                            data.Active = (s as CheckBox).Checked;
                            DetermineHeaderState(controlNumberCopy);
                            canvas.Invalidate();
                        };
                    panel.Controls.Add(check);
                }

                accordion1.Add(panel, group.Name, collapsible: numberOfControls > 1);
                ++controlNumber;
            }
        }

        private void DetermineHeaderState(int controlID)
        {
            CheckBox header = accordion1.CheckBox(controlID);
            CheckBox me = header.Controls[0] as CheckBox;

            if (me == null) return;

            int total = 0, checkedOnes = 0;
            Control main = accordion1.Content(controlID);

            foreach (Control c in main.Controls)
            {
                if (c is CheckBox)
                {
                    ++total;
                    if ((c as CheckBox).Checked)
                        ++checkedOnes;
                }
            }

            if (checkedOnes == 0)
                me.CheckState = CheckState.Unchecked;
            else if (total == checkedOnes)
                me.CheckState = CheckState.Checked;
            else
                me.CheckState = CheckState.Indeterminate;
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            // comment 2
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.White);

            if (triangle.IsTriangle)
                foreach (IVisualizer v in visualizers)
                    v.Visualize(e.Graphics, triangle);

            for (int i = 0; i < 3; ++i)
                e.Graphics.DrawLine(trianglePen, triangle[i], triangle[(i + 1) % 3]);

            for(int i = 0; i < 3; ++i)
            {
                e.Graphics.FillEllipse(Brushes.Blue, Helpers.EllipseToRectangle(triangle[i], 5));
            }

            if (selected != null)
                e.Graphics.DrawEllipse(Pens.Green, Helpers.EllipseToRectangle(selected, 10));

            PointF centroid = triangle.BarycentricToCartesian(1.0f / 3, 1.0f / 3, 1.0f / 3);
            for(int i = 0; i < 3; ++i)
            {
                string name = ((char)('A' + i)).ToString();
                PointF centroidToPoint = triangle[i] - centroid;
                centroidToPoint = Helpers.Normalize(centroidToPoint);
                e.Graphics.DrawString(name, SystemFonts.DefaultFont,
                    selected == triangle[i] ? Brushes.Green : Brushes.Blue,
                    triangle[i].X + 25 * centroidToPoint.X - 6,
                    triangle[i].Y + 25 * centroidToPoint.Y - 6);
            }

           
            
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                middleMouseDown = true;
                startPoint = new Point(e.X, e.Y);
                return;
            }

            leftMouseDown = true;
            PointF current = new PointF(e.X, e.Y);

            bool pointSelected = selected != null;

            selected = null;
            for (int i = 0; i < 3; ++i)
                if (Helpers.DistanceSquared(triangle[i], current) < selectTolerance * selectTolerance)
                    selected = triangle[i];

            if (selected != null)
                (sender as Panel).Invalidate();

            if(pointSelected && selected == null)
                (sender as Panel).Invalidate();
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == System.Windows.Forms.MouseButtons.Left)
                leftMouseDown = false;

            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
                middleMouseDown = false;
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(leftMouseDown && selected != null)
            {
                selected.X = e.X; selected.Y = e.Y;
                (sender as Panel).Invalidate();
            }

            else if(middleMouseDown)
            {
                int dx = startPoint.X - e.X;
                int dy = startPoint.Y - e.Y;

                startPoint = new Point(e.X, e.Y);
                for(int i = 0; i < 3; ++i)
                {
                    triangle[i].X -= dx;
                    triangle[i].Y -= dy;
                    
                }
                (sender as Panel).Invalidate();  
            }

        }

        private void splitContainer1_MouseUp(object sender, MouseEventArgs e)
        {
            this.ActiveControl = null;
        }
    }
}
