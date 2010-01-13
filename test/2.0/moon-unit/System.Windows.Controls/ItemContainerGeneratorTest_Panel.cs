//
// ItemContainerGenerator Unit Tests
//
// Contact:
//   Moonlight List (moonlight-list@lists.ximian.com)
//
// Copyright 2009 Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;
using System.Windows.Markup;
using Mono.Moonlight.UnitTesting;
using System.Collections.Generic;

namespace MoonTest.System.Windows.Controls
{
	public class CustomVirtualizingPanel : VirtualizingPanel { }

	[TestClass]
	public partial class IItemContainerGeneratorTest : SilverlightTest {
		ItemsControlPoker Control {
			get; set;
		}

		ItemContainerGenerator Generator {
			get { return (ItemContainerGenerator) Panel.ItemContainerGenerator; }
		}

		IRecyclingItemContainerGenerator IGenerator {
			get { return (IRecyclingItemContainerGenerator) Panel.ItemContainerGenerator; }
		}

		CustomVirtualizingPanel Panel {
			get {
				return (CustomVirtualizingPanel) VisualTreeHelper.GetChild (VisualTreeHelper.GetChild (Control, 0), 0);
			}
		}

		[TestInitialize]
		public void Initialize ()
		{
			Control = new ItemsControlPoker ();
			Control.ItemsPanel = (ItemsPanelTemplate) XamlReader.Load (@"
<ItemsPanelTemplate xmlns=""http://schemas.microsoft.com/client/2007""
            xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
			xmlns:clr=""clr-namespace:MoonTest.System.Windows.Controls;assembly=moon-unit"">
		<clr:CustomVirtualizingPanel x:Name=""Virtual"" />
</ItemsPanelTemplate>");
			for (int i = 0; i < 5; i++)
				Control.Items.Add (i.ToString ());
		}

		[TestMethod]
		[Asynchronous]
		public void AllowStartAtRealized_True ()
		{
			// Create all the containers, then try to create the one at index 0.
			bool fresh;
			object first, second;
			CreateAsyncTest (Control, () => {
				var position = IGenerator.GeneratorPositionFromIndex (0);
				using (var g = IGenerator.StartAt (position, GeneratorDirection.Forward, true))
					for (int i = 0; i < Control.Items.Count; i++)
						IGenerator.GenerateNext (out fresh);

				first = Generator.ContainerFromIndex (0);
				position = IGenerator.GeneratorPositionFromIndex (0);
				using (var g = IGenerator.StartAt (position, GeneratorDirection.Forward, true))
					second = IGenerator.GenerateNext (out fresh);
				Assert.AreSame (first, second, "#1");
			});
		}

		[TestMethod]
		[Asynchronous]
		public void AllowStartAtUnrealized_False ()
		{
			// Create all the containers, then try to create the one at index 0.
			bool fresh;
			object first, second;
			CreateAsyncTest (Control, () => {
				var position = IGenerator.GeneratorPositionFromIndex (0);
				using (var g = IGenerator.StartAt (position, GeneratorDirection.Forward, true))
					for (int i = 0; i < Control.Items.Count; i++)
						IGenerator.GenerateNext (out fresh);

				first = Generator.ContainerFromIndex (0);
				position = IGenerator.GeneratorPositionFromIndex (0);
				using (var g = IGenerator.StartAt (position, GeneratorDirection.Forward, false))
					second = IGenerator.GenerateNext (out fresh);

				Assert.AreNotSame (first, second, "#1");
				Assert.AreSame (Generator.ContainerFromIndex (1), second, "#2");
			});
		}

		[TestMethod]
		[Asynchronous]
		public void AllowStartAtUnrealized_False_PositiveOffset ()
		{
			// Create all the containers, then try to create the one at index 0.
			bool fresh;
			object first, second;
			CreateAsyncTest (Control, () => {
				var position = IGenerator.GeneratorPositionFromIndex (0);
				using (var g = IGenerator.StartAt (position, GeneratorDirection.Forward, true))
					for (int i = 0; i < Control.Items.Count; i++)
						IGenerator.GenerateNext (out fresh);

				first = Generator.ContainerFromIndex (0);
				position = new GeneratorPosition (0, 1);
				using (var g = IGenerator.StartAt (position, GeneratorDirection.Forward, false))
					second = IGenerator.GenerateNext (out fresh);

				Assert.AreNotSame (first, second, "#1");
				Assert.AreSame (Generator.ContainerFromIndex (1), second, "#2");
			});
		}

		[TestMethod]
		[Asynchronous]
		public void AllowStartAtUnrealized_False_ZeroOffset ()
		{
			// Create all the containers, then try to create the one at index 0.
			bool fresh;
			object first, second;
			CreateAsyncTest (Control, () => {
				var position = IGenerator.GeneratorPositionFromIndex (0);
				using (var g = IGenerator.StartAt (position, GeneratorDirection.Forward, true))
					for (int i = 0; i < Control.Items.Count; i++)
						IGenerator.GenerateNext (out fresh);

				first = Generator.ContainerFromIndex (0);
				position = new GeneratorPosition (0, 0);
				using (var g = IGenerator.StartAt (position, GeneratorDirection.Forward, false))
					second = IGenerator.GenerateNext (out fresh);

				Assert.AreNotSame (first, second, "#1");
				Assert.AreSame (Generator.ContainerFromIndex (1), second, "#2");
			});
		}

		[TestMethod]
		[Asynchronous]
		public void AllowStartAtUnrealized_False_Backwards ()
		{
			// Create all the containers, then try to create the one at index 0.
			bool fresh;
			object first, second;
			CreateAsyncTest (Control, () => {
				var position = IGenerator.GeneratorPositionFromIndex (0);
				using (var g = IGenerator.StartAt (position, GeneratorDirection.Forward, true))
					for (int i = 0; i < Control.Items.Count; i++)
						IGenerator.GenerateNext (out fresh);

				first = Generator.ContainerFromIndex (1);
				position = IGenerator.GeneratorPositionFromIndex (1);
				using (var g = IGenerator.StartAt (position, GeneratorDirection.Backward, false))
					second = IGenerator.GenerateNext (out fresh);

				Assert.AreNotSame (first, second, "#1");
				Assert.AreSame (Generator.ContainerFromIndex (0), second, "#2");
			});
		}

		[TestMethod]
		[Asynchronous]
		public void GeneratedStaggeredTest ()
		{
			bool fresh;
			CreateAsyncTest (Control, () => {
				// Start at 0, generate 1
				var position = IGenerator.GeneratorPositionFromIndex (0);
				Assert.AreEqual (new GeneratorPosition (-1, 1), position, "#1");
				using (var g = IGenerator.StartAt (position, GeneratorDirection.Forward, false))
					Assert.IsNotNull (IGenerator.GenerateNext (out fresh), "#2");

				// Start at 2, generate 6
				position = IGenerator.GeneratorPositionFromIndex (2);
				Assert.AreEqual (new GeneratorPosition (0, 2), position, "#3");
				using (var g = IGenerator.StartAt (position, GeneratorDirection.Forward, false)) {
					for (int i = 0; i < 100; i++) {
						if (i < 3)
							Assert.IsNotNull (IGenerator.GenerateNext (out fresh), "#4." + i);
						else
							Assert.IsNull (IGenerator.GenerateNext (out fresh), "#4." + i);
					}
				}
			});
		}

		[TestMethod]
		[Asynchronous]
		public void GeneratedStaggeredTest2 ()
		{
			bool fresh;
			CreateAsyncTest (Control, () => {
				// Start at 0, generate 1
				var position = IGenerator.GeneratorPositionFromIndex (0);
				Assert.AreEqual (new GeneratorPosition (-1, 1), position, "#1");
				using (var g = IGenerator.StartAt (position, GeneratorDirection.Forward, false))
					Assert.IsNotNull (IGenerator.GenerateNext (out fresh), "#2");

				// Start at 2, and generate 3
				position = new GeneratorPosition (0, 2);
				using (var g = IGenerator.StartAt (position, GeneratorDirection.Forward, false)) {
					for (int i = 0; i < 3; i++)
						Assert.IsNotNull (IGenerator.GenerateNext (out fresh), "#4." + i);
				}
				for (int i = 0; i < Control.Items.Count; i++)
					if (i == 1)
						Assert.IsNull (Generator.ContainerFromIndex (i), "#5." + i);
					else
						Assert.IsNotNull (Generator.ContainerFromIndex (i), "#5." + i);
			});
		}

		[TestMethod]
		[Asynchronous]
		public void GenerateAllTest ()
		{
			bool realised;
			CreateAsyncTest (Control, () => {
				using (var g = IGenerator.StartAt (IGenerator.GeneratorPositionFromIndex (0), GeneratorDirection.Forward, true)) {
					// Make all 5
					for (int i = 0; i < Control.Items.Count; i++) {
						var container = IGenerator.GenerateNext (out realised);

						Assert.AreSame (container, Generator.ContainerFromItem (Control.Items [i]), "#1." + i);
						Assert.AreSame (container, Generator.ContainerFromIndex (i), "#2." + i);
						Assert.AreEqual (i, Generator.IndexFromContainer (container), "#3." + i);
					}
				}
			});
		}

		[TestMethod]
		[Asynchronous]
		public void GenerateSameThrice ()
		{
			bool fresh;
			object first;
			object second;
			object third;
			CreateAsyncTest (Control, () => {
				var position = Generator.GeneratorPositionFromIndex (0);
				using (var g = IGenerator.StartAt (position, GeneratorDirection.Forward, false))
					first = IGenerator.GenerateNext (out fresh);
				Assert.IsTrue (fresh, "#1");

				using (var g = IGenerator.StartAt (position, GeneratorDirection.Forward, false))
					second = IGenerator.GenerateNext (out fresh);
				Assert.IsFalse (fresh, "#2");

				using (var g = IGenerator.StartAt (position, GeneratorDirection.Forward, false))
					third = IGenerator.GenerateNext (out fresh);
				Assert.IsFalse (fresh, "#3");

				Assert.IsNotNull (first, "#4");
				Assert.AreSame (first, second, "#5");
				Assert.AreSame (second, third, "#6");
			});
		}

		[TestMethod]
		[Asynchronous]
		public void GeneratorTypeTest ()
		{
			CreateAsyncTest (Control, () => {
				Assert.IsInstanceOfType<ItemContainerGenerator> (IGenerator, "#1");
			});
		}

		[TestMethod]
		[Asynchronous]
		public void IndexFromPosition_CrossRealisedTest ()
		{
			// Realise elements 0 and 2 and then try to get the index of an element as if we hadn't
			// realised element 2
			bool realised;
			CreateAsyncTest (Control, () => {
				using (var v = IGenerator.StartAt (Generator.GeneratorPositionFromIndex (0), GeneratorDirection.Forward, false))
					IGenerator.GenerateNext (out realised);
				using (var v = IGenerator.StartAt (Generator.GeneratorPositionFromIndex (2), GeneratorDirection.Forward, false))
					IGenerator.GenerateNext (out realised);
				
				// Get the index of the 3'rd unrealised element starting at the first realised element
				int index = Generator.IndexFromGeneratorPosition (new GeneratorPosition (0, 4));
				Assert.AreEqual (4, index, "#1");
			});
		}

		[TestMethod]
		[Asynchronous]
		public void IndexFromPosition_LandOnRealisedTest ()
		{
			// Realise elements 0 and 2 and then try to get the index of an element as if we hadn't
			// realised element 2
			bool realised;
			CreateAsyncTest (Control, () => {
				using (var v = IGenerator.StartAt (Generator.GeneratorPositionFromIndex (0), GeneratorDirection.Forward, false))
					IGenerator.GenerateNext (out realised);
				using (var v = IGenerator.StartAt (Generator.GeneratorPositionFromIndex (2), GeneratorDirection.Forward, false))
					IGenerator.GenerateNext (out realised);

				// Get the index of the 3'rd unrealised element starting at the first realised element
				int index = Generator.IndexFromGeneratorPosition (new GeneratorPosition (0, 2));
				Assert.AreEqual (2, index, "#1");
			});
		}

		[TestMethod]
		[Asynchronous]
		public void IndexFromPosition_NoRealisedTest ()
		{
			CreateAsyncTest (Control, () => {
				Assert.AreEqual (4, Generator.IndexFromGeneratorPosition (new GeneratorPosition (-1, -1)), "#1");
				Assert.AreEqual (1, Generator.IndexFromGeneratorPosition (new GeneratorPosition (-1, 2)), "#2");
				Assert.AreEqual (2, Generator.IndexFromGeneratorPosition (new GeneratorPosition (-1, 3)), "#3");
				Assert.AreEqual (3, Generator.IndexFromGeneratorPosition (new GeneratorPosition (-1, 4)), "#4");
				Assert.AreEqual (4, Generator.IndexFromGeneratorPosition (new GeneratorPosition (-1, 5)), "#5");
			});
		}

		[TestMethod]
		[Asynchronous]
		public void IndexFromPosition_OutOfRangeTest ()
		{
			CreateAsyncTest (Control, () => {
				Assert.AreEqual (-3, Generator.IndexFromGeneratorPosition (new GeneratorPosition (-2, -1)), "#1");
				Assert.AreEqual (3, Generator.IndexFromGeneratorPosition (new GeneratorPosition (-1, -2)), "#2");

				Assert.AreEqual (-1, Generator.IndexFromGeneratorPosition (new GeneratorPosition (10, -1)), "#3");
				Assert.AreEqual (-1, Generator.IndexFromGeneratorPosition (new GeneratorPosition (10, 0)), "#4");
				Assert.AreEqual (9, Generator.IndexFromGeneratorPosition (new GeneratorPosition (-1, 10)), "#5");
			});
		}

		[TestMethod]
		[Asynchronous]
		public void IndexFromPosition_FirstRealisedTest ()
		{
			bool realised;
			CreateAsyncTest (Control,
				() => {
					using (var v = IGenerator.StartAt (Generator.GeneratorPositionFromIndex (0), GeneratorDirection.Forward, true))
						IGenerator.GenerateNext (out realised);
				}, () => {
					Assert.AreEqual (0, Generator.IndexFromGeneratorPosition (new GeneratorPosition (0, 0)), "#1");
					Assert.AreEqual (1, Generator.IndexFromGeneratorPosition (new GeneratorPosition (0, 1)), "#2");
					Assert.AreEqual (2, Generator.IndexFromGeneratorPosition (new GeneratorPosition (0, 2)), "#3");
					Assert.AreEqual (3, Generator.IndexFromGeneratorPosition (new GeneratorPosition (0, 3)), "#4");
					Assert.AreEqual (4, Generator.IndexFromGeneratorPosition (new GeneratorPosition (0, 4)), "#5");
				}
			);
		}

		[TestMethod]
		[Asynchronous]
		public void IndexFromPosition_StartOrEndTest ()
		{
			CreateAsyncTest (Control, () => {
				Assert.AreEqual (2, Generator.IndexFromGeneratorPosition (new GeneratorPosition (-1, 3)), "#0");
				Assert.AreEqual (1, Generator.IndexFromGeneratorPosition (new GeneratorPosition (-1, 2)), "#1");
				Assert.AreEqual (0, Generator.IndexFromGeneratorPosition (new GeneratorPosition (-1, 1)), "#2");
				Assert.AreEqual (-1, Generator.IndexFromGeneratorPosition (new GeneratorPosition (-1, 0)), "#3");
				Assert.AreEqual (4, Generator.IndexFromGeneratorPosition (new GeneratorPosition (-1, -1)), "#4");
				Assert.AreEqual (3, Generator.IndexFromGeneratorPosition (new GeneratorPosition (-1, -2)), "#5");
				Assert.AreEqual (2, Generator.IndexFromGeneratorPosition (new GeneratorPosition (-1, -3)), "#5");
			});
		}

		[TestMethod]
		[Asynchronous]
		public void IndexFromPosition_First_ThirdRealisedTest ()
		{
			bool realised;
			CreateAsyncTest (Control,
				() => {
					using (var v = IGenerator.StartAt (Generator.GeneratorPositionFromIndex (0), GeneratorDirection.Forward, true))
						IGenerator.GenerateNext (out realised);
					using (var v = IGenerator.StartAt (Generator.GeneratorPositionFromIndex (2), GeneratorDirection.Forward, true))
						IGenerator.GenerateNext (out realised);
				}, () => {
					Assert.AreEqual (0, Generator.IndexFromGeneratorPosition (new GeneratorPosition (0, 0)), "#1");
					Assert.AreEqual (1, Generator.IndexFromGeneratorPosition (new GeneratorPosition (0, 1)), "#2");
					Assert.AreEqual (2, Generator.IndexFromGeneratorPosition (new GeneratorPosition (1, 0)), "#3");
					Assert.AreEqual (3, Generator.IndexFromGeneratorPosition (new GeneratorPosition (1, 1)), "#4");
					Assert.AreEqual (4, Generator.IndexFromGeneratorPosition (new GeneratorPosition (1, 2)), "#5");
				}
			);
		}

		[TestMethod]
		[Asynchronous]
		[MoonlightBug ("Initially there's no link between the item and it's container. ItemsControl creates this link somehow...")]
		public void ItemFromContainerTest ()
		{
			bool realised;
			CreateAsyncTest (Control, () => {
				using (var g = IGenerator.StartAt (IGenerator.GeneratorPositionFromIndex (0), GeneratorDirection.Forward, true)) {
					var container = IGenerator.GenerateNext (out realised);
					var item = Generator.ItemFromContainer (container);
					Assert.IsNotNull (item, "#1.");
					Assert.AreEqual (typeof (object), item.GetType (), "#2");
					Assert.AreNotEqual (item, Control.Items [0], "#3");
				}
			});
		}

		[TestMethod]
		[Asynchronous]
		public void PositionFromIndex_NoRealisedTest ()
		{
			CreateAsyncTest (Control, () => {
				Assert.AreEqual (new GeneratorPosition (-1, 1), Generator.GeneratorPositionFromIndex (0), "#1");
				Assert.AreEqual (new GeneratorPosition (-1, 2), Generator.GeneratorPositionFromIndex (1), "#2");
				Assert.AreEqual (new GeneratorPosition (-1, 3), Generator.GeneratorPositionFromIndex (2), "#3");
				Assert.AreEqual (new GeneratorPosition (-1, 4), Generator.GeneratorPositionFromIndex (3), "#4");
				Assert.AreEqual (new GeneratorPosition (-1, 5), Generator.GeneratorPositionFromIndex (4), "#5");
			});
		}

		[TestMethod]
		[Asynchronous]
		public void PositionFromIndex_FirstRealisedTest ()
		{
			bool realised;
			CreateAsyncTest (Control,
				() => {
					using (var v = IGenerator.StartAt (Generator.GeneratorPositionFromIndex (0), GeneratorDirection.Forward, true))
						IGenerator.GenerateNext (out realised);
				}, () => {
					Assert.AreEqual (new GeneratorPosition (0, 0), Generator.GeneratorPositionFromIndex (0), "#1");
					Assert.AreEqual (new GeneratorPosition (0, 1), Generator.GeneratorPositionFromIndex (1), "#2");
					Assert.AreEqual (new GeneratorPosition (0, 2), Generator.GeneratorPositionFromIndex (2), "#3");
					Assert.AreEqual (new GeneratorPosition (0, 3), Generator.GeneratorPositionFromIndex (3), "#4");
					Assert.AreEqual (new GeneratorPosition (0, 4), Generator.GeneratorPositionFromIndex (4), "#5");
				}
			);
		}

		[TestMethod]
		[Asynchronous]
		public void PositionFromIndex_First_ThirdRealisedTest ()
		{
			bool realised;
			CreateAsyncTest (Control,
				() => {
					using (var v = IGenerator.StartAt (Generator.GeneratorPositionFromIndex (0), GeneratorDirection.Forward, true))
						IGenerator.GenerateNext (out realised);
					using (var v = IGenerator.StartAt (Generator.GeneratorPositionFromIndex (2), GeneratorDirection.Forward, true))
						IGenerator.GenerateNext (out realised);
				}, () => {
					Assert.AreEqual (new GeneratorPosition (0, 0), Generator.GeneratorPositionFromIndex (0), "#1");
					Assert.AreEqual (new GeneratorPosition (0, 1), Generator.GeneratorPositionFromIndex (1), "#2");
					Assert.AreEqual (new GeneratorPosition (1, 0), Generator.GeneratorPositionFromIndex (2), "#3");
					Assert.AreEqual (new GeneratorPosition (1, 1), Generator.GeneratorPositionFromIndex (3), "#4");
					Assert.AreEqual (new GeneratorPosition (1, 2), Generator.GeneratorPositionFromIndex (4), "#5");
				}
			);
		}

		[TestMethod]
		[Asynchronous]
		public void Recycle_OneContainer ()
		{
			bool fresh;
			object first, second;
			
			CreateAsyncTest (Control, () => {
				using (var g = IGenerator.StartAt (new GeneratorPosition (-1, 0), GeneratorDirection.Forward, false))
					first = IGenerator.GenerateNext (out fresh);
				Assert.IsTrue (fresh, "#1");

				IGenerator.Recycle(new GeneratorPosition (0, 0), 1);
				Assert.IsNull (Generator.ContainerFromIndex (0), "#2");

				using (var g = IGenerator.StartAt (new GeneratorPosition (-1, 0), GeneratorDirection.Forward, false))
					second = IGenerator.GenerateNext (out fresh);
				
				Assert.IsFalse (fresh, "#3");
				Assert.AreSame (first, second, "#4");

			});
		}

		[TestMethod]
		[Asynchronous]
		public void Recycle_CountOutOfRange ()
		{
			bool fresh;
			CreateAsyncTest (Control, () => {
				using (var g = IGenerator.StartAt (new GeneratorPosition (-1, 0), GeneratorDirection.Forward, false))
					IGenerator.GenerateNext (out fresh);

				// You can't Recycle elements which have not been realised.
				Assert.Throws<InvalidOperationException> (() => {
					IGenerator.Recycle (new GeneratorPosition (0, 0), 5);
				});
			});
		}

		[TestMethod]
		[Asynchronous]
		public void Recycle_NegativeOffset ()
		{
			bool fresh;
			CreateAsyncTest (Control, () => {
				using (var g = IGenerator.StartAt (new GeneratorPosition (-1, 0), GeneratorDirection.Forward, false))
					IGenerator.GenerateNext (out fresh);

				// Offset must be zero as we have to refer to a realized element.
				Assert.Throws<ArgumentException> (() => {
					IGenerator.Recycle (new GeneratorPosition (0, -1), 5);
				});
			});
		}

		[TestMethod]
		[Asynchronous]
		public void Recycle_PositiveOffset ()
		{
			bool fresh;
			CreateAsyncTest (Control, () => {
				using (var g = IGenerator.StartAt (new GeneratorPosition (-1, 0), GeneratorDirection.Forward, false))
					IGenerator.GenerateNext (out fresh);

				// Offset must be zero as we have to refer to a realized element.
				Assert.Throws<ArgumentException> (() => {
					IGenerator.Recycle (new GeneratorPosition (0, 1), 5);
				});
			});
		}

		[TestMethod]
		[Asynchronous]  
		public void StartAt_NegativeTest ()
		{
			CreateAsyncTest (Control, () => {
				IGenerator.StartAt (new GeneratorPosition (-100, -100), GeneratorDirection.Forward, true);
			});
		}

		[TestMethod]
		[Asynchronous]
		public void StartAt_Negative_GenerateTest ()
		{
			bool realized;
			CreateAsyncTest (Control, () => {
				using (var v = IGenerator.StartAt (new GeneratorPosition (-100, -100), GeneratorDirection.Forward, true))
					Assert.IsNull(IGenerator.GenerateNext (out realized));
			});
		}

		[TestMethod]
		[Asynchronous]
		public void StartAt_LastUnrealised_GenerateTest ()
		{
			// Generates the last unrealized item
			bool realized;
			CreateAsyncTest (Control, () => {
				using (var v = IGenerator.StartAt (new GeneratorPosition (-1, -1), GeneratorDirection.Forward, true))
					Assert.IsInstanceOfType<ContentPresenter> (IGenerator.GenerateNext (out realized), "#1");
				Assert.IsInstanceOfType<ContentPresenter> (Generator.ContainerFromIndex (4), "#2");
			});
		}

		[TestMethod] 
		[Asynchronous]
		public void StartAt_FirstUnrealised_GenerateTest ()
		{
			// Generates the last unrealized item
			bool realized;
			CreateAsyncTest (Control, () => {
				using (var v = IGenerator.StartAt (new GeneratorPosition (-1, 0), GeneratorDirection.Forward, true))
					Assert.IsInstanceOfType<ContentPresenter> (IGenerator.GenerateNext (out realized), "#1");
				Assert.IsInstanceOfType<ContentPresenter> (Generator.ContainerFromIndex (0), "#2");
			});
		}

		[TestMethod]
		[Asynchronous]
		public void StartAt_Middle_Then_FirstTest ()
		{
			// Generates the last unrealized item
			bool realized;
			CreateAsyncTest (Control, () => {
				using (var v = IGenerator.StartAt (new GeneratorPosition (-1, 3), GeneratorDirection.Forward, true))
					Assert.IsInstanceOfType<ContentPresenter> (IGenerator.GenerateNext (out realized), "#1");

				using (var v = IGenerator.StartAt (new GeneratorPosition (0, -2), GeneratorDirection.Forward, true))
					Assert.IsInstanceOfType<ContentPresenter> (IGenerator.GenerateNext (out realized), "#2");

				Assert.IsInstanceOfType<ContentPresenter> (Generator.ContainerFromIndex (0), "#3");
				Assert.IsNull (Generator.ContainerFromIndex (1), "#4");
				Assert.IsInstanceOfType<ContentPresenter> (Generator.ContainerFromIndex (2), "#5");
				Assert.IsNull (Generator.ContainerFromIndex (3), "#6");
				Assert.IsNull (Generator.ContainerFromIndex (4), "#7");
			});
		}

		[TestMethod]
		[Asynchronous]
		public void StartAt_Middle_Then_First2Test ()
		{
			// Generates the last unrealized item
			bool realized;
			CreateAsyncTest (Control, () => {
				using (var v = IGenerator.StartAt (new GeneratorPosition (-1, 0), GeneratorDirection.Forward, true))
					Assert.IsInstanceOfType<ContentPresenter> (IGenerator.GenerateNext (out realized), "#1");
				using (var v = IGenerator.StartAt (new GeneratorPosition (0, 2), GeneratorDirection.Forward, true))
					Assert.IsInstanceOfType<ContentPresenter> (IGenerator.GenerateNext (out realized), "#2");

				Assert.IsInstanceOfType<ContentPresenter> (Generator.ContainerFromIndex (0), "#3");
				Assert.IsNull (Generator.ContainerFromIndex (1), "#4");
				Assert.IsInstanceOfType<ContentPresenter> (Generator.ContainerFromIndex (2), "#5");
				Assert.IsNull (Generator.ContainerFromIndex (3), "#6");
				Assert.IsNull (Generator.ContainerFromIndex (4), "#7");
			});
		}

		[TestMethod]
		[Asynchronous]
		public void StartAt_FirstTest ()
		{
			// Generates the last unrealized item
			bool realized;
			CreateAsyncTest (Control, () => {
				using (var v = IGenerator.StartAt (new GeneratorPosition (-1, 0), GeneratorDirection.Forward, true))
					Assert.IsInstanceOfType<ContentPresenter> (IGenerator.GenerateNext (out realized), "#1");
				Assert.IsInstanceOfType<ContentPresenter> (Generator.ContainerFromIndex (0), "#2");
			});
		}

		[TestMethod]
		[Asynchronous]
		public void StartAt_TwiceTest ()
		{
			CreateAsyncTest (Control, () => {
				IGenerator.StartAt (new GeneratorPosition (-100, -100), GeneratorDirection.Forward, true);
				Assert.Throws<InvalidOperationException> (() => {
					IGenerator.StartAt (new GeneratorPosition (-100, -100), GeneratorDirection.Forward, true);
				});
			});
		}

		[TestMethod]
		[Asynchronous]
		public void Remove_NegativeOffsetTest ()
		{
			CreateAsyncTest (Control, () => {
				Assert.Throws<ArgumentException> (() => {
					IGenerator.Remove (new GeneratorPosition (0, -1), 1);
				});
			});
		}

		[TestMethod]
		[Asynchronous]
		public void Remove_PositiveOffsetTest ()
		{
			CreateAsyncTest (Control, () => {
				Assert.Throws<ArgumentException> (() => {
					IGenerator.Remove (new GeneratorPosition (0, 1), 1);
				});
			});
		}

		[TestMethod]
		[Asynchronous]
		[MoonlightBug ("The correct exception should be an InvalidOperation, not NullRef")]
		public void Remove_BeforeGenerateTest ()
		{
			// If you try to remove an item *before* you generate any containers
			// you hit a null ref.
			CreateAsyncTest (Control, () => {
				Assert.Throws<NullReferenceException> (() => {
					IGenerator.Remove (new GeneratorPosition (0, 0), 1);
				});
			});
		}

		[TestMethod]
		[Asynchronous]
		public void Remove_0Elements_1RealizedTest ()
		{
			CreateAsyncTest (Control, () => {
				Generate (0, 1);
				Assert.IsNotNull (Generator.ContainerFromIndex (0), "#1");
				IGenerator.Remove (new GeneratorPosition (0, 0), 1);
				Assert.IsNull (Generator.ContainerFromIndex (0), "#2");
			});
		}

		[TestMethod]
		[Asynchronous]
		public void Remove_1Element_1RealizedTest ()
		{
			CreateAsyncTest (Control, () => {
				AddElements (1);
				Generate (0, 1);
				Assert.IsNotNull (Generator.ContainerFromIndex (0), "#1");
				IGenerator.Remove (new GeneratorPosition (0, 0), 1);
				Assert.IsNull (Generator.ContainerFromIndex (0), "#2");
			});
		}

		[TestMethod]
		[Asynchronous]
		[MoonlightBug ("The correct exception should be an InvalidOperation, not NullRef")]
		public void Remove_BeyondRealizedRangeTest ()
		{
			// Removing an item beyond the range which has been generated
			// throws a null ref.
			CreateAsyncTest (Control, () => {
				Generate (0, 1);
				Assert.Throws<NullReferenceException> (() => {
					IGenerator.Remove (new GeneratorPosition (4, 0), 1);
				});
			});
		}

		[TestMethod]
		[Asynchronous]
		[MoonlightBug ("The correct exception should be an InvalidOperation, not NullRef")]
		public void Remove_BeyondRealizedRangeTest2 ()
		{
			// Removing an item beyond the range which has been generated
			// throws a null ref.
			CreateAsyncTest (Control, () => {
				Generate (0, 10);
				IGenerator.RemoveAll ();
				Assert.Throws<NullReferenceException> (() => {
					IGenerator.Remove (new GeneratorPosition (4, 0), 1);
				});
			});
		}

		[TestMethod]
		[Asynchronous]
		public void Remove_NotRealized_SpanTest ()
		{
			// You can't remove items which don't exist
			CreateAsyncTest (Control, () => {
				Generate (0, 1);
				Assert.Throws<InvalidOperationException> (() => {
					IGenerator.Remove (new GeneratorPosition (0, 0), 2);
				});
			});
		}

		[TestMethod]
		[Asynchronous]
		public void Remove_DoesntExistTest ()
		{
			// You can't remove items which don't exist
			CreateAsyncTest (Control, () => {
				Generate (0, 1);
				Assert.Throws<InvalidOperationException> (() => {
					IGenerator.Remove (new GeneratorPosition (0, 0), 2);
				});
			});
		}

		[TestMethod]
		[Asynchronous]
		public void RemoveAll_NonGeneratedTest ()
		{
			CreateAsyncTest (Control, () => {
				IGenerator.RemoveAll ();
			});
		}

		[TestMethod]
		[Asynchronous]
		public void RemoveAll_OneGeneratedTest ()
		{
			CreateAsyncTest (Control, () => {
				Generate (0, 1);
				IGenerator.RemoveAll ();
				Assert.IsNull (Generator.ContainerFromIndex (0), "#1");
			});
		}

		[TestMethod]
		[Asynchronous]
		public void RemoveAll_ManyGeneratedTest ()
		{
			CreateAsyncTest (Control, () => {
				Generate (0, 1);
				Generate (2, 6);
				Generate (9, 1);
				IGenerator.RemoveAll ();
				for (int i = 0; i < 15; i++)
					Assert.IsNull (Generator.ContainerFromIndex (i), "#" + i);
			});
		}

		void AddElements (int count)
		{
			while (count-- > 0)
				Panel.Children.Add (new Rectangle { Name = count.ToString () }); ;
		}

		void Generate (int index, int count)
		{
			bool realized;
			var p = IGenerator.GeneratorPositionFromIndex (index);
			using (var d = IGenerator.StartAt (p, GeneratorDirection.Forward, false))
				while (count-- > 0)
					IGenerator.GenerateNext (out realized);
		}
	}
}
