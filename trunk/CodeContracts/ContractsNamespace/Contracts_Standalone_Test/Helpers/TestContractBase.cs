using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics.Contracts;
using System.Diagnostics;
using System.Reflection;
using System.Diagnostics.Contracts.Internal;

namespace ContractsTests.Helpers {
	public class TestContractBase {

		protected TestContractBase() {
			this.ContractExceptionType = Type.GetType("System.Diagnostics.Contracts.ContractException");
			if (this.ContractExceptionType == null) {
				// Special code for when Contracts namespace is not in CorLib
				var m = typeof(Contract).GetMethod("GetContractExceptionType", BindingFlags.NonPublic | BindingFlags.Static);
				this.ContractExceptionType = (Type)m.Invoke(null, null);
			}
		}

		protected AssertListener asserts;

		[SetUp]
		public void Setup() {
			// Remove all event handlers from Contract.ContractFailed
			var eventField = typeof(Contract).GetField("ContractFailed", BindingFlags.Static | BindingFlags.NonPublic);
            if (eventField == null) {
				// But in MS.NET it's done this way.
                eventField = typeof(ContractHelper).GetField("contractFailedEvent", BindingFlags.Static | BindingFlags.NonPublic);
            }
			eventField.SetValue(null, null);
			// Set up the assert listener
			this.asserts = new AssertListener();
			Debug.Listeners.Clear();
			Debug.Listeners.Add(this.asserts);
		}

		[TearDown]
		public void TearDown() {
			Debug.Listeners.Clear();
			this.asserts = null;
		}

		protected Type ContractExceptionType { get; private set; }

	}
}
