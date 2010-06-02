using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace ContractsTests.Helpers {
	public class AssertListener : TraceListener {

		public class Assertion {

			public Assertion(string message) {
				this.Message = message;
			}

			public string Message{get;private set;}

		}

		public AssertListener() {
			this.assertations = new List<Assertion>();
			this.Asserts = new ReadOnlyCollection<Assertion>(this.assertations);
		}

		private List<Assertion> assertations;

		public override void Write(string message) {
			this.CreateAssertation(message);
		}

		public override void WriteLine(string message) {
			this.CreateAssertation(message);
		}

		public ReadOnlyCollection<Assertion> Asserts { get; private set; }

		private void CreateAssertation(string message) {
			if (message.StartsWith("Fail: ")) {
				message = message.Substring(6);
			}
			var assertation = new Assertion(message);
			this.assertations.Add(assertation);
		}

	}
}
