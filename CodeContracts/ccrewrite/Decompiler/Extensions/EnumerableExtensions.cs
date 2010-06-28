using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decompiler.Extensions {
	static class EnumerableExtensions {

		public static T Second<T> (this IEnumerable<T> en)
		{
			return en.ElementAt (1);
		}

	}
}
