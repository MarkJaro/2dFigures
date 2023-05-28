using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace WpfApp
{
    public class Circle : Shape
	{

		protected Point2i center;
		protected int radius;
		protected Color c;
		public Circle(Point2i point, int radius, Color c)
		{
			this.center = point;
			this.radius = radius;
			this.c = c;
		}

		public virtual void Draw(Screen screen)
        {
            throw new NotImplementedException();
        }

        public void InitializeFromCsvLine(string line)
        {
            throw new NotImplementedException();
        }
        
    }

	public class HollowCircle : Circle
	{
		public HollowCircle(Point2i point, int radius, Color c) : base(point, radius, c) {  }
		public override void Draw(Screen screen)
		{
			double distance;
			for (int y = -radius; y < radius; y++)
			{
				for (int x = -radius; x < radius; x++)
				{
					distance = Math.Sqrt(x*x + y*y);

					if ((int)distance == radius || (int)distance == radius - 1)
					{
						screen.Add(x + center.x, y+center.y, c);
						
					}
				}
			}
		}
		new public static HollowCircle InitializeFromCsvLine(string line)
		{
			var values = line.Split(',');
			Color c = Color.FromRgb((byte)Int32.Parse(values[4]), (byte)Int32.Parse(values[5]), (byte)Int32.Parse(values[6]));
			return new HollowCircle(new Point2i(Int32.Parse(values[1]), Int32.Parse(values[2])), Int32.Parse(values[3]), c);
		}
	}



	public class FilledCircle : Circle
	{
		public FilledCircle(Point2i point, int radius, Color c) : base(point, radius, c) { }
		public override void Draw(Screen screen)
		{
			double distance;
			for (int y = -radius; y < radius; y++)
			{
				for (int x = -radius; x < radius; x++)
				{
					distance = Math.Sqrt(x * x + y * y);

					if ((int)distance < radius)
					{
						screen.Add(x + center.x, y + center.y, c);
					}
				}
			}
		}
		new public static FilledCircle InitializeFromCsvLine(string line)
		{
			var values = line.Split(',');
			Color c = Color.FromRgb((byte)Int32.Parse(values[4]), (byte)Int32.Parse(values[5]), (byte)Int32.Parse(values[6]));
			return new FilledCircle(new Point2i(Int32.Parse(values[1]), Int32.Parse(values[2])), Int32.Parse(values[3]), c);
		}
	}

	public class Gradient1Circle : Circle
	{
		Color c1;
		Color c2;
		public Gradient1Circle(Point2i point, int radius, Color c, Color c2) : base(point, radius, c) { c1 = c; this.c2 = c2; }
		public override void Draw(Screen screen)
		{
			double distance;
			for (int y = -radius; y < radius; y++)
			{
				for (int x = -radius; x < radius; x++)
				{
					distance = Math.Sqrt(x * x + y * y);

					if ((int)distance < radius)
					{
						Color c = new Color();
						if (x < 0)
						{
							c.R = (byte)(c1.R + (c2.R - c1.R) * ((double)(x / (double)radius)));
							c.G = (byte)(c1.G + (c2.G - c1.G) * ((double)(x / (double)radius)));
							c.B = (byte)(c1.B + (c2.B - c1.B) * ((double)(x / (double)radius)));
						}
						else if (x > 0)
						{
							c.R = (byte)(c2.R + (c1.R - c2.R) * ((double)(x / (double)radius)));
							c.G = (byte)(c2.G + (c1.G - c2.G) * ((double)(x / (double)radius)));
							c.B = (byte)(c2.B + (c1.B - c2.B) * ((double)(x / (double)radius)));
						}
						else { c = c2; }

						screen.Add(x + center.x, y + center.y, c);
					}
				}
			}
		}
		new public static Gradient1Circle InitializeFromCsvLine(string line)
		{
			var values = line.Split(',');
			Color c = Color.FromRgb((byte)Int32.Parse(values[4]), (byte)Int32.Parse(values[5]), (byte)Int32.Parse(values[6]));
			Color c1 = Color.FromRgb((byte)Int32.Parse(values[7]), (byte)Int32.Parse(values[8]), (byte)Int32.Parse(values[9]));
			return new Gradient1Circle(new Point2i(Int32.Parse(values[1]), Int32.Parse(values[2])), Int32.Parse(values[3]), c, c1);
		}
	}

	public class Gradient2Circle : Circle
	{
		Color c1;
		Color c2;
		public Gradient2Circle(Point2i point, int radius, Color c, Color c2) : base(point, radius, c) { c1 = c; this.c2 = c2; }
		public override void Draw(Screen screen)
		{
			double distance;
			for (int y = -radius; y < radius; y++)
			{
				for (int x = -radius; x < radius; x++)
				{
					distance = Math.Sqrt(x * x + y * y);

					if ((int)distance < radius)
					{
						Color c = new Color();
						
						c.R = (byte)(c1.R + (c2.R - c1.R) * ((double)(distance / (double)radius)));
						c.G = (byte)(c1.G + (c2.G - c1.G) * ((double)(distance / (double)radius)));
						c.B = (byte)(c1.B + (c2.B - c1.B) * ((double)(distance / (double)radius)));

						screen.Add(x + center.x, y + center.y, c);
					}
				}
			}

		}
		new public static Gradient2Circle InitializeFromCsvLine(string line)
		{
			var values = line.Split(',');
			Color c = Color.FromRgb((byte)Int32.Parse(values[4]), (byte)Int32.Parse(values[5]), (byte)Int32.Parse(values[6]));
			Color c1 = Color.FromRgb((byte)Int32.Parse(values[7]), (byte)Int32.Parse(values[8]), (byte)Int32.Parse(values[9]));
			return new Gradient2Circle(new Point2i(Int32.Parse(values[1]), Int32.Parse(values[2])), Int32.Parse(values[3]), c, c1);
		}
	}

	public class PictureCircle : Circle
	{
		Color c1;
		Color c2;
		List<Point2i> points = new List<Point2i>();
		public PictureCircle(Point2i point, int radius, Color c, Color c2) : base(point, radius, c) { c1 = c; this.c2 = c2; }
		public override void Draw(Screen screen)
		{

			double x = center.x;
			double y = center.y;
			Point2i p;
			MultiLine line;
			for (double i = 0; i < 7; i+=0.3)
			{
				x = -Math.Sin(i + 2.0 * Math.PI);
				y = Math.Cos(i + 2.0 * Math.PI);


				p = new Point2i((int)(center.x + radius * x), (int)(center.y + radius * y));
				points.Add(p);
			}

			points.Add(points[0]);



			Vertex v1;
			Vertex v2;
			Vertex v3;
			PictureTriangle tri;
			for (int i = 1; i < points.Count; i++)
			{
				v1 = new Vertex(points[i-1], Colors.Red);
				v2 = new Vertex(center, Colors.Red);
				v3 = new Vertex(points[i], Colors.Red);

				tri = new PictureTriangle(v1, v2, v3);

				//if debug
				HollowTriangle tri2 = new HollowTriangle(points[i - 1], center, points[i], Colors.Red);

				
				tri2.Draw(screen);

				tri.AddPicture(points[i - 1], center, points[i], @"owl1.jpg");
				tri.Draw(screen);

				//if debug
				tri2.Draw(screen);
			}





		}
	}




}
