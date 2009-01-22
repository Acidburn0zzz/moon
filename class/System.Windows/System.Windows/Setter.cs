//
// Setter.cs
//
// Contact:
//   Moonlight List (moonlight-list@lists.ximian.com)
//
// Copyright 2008 Novell, Inc.
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

using Mono;
using System.Collections.Generic;

namespace System.Windows {

	public sealed partial class Setter : SetterBase {
		internal static readonly DependencyProperty PropertyProperty = DependencyProperty.Lookup (Kind.SETTER, "Property", typeof (DependencyProperty));
		internal static readonly DependencyProperty ValueProperty = DependencyProperty.Lookup (Kind.SETTER, "Value", typeof (object));
		internal static readonly DependencyProperty ConvertedValueProperty = DependencyProperty.Lookup (Kind.SETTER, "ConvertedValue", typeof (object));

		public Setter (DependencyProperty property, object value)
		{
			if (property == null)
				throw new NullReferenceException ();
			
			Property = property;
			Value = value;
		}

		public DependencyProperty Property {
			get { return (DependencyProperty) GetValue (PropertyProperty); }
			set { SetValue (PropertyProperty, value); }
		}

		public object Value {
			get { return GetValue (ManagedValueProperty); }
			set { SetValue (ManagedValueProperty, value); }
		}

		public object ConvertedValue {
			get { return GetValue (ConvertedValueProperty); }
			set { SetValue (ConvertedValueProperty, value); }
		}
	}
}