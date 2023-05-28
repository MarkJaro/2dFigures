using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace WpfApp
{
	public class Triangle : Shape
	{
		protected Point2i point1;
		protected Point2i point2;
		protected Point2i point3;
		protected List<Point2i> list = new List<Point2i>();
		protected Color c;

		public Triangle(Point2i point1, Point2i point2, Point2i point3, Color c)
		{
			this.point1 = point1;
			this.point2 = point2;
			this.point3 = point3;
			this.c = c;

			list.Add(point1);
			list.Add(point2);
			list.Add(point3);
		}

		public Triangle(int x1, int y1, int x2, int y2, int x3, int y3, Color c)
		{
			point1 = new Point2i(x1,y1);
			point2 = new Point2i(x2,y2);
			point3 = new Point2i(x3,y3);
			this.c = c;

			list.Add(point1);
			list.Add(point2);
			list.Add(point3);
		}

		public void InitializeFromCsvLine(string line) { }

		public virtual void Draw(Screen screen) { }
	};





	public class HollowTriangle : Triangle
	{
		public HollowTriangle(Point2i point1, Point2i point2, Point2i point3, Color c) : base(point1, point2, point3, c) { }
		public HollowTriangle(int x1, int y1, int x2, int y2, int x3, int y3, Color c) : base(x1, y1, x2, y2, x3, y3, c) { }
		public static HollowTriangle InitializeFromCsvLine(string line)
		{
			var values = line.Split(',');
			Color c = Color.FromRgb((byte)Int32.Parse(values[7]), (byte)Int32.Parse(values[8]), (byte)Int32.Parse(values[9]));

			return new HollowTriangle(Int32.Parse(values[1]), Int32.Parse(values[2]), Int32.Parse(values[3]),
                                                                    Int32.Parse(values[4]), Int32.Parse(values[5]), Int32.Parse(values[6]), c);
		}
		public override void Draw(Screen screen)
		{
			MultiLine line1 = new MultiLine(point1, point2, c);
			MultiLine line2 = new MultiLine(point2, point3, c);
			MultiLine line3 = new MultiLine(point3, point1, c);

			line1.Draw(screen);
			line2.Draw(screen);
			line3.Draw(screen);
		}
	};

	public class FilledTriangle : Triangle
	{
		public FilledTriangle(Point2i point1, Point2i point2, Point2i point3, Color c) : base(point1, point2, point3, c) { }
		public FilledTriangle(int x1, int y1, int x2, int y2, int x3, int y3, Color c) : base(x1, y1, x2, y2, x3, y3, c) { }
		new public static FilledTriangle InitializeFromCsvLine(string line)
		{
			var values = line.Split(',');
			Color c = Color.FromRgb((byte)Int32.Parse(values[7]), (byte)Int32.Parse(values[8]), (byte)Int32.Parse(values[9]));

			return new FilledTriangle(Int32.Parse(values[1]), Int32.Parse(values[2]), Int32.Parse(values[3]),
											Int32.Parse(values[4]), Int32.Parse(values[5]), Int32.Parse(values[6]), c);
		}
		public override void Draw(Screen screen)
		{
			MultiLine line1 = new MultiLine(point1, point2, c);
			MultiLine line2 = new MultiLine(point2, point3, c);
			MultiLine line3 = new MultiLine(point3, point1, c);

			List<MultiLine> lines = new List<MultiLine>();
			lines.Add(line1);
			lines.Add(line2);
			lines.Add(line3);
			lines.Sort((line1, line2) =>
			{
				double k1 = line1.lineFunc.k;
				double k2 = line2.lineFunc.k;

				if (k1 < 0 && k2 > 0) { return (int)k1; }
				else if (k1 > 0 && k2 < 0) { return (int)k2; }
				else if (k1 > 0 && k2 > 0) { return (int)(k1 - k2); }
				else if (k1 < 0 && k2 < 0) { return (int)(k1 + Math.Abs(k2)); }
				else { return 0; } 

			});

			int minY = list.Min((point) => point.y);
			int maxY = list.Max((point) => point.y);

			list.Sort((point, point2) => point.y - point2.y);
			int midX = list[1].x;
			int midY = list[1].y;

			if (list[0].y == list[1].y)
			{
				Drawfunc2(midY, maxY, lines[1].lineFunc, lines[2].lineFunc, screen);
			}
			else if (list[1].y == list[2].y) 
			{ 
				Drawfunc2(minY, midY, lines[0].lineFunc, lines[1].lineFunc, screen);
			}
			else
			{
				Drawfunc2(minY, midY, lines[0].lineFunc, lines[1].lineFunc, screen);
				Drawfunc2(midY + 1, maxY, lines[1].lineFunc, lines[2].lineFunc, screen);
			}



			// if debug
			line1.Draw(screen);
			line2.Draw(screen);
			line3.Draw(screen);



		}

		public void Drawfunc2(int minY, int maxY, LinearFunction linefunc, LinearFunction linefunc2, Screen screen)
		{
			for (int y = minY; y <= maxY; y++)
			{
				int start = (int)linefunc.x(y);
				int end = (int)linefunc2.x(y);

				if (start > end)
				{
					int o = start;
					start = end;
					end = o;
				}

				for (int x = start; x <= end; x++)
				{
					screen.Add(x, y, c);
				}
			}
		}
	};


	public class VertexTriangle : Shape
	{
		public Vertex p1 { get; set; }
		public Vertex p2 { get; set; }
		public Vertex p3 { get; set; }

		public List<Vertex> list = new List<Vertex>();
		public VertexTriangle(Vertex point1, Vertex point2, Vertex point3)
		{
			p1 = point1;
			p2 = point2;
			p3 = point3;

			list.Add(p1);
			list.Add(p2);
			list.Add(p3);
		}
		public virtual void Draw(Screen screen) { }
		public void InitializeFromCsvLine(string line) { }
	}


	public class GradientTriangle : VertexTriangle
	{
		public GradientTriangle(Vertex point1, Vertex point2, Vertex point3) : base(point1, point2, point3) { }
		public Color InterpolateColor(BarycentricCoordinate coords)
		{
			double r = coords.L1 * p1.c.R + coords.L2 * p2.c.R + coords.L3 * p3.c.R; 
			double g = coords.L1 * p1.c.G + coords.L2 * p2.c.G + coords.L3 * p3.c.G;
			double b = coords.L1 * p1.c.B + coords.L2 * p2.c.B + coords.L3 * p3.c.B;

			return Color.FromRgb((byte)r, (byte)g, (byte)b);
		}
		public static GradientTriangle InitializeFromCsvLine(string line)
		{
			var values = line.Split(',');

			Point2i p1 = new Point2i(Int32.Parse(values[1]), Int32.Parse(values[2]));
			Point2i p2 = new Point2i(Int32.Parse(values[6]), Int32.Parse(values[7]));
			Point2i p3 = new Point2i(Int32.Parse(values[11]), Int32.Parse(values[12]));

			Color c1 = Color.FromRgb((byte)Int32.Parse(values[3]), (byte)Int32.Parse(values[4]), (byte)Int32.Parse(values[5]));
			Color c2 = Color.FromRgb((byte)Int32.Parse(values[8]), (byte)Int32.Parse(values[9]), (byte)Int32.Parse(values[10]));
			Color c3 = Color.FromRgb((byte)Int32.Parse(values[13]), (byte)Int32.Parse(values[14]), (byte)Int32.Parse(values[15]));

			Vertex v1 = new Vertex(p1, c1);
			Vertex v2 = new Vertex(p2, c2);
			Vertex v3 = new Vertex(p3, c3);

			return new GradientTriangle(v1, v2, v3);
		}
		public override void Draw(Screen screen)
		{
			MultiLine line1 = new MultiLine(p1.pos, p2.pos);
			MultiLine line2 = new MultiLine(p2.pos, p3.pos);
			MultiLine line3 = new MultiLine(p3.pos, p1.pos);

			List<MultiLine> lines = new List<MultiLine>();
			lines.Add(line1);
			lines.Add(line2);
			lines.Add(line3);
			lines.Sort((line1, line2) =>
			{
				double k1 = line1.lineFunc.k;
				double k2 = line2.lineFunc.k;

				if (k1 < 0 && k2 > 0) { return (int)k1; }
				else if (k1 > 0 && k2 < 0) { return (int)k2; }
				else if (k1 > 0 && k2 > 0) { return (int)(k1 - k2); }
				else if (k1 < 0 && k2 < 0) { return (int)(k1 + Math.Abs(k2)); }
				else { return 0; }

			});

			int minX = list.Min((point) => point.pos.x);
			int minY = list.Min((point) => point.pos.y);
			int maxX = list.Max((point) => point.pos.x);
			int maxY = list.Max((point) => point.pos.y);

			list.Sort((point, point2) => point.pos.y - point2.pos.y);
			int midX = list[1].pos.x;
			int midY = list[1].pos.y;
			
			if (list[0].pos.y == list[1].pos.y)
			{
				Drawfunc2(midY, maxY, lines[1].lineFunc, lines[2].lineFunc, screen);
			}
			else if (list[1].pos.y == list[2].pos.y)
			{
				Drawfunc2(minY, midY, lines[0].lineFunc, lines[1].lineFunc, screen);
			}
			else
			{
				Drawfunc2(minY,  midY, lines[0].lineFunc, lines[1].lineFunc, screen);
				Drawfunc2(midY + 1, maxY, lines[1].lineFunc, lines[2].lineFunc, screen);
			}

			/*
			//if debug
			line1.Draw(screen);
			line2.Draw(screen);
			line3.Draw(screen);
			*/
		}

		protected Color colorize(int x, int y)
		{
			var coord = BarycentricCoordinate.Compute(p1, p2, p3, new Point2i(x, y));

			return InterpolateColor(coord);
		}

		public void Drawfunc2(int minY, int maxY, LinearFunction linefunc, LinearFunction linefunc2, Screen screen)
		{
			for (int y = minY; y <= maxY; y++)
			{
				int start = (int)linefunc.x(y);
				int end = (int)linefunc2.x(y);

				if (start > end)
				{
					int o = start;
					start = end;
					end = o;
				}

				for (int x = start; x <= end; x++)
                {
					Color c = colorize(x, y);
					if (c.A != 0) { screen.Add(x, y, c); }
                }
					
			}
		}
	};

	public class PictureTriangle : VertexTriangle
	{
		public System.Drawing.Bitmap Picture;

		public PictureTriangle(Vertex point1, Vertex point2, Vertex point3) : base(point1, point2, point3) { }
		
		public Color InterpolateColor(BarycentricCoordinate coords) 
		{
			double r = coords.L1 * p1.c.R + coords.L2 * p2.c.R + coords.L3 * p3.c.R; 
			double g = coords.L1 * p1.c.G + coords.L2 * p2.c.G + coords.L3 * p3.c.G;
			double b = coords.L1 * p1.c.B + coords.L2 * p2.c.B + coords.L3 * p3.c.B;

			return Color.FromRgb((byte)r, (byte)g, (byte)b);
		}
		public static PictureTriangle InitializeFromCsvLine(string line)
		{
			var values = line.Split(',');

			Point2i p1 = new Point2i(Int32.Parse(values[1]), Int32.Parse(values[2]));
			Point2i p2 = new Point2i(Int32.Parse(values[3]), Int32.Parse(values[4]));
			Point2i p3 = new Point2i(Int32.Parse(values[5]), Int32.Parse(values[6]));

			Point2i PicP1 = new Point2i(Int32.Parse(values[7]), Int32.Parse(values[8]));
			Point2i PicP2 = new Point2i(Int32.Parse(values[9]), Int32.Parse(values[10]));
			Point2i PicP3 = new Point2i(Int32.Parse(values[11]), Int32.Parse(values[12]));

			Color c = new Color();

			Vertex v1 = new Vertex(p1, c);
			Vertex v2 = new Vertex(p2, c);
			Vertex v3 = new Vertex(p3, c);

			PictureTriangle tri = new PictureTriangle(v1, v2, v3);

			tri.AddPicture(PicP1, PicP2, PicP3, values[13]);

			return tri;
		}

		public void AddPicture(Point2i p1, Point2i p2, Point2i p3, string picture)
		{
			this.p1.texPos = p1;
			this.p2.texPos = p2;
			this.p3.texPos = p3;

			Picture = new System.Drawing.Bitmap(picture);
		}

		public override void Draw(Screen screen)
		{
			MultiLine line1 = new MultiLine(p1.pos, p2.pos);
			MultiLine line2 = new MultiLine(p2.pos, p3.pos);
			MultiLine line3 = new MultiLine(p3.pos, p1.pos);

			List<MultiLine> lines = new List<MultiLine>();
			lines.Add(line1);
			lines.Add(line2);
			lines.Add(line3);
			lines.Sort((line1, line2) =>
			{
				double k1 = line1.lineFunc.k;
				double k2 = line2.lineFunc.k;

				if (k1 < 0 && k2 > 0) { return (int)k1; }
				else if (k1 > 0 && k2 < 0) { return (int)k2; }
				else if (k1 > 0 && k2 > 0) { return (int)(k1 - k2); }
				else if (k1 < 0 && k2 < 0) { return (int)(k1 + Math.Abs(k2)); }
				else { return 0; }

			});

			int minX = list.Min((point) => point.pos.x);
			int minY = list.Min((point) => point.pos.y);
			int maxX = list.Max((point) => point.pos.x);
			int maxY = list.Max((point) => point.pos.y);

			list.Sort((point, point2) => point.pos.y - point2.pos.y);
			int midX = list[1].pos.x;
			int midY = list[1].pos.y;

			if (list[0].pos.y == list[1].pos.y)
			{
				Drawfunc2(midY, maxY, lines[1].lineFunc, lines[2].lineFunc, screen);
			}
			else if (list[1].pos.y == list[2].pos.y)
			{
				Drawfunc2(minY, midY, lines[0].lineFunc, lines[1].lineFunc, screen);
			}
			else
			{
				Drawfunc2(minY, midY, lines[0].lineFunc, lines[1].lineFunc, screen);
				Drawfunc2(midY + 1, maxY, lines[1].lineFunc, lines[2].lineFunc, screen);
			}

			/*
			// if debug
			line1.Draw(screen);
			line2.Draw(screen);
			line3.Draw(screen);
			*/
		}

		protected Color colorize(int x, int y)
		{
			var coord = BarycentricCoordinate.Compute(p1, p2, p3, new Point2i(x, y));

			Point2i texcoord = CreateUV(coord);
			var c = Picture.GetPixel(texcoord.x, texcoord.y);

			return Color.FromRgb(c.R, c.G, c.B);
		}

		public Point2i CreateUV(BarycentricCoordinate coords)
		{
			double u = coords.L1 * p1.texPos.x + coords.L2 * p2.texPos.x + coords.L3 * p3.texPos.x;
			double v = coords.L1 * p1.texPos.y + coords.L2 * p2.texPos.y + coords.L3 * p3.texPos.y;

			return new Point2i((int)u, (int)v);
		}

		public void Drawfunc2(int minY, int maxY, LinearFunction linefunc, LinearFunction linefunc2, Screen screen)
		{
			for (int y = minY; y <= maxY; y++)
			{
				int start = (int)linefunc.x(y);
				int end = (int)linefunc2.x(y);

				if (start > end)
				{
					int o = start;
					start = end;
					end = o;
				}

				for (int x = start; x <= end; x++)
				{
					Color c = colorize(x, y);
					if (c.A != 0) { screen.Add(x, y, c); }
				}

			}
		}
	};
}