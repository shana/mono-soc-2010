using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;

namespace AntlrParser
{
	static class JavaCompatibility
	{
		public static string getText(this IToken token) {
			return token.Text;
		}

		public static bool equals(this string str, string other) {
			return str.Equals(other);
		}
	}
}
