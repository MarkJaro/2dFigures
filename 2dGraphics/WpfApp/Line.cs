using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace WpfApp
{
	public class Line : Shape
	{
		protected Line() { }

		public virtual void Draw(Screen screen){ }

		public void InitializeFromCsvLine(string line) { }
	};

	public class HorizontalLine : Line
	{
		private int x1;
		private int x2;
		private int y;
		Color c;

		public HorizontalLine(int x1, int x2, int y, Color c)
		{
			this.x1 = x1;
			this.x2 = x2;
			this.y = y;
			this.c = c;
		}
		public HorizontalLine(Point2i p1, Point2i p2, Color c)
		{
			this.x1 = p1.x;
			this.x2 = p2.x;
			this.y = p1.y;
			this.c = c;
		}
		public override void Draw(Screen screen)
		{
			if (x1 < x2)
			{
				for (int i = x1; i < x2; i++)
				{
					screen.Add(i, y, c);
				}
			}
			else if (x1 > x2)
			{
				for (int i = x1; i > x2; i--)
				{
					screen.Add(i, y, c);
				}
			}
		}

		new public static HorizontalLine InitializeFromCsvLine(string line)
		{
			var values = line.Split(',');
			Color c1 = Color.FromRgb((byte)Int32.Parse(values[4]), (byte)Int32.Parse(values[5]), (byte)Int32.Parse(values[6]));
			return new HorizontalLine(Int32.Parse(values[1]), Int32.Parse(values[2]), Int32.Parse(values[3]), c1);
			
		}
	};

	public class VerticallLine : Line
	{
		private int y1;
		private int y2;
		private int x;
		Color c;
		
		public VerticallLine(int y1, int y2, int x, Color c)
		{
			this.y1 = y1;
			this.y2 = y2;
			this.x = x;
			this.c = c;
		}
		public VerticallLine(Point2i p1, Point2i p2, Color c)
		{
			this.y1 = p1.y;
			this.y2 = p2.y;
			this.x = p1.x;
			this.c = c;
		}

		public override void Draw(Screen screen)
		{
			if (y1 < y2)
			{
				for (int i = y1; i < y2; i++)
				{
					screen.Add(x, i, c);
				}
			}
			else if (y1 > y2)
			{
				for (int i = y1; i > y2; i--)
				{
					screen.Add(x, i, c);
				}
			}
		}

		new public static VerticallLine InitializeFromCsvLine(string line)
		{
			var values = line.Split(',');
			Color c1 = Color.FromRgb((byte)Int32.Parse(values[4]), (byte)Int32.Parse(values[5]), (byte)Int32.Parse(values[6]));
			return new VerticallLine(Int32.Parse(values[1]), Int32.Parse(values[2]), Int32.Parse(values[3]), c1);
		}
	};

	public class MultiLine : Line 
	{
		Point2i p1; 
		Point2i p2; 
		List<Point2i> list = new List<Point2i>();
		Color c;
		public LinearFunction lineFunc { get; private set; }
		public MultiLine() { }
		public MultiLine(int x1, int y1, int x2, int y2, Color c) 
		{
			p1 = new Point2i(x1, y1);
			p2 = new Point2i(x2, y2);
			this.c = c;

			list.Add(p1);
			list.Add(p2);

			lineFunc = new CommonLinearFunction(p1, p2); ;
			if (lineFunc.k == 0) { lineFunc = new VerticalLineFunction(p1.x); }
			if (double.IsInfinity(lineFunc.k)) { lineFunc = new VerticalLineFunction(p1.x); }
		}

		public MultiLine(Point2i p1, Point2i p2)
		{
			this.p1 = p1;
			this.p2 = p2;

			Color red = new Color();
			red.R = 255;
			red.G = 0;
			red.B = 0;
			this.c = red;

			list.Add(p1);
			list.Add(p2);

			lineFunc = new CommonLinearFunction(p1, p2); ;
			if (lineFunc.k == 0) { lineFunc = new VerticalLineFunction(p1.x); }
			if (double.IsInfinity(lineFunc.k)) { lineFunc = new VerticalLineFunction(p1.x); }
		}
		public MultiLine(Point2i p1, Point2i p2, Color c) 
		{
			this.p1 = p1;
			this.p2 = p2;
			this.c = c;

			list.Add(p1);
			list.Add(p2);


			lineFunc = new CommonLinearFunction(p1, p2); ;
			if (lineFunc.k == 0) { lineFunc = new VerticalLineFunction(p1.x); }
			if (double.IsInfinity(lineFunc.k)) { lineFunc = new VerticalLineFunction(p1.x); }
		}

		public override void Draw(Screen screen) 
		{
			if (p1.x == p2.x) { VerticallLine line = new VerticallLine(p1.y, p2.y, p1.x, c); line.Draw(screen); lineFunc = new VerticalLineFunction(p1.x); }
			else if (p1.y == p2.y) { HorizontalLine line = new HorizontalLine(p1.x, p2.x, p1.y, c); line.Draw(screen); lineFunc = new HorizontalLineFunction(p1.y); }
			else 
			{
				int minY = list.Min((point) => point.y);
				int maxY = list.Max((point) => point.y);
				for (int y = minY; y <= maxY; y++)
				{
					screen.Add((int)lineFunc.x(y)-1, y, c);
					screen.Add((int)lineFunc.x(y), y, c);
					screen.Add((int)lineFunc.x(y)+1, y, c);
				}
			}
		}

		new public static MultiLine InitializeFromCsvLine(string line)
		{
			var values = line.Split(',');
			Color c = Color.FromRgb((byte)Int32.Parse(values[5]), (byte)Int32.Parse(values[6]), (byte)Int32.Parse(values[7]));
			return new MultiLine(Int32.Parse(values[1]), Int32.Parse(values[2]), Int32.Parse(values[3]), Int32.Parse(values[4]), c);
		}

	};


		
};
