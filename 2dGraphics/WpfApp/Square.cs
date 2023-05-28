using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;


namespace WpfApp
{
	public class Square : Shape
	{
		protected Point2i start;
		protected int width;
		protected int height;


		public Square(Point2i start1, Point2i end)
		{
			start = start1;
			width = end.x - start.x;
			height = end.y - start.y;
		}

		public Square(Point2i start1, int x, int y)
		{
			start = start1;
			width = x;
			height = y;
		}

		public Square(int x, int y, int w, int h)
		{
			start = new Point2i(x, y);
			width = w;
			height = h;
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


	public class HollowSquare : Square
    {
		Color c;
		public HollowSquare(Point2i start1, int x, int y) : base(start1, x, y) { }
		public HollowSquare(int x, int y, int w, int h, Color c) : base(x, y, w, h) { this.c = c; }
		public HollowSquare(Point2i start, Point2i end) : base(start, end) { }

		public override void Draw(Screen screen)
		{
			MultiLine line1 = new MultiLine(start, new Point2i(start.x + width, start.y), c);
			MultiLine line2 = new MultiLine(start, new Point2i(start.x, start.y + height), c);
			MultiLine line3 = new MultiLine(new Point2i(start.x, start.y + height), new Point2i(start.x + width, start.y + height), c);
			MultiLine line4 = new MultiLine(new Point2i(start.x + width, start.y), new Point2i(start.x + width, start.y + height), c);

			line1.Draw(screen);
			line2.Draw(screen);
			line3.Draw(screen);
			line4.Draw(screen);
		}

		new public static  HollowSquare InitializeFromCsvLine(string line)
		{
			var values = line.Split(',');
			Color c = Color.FromRgb((byte)Int32.Parse(values[5]), (byte)Int32.Parse(values[6]), (byte)Int32.Parse(values[7]));
			return new HollowSquare(Int32.Parse(values[1]), Int32.Parse(values[2]), Int32.Parse(values[3]), Int32.Parse(values[4]), c);
		}
	}



	public class FilledSquare : Square
	{
		Color c;
		public FilledSquare(Point2i start1, int x, int y, Color c) : base(start1, x, y) { this.c = c; }
		public FilledSquare(int x, int y, int w, int h, Color c) : base(x, y, w, h) { this.c = c; }
		public FilledSquare(Point2i start, Point2i end, Color c) : base(start, end) { this.c = c;}

		public override void Draw(Screen screen)
		{
			for (int y = start.y; y < start.y + height; y++)
			{
				for (int x = start.x; x < start.x + width; x++)
				{
					screen.Add(x, y, c);
				}
			}
		}
		new public static FilledSquare InitializeFromCsvLine(string line)
		{
			var values = line.Split(',');
			Color c = Color.FromRgb((byte)Int32.Parse(values[5]), (byte)Int32.Parse(values[6]), (byte)Int32.Parse(values[7]));
			return new FilledSquare(Int32.Parse(values[1]), Int32.Parse(values[2]), Int32.Parse(values[3]), Int32.Parse(values[4]), c);
		}
	}


	public class GradientSquare : Square
	{
		Color c1;
		Color c2;
		public GradientSquare(Point2i start1, int x, int y, Color c1, Color c2) : base(start1, x, y) { this.c1 = c1; this.c2 = c2; }
		public GradientSquare(int x, int y, int w, int h, Color c1, Color c2) : base(x, y, w, h) { this.c1 = c1; this.c2 = c2; }
		public GradientSquare(Point2i start, Point2i end, Color c1, Color c2) : base(start, end) { this.c1 = c1; this.c2 = c2; }

		public override void Draw(Screen screen)
		{
			Color c = new Color();

			for (int y = start.y; y < start.y + height; y++)
			{
				for (int x = start.x; x < start.x + width; x++)
				{
					c.R = (byte)(c1.R + (c2.R - c1.R) * ((double)(x - start.x) / (double)((start.x + width) - start.x)));
					c.G = (byte)(c1.G + (c2.G - c1.G) * ((double)(x - start.x) / (double)((start.x + width) - start.x)));
					c.B = (byte)(c1.B + (c2.B - c1.B) * ((double)(x - start.x) / (double)((start.x + width) - start.x)));

					screen.Add(x, y, c);
				}
			}
		}
		new public static GradientSquare InitializeFromCsvLine(string line)
		{
			var values = line.Split(',');
			Color c = Color.FromRgb((byte)Int32.Parse(values[5]), (byte)Int32.Parse(values[6]), (byte)Int32.Parse(values[7]));
			Color c1 = Color.FromRgb((byte)Int32.Parse(values[8]), (byte)Int32.Parse(values[9]), (byte)Int32.Parse(values[10]));
			return new GradientSquare(Int32.Parse(values[1]), Int32.Parse(values[2]), Int32.Parse(values[3]), Int32.Parse(values[4]), c, c1);
		}
	}


	public class PictureSquare : Square
	{
		private Vertex P1; // левый верхний угол
		private Vertex P2; // правый верхний угол
		private Vertex P3; // левый нижний угол
		private Vertex P4; // правый нижний угол
		private PictureTriangle tri1;
		private PictureTriangle tri2;

		public PictureSquare(Point2i start1, int x, int y) : base(start1, x, y) 
		{
			Point2i p1 = start;
			Point2i p2 = new Point2i(start.x + width, start.y);
			Point2i p3 = new Point2i(start.x, start.y + height);
			Point2i p4 = new Point2i(start.x + width, start.y + height);
			P1 = new Vertex(p1);
			P2 = new Vertex(p2);
			P3 = new Vertex(p3);
			P4 = new Vertex(p4);


			tri1 = new PictureTriangle(P3, P1, P2);
			tri2 = new PictureTriangle(P3, P4, P2);
		}
		public PictureSquare(int x, int y, int w, int h) : base(x, y, w, h) 
		{
			Point2i p1 = start;
			Point2i p2 = new Point2i(start.x + width, start.y);
			Point2i p3 = new Point2i(start.x, start.y + height);
			Point2i p4 = new Point2i(start.x + width, start.y + height);
			P1 = new Vertex(p1);
			P2 = new Vertex(p2);
			P3 = new Vertex(p3);
			P4 = new Vertex(p4);


			tri1 = new PictureTriangle(P3, P1, P2);
			tri2 = new PictureTriangle(P3, P4, P2);

		}
		public PictureSquare(Point2i start, Point2i end) : base(start, end) 
		{
			Point2i p1 = start;
			Point2i p2 = new Point2i(start.x + width, start.y);
			Point2i p3 = new Point2i(start.x, start.y + height);
			Point2i p4 = new Point2i(start.x + width, start.y + height);
			P1 = new Vertex(p1);
			P2 = new Vertex(p2);
			P3 = new Vertex(p3);
			P4 = new Vertex(p4);


			tri1 = new PictureTriangle(P3, P1, P2);
			tri2 = new PictureTriangle(P3, P4, P2);
		}

		public void CreatePicture(Point2i start1, int x, int y, string picture)
		{
			tri1.p1.texPos = new Point2i(start1.x, start1.y + y);
			tri1.p2.texPos = start1;
			tri1.p3.texPos = new Point2i(start1.x + x, start1.y);
			
			tri2.p1.texPos = new Point2i(start1.x, start1.y + y);
			tri2.p2.texPos = new Point2i(start1.x + x, start1.y + y);
			tri2.p3.texPos = new Point2i(start1.x + x, start1.y);

			tri1.Picture = new System.Drawing.Bitmap(picture);
			tri2.Picture = new System.Drawing.Bitmap(picture);
		}

		public override void Draw(Screen screen)
		{
			tri1.Draw(screen);
			tri2.Draw(screen);
		}

		new public static PictureSquare InitializeFromCsvLine(string line)
		{
			var values = line.Split(',');
			PictureSquare square = new PictureSquare(Int32.Parse(values[1]), Int32.Parse(values[2]), Int32.Parse(values[3]), Int32.Parse(values[4]));
			square.CreatePicture(new Point2i(Int32.Parse(values[5]), Int32.Parse(values[6])), Int32.Parse(values[7]), Int32.Parse(values[8]), values[9]);
			return square;
		}
	}
}
