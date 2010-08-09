using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil.Cil;
using Mono.Cecil;

namespace cccheck {
	class Environment {

		public Environment ()
		{
			this.intVars = new Dictionary<VariableDefinition, IntervalDomain<int>> ();
			this.intParams = new Dictionary<ParameterDefinition, IntervalDomain<int>> ();
		}

		public Environment (IEnumerable<ParameterDefinition> defs, IEnumerable<IntervalDomain<int>> parameters)
		{
			this.intVars = new Dictionary<VariableDefinition, IntervalDomain<int>> ();
			this.intParams = new Dictionary<ParameterDefinition, IntervalDomain<int>> ();

			var defsA = defs.ToArray ();
			var parsA = parameters.ToArray ();
			for (int i = 0; i < defsA.Length; i++) {
				this.intParams.Add (defsA [i], parsA [i]);
			}
		}

		private Environment (Environment from, VariableDefinition variable, IntervalDomain<int> value)
		{
			this.intVars = new Dictionary<VariableDefinition, IntervalDomain<int>> (from.intVars);
			this.intParams = new Dictionary<ParameterDefinition, IntervalDomain<int>> (from.intParams);
			this.intVars [variable] = value;
		}

		private Environment (Environment from, ParameterDefinition parameter, IntervalDomain<int> value)
		{
			this.intVars = new Dictionary<VariableDefinition, IntervalDomain<int>> (from.intVars);
			this.intParams = new Dictionary<ParameterDefinition, IntervalDomain<int>> (from.intParams);
			this.intParams [parameter] = value;
		}

		private Dictionary<VariableDefinition, IntervalDomain<int>> intVars;
		private Dictionary<ParameterDefinition, IntervalDomain<int>> intParams;

		public Environment Set (VariableDefinition variable, IntervalDomain<int> value)
		{
			return new Environment (this, variable, value);
		}

		public IntervalDomain<int> Get (VariableDefinition variable)
		{
			return this.intVars [variable];
		}

		public Environment Set (ParameterDefinition parameter, IntervalDomain<int> value)
		{
			return new Environment (this, parameter, value);
		}

		public IntervalDomain<int> Get (ParameterDefinition parameter)
		{
			return this.intParams [parameter];
		}

	}
}
