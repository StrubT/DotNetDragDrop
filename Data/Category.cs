using System;
using Windows.UI;

namespace StrubT.BFH.DotNet.DragDrop.Data {

	public class Category {

		public Guid Guid { get; }

		public Color Color { get; set; }

		public string Name { get; set; }

		Category(Guid guid) {

			Guid = guid;
		}

		public Category() : this(Guid.NewGuid()) { }

		public Category(Guid guid, Color color, string name) : this(guid) {

			if (name == null)
				throw new ArgumentNullException(nameof(name));

			Color = color;
			Name = name;
		}
	}
}
