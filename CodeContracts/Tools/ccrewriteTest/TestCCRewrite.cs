﻿#define CONTRACTS_FULL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace ccrewriteTest {

	[TestFixture]
	public class TestCCRewrite {

		private RewriteAndLoad ral = null;

		[TestFixtureSetUp]
		public void FixtureSetup ()
		{
			this.ral = new RewriteAndLoad ();
			this.ral.Load ();
		}

		[TestFixtureTearDown]
		public void FixtureTearDown ()
		{
			if (this.ral != null) {
				this.ral.Dispose ();
			}
		}

		// ==

		private static void TestEqualsByte (byte value)
		{
			Contract.Requires (value == 0);
		}

		private static void TestEqualsSByte (sbyte value)
		{
			Contract.Requires (value == 0);
		}

		private static void TestEqualsUShort (ushort value)
		{
			Contract.Requires (value == 0);
		}

		private static void TestEqualsShort (short value)
		{
			Contract.Requires (value == 0);
		}

		private static void TestEqualsUInt (uint value)
		{
			Contract.Requires (value == 0);
		}

		private static void TestEqualsInt (int value)
		{
			Contract.Requires (value == 0);
		}

		private static void TestEqualsULong (ulong value)
		{
			Contract.Requires (value == 0);
		}

		private static void TestEqualsLong (long value)
		{
			Contract.Requires (value == 0);
		}

		private static void TestEqualsFloat (float value)
		{
			Contract.Requires (value == 0);
		}

		private static void TestEqualsDouble (double value)
		{
			Contract.Requires (value == 0);
		}

		// <

		private static void TestLessThanInt (int value)
		{
			Contract.Requires (value < 10);
		}

		private static void TestLessThanUInt (uint value)
		{
			Contract.Requires (value < 10);
		}

		private static void TestLessThanLong (long value)
		{
			Contract.Requires (value < 10);
		}

		private static void TestLessThanULong (ulong value)
		{
			Contract.Requires (value < 10);
		}

		private static void TestLessThanFloat (float value)
		{
			Contract.Requires (value < 10);
		}

		private static void TestLessThanDouble (double value)
		{
			Contract.Requires (value < 10);
		}

		// <=

		private static void TestLessThanOrEqualInt (int value)
		{
			Contract.Requires (value <= 10);
		}

		private static void TestLessThanOrEqualUInt (uint value)
		{
			Contract.Requires (value <= 10);
		}

		private static void TestLessThanOrEqualLong (long value)
		{
			Contract.Requires (value <= 10);
		}

		private static void TestLessThanOrEqualULong (ulong value)
		{
			Contract.Requires (value <= 10);
		}

		private static void TestLessThanOrEqualFloat (float value)
		{
			Contract.Requires (value <= 10);
		}

		private static void TestLessThanOrEqualDouble (double value)
		{
			Contract.Requires (value <= 10);
		}

		// >

		private static void TestGreaterThanInt (int value)
		{
			Contract.Requires (value > 10);
		}

		private static void TestGreaterThanUInt (uint value)
		{
			Contract.Requires (value > 10);
		}

		private static void TestGreaterThanLong (long value)
		{
			Contract.Requires (value > 10);
		}

		private static void TestGreaterThanULong (ulong value)
		{
			Contract.Requires (value > 10);
		}

		private static void TestGreaterThanFloat (float value)
		{
			Contract.Requires (value > 10);
		}

		private static void TestGreaterThanDouble (double value)
		{
			Contract.Requires (value > 10);
		}

		// >=

		private static void TestGreaterThanOrEqualInt (int value)
		{
			Contract.Requires (value >= 10);
		}

		private static void TestGreaterThanOrEqualUInt (uint value)
		{
			Contract.Requires (value >= 10);
		}

		private static void TestGreaterThanOrEqualLong (long value)
		{
			Contract.Requires (value >= 10);
		}

		private static void TestGreaterThanOrEqualULong (ulong value)
		{
			Contract.Requires (value >= 10);
		}

		private static void TestGreaterThanOrEqualFloat (float value)
		{
			Contract.Requires (value >= 10);
		}

		private static void TestGreaterThanOrEqualDouble (double value)
		{
			Contract.Requires (value >= 10);
		}

		// object ==

		private static void TestObjectEquals (object value)
		{
			Contract.Requires (value == null);
		}

		// object !=

		private static void TestObjectNotEquals (object value)
		{
			Contract.Requires (value != null);
		}



		private void CheckException (Expression<Action> fnExpr, params string [] messageContains)
		{
			try {
				this.ral.Call (fnExpr);
			} catch (Exception e) {
				string m = e.Message.Split ('\n', '\r') [0].Trim ();
				foreach (var contains in messageContains) {
					//Assert.IsTrue (m.Contains (contains));
					StringAssert.Contains (contains, m);
				}
				return;
			}
			Assert.Fail ("Contract should have thrown an exception");
		}

		private void CheckTwice (Expression<Action> fnGood, Expression<Action> fnBad, string condition)
		{
			const string PreFail = "Precondition failed";
			this.ral.Call (fnGood);
			this.CheckException (fnBad, PreFail, condition);
		}

		[Test]
		public void TestTrivial ()
		{
			const string CondEquals = "value == 0";

			this.CheckTwice (() => TestEqualsByte (0), () => TestEqualsByte (1), CondEquals);
			this.CheckTwice (() => TestEqualsSByte (0), () => TestEqualsSByte (1), CondEquals);
			this.CheckTwice (() => TestEqualsUShort (0), () => TestEqualsUShort (1), CondEquals);
			this.CheckTwice (() => TestEqualsShort (0), () => TestEqualsShort (1), CondEquals);
			this.CheckTwice (() => TestEqualsUInt (0), () => TestEqualsUInt (1), CondEquals);
			this.CheckTwice (() => TestEqualsInt (0), () => TestEqualsInt (1), CondEquals);
			this.CheckTwice (() => TestEqualsULong (0), () => TestEqualsULong (1), CondEquals);
			this.CheckTwice (() => TestEqualsLong (0), () => TestEqualsLong (1), CondEquals);
			this.CheckTwice (() => TestEqualsDouble (0), () => TestEqualsDouble (1), CondEquals);
			this.CheckTwice (() => TestEqualsFloat (0), () => TestEqualsFloat (1), CondEquals);

			const string CondLessThan = "value < 10";

			this.CheckTwice (() => TestLessThanInt (9), () => TestLessThanInt (10), CondLessThan);
			this.CheckTwice (() => TestLessThanUInt (9), () => TestLessThanUInt (10), CondLessThan);
			this.CheckTwice (() => TestLessThanLong (9), () => TestLessThanLong (10), CondLessThan);
			this.CheckTwice (() => TestLessThanULong (9), () => TestLessThanULong (10), CondLessThan);
			this.CheckTwice (() => TestLessThanFloat (9.9f), () => TestLessThanFloat (10), CondLessThan);
			this.CheckTwice (() => TestLessThanDouble (9.9), () => TestLessThanDouble (10), CondLessThan);

			const string CondLessThanOrEqual = "value <= 10";

			this.CheckTwice (() => TestLessThanOrEqualInt (10), () => TestLessThanOrEqualInt (11), CondLessThanOrEqual);
			this.CheckTwice (() => TestLessThanOrEqualUInt (10), () => TestLessThanOrEqualUInt (11), CondLessThanOrEqual);
			this.CheckTwice (() => TestLessThanOrEqualLong (10), () => TestLessThanOrEqualLong (11), CondLessThanOrEqual);
			this.CheckTwice (() => TestLessThanOrEqualULong (10), () => TestLessThanOrEqualULong (11), CondLessThanOrEqual);
			this.CheckTwice (() => TestLessThanOrEqualFloat (10.0f), () => TestLessThanOrEqualFloat (10.1f), CondLessThanOrEqual);
			this.CheckTwice (() => TestLessThanOrEqualDouble (10.0), () => TestLessThanOrEqualDouble (10.1), CondLessThanOrEqual);

			const string CondGreaterThan = "value > 10";

			this.CheckTwice (() => TestGreaterThanInt (11), () => TestGreaterThanInt (10), CondGreaterThan);
			this.CheckTwice (() => TestGreaterThanUInt (11), () => TestGreaterThanUInt (10), CondGreaterThan);
			this.CheckTwice (() => TestGreaterThanLong (11), () => TestGreaterThanLong (10), CondGreaterThan);
			this.CheckTwice (() => TestGreaterThanULong (11), () => TestGreaterThanULong (10), CondGreaterThan);
			this.CheckTwice (() => TestGreaterThanFloat (10.1f), () => TestGreaterThanFloat (10), CondGreaterThan);
			this.CheckTwice (() => TestGreaterThanDouble (10.1), () => TestGreaterThanDouble (10), CondGreaterThan);

			const string CondGreaterThanOrEqual = "value >= 10";

			this.CheckTwice (() => TestGreaterThanOrEqualInt (10), () => TestGreaterThanOrEqualInt (9), CondGreaterThanOrEqual);
			this.CheckTwice (() => TestGreaterThanOrEqualUInt (10), () => TestGreaterThanOrEqualUInt (9), CondGreaterThanOrEqual);
			this.CheckTwice (() => TestGreaterThanOrEqualLong (10), () => TestGreaterThanOrEqualLong (9), CondGreaterThanOrEqual);
			this.CheckTwice (() => TestGreaterThanOrEqualULong (10), () => TestGreaterThanOrEqualULong (9), CondGreaterThanOrEqual);
			this.CheckTwice (() => TestGreaterThanOrEqualFloat (10.0f), () => TestGreaterThanOrEqualFloat (9.9f), CondGreaterThanOrEqual);
			this.CheckTwice (() => TestGreaterThanOrEqualDouble (10.0), () => TestGreaterThanOrEqualDouble (9.9), CondGreaterThanOrEqual);

			object o = new object ();
			this.CheckTwice (() => TestObjectEquals (null), () => TestObjectEquals (o), "value == null");
			this.CheckTwice (() => TestObjectNotEquals (o), () => TestObjectNotEquals (null), "value != null");
		}

	}

}
