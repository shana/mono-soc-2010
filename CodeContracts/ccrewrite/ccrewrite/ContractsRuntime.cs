using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.Diagnostics.Contracts;
using Mono.Cecil.Cil;
using System.Diagnostics.Contracts.Internal;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Runtime.CompilerServices;

namespace ccrewrite {
	static class ContractsRuntime {

		private const string Namespace = "System.Diagnostics.Contracts";

		private static ModuleDefinition module;
		private static TypeDefinition typeContractsRuntime = null;
		private static TypeDefinition typeContractException = null;
		private static MethodDefinition methodContractExceptionCons = null;
		private static MethodDefinition methodTriggerFailure = null;
		private static MethodDefinition methodReportFailure = null;
		private static MethodDefinition methodRequires = null;

		public static void Initialise(ModuleDefinition module) {
			ContractsRuntime.module = module;
		}

		private static void EnsureTypeContractRuntime ()
		{
			if (typeContractsRuntime == null) {
				// Create type
				TypeReference typeObject = module.Import (typeof (object));
				TypeDefinition type = new TypeDefinition (Namespace, "__ContractsRuntime",
					TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.NotPublic | TypeAttributes.AnsiClass | TypeAttributes.AutoClass,
					typeObject);
				module.Types.Add (type);
				// Attach custom attributes
				var attrCompilerGeneratedCons = typeof (CompilerGeneratedAttribute).GetConstructor (Type.EmptyTypes);
				CustomAttribute attrCompilerGenerated = new CustomAttribute (module.Import (attrCompilerGeneratedCons));
				type.CustomAttributes.Add (attrCompilerGenerated);
				// Store type
				typeContractsRuntime = type;
			}
		}

		private static void EnsureTypeContractException ()
		{
			if (CmdOptions.ThrowOnFailure && typeContractException == null) {
				TypeReference typeVoid = module.Import (typeof (void));
				TypeReference typeContractFailureKind = module.Import (typeof (ContractFailureKind));
				TypeReference typeString = module.Import (typeof (string));
				TypeReference typeException = module.Import (typeof (Exception));
				// Create type
				TypeDefinition type = new TypeDefinition (Namespace, "ContractException",
					TypeAttributes.NestedPrivate | TypeAttributes.AnsiClass | TypeAttributes.AutoClass, typeException);
				typeContractsRuntime.NestedTypes.Add (type);
				// Create constructor
				MethodDefinition cons = new MethodDefinition (".ctor",
					MethodAttributes.Assem | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, typeVoid);
				cons.Parameters.Add (new ParameterDefinition ("kind", ParameterAttributes.None, typeContractFailureKind));
				cons.Parameters.Add (new ParameterDefinition ("failure", ParameterAttributes.None, typeString));
				cons.Parameters.Add (new ParameterDefinition ("usermsg", ParameterAttributes.None, typeString));
				cons.Parameters.Add (new ParameterDefinition ("condition", ParameterAttributes.None, typeString));
				cons.Parameters.Add (new ParameterDefinition ("inner", ParameterAttributes.None, typeException));
				var il = cons.Body.GetILProcessor ();
				il.Emit (OpCodes.Ldarg_0);
				il.Emit (OpCodes.Ldarg_2);
				il.Emit (OpCodes.Ldarg_S, cons.Parameters [4]);
				MethodReference mExceptionCons = module.Import (typeof (Exception).GetConstructor (new [] { typeof (string), typeof (Exception) }));
				il.Emit (OpCodes.Call, mExceptionCons);
				il.Emit (OpCodes.Ret);
				type.Methods.Add (cons);
				// Attach custom attributes
				var attrCompilerGeneratedCons = typeof (CompilerGeneratedAttribute).GetConstructor (Type.EmptyTypes);
				CustomAttribute attrCompilerGenerated = new CustomAttribute (module.Import (attrCompilerGeneratedCons));
				type.CustomAttributes.Add (attrCompilerGenerated);
				// Store constructor and type
				methodContractExceptionCons = cons;
				typeContractException = type;
			}
		}

		private static void EnsureMethodTriggerFailure ()
		{
			if (methodTriggerFailure == null) {
				TypeReference typeVoid = module.Import (typeof (void));
				TypeReference typeContractFailureKind = module.Import (typeof (ContractFailureKind));
				TypeReference typeString = module.Import (typeof (string));
				TypeReference typeException = module.Import (typeof (Exception));
				// Create method
				MethodDefinition method = new MethodDefinition ("TriggerFailure",
					MethodAttributes.Assem | MethodAttributes.Static, typeVoid);
				method.Parameters.Add (new ParameterDefinition ("kind", ParameterAttributes.None, typeContractFailureKind));
				method.Parameters.Add (new ParameterDefinition ("message", ParameterAttributes.None, typeString));
				method.Parameters.Add (new ParameterDefinition ("userMessage", ParameterAttributes.None, typeString));
				method.Parameters.Add (new ParameterDefinition ("conditionText", ParameterAttributes.None, typeString));
				method.Parameters.Add (new ParameterDefinition ("inner", ParameterAttributes.None, typeException));
				var il = method.Body.GetILProcessor ();
				if (CmdOptions.ThrowOnFailure) {
					il.Emit (OpCodes.Ldarg_0);
					il.Emit (OpCodes.Ldarg_1);
					il.Emit (OpCodes.Ldarg_2);
					il.Emit (OpCodes.Ldarg_3);
					il.Emit (OpCodes.Ldarg_S, method.Parameters [4]);
					il.Emit (OpCodes.Newobj, methodContractExceptionCons);
					il.Emit (OpCodes.Throw);
				} else {
					var mDebugFail = typeof (Debug).GetMethod ("Fail", new [] { typeof (string), typeof(string) });
					MethodReference methodDebugFail = module.Import (mDebugFail);
					il.Emit (OpCodes.Ldarg_1);
					il.Emit (OpCodes.Ldarg_2);
					il.Emit (OpCodes.Call, methodDebugFail);
					il.Emit (OpCodes.Ret);
				}
				typeContractsRuntime.Methods.Add (method);
				methodTriggerFailure = method;
			}
		}

		private static void EnsureMethodReportFailure ()
		{
			if (methodReportFailure == null) {
				TypeReference typeVoid = module.Import (typeof (void));
				TypeReference typeContractFailureKind = module.Import (typeof (ContractFailureKind));
				TypeReference typeString = module.Import (typeof (string));
				TypeReference typeException = module.Import (typeof (Exception));
				MethodReference mRaiseContractFailedEvent = module.Import(typeof (ContractHelper).GetMethod ("RaiseContractFailedEvent"));
				// Create method
				MethodDefinition method = new MethodDefinition ("ReportFailure",
					MethodAttributes.Assem | MethodAttributes.Static, typeVoid);
				method.Parameters.Add (new ParameterDefinition ("kind", ParameterAttributes.None, typeContractFailureKind));
				method.Parameters.Add (new ParameterDefinition ("message", ParameterAttributes.None, typeString));
				method.Parameters.Add (new ParameterDefinition ("conditionText", ParameterAttributes.None, typeString));
				method.Parameters.Add (new ParameterDefinition ("inner", ParameterAttributes.None, typeException));
				VariableDefinition vMsg = new VariableDefinition ("msg", typeString);
				method.Body.Variables.Add (vMsg);
				var il = method.Body.GetILProcessor ();
				il.Emit (OpCodes.Ldarg_0);
				il.Emit (OpCodes.Ldarg_1);
				il.Emit (OpCodes.Ldarg_2);
				il.Emit (OpCodes.Ldarg_3);
				il.Emit (OpCodes.Call, mRaiseContractFailedEvent);
				il.Emit (OpCodes.Stloc_0);
				il.Emit (OpCodes.Ldloc_0);
				var instRet = il.Create (OpCodes.Ret);
				il.Emit (OpCodes.Brfalse_S, instRet);
				il.Emit (OpCodes.Ldarg_0);
				il.Emit (OpCodes.Ldloc_0);
				il.Emit (OpCodes.Ldarg_1);
				il.Emit (OpCodes.Ldarg_2);
				il.Emit (OpCodes.Ldarg_3);
				il.Emit (OpCodes.Call, methodTriggerFailure);
				il.Append (instRet);
				typeContractsRuntime.Methods.Add (method);
				methodReportFailure = method;
			}
		}

		private static void EnsureGlobal ()
		{
			EnsureTypeContractRuntime ();
			EnsureTypeContractException ();
			EnsureMethodTriggerFailure ();
			EnsureMethodReportFailure ();
		}

		public static MethodDefinition GetRequires ()
		{
			EnsureGlobal ();
			if (methodRequires == null) {
				TypeReference typeVoid = module.Import (typeof (void));
				TypeReference typeBoolean = module.Import (typeof (bool));
				TypeReference typeString = module.Import (typeof (string));
				// Create method
				MethodDefinition method = new MethodDefinition ("Requires",
				    MethodAttributes.Assem | MethodAttributes.Static, typeVoid);
				method.Parameters.Add (new ParameterDefinition ("condition", ParameterAttributes.None, typeBoolean));
				method.Parameters.Add (new ParameterDefinition ("message", ParameterAttributes.None, typeString));
				method.Parameters.Add (new ParameterDefinition ("conditionText", ParameterAttributes.None, typeString));
				var il = method.Body.GetILProcessor ();
				il.Emit (OpCodes.Ldarg_0);
				var instRet = il.Create(OpCodes.Ret);
				il.Emit (OpCodes.Brtrue_S, instRet);
				il.Emit (OpCodes.Ldc_I4_0); // Assumes ContractFailureKind.Precondition == 0
				il.Emit (OpCodes.Ldarg_1);
				il.Emit (OpCodes.Ldarg_2);
				il.Emit (OpCodes.Ldnull);
				il.Emit (OpCodes.Call, methodReportFailure);
				il.Append (instRet);
				typeContractsRuntime.Methods.Add (method);
				// Attach custom attributes
				var attrDebugNonUserCodeCons = typeof (DebuggerNonUserCodeAttribute).GetConstructor (Type.EmptyTypes);
				CustomAttribute attrDebugNonUserCode = new CustomAttribute (module.Import (attrDebugNonUserCodeCons));
				method.CustomAttributes.Add (attrDebugNonUserCode);
				var attrReliabilityContractCons = typeof (ReliabilityContractAttribute).GetConstructor (new [] { typeof (Consistency), typeof (Cer) });
				// Blob for attribute: new ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)
				byte [] blob = new byte [] { 1, 0, 3, 0, 0, 0, 1, 0, 0, 0 };
				CustomAttribute attrReliabilityContract = new CustomAttribute (module.Import (attrReliabilityContractCons), blob);
				method.CustomAttributes.Add (attrReliabilityContract);
				// Store method
				methodRequires = method;
			}
			return methodRequires;
		}

	}
}
