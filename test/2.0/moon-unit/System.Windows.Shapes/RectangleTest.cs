using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Mono.Moonlight.UnitTesting;
using System.Windows.Shapes;

namespace MoonTest.System.Windows.Shapes
{
	[TestClass]
	public class RectangleTest
	{
		[TestMethod]
		public void MeasureTest1 ()
		{
			Rectangle r = new Rectangle ();
			r.Width = 10;
			r.Height = 20;

			r.Measure (new Size (50, 50));

			Assert.AreEqual (new Size (0, 0), r.DesiredSize);
		}

		[TestMethod]
		public void MeasureTest2 ()
		{
			Canvas c = new Canvas ();
			Rectangle r = new Rectangle ();
			r.Width = 10;
			r.Height = 20;

			r.Measure (new Size (50, 50));
			Assert.AreEqual (new Size (0, 0), r.DesiredSize);
			
			c.Children.Add (r);

			r.Measure (new Size (50, 50));
			Assert.AreEqual (new Size (0, 0), r.DesiredSize);

			c.Measure (new Size (50, 50));

			Assert.AreEqual (new Size (0, 0), c.DesiredSize, "Canvas desired");
			Assert.AreEqual (new Size (0, 0), r.DesiredSize, "Rectangle desired");
		}

		[TestMethod]
		public void MeasureTest3 ()
		{
			Border c = new Border ();
			Rectangle r = new Rectangle ();
			r.Width = 10;
			r.Height = 20;

			r.Measure (new Size (50, 50));

			Assert.AreEqual (new Size (0, 0), r.DesiredSize);
			
			c.Child = r;
			c.Measure (new Size (50, 50));

			Assert.AreEqual (new Size (10, 20), c.DesiredSize, "Border desired");
			Assert.AreEqual (new Size (10, 20), r.DesiredSize, "Rectangle desired");
		}

		[TestMethod]
		public void MeasureTest4 ()
		{
			Border c = new Border ();
			Rectangle r = new Rectangle ();
			r.Width = 10;
			r.Height = 20;

			r.Measure (new Size (50, 50));
			Assert.AreEqual (new Size (0, 0), r.DesiredSize);
			
			c.Child = r;

			r.Measure (new Size (50, 50));
			Assert.AreEqual (new Size (10, 20), r.DesiredSize);

			c.Measure (new Size (50, 50));

			Assert.AreEqual (new Size (10, 20), c.DesiredSize, "Border desired");
			Assert.AreEqual (new Size (10, 20), r.DesiredSize, "Rectangle desired");
		}
	}
}
