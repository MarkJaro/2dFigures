using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace WpfApp
{
    public interface Shape
    {
        void Draw(Screen screen);
        void InitializeFromCsvLine(string line);
    }

    public class Point2i
    {
        public int x { get; private set; }
        public int y { get; private set; }

        public Point2i(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class Vertex
    {
        public Point2i pos;
        public Color c;
        public Point2i texPos;

        public int x { get; set; }
        public int y { get; set; }

        public Vertex(Point2i pos)
        {
            this.pos = pos;
        }
        public Vertex(Point2i pos, Color c)
        {
            this.pos = pos;
            this.c = c;
        }
    }

    public class BarycentricCoordinate
    {
        public double L1;
        public double L2;
        public double L3;

        public BarycentricCoordinate(double L1, double L2, double L3)
        {
            this.L1 = L1;
            this.L2 = L2;
            this.L3 = L3;
        }

        public static BarycentricCoordinate Compute(Vertex p1, Vertex p2, Vertex p3, Point2i pt)
        {
            double delitel = (p2.pos.y - p3.pos.y) * (p1.pos.x - p3.pos.x) + (p3.pos.x - p2.pos.x) * (p1.pos.y - p3.pos.y); //(y_2-y_3)(x-x_3)+(x_3-x_2)(y-y_3)

            double l1 = ((p2.pos.y - p3.pos.y) * (pt.x - p3.pos.x) + (p3.pos.x - p2.pos.x) * (pt.y - p3.pos.y)) / delitel; //(y_2-y_3 )(x-x_3 )+(x_3-x_2)(y-y_3)
            double l2 = ((p3.pos.y - p1.pos.y) * (pt.x - p3.pos.x) + (p1.pos.x - p3.pos.x) * (pt.y - p3.pos.y)) / delitel; //(y_3-y_1 )(x-x_3 )+(x_1-x_3)(y-y_3)
            double l3 = 1.0 - l1 - l2; // λ_3=1-λ_1-λ_2

            return new BarycentricCoordinate(l1, l2, l3);
        }


    }

}
