// 
// TaskVisitor.cs
//  
// Author:
//       Nikhil Sarda <diff.operator@gmail.com>
// 
// Copyright (c) 2010 Nikhil Sarda
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using MonoDevelop.CSharp.Dom;
using MonoDevelop.Projects.Dom;

namespace MonoDevelop.FreeSharper
{
	public class TaskAstVisitor : AbtractCSharpDomVisitor<object, object>
	{
		private AbstractAnalysisTask taskContext;
		private VisitorMetadata visitorMetaData;
		
		public static void VisitAstNodes(AbstractAnalysisTask taskCtx, MonoDevelop.Projects.Dom.INode node)
		{
			TaskAstVisitor visitor = new TaskAstVisitor(taskCtx);
			if(node != null)
				visitor.VisitChildren(node as ICSharpNode, null);
		}
		
		private TaskAstVisitor(AbstractAnalysisTask taskCtx) 
		{
			this.taskContext = taskCtx;
			this.visitorMetaData = VisitorMetadata.MetaDataFactory();
		}
		
		public override string ToString ()
		{
			return string.Format("[TaskAstVisitor, {0}]", this.taskContext.ToString());
		}
		
		public override object VisitAccessorDeclaration (Accessor accessorDeclaration, object data)
		{
			
			this.taskContext.ExecuteRules(accessorDeclaration);
			return base.VisitAccessorDeclaration(accessorDeclaration, data);
		}
		
		public override object VisitAnonymousMethodExpression (AnonymousMethodExpression anonymousMethodExpression, object data)
		{
			this.taskContext.ExecuteRules(anonymousMethodExpression);
			return base.VisitAnonymousMethodExpression(anonymousMethodExpression, data);
		}
		
		public override object VisitArgListExpression (ArgListExpression argListExpression, object data)
		{
			this.taskContext.ExecuteRules(argListExpression);
			return base.VisitArgListExpression(argListExpression, data);
		}
		
		public override object VisitArrayObjectCreateExpression (ArrayObjectCreateExpression arrayObjectCreateExpression, object data)
		{
			this.taskContext.ExecuteRules(arrayObjectCreateExpression);
			return base.VisitArrayObjectCreateExpression(arrayObjectCreateExpression, data);
		}
		
		public override object VisitAsExpression (AsExpression asExpression, object data)
		{
			this.taskContext.ExecuteRules(asExpression);
			return base.VisitAsExpression(asExpression, data);
		}
		
		public override object VisitAssignmentExpression (AssignmentExpression assignmentExpression, object data)
		{
			this.taskContext.ExecuteRules(assignmentExpression);
			return base.VisitAssignmentExpression(assignmentExpression, data);
		}
		
		public override object VisitAttribute (MonoDevelop.CSharp.Dom.Attribute attribute, object data)
		{
			this.taskContext.ExecuteRules(attribute);
			return base.VisitAttribute(attribute, data);
		}
		
		public override object VisitAttributeSection (AttributeSection attributeSection, object data)
		{
			this.taskContext.ExecuteRules(attributeSection);
			return base.VisitAttributeSection(attributeSection, data);
		}
		
		public override object VisitBaseReferenceExpression (BaseReferenceExpression baseReferenceExpression, object data)
		{
			this.taskContext.ExecuteRules(baseReferenceExpression);
			return base.VisitBaseReferenceExpression(baseReferenceExpression, data);
		}
		
		public override object VisitBinaryOperatorExpression (BinaryOperatorExpression binaryOperatorExpression, object data)
		{
			this.taskContext.ExecuteRules(binaryOperatorExpression);
			return base.VisitBinaryOperatorExpression(binaryOperatorExpression, data);
		}
		
		public override object VisitBlockStatement (BlockStatement blockStatement, object data)
		{
			this.taskContext.ExecuteRules(blockStatement);
			return base.VisitBlockStatement(blockStatement, data);
		}
		
		public override object VisitBreakStatement (BreakStatement breakStatement, object data)
		{
			this.taskContext.ExecuteRules(breakStatement);
			return base.VisitBreakStatement(breakStatement, data);
		}
		
		public override object VisitCaseLabel (CaseLabel caseLabel, object data)
		{
			this.taskContext.ExecuteRules(caseLabel);
			return base.VisitCaseLabel(caseLabel, data);
		}
		
		public override object VisitCastExpression (CastExpression castExpression, object data)
		{
			this.taskContext.ExecuteRules(castExpression);
			return base.VisitCastExpression(castExpression, data);
		}
		
		public override object VisitCatchClause (CatchClause catchClause, object data)
		{
			this.taskContext.ExecuteRules(catchClause);
			return base.VisitCatchClause(catchClause, data);
		}
		
		public override object VisitCheckedExpression (CheckedExpression checkedExpression, object data)
		{
			this.taskContext.ExecuteRules(checkedExpression);
			return base.VisitCheckedExpression(checkedExpression, data);
		}
		
		public override object VisitCheckedStatement (CheckedStatement checkedStatement, object data)
		{
			this.taskContext.ExecuteRules(checkedStatement);
			return base.VisitCheckedStatement(checkedStatement, data);
		}
		
		public override object VisitCompilationUnit (MonoDevelop.CSharp.Dom.CompilationUnit unit, object data)
		{
			this.taskContext.ExecuteRules(unit);
			return base.VisitCompilationUnit(unit, data);
		}
		
		public override object VisitConditionalExpression (ConditionalExpression conditionalExpression, object data)
		{
			this.taskContext.ExecuteRules(conditionalExpression);
			return base.VisitConditionalExpression(conditionalExpression, data);
		}
		
		public override object VisitConstraint (Constraint constraint, object data)
		{
			this.taskContext.ExecuteRules(constraint);
			return base.VisitConstraint(constraint, data);
		}
		
		public override object VisitConstructorDeclaration (ConstructorDeclaration constructorDeclaration, object data)
		{
			this.taskContext.ExecuteRules(constructorDeclaration);
			return base.VisitConstructorDeclaration(constructorDeclaration, data);
		}
		
		public override object VisitConstructorInitializer (ConstructorInitializer constructorInitializer, object data)
		{
			this.taskContext.ExecuteRules(constructorInitializer);
			return base.VisitConstructorInitializer(constructorInitializer, data);
		}
		
		public override object VisitContinueStatement (ContinueStatement continueStatement, object data)
		{
			this.taskContext.ExecuteRules(continueStatement);
			return base.VisitContinueStatement(continueStatement, data);
		}
		
		public override object VisitDefaultValueExpression (DefaultValueExpression defaultValueExpression, object data)
		{
			this.taskContext.ExecuteRules(defaultValueExpression);
			return base.VisitDefaultValueExpression(defaultValueExpression, data);
		}
		
		public override object VisitDelegateDeclaration (DelegateDeclaration delegateDeclaration, object data)
		{
			this.taskContext.ExecuteRules(delegateDeclaration);
			return base.VisitDelegateDeclaration(delegateDeclaration, data);
		}
		
		public override object VisitDestructorDeclaration (DestructorDeclaration destructorDeclaration, object data)
		{
			this.taskContext.ExecuteRules(destructorDeclaration);
			return base.VisitDestructorDeclaration(destructorDeclaration, data);
		}
		
		public override object VisitEmptyStatement (EmptyStatement emptyStatement, object data)
		{
			return base.VisitEmptyStatement(emptyStatement, data);
		}
		
		public override object VisitEnumDeclaration (EnumDeclaration enumDeclaration, object data)
		{
			this.taskContext.ExecuteRules(enumDeclaration);
			return base.VisitEnumDeclaration(enumDeclaration, data);
		}
		
		public override object VisitEnumMemberDeclaration (EnumMemberDeclaration enumMemberDeclaration, object data)
		{
			this.taskContext.ExecuteRules(enumMemberDeclaration);
			return base.VisitEnumMemberDeclaration(enumMemberDeclaration, data);
		}
		
		public override object VisitEventDeclaration (EventDeclaration eventDeclaration, object data)
		{
			this.taskContext.ExecuteRules(eventDeclaration);
			return base.VisitEventDeclaration(eventDeclaration, data);
		}
		
		public override object VisitExpressionStatement (ExpressionStatement expressionStatement, object data)
		{
			this.taskContext.ExecuteRules(expressionStatement);
			return base.VisitExpressionStatement(expressionStatement, data);
		}
		
		public override object VisitFieldDeclaration (FieldDeclaration fieldDeclaration, object data)
		{
			this.taskContext.ExecuteRules(fieldDeclaration);
			return base.VisitFieldDeclaration(fieldDeclaration, data);
		}
		
		public override object VisitFixedStatement (FixedStatement fixedStatement, object data)
		{
			this.taskContext.ExecuteRules(fixedStatement);
			return base.VisitFixedStatement(fixedStatement, data);
		}
		
		public override object VisitForeachStatement (ForeachStatement foreachStatement, object data)
		{
			this.taskContext.ExecuteRules(foreachStatement);
			return base.VisitForeachStatement(foreachStatement, data);
		}
		
		public override object VisitForStatement (ForStatement forStatement, object data)
		{
			this.taskContext.ExecuteRules(forStatement);
			return base.VisitForStatement(forStatement, data);
		}
		
		public override object VisitFullTypeName (FullTypeName fullTypeName, object data)
		{
			this.taskContext.ExecuteRules(fullTypeName);
			return base.VisitFullTypeName(fullTypeName, data);
		}
		
		public override object VisitGotoStatement (GotoStatement gotoStatement, object data)
		{
			this.taskContext.ExecuteRules(gotoStatement);
			return base.VisitGotoStatement(gotoStatement, data);
		}
		
		public override object VisitIdentifierExpression (IdentifierExpression identifierExpression, object data)
		{
			this.taskContext.ExecuteRules(identifierExpression);
			return base.VisitIdentifierExpression(identifierExpression, data);
		}
		
		public override object VisitIdentifier (Identifier identifier, object data)
		{
			this.taskContext.ExecuteRules(identifier);
			return base.VisitIdentifier(identifier, data);
		}
		
		public override object VisitIfElseStatement (IfElseStatement ifElseStatement, object data)
		{
			this.taskContext.ExecuteRules(ifElseStatement);
			return base.VisitIfElseStatement(ifElseStatement, data);
		}
		
		public override object VisitIndexerDeclaration (IndexerDeclaration indexerDeclaration, object data)
		{
			this.taskContext.ExecuteRules(indexerDeclaration);
			return base.VisitIndexerDeclaration(indexerDeclaration, data);
		}
		
		public override object VisitIndexerExpression (IndexerExpression indexerExpression, object data)
		{
			this.taskContext.ExecuteRules(indexerExpression);
			return base.VisitIndexerExpression(indexerExpression, data);
		}
		
		public override object VisitInvocationExpression (InvocationExpression invocationExpression, object data)
		{
			this.taskContext.ExecuteRules(invocationExpression);
			return base.VisitInvocationExpression(invocationExpression, data);
		}
		
		public override object VisitIsExpression (IsExpression isExpression, object data)
		{
			this.taskContext.ExecuteRules(isExpression);
			return base.VisitIsExpression(isExpression, data);
		}
		
		public override object VisitLabelStatement (LabelStatement labelStatement, object data)
		{
			this.taskContext.ExecuteRules(labelStatement);
			return base.VisitLabelStatement(labelStatement, data);
		}
		
		public override object VisitLambdaExpression (LambdaExpression lambdaExpression, object data)
		{
			this.taskContext.ExecuteRules(lambdaExpression);
			return base.VisitLambdaExpression(lambdaExpression, data);
		}
		
		public override object VisitLockStatement (LockStatement lockStatement, object data)
		{
			this.taskContext.ExecuteRules(lockStatement);
			return base.VisitLockStatement(lockStatement, data);
		}
		
		public override object VisitMemberReferenceExpression (MemberReferenceExpression memberReferenceExpression, object data)
		{
			this.taskContext.ExecuteRules(memberReferenceExpression);
			return base.VisitMemberReferenceExpression(memberReferenceExpression, data);
		}
		
		public override object VisitMethodDeclaration (MethodDeclaration methodDeclaration, object data)
		{
			if(!visitorMetaData.ActiveMethod.Equals(null))
				this.taskContext.ExecuteRules(methodDeclaration);
			this.visitorMetaData.ActiveMethod = methodDeclaration;
			return base.VisitMethodDeclaration(methodDeclaration, data);
		}
		
		public override object VisitNamespaceDeclaration (NamespaceDeclaration namespaceDeclaration, object data)
		{
			this.taskContext.ExecuteRules(namespaceDeclaration);
			return base.VisitNamespaceDeclaration(namespaceDeclaration, data);
		}
		
		public override object VisitNullReferenceExpression (NullReferenceExpression nullReferenceExpression, object data)
		{
			this.taskContext.ExecuteRules(nullReferenceExpression);
			return base.VisitNullReferenceExpression(nullReferenceExpression, data);
		}
		
		public override object VisitObjectCreateExpression (ObjectCreateExpression objectCreateExpression, object data)
		{
			this.taskContext.ExecuteRules(objectCreateExpression);
			return base.VisitObjectCreateExpression(objectCreateExpression, data);
		}
		
		public override object VisitOperatorDeclaration (OperatorDeclaration operatorDeclaration, object data)
		{
			this.taskContext.ExecuteRules(operatorDeclaration);
			return base.VisitOperatorDeclaration(operatorDeclaration, data);
		}
		
		public override object VisitParameterDeclarationExpression (ParameterDeclarationExpression parameterDeclarationExpression, object data)
		{
			this.taskContext.ExecuteRules(parameterDeclarationExpression);
			return base.VisitParameterDeclarationExpression(parameterDeclarationExpression, data);
		}
		
		public override object VisitParenthesizedExpression (ParenthesizedExpression parenthesizedExpression, object data)
		{
			this.taskContext.ExecuteRules(parenthesizedExpression);
			return base.VisitParenthesizedExpression(parenthesizedExpression, data);
		}
		
		public override object VisitPointerReferenceExpression (PointerReferenceExpression pointerReferenceExpression, object data)
		{
			this.taskContext.ExecuteRules(pointerReferenceExpression);
			return base.VisitPointerReferenceExpression(pointerReferenceExpression, data);
		}
		
		public override object VisitPrimitiveExpression (PrimitiveExpression primitiveExpression, object data)
		{
			this.taskContext.ExecuteRules(primitiveExpression);
			return base.VisitPrimitiveExpression(primitiveExpression, data);
		}
		
		public override object VisitPropertyDeclaration (PropertyDeclaration propertyDeclaration, object data)
		{
			this.taskContext.ExecuteRules(propertyDeclaration);
			return base.VisitPropertyDeclaration(propertyDeclaration, data);
		}
		
		public override object VisitQueryExpressionFromClause (QueryExpressionFromClause queryExpressionFromClause, object data)
		{
			this.taskContext.ExecuteRules(queryExpressionFromClause);
			return base.VisitQueryExpressionFromClause(queryExpressionFromClause, data);
		}
		
		public override object VisitQueryExpressionGroupClause (QueryExpressionGroupClause queryExpressionGroupClause, object data)
		{
			this.taskContext.ExecuteRules(queryExpressionGroupClause);
			return base.VisitQueryExpressionGroupClause(queryExpressionGroupClause, data);
		}
		
		public override object VisitQueryExpressionJoinClause (QueryExpressionJoinClause queryExpressionJoinClause, object data)
		{
			this.taskContext.ExecuteRules(queryExpressionJoinClause);
			return base.VisitQueryExpressionJoinClause(queryExpressionJoinClause, data);
		}
		
		public override object VisitQueryExpressionLetClause (QueryExpressionLetClause queryExpressionLetClause, object data)
		{
			this.taskContext.ExecuteRules(queryExpressionLetClause);
			return base.VisitQueryExpressionLetClause(queryExpressionLetClause, data);
		}
		
		public override object VisitQueryExpressionOrderClause (QueryExpressionOrderClause queryExpressionOrderClause, object data)
		{
			this.taskContext.ExecuteRules(queryExpressionOrderClause);
			return base.VisitQueryExpressionOrderClause(queryExpressionOrderClause, data);
		}
		
		public override object VisitQueryExpressionOrdering (QueryExpressionOrdering queryExpressionOrdering, object data)
		{
			this.taskContext.ExecuteRules(queryExpressionOrdering);
			return base.VisitQueryExpressionOrdering(queryExpressionOrdering, data);
		}
		
		public override object VisitQueryExpressionSelectClause (QueryExpressionSelectClause queryExpressionSelectClause, object data)
		{
			this.taskContext.ExecuteRules(queryExpressionSelectClause);
			return base.VisitQueryExpressionSelectClause(queryExpressionSelectClause, data);
		}
		
		public override object VisitQueryExpressionWhereClause (QueryExpressionWhereClause queryExpressionWhereClause, object data)
		{
			this.taskContext.ExecuteRules(queryExpressionWhereClause);
			return base.VisitQueryExpressionWhereClause(queryExpressionWhereClause, data);
		}
		
		public override object VisitReturnStatement (ReturnStatement returnStatement, object data)
		{
			this.taskContext.ExecuteRules(returnStatement);
			return base.VisitReturnStatement(returnStatement, data);
		}
		
		public override object VisitSizeOfExpression (SizeOfExpression sizeOfExpression, object data)
		{
			this.taskContext.ExecuteRules(sizeOfExpression);
			return base.VisitSizeOfExpression(sizeOfExpression, data);
		}
		
		public override object VisitStackAllocExpression (StackAllocExpression stackAllocExpression, object data)
		{
			this.taskContext.ExecuteRules(stackAllocExpression);
			return base.VisitStackAllocExpression(stackAllocExpression, data);
		}
		
		public override object VisitSwitchSection (SwitchSection switchSection, object data)
		{
			this.taskContext.ExecuteRules(switchSection);
			return base.VisitSwitchSection(switchSection, data);
		}
		
		public override object VisitSwitchStatement (SwitchStatement switchStatement, object data)
		{
			this.taskContext.ExecuteRules(switchStatement);
			return base.VisitSwitchStatement(switchStatement, data);
		}
		
		public override object VisitThisReferenceExpression (ThisReferenceExpression thisReferenceExpression, object data)
		{
			this.taskContext.ExecuteRules(thisReferenceExpression);
			return base.VisitThisReferenceExpression(thisReferenceExpression, data);
		}
		
		public override object VisitThrowStatement (ThrowStatement throwStatement, object data)
		{
			this.taskContext.ExecuteRules(throwStatement);
			return base.VisitThrowStatement(throwStatement, data);
		}
		
		public override object VisitTryCatchStatement (TryCatchStatement tryCatchStatement, object data)
		{
			this.taskContext.ExecuteRules(tryCatchStatement);
			return base.VisitTryCatchStatement(tryCatchStatement, data);
		}
		
		public override object VisitTypeDeclaration (TypeDeclaration typeDeclaration, object data)
		{
			this.VisitChildren(typeDeclaration as ICSharpNode, data);
			this.taskContext.ExecuteRules(typeDeclaration);
			return base.VisitTypeDeclaration(typeDeclaration, data);
		}
		
		public override object VisitTypeOfExpression (TypeOfExpression typeOfExpression, object data)
		{
			this.taskContext.ExecuteRules(typeOfExpression);
			return base.VisitTypeOfExpression(typeOfExpression, data);
		}
		
		public override object VisitUnaryOperatorExpression (UnaryOperatorExpression unaryOperatorExpression, object data)
		{
			this.taskContext.ExecuteRules(unaryOperatorExpression);
			return base.VisitUnaryOperatorExpression(unaryOperatorExpression, data);
		}
		
		public override object VisitUncheckedExpression (UncheckedExpression uncheckedExpression, object data)
		{
			this.taskContext.ExecuteRules(uncheckedExpression);
			return base.VisitUncheckedExpression(uncheckedExpression, data);
		}
		
		public override object VisitUncheckedStatement (UncheckedStatement uncheckedStatement, object data)
		{
			this.taskContext.ExecuteRules(uncheckedStatement);
			return base.VisitUncheckedStatement(uncheckedStatement, data);
		}
		
		public override object VisitUnsafeStatement (UnsafeStatement unsafeStatement, object data)
		{
			this.taskContext.ExecuteRules(unsafeStatement);
			return base.VisitUnsafeStatement(unsafeStatement, data);
		}
		
		public override object VisitUsingAliasDeclaration (UsingAliasDeclaration usingDeclaration, object data)
		{
			this.taskContext.ExecuteRules(usingDeclaration);
			return base.VisitUsingAliasDeclaration(usingDeclaration, data);
		}
		
		public override object VisitUsingDeclaration (UsingDeclaration usingDeclaration, object data)
		{
			this.taskContext.ExecuteRules(usingDeclaration);
			return base.VisitUsingDeclaration(usingDeclaration, data);
		}
		
		public override object VisitUsingStatement (UsingStatement usingStatement, object data)
		{
			this.taskContext.ExecuteRules(usingStatement);
			return base.VisitUsingStatement(usingStatement, data);
		}
		
		public override object VisitVariableDeclarationStatement (VariableDeclarationStatement variableDeclarationStatement, object data)
		{
			
			this.taskContext.ExecuteRules(variableDeclarationStatement);
			return base.VisitVariableDeclarationStatement(variableDeclarationStatement, data);
		}
		
		public override object VisitVariableInitializer (VariableInitializer variableInitializer, object data)
		{
			this.visitorMetaData.AddActiveMethodVariable(variableInitializer);
			this.taskContext.ExecuteRules(variableInitializer);
			return base.VisitVariableInitializer(variableInitializer, data);
		}
		
		public override object VisitWhileStatement (WhileStatement whileStatement, object data)
		{
			this.taskContext.ExecuteRules(whileStatement);
			return base.VisitWhileStatement(whileStatement, data);
		}
		
		public override object VisitYieldStatement (YieldStatement yieldStatement, object data)
		{
			this.taskContext.ExecuteRules(yieldStatement);
			return base.VisitYieldStatement(yieldStatement, data);
		}
	}
}