using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Log2CyclePrototype.Utilities;
using System.Diagnostics;

namespace Log2CyclePrototype.Controls
{
    public partial class TrianglePicker : UserControl
    {

        public int Width { get; set; }
        public int Height { get; set; }

        private Point p1, p2, p3;
        private PointF dotPoint;
        private Graphics g;
        private Bitmap bm;
        private bool updated;

        public TrianglePicker()
        {
            InitializeComponent();

            Width = 100;
            Height = 100;

            bm = new Bitmap(Width, Height);
            g = Graphics.FromImage(bm);

            updated = true;

            p1 = new Point(1, 80 - 1);
            p2 = new Point((int)(80 / 2f), 1);
            p3 = new Point(80 - 1, 80 - 1);
            dotPoint = new PointF((p1.X + p2.X + p3.X) / 3, (p1.Y + p2.Y + p3.Y) / 3);
        }


        protected override void OnMouseClick(MouseEventArgs e)
        {

            base.OnMouseClick(e);

            if (!((MouseEventArgs)e).Button.Equals(MouseButtons.Left))
                return;

            Point p = new Point(((MouseEventArgs)e).Location.X, ((MouseEventArgs)e).Location.Y);



            //Debug.WriteLine("Click @:" + ((MouseEventArgs)e).Location.X + "," + ((MouseEventArgs)e).Location.Y);
            if (MathUtilities.PointInTriangle(p, p1, p2, p3))
            {
                //Debug.WriteLine("Inside!");
                dotPoint = p;
                var d1 = MathUtilities.DistanceBetweenPoints(p, p1);
                var d2 = MathUtilities.DistanceBetweenPoints(p, p2);
                var d3 = MathUtilities.DistanceBetweenPoints(p, p3);
                //Debug.WriteLine(d1 + ", " + d2 + ", " + d3);
            }
            else {
                //Debug.WriteLine("Outside :(");
                if (p.X <= p2.X && p.Y <= p1.Y) //top left corner
                {
                    //if(MathUtilities.LineSide(p1, p2, p) < 0)
                    MathUtilities.FindDistanceToSegment(p, p1, p2, out dotPoint);
                }
                else if (p.X > p2.X && p.Y <= p1.Y)
                { //top right corner
                    //if(MathUtilities.LineSide(p2, p3, p) < 0)
                    MathUtilities.FindDistanceToSegment(p, p2, p3, out dotPoint);
                }
                else if (p.Y > p1.Y) //below p1 && p3
                {
                    //if(MathUtilities.LineSide(p3, p1, p) < 0)
                    MathUtilities.FindDistanceToSegment(p, p3, p1, out dotPoint);
                }
            }
            updated = true;
            //Invalidate();

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Debug.WriteLine("Derp");
            if (!e.Button.Equals(MouseButtons.Left))
                return;
            Point p = new Point(((MouseEventArgs)e).Location.X, ((MouseEventArgs)e).Location.Y);
            //Debug.WriteLine("Click @:" + ((MouseEventArgs)e).Location.X + "," + ((MouseEventArgs)e).Location.Y);
            if (MathUtilities.PointInTriangle(p, p1, p2, p3))
            {
                //Debug.WriteLine("Inside!");
                dotPoint = p;
                var d1 = MathUtilities.DistanceBetweenPoints(p, p1);
                var d2 = MathUtilities.DistanceBetweenPoints(p, p2);
                var d3 = MathUtilities.DistanceBetweenPoints(p, p3);
                //Debug.WriteLine(d1 + ", " + d2 + ", " + d3);
            }
            else {
                //Debug.WriteLine("Outside :(");
                if (p.X <= p2.X && p.Y <= p1.Y) //top left corner
                {
                    //if(MathUtilities.LineSide(p1, p2, p) < 0)
                    MathUtilities.FindDistanceToSegment(p, p1, p2, out dotPoint);
                }
                else if (p.X > p2.X && p.Y <= p1.Y)
                { //top right corner
                    //if(MathUtilities.LineSide(p2, p3, p) < 0)
                    MathUtilities.FindDistanceToSegment(p, p2, p3, out dotPoint);
                }
                else if (p.Y > p1.Y) //below p1 && p3
                {
                    //if(MathUtilities.LineSide(p3, p1, p) < 0)
                    MathUtilities.FindDistanceToSegment(p, p3, p1, out dotPoint);
                }
            }
            updated = true;
            //this.Invalidate();

        }


        //private void trianglePanel_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (!e.Button.Equals(MouseButtons.Left))
        //        return;
        //    Point p = new Point(((MouseEventArgs)e).Location.X, ((MouseEventArgs)e).Location.Y);
        //    //Debug.WriteLine("Click @:" + ((MouseEventArgs)e).Location.X + "," + ((MouseEventArgs)e).Location.Y);
        //    if (MathUtilities.PointInTriangle(p, p1, p2, p3))
        //    {
        //        //Debug.WriteLine("Inside!");
        //        dotPoint = p;
        //        var d1 = MathUtilities.DistanceBetweenPoints(p, p1);
        //        var d2 = MathUtilities.DistanceBetweenPoints(p, p2);
        //        var d3 = MathUtilities.DistanceBetweenPoints(p, p3);
        //        //Debug.WriteLine(d1 + ", " + d2 + ", " + d3);
        //    }
        //    else {
        //        //Debug.WriteLine("Outside :(");
        //        if (p.X <= p2.X && p.Y <= p1.Y) //top left corner
        //        {
        //            //if(MathUtilities.LineSide(p1, p2, p) < 0)
        //            MathUtilities.FindDistanceToSegment(p, p1, p2, out dotPoint);
        //        }
        //        else if (p.X > p2.X && p.Y <= p1.Y)
        //        { //top right corner
        //            //if(MathUtilities.LineSide(p2, p3, p) < 0)
        //            MathUtilities.FindDistanceToSegment(p, p2, p3, out dotPoint);
        //        }
        //        else if (p.Y > p1.Y) //below p1 && p3
        //        {
        //            //if(MathUtilities.LineSide(p3, p1, p) < 0)
        //            MathUtilities.FindDistanceToSegment(p, p3, p1, out dotPoint);
        //        }
        //    }

        //    DrawTriangle();
        //}


        //private void trianglePanel_Click(object sender, EventArgs e)
        //{
        //    if (!((MouseEventArgs)e).Button.Equals(MouseButtons.Left))
        //        return;

        //    Point p = new Point(((MouseEventArgs)e).Location.X, ((MouseEventArgs)e).Location.Y);



        //    //Debug.WriteLine("Click @:" + ((MouseEventArgs)e).Location.X + "," + ((MouseEventArgs)e).Location.Y);
        //    if (MathUtilities.PointInTriangle(p, p1, p2, p3))
        //    {
        //        //Debug.WriteLine("Inside!");
        //        dotPoint = p;
        //        var d1 = MathUtilities.DistanceBetweenPoints(p, p1);
        //        var d2 = MathUtilities.DistanceBetweenPoints(p, p2);
        //        var d3 = MathUtilities.DistanceBetweenPoints(p, p3);
        //        //Debug.WriteLine(d1 + ", " + d2 + ", " + d3);
        //    }
        //    else {
        //        //Debug.WriteLine("Outside :(");
        //        if (p.X <= p2.X && p.Y <= p1.Y) //top left corner
        //        {
        //            //if(MathUtilities.LineSide(p1, p2, p) < 0)
        //            MathUtilities.FindDistanceToSegment(p, p1, p2, out dotPoint);
        //        }
        //        else if (p.X > p2.X && p.Y <= p1.Y)
        //        { //top right corner
        //            //if(MathUtilities.LineSide(p2, p3, p) < 0)
        //            MathUtilities.FindDistanceToSegment(p, p2, p3, out dotPoint);
        //        }
        //        else if (p.Y > p1.Y) //below p1 && p3
        //        {
        //            //if(MathUtilities.LineSide(p3, p1, p) < 0)
        //            MathUtilities.FindDistanceToSegment(p, p3, p1, out dotPoint);
        //        }
        //    }

        //    DrawTriangle();

        //}


        private void DrawTriangle()
        {
            var dotRadius = 8;
            //Point p1 = new Point(1, trianglePanel.Height - 1);
            //Point p2 = new Point((int)(trianglePanel.Width / 2f), 1);
            //Point p3 = new Point(trianglePanel.Width - 1, trianglePanel.Height - 1);
            g.Clear(Color.White);
            Pen p = new Pen(Color.Black);
            g.DrawPolygon(p, new Point[] { p1, p2, p3 });

            g.FillEllipse(new SolidBrush(Color.Black), new RectangleF(new PointF(dotPoint.X - dotRadius, dotPoint.Y - dotRadius), new SizeF(new PointF(dotRadius * 2, dotRadius * 2))));
            p.Dispose();

            this.BackgroundImage = bm;
            this.Refresh();

        }



        protected override void OnPaint(PaintEventArgs e)
        {
            if (updated)
            {
                DrawTriangle();
                updated = false;
            }
            base.OnPaint(e);
        }



    }
}
