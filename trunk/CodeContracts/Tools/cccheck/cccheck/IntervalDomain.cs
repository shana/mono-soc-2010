using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cccheck {

	abstract class IntervalDomain {

		public abstract object AObj { get; }
		public abstract object BObj { get; }

		public abstract bool IsBottom { get; }
		public abstract bool IsTop { get; }
		public abstract bool IsSingleValue { get; }

		public abstract IntervalDomain Add (IntervalDomain other);

		public abstract Type TType { get; }

		public bool IsInt
		{
			get { return Type.GetTypeCode (this.TType) == TypeCode.Int32; }
		}

		public bool IsBool
		{
			get { return Type.GetTypeCode (this.TType) == TypeCode.Boolean; }
		}

		public abstract bool DoesIntersect (IntervalDomain other);

	}

	class IntervalDomain<T>:IntervalDomain {

		enum State {
			Bottom = 0,
			Top,
			Interval
		}

		public static readonly IntervalDomain<T> Bottom = new IntervalDomain<T> (State.Bottom);
		public static readonly IntervalDomain<T> Top = new IntervalDomain<T> (State.Top);

		public IntervalDomain (T a, T b)
		{
			this.state = State.Interval;
			this.A = a;
			this.B = b;
		}

		public IntervalDomain (T value)
		{
			this.state = State.Interval;
			this.A = value;
			this.B = value;
		}

		private IntervalDomain (State state)
		{
			this.state = state;
		}

		private State state;

		public T A { get; private set; }
		public T B { get; private set; }

		public override object AObj
		{
			get { return this.A; }
		}

		public override object BObj
		{
			get { return this.B; }
		}

		public override Type TType
		{
			get { return typeof (T); }
		}

		public override bool IsBottom
		{
			get { return this.state == State.Bottom; }
		}

		public override bool IsTop
		{
			get { return this.state == State.Top; }
		}

		public bool IsInterval
		{
			get { return this.state == State.Interval; }
		}

		public override bool IsSingleValue
		{
			get { return this.IsInterval && this.A.Equals(this.B); }
		}

		public override IntervalDomain Add (IntervalDomain other)
		{
			var o = (IntervalDomain<T>) other;
			T a = (T) (object) ((int) (object) this.A + (int) (object) o.A);
			T b = (T) (object) ((int) (object) this.B + (int) (object) o.B);
			return new IntervalDomain<T> (a, b);
		}

		public override bool DoesIntersect (IntervalDomain other)
		{
			var o = (IntervalDomain<T>) other;
			int a = (int)(object)this.A;
			int b = (int)(object)this.B;
			int oa = (int) (object) o.A;
			int ob = (int) (object) o.B;

			return
				!(a < oa && b < oa) || (a > ob && b > ob);
		}

		public override string ToString ()
		{
			switch (this.state) {
			case State.Bottom:
				return "Bottom";
			case State.Top:
				return "Top";
			case State.Interval:
				return this.A + "->" + this.B;
			default:
				return "Invalid";
			}
		}

	}
}
