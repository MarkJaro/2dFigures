using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace WpfApp
{
	public interface LinearFunction
	{
		public double k { get; set; }
		public double b { get; set; }
		public double x(int y) => (y - b) / k;
		public double y(int x) => k * x - b;
	};

	public class CommonLinearFunction : LinearFunction
	{
		public double k { get; set; }
		public double b { get; set; }
		public double x(int y) => (y - b) / k;
		public double y(int x) => k * x + b;

		public CommonLinearFunction(Point2i p1, Point2i p2) // find Linear Function by 2 points
		{
			k = (double)(p1.y - p2.y) / (double)(p1.x - p2.x);

			b = (double)(-(k * p1.x - p1.y));
		}
		public CommonLinearFunction(double k, double b)
		{
			this.k = k;
			this.b = b;
		}

		

		public void Draw(int x, Screen screen, Color c)
		{
			screen.Add(x, (int)y(x), c); 
		}


	};

	public class VerticalLineFunction : LinearFunction
	{
		public double k { get; set; }
		public double b { get; set; }
		public double xValue;

		public VerticalLineFunction(double x)
		{
			xValue = x;
		}

		public double x(int y)
		{
			return xValue;
		}
	};

	public class HorizontalLineFunction : LinearFunction
	{
		public double k { get; set; }
		public double b { get; set; }
		public double yValue;

		public HorizontalLineFunction(double y)
		{
			yValue = y;
		}

		public double x(int y)
		{
			return yValue;
		}
	};

};

