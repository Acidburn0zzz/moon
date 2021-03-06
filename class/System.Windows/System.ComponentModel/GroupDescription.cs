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
// Copyright (c) 2008 Novell, Inc. (http://www.novell.com)
//
// Authors:
//	Chris Toshok (toshok@ximian.com)
//

using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace System.ComponentModel {

	public abstract class GroupDescription : INotifyPropertyChanged
	{
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged {
			add { PropertyChanged += value; }
			remove { PropertyChanged -= value; }
		}

		protected virtual event PropertyChangedEventHandler PropertyChanged;

		public ObservableCollection<object> GroupNames {
			get; private set;
		}

		protected GroupDescription ()
		{
			GroupNames = new ObservableCollection<object> ();

			// FIXME: Write test on this
			GroupNames.CollectionChanged += delegate {
				OnPropertyChanged (new PropertyChangedEventArgs ("GroupNames"));
			};
		}

		public abstract object GroupNameFromItem (object item, int level, CultureInfo culture);

		public virtual bool NamesMatch (object groupName, object itemName)
		{
			return object.Equals (groupName, itemName);
		}

		protected virtual void OnPropertyChanged (PropertyChangedEventArgs e)
		{
			var h = PropertyChanged;
			if (h != null)
				h (this, e);
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		public bool ShouldSerializeGroupNames ()
		{
			return GroupNames.Count != 0;
		}
	}
}
