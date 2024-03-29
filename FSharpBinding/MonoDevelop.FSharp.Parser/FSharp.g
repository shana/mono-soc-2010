grammar FSharp;

options {
	language=CSharp2;
	k = 1;
	output = AST;
}

tokens {
	DO_ = 'do!';
	YIELD_ = 'yield!';
	RETURN_ = 'return!';
	LET_ = 'let!';
	USER_ = 'user!';

	ABSTRACT = 'abstract';
	AND = 'and';
	AS = 'as';
	ASSERT = 'assert';
	BASE = 'base';
	BEGIN = 'begin';
	CLASS = 'class';
	DEFAULT = 'default';
	DELEGATE = 'delegate';
	DO = 'do';
	DONE = 'done';
	DOWNCAST = 'downcast';
	DOWNTO = 'downto';
	ELIF = 'elif';
	ELSE = 'else';
	END = 'end';
	ENUM = 'enum';
	EXCEPTION = 'exception';
	EXTERN = 'extern';
	FALSE = 'false';
	FINALLY = 'finally';
	FOR = 'for';
	FUN = 'fun';
	FUNCTION = 'function';
	GLOBAL = 'global';
	IF = 'if';
	IN = 'in';
	INHERIT = 'inherit';
	INLINE = 'inline';
	INTERFACE = 'interface';
	INTERNAL = 'internal';
	LAZY = 'lazy';
	LET = 'let';
	MATCH = 'match';
	MEMBER = 'member';
	MODULE = 'module';
	MUTABLE = 'mutable';
	NAMESPACE = 'namespace';
	NEW = 'new';
	NULL = 'null';
	OF = 'of';
	OPEN = 'open';
	OR = 'or';
	OVERRIDE = 'override';
	PRIVATE = 'private';
	PUBLIC = 'public';
	REC = 'rec';
	RETURN = 'return';
	SIG = 'sig';
	STATIC = 'static';
	STRUCT = 'struct';
	THEN = 'then';
	TO = 'to';
	TRUE = 'true';
	TRY = 'try';
	TYPE = 'type';
	UNIT_VALUE = '()';
	UNMANAGED = 'unmanaged';
	UPCAST = 'upcast';
	USE = 'use';
	VAL = 'val';
	VOID = 'void';
	WHEN = 'when';
	WHILE = 'while';
	WITH = 'with';
	YIELD = 'yield';
	LPAREN = '(';
	RPAREN = ')';	
	VBAR = '|';
	BAR_BAR	= '||';
	UNDERSCORE = '_';
	OP_QUESTION = '?';
	
	ABBREV;
	ACCESS;
	ACCESSORS;	
	ACTIVE_PATTERN;
	AND_PAT;
	APP;
	ARG;
	ARGS;
	ARG_NAME;
	ARRAY;
	AS_PAT;
	ATTRIBUTE;
	ATTRIBUTES;
	ATTRIBUTE_TARGET;
	ATTR_PAT;
	CASE;
	CONSTRAINT;
	CONSTRAINTS;
	CONSTRUCTION;
	CONS_PAT;
	EXPR;
	FIELD;
	FIN;
	FUNC;
	IDENT_BINDING;
	IMPLEMENTATION;
	INSTANCE;
	LIST;
	LONG_IDENT;
	MEMBER_SIG;
	MEMBER_SPEC;
	MODULE_ABBREV;
	NAME;
	NAMED_PAT;
	NOT_STRUCT;
	OR_PAT;
	PAT_BINDING;
	PRE;
	PREFIX;
	PROPERTY;
	RANGE;
	RECORD;
	RECURSE;
	RULE;
	SCRIPT_FRAG;
	SHORT_COMP;
	SLICE;
	START;
	STATIC_INVOKE;
	STRUCT;
	SUFFIX;
	THIS;
	TUPLE;
	TUPLE_PAT;
	TYPAR;
	TYPARS;
	TYPE_PAT;
	TYPE_SPEC;
	UNION;
	USIG;
	
/*	AMP;
	AMP_AMP;
	MINUS;
	MINUS_DOT;
	PERCENT;
	PLUS;
	PLUS_DOT;*/
}

@namespace {AntlrParser}

@members {
	Boolean light = true;
}

op	:	LPAREN! opName RPAREN!
	|	OP_MUL_NAME^;

cmpOp	:	OP_LESS | OP_GREATER | OP_NE | '<' | '>';

identOrOp
	:	IDENT
	|	op;

symbolicOp
	:	OP_QLM
	|	OP_QUESTION
	|	OP_LESS	| OP_GREATER | OP_NE | OP_OR | OP_AND | OP_DOLLAR | OP_CONCAT
	|	OP_TILDA | OP_PLUS | OP_MINUS | OP_EXP | OP_MUL | OP_DIV | OP_MOD
	|	PLUS | MINUS | '<' | '>' | PREFIX_PLUS | PREFIX_MINUS | PREFIX_PERCENT | PREFIX_PLUS_DOT | PREFIX_MINUS_DOT
	|	PREFIX_AMP | PREFIX_AMP_AMP;

opName	:	symbolicOp
	|	rangeOpName
	|	activePatternOpName;

rangeOpName
	:	DOT_DOT^ DOT_DOT?;

activePatternOpName
	:	VBAR (IDENT VBAR)+ (UNDERSCORE VBAR)? -> ^(ACTIVE_PATTERN IDENT+ UNDERSCORE?);

longIdent
	:	IDENT ('.' IDENT)* -> ^(LONG_IDENT IDENT+);

constant	:	INTEGER | INT
	|	IEEE | BIGNUM | CHAR | STRING | VERBATIM_STRING | BYTE_ARRAY
	|	VERBATIM_BYTE_ARRAY | BYTE_CHAR | FALSE | TRUE | UNIT_VALUE; 

infixOrPrefixOp
	:	PLUS | MINUS | PLUS_DOT | MINUS_DOT | PERCENT | AMP | AMP_AMP;

prefixOp:	infixOrPrefixOp | prefixOnlyOp;

// TODO: !symbolicOp ?????
prefixOnlyOp
	:	PREFIX_AMP | PREFIX_AMP_AMP | PREFIX_MINUS | PREFIX_MINUS_DOT
	|	PREFIX_PLUS | PREFIX_PLUS_DOT | PREFIX_PERCENT
	|	OP_TILDA
	|	'!'^ symbolicOp;
	
/*infixOp	:	infixOrPrefixOp
	|	'.'* (OP_MINUS | OP_PLUS | OP_LESS | OP_GREATER | OP_OR | OP_AND | OP_CONCAT | OP_MUL | OP_DIV | OP_MOD)
	|	'!=' | BAR_BAR | '='
	|	':=' | '::' | '$' | OR | OP_QUESTION;*/

funcType:	tupleType (('->') => '->'^ funcType)?;

tupleType
	:	suffixType ( -> suffixType
	|	('*' suffixType)+ -> ^(TUPLE suffixType+));

// TODO: in not using longIdent alt here after inherit
suffixType
	:	constrainedType (((longIdent | '[') => arrayOrSuffix)+ -> ^(SUFFIX constrainedType arrayOrSuffix+)
	|	(~'[' | EOF)=> -> constrainedType);

arrayOrSuffix
	:	('[' ','* ']') => '['^ ','* ']'!
	|	(longIdent) => longIdent;

constrainedType // first alt should not work in exprs
	:	'#'+ (typar ((':>') => ':>' concreteType -> ^('#' ^(':>' typar concreteType))| -> ^('#' typar))
	|	concreteType -> ^('#' concreteType))
	|	typar ((':>') => ':>' concreteType -> ^(':>' typar concreteType)| -> typar)
	|	concreteType;

concreteType
	:	LPAREN! type RPAREN!
	|	longIdent ((('<' (funcType (',' funcType)*)? '>')) => '<' (funcType (',' funcType)*)? '>')?
	-> ^(NAME longIdent funcType*);

types	:	type (','! type)*;

atomicType
	:	'#'+ (LPAREN type RPAREN -> ^('#' type)
	|	typar -> ^('#' typar)
	|	longIdent (('<') => '<' types '>')? -> ^('#' ^(NAME longIdent types?)))
	|	(LPAREN type RPAREN -> type
	|	typar -> typar
	|	longIdent (('<') => '<' types '>')? -> ^(NAME longIdent types?));

type:	funcType;

typar	:	UNDERSCORE
	|	'\u0027'^ IDENT
	|	'^'^ IDENT;

constraint
	:	(genericConstraint) => genericConstraint
	|	staticTypars ':' LPAREN memberSig RPAREN -> ^(SIG staticTypars memberSig);

genericConstraint
	:	typar (':>'^ type
	|	':'! (NULL^
	|	LPAREN! NEW^ ':'! keyUnit! '->'! '\u0027T'! RPAREN!
	|	STRUCT^
	|	keyNot^ STRUCT
	|	ENUM^ '<'! type '>'!
	|	UNMANAGED^
	|	DELEGATE^ '<'! type ','! type '>'!));

constraints
	:	constraint (AND constraint)* -> ^(CONSTRAINTS constraint+);

typarDefn
	:	attributes? typar;

typarDefns
	:	'<'! typarDefn (','! typarDefn)* typarConstraints? '>'!;

typarConstraints
	:	WHEN constraint (AND constraint)* -> ^(CONSTRAINT constraint)+;

staticTypars
	:	'^'! IDENT
	|	LPAREN '^' IDENT (OR '^' IDENT)* RPAREN -> IDENT+;

semiExpr:	letExpr (';'^ semiExpr)?;

letExpr:	LET REC? binding (AND binding)* IN letExpr -> ^(LET ^(RECURSE REC?) ^(LET binding*) letExpr)
	|	USE binding (AND binding)* IN letExpr -> ^(USE ^(USE binding*) letExpr)
	|	blockExpr -> blockExpr;

blockExpr
	:	FUN^ argumentPats '->'! letExpr
	|	FUNCTION^ rules
	|	MATCH^ expr WITH! rules
	|	TRY^ expr (WITH rules |FINALLY letExpr) 
	|	ifExpr;

ifExpr	:	IF^ assignExpr THEN! assignExpr elifBranches? elseBranch?
	|	assignExpr;

assignExpr
	:	commaExpr ((':='^ | '<-'^) assignExpr)?;

commaExpr	:	orExpr (','^ orExpr) *;

orExpr	:	andExpr ((BAR_BAR^ | OR^) andExpr)*;

andExpr	:	castExpr ((AMP^ | AMP_AMP^) castExpr)*;

castExpr
	:	cmpExpr ((':>'^ | ':?>'^) type)?;

cmpExpr	:	concatExpr ((cmpOp^ | EQ^ | OP_OR^ | OP_AND^ | OP_DOLLAR^) concatExpr)*;

concatExpr	:	listExpr ((OP_CONCAT^ | '^'^) concatExpr)?;

listExpr	:
	typeTestExpr ('::'^ listExpr)?;

typeTestExpr	:
	infixPlusExpr (':?'^ type)?;

infixPlusExpr	:	mulExpr ((OP_PLUS | OP_MINUS | PLUS | MINUS) => (OP_PLUS^ | OP_MINUS^ | PLUS^ | MINUS^) mulExpr)*;

mulExpr	:	expExpr (('*'^ | PERCENT^ | OP_MUL^ | OP_DIV^ | OP_MOD^) expExpr)*;

expExpr	:	appOpExpr (OP_EXP^ expExpr)?;


appOpExpr
	:	(LAZY^ | ASSERT^ | UPCAST^ | DOWNCAST^)* appExpr;

// TODO: new can handle named types only
// TODO: AST

appExpr	:
	prefixExpr ((prefixOnlyExpr) =>((prefixOnlyExpr) => prefixOnlyExpr)+ ('{' compOrRangeExpr '}')* -> ^(APP prefixExpr prefixOnlyExpr+ compOrRangeExpr*)
	|	('{' compOrRangeExpr '}')+ -> ^(APP prefixExpr compOrRangeExpr+)
	|	-> prefixExpr)
	|	(NEW type) => NEW type prefixExpr (('{' compOrRangeExpr '}')+ -> ^(APP ^(NEW type prefixExpr) compOrRangeExpr+)
	|	-> ^(NEW type prefixExpr));

prefixExpr	:	prefixOp+ dotExpr -> ^(PREFIX dotExpr prefixOp+)
	|	dotExpr;

prefixOnlyExpr
	:	prefixOnlyOp+ dotExpr -> ^(PREFIX dotExpr prefixOnlyOp+)
	|	dotExpr;

// TODO: ranges vs slice expressions
dotExpr	:	callExpr ('.'^ (identOrOp | '['^ ((sliceRange) => sliceRange (',' sliceRange)? | expr) ']'!))*;
	
callExpr	:	typeExpr /*(LS_APP LPAREN expr RPAREN)**/;

typeExpr	:	simpleExpr /*(LS_TYAPP '<' types '>')**/;

newObjExpr
	:	NEW^ baseCall objectMembers interfaceImpls;

simpleExpr	
	:	(identOrOp) => identOrOp
	|	LPAREN ( (staticTypars) => staticTypars ':' LPAREN memberSig RPAREN expr RPAREN -> ^(STATIC_INVOKE expr memberSig staticTypars)
			|	expr RPAREN -> expr) 
	|	BEGIN! expr END!
//	|	{light?} LS_BEGIN expr LS_END
	|	'{'^ ((newObjExpr) => newObjExpr
	|	(fieldBinds) => fieldBinds
	|	expr WITH^ fieldBinds) '}'!
	|	'[' (((compOrRangeExpr) => compOrRangeExpr -> ^(LIST compOrRangeExpr) | letExpr (';' letExpr)*) -> ^(LIST letExpr+) | -> LIST) ']'
	|	'[|'(((compOrRangeExpr) => compOrRangeExpr -> ^(ARRAY compOrRangeExpr) | letExpr (';' letExpr)*) -> ^(ARRAY letExpr+) | -> ARRAY) '|]'
	|	NULL
	|	WHILE^ expr DO! expr DONE!
	|	FOR ((IDENT '=') => IDENT '=' st=expr TO fin=expr DO act=expr DONE -> ^(FOR '=' IDENT $st $fin $act)
	|	pat IN exprOrRangeExpr DO expr DONE -> ^(FOR IN pat exprOrRangeExpr expr))
	|	'<@'^ expr '@>'!
	|	'<@@'^ expr '@@>'!
	|	constant;

expr	:	semiExpr (':' type)?;
	
// exprs	:	expr (',' expr)*;

exprOrRangeExpr
	:	(rangeExpr) => rangeExpr
	|	letExpr;
	
elifBranches
	:	elifBranch+;

elifBranch
	:	ELIF^ assignExpr THEN! assignExpr;

elseBranch
	:	ELSE^ assignExpr;

// TODO: combine these options via namedPatOrOp	
binding	:	(identBinding) => identBinding
	|	patternBinding;

identBinding
	:	INLINE? access? identOrOp typarDefns? argumentPats returnType? '=' expr ->
	^(IDENT_BINDING identOrOp ^(TYPAR typarDefns?) ^(ARGS argumentPats) expr returnType? INLINE? access?);

patternBinding
	:	MUTABLE? access? namedPat typarDefns? returnType? '=' expr ->
	^(PAT_BINDING namedPat ^(TYPAR typarDefns?) expr returnType? MUTABLE? access?);

returnType
	:	':'^ type;

bindings:	binding (AND binding)* -> ^(AND binding+);

argumentPats
	:	atomicPat+;

fieldBind
	:	longIdent '=' letExpr -> ^(FIELD longIdent letExpr);

fieldBinds
	:	fieldBind (';' fieldBind)* -> fieldBind+;
	
objectConstruction
	:	type prefixExpr?;

baseCall:	objectConstruction (AS IDENT)?;

interfaceImpls
	:	interfaceImpl+;

interfaceImpl
	:	INTERFACE^ type objectMembers?;

objectMembers
	:	WITH^ valOrMemberDefns END!;
	
valOrMemberDefns
	:	(memberDefns) => memberDefns
	|	bindings;

memberDefns
	:	memberDefn+;

compOrRangeExpr
	:	(shortCompExpr) => shortCompExpr
	|	(rangeExpr) => rangeExpr
	|	compExpr;
	
compExpr:	LET_^ pat '='! expr IN! compExpr
	|	DO_^ expr IN! compExpr
	|	USER_^ pat '='! expr IN! compExpr
	|	YIELD_^ expr
	|	YIELD^ expr
	|	RETURN_^ expr
	|	RETURN^ expr
	|	expr;

shortCompExpr
	:	FOR pat IN exprOrRangeExpr '->' expr -> ^(SHORT_COMP pat exprOrRangeExpr expr);

rangeExpr
	:	orExpr '..' orExpr ('..' orExpr)? -> ^(RANGE orExpr+);
	
sliceRange
	:	beg=orExpr '..' fin=orExpr? -> ^(SLICE ^(START $beg) ^(FIN $fin)?)
	|	'..' orExpr -> ^(SLICE ^(FIN orExpr))
	|	'*' -> SLICE;
	
rule	:	pat patternGuard? '->' ifExpr -> ^(RULE pat ifExpr patternGuard?);

patternGuard
	:	WHEN! cmpExpr;

pat	:	attrPat;

attrPat
	:	attributes? asPat -> ^(ATTR_PAT asPat attributes?);

asPat	:
	orPat ((AS IDENT) => AS IDENT)* -> ^(AS_PAT orPat IDENT*);

orPat	:
	andPat (VBAR andPat)* -> ^(OR_PAT andPat+);

andPat	:
	tuplePat (AMP tuplePat)* -> ^(AND_PAT tuplePat+);

tuplePat	:
	consPat (',' consPat)* -> ^(TUPLE_PAT consPat+);

consPat	:
	typePat ('::' typePat)* -> ^(CONS_PAT typePat+);

typePat	:
	namedPat (':' type)* -> ^(TYPE_PAT namedPat type*);

namedPat:	longIdent simplePat? -> ^(NAMED_PAT longIdent simplePat?)
	|	simplePat;

simplePat
	:	constant
	|	UNDERSCORE
	|	LPAREN! pat RPAREN!
	|	listPat
	|	arrayPat
	|	recordPat
	|	':?'^ atomicType ((AS) => AS! IDENT)?
	|	NULL;
	
listPat	:	'[' (pat (';' pat)*)? ']' -> ^(LIST pat+);
	
arrayPat
	:	'[|' (pat (';' pat)*)? '|]' -> ^(ARRAY pat+);
	
recordPat
	:	'{' fieldPat (';' fieldPat)* '}' -> fieldPat+;

atomicPat
	:	constant
	|	longIdent
	|	listPat
	|	recordPat
	|	arrayPat
	|	LPAREN! pat RPAREN!
	|	':?'^ atomicType
	|	NULL
	|	UNDERSCORE;
	
fieldPat:	longIdent '=' pat -> ^(FIELD longIdent pat);

// http://research.microsoft.com/en-us/um/cambridge/projects/fsharp/manual/spec.html#_Toc264041966
/* nonIdentPatParam
	:	constant
	|	'['patParam (';'patParam)* ']'
	|	LPARENpatParam (','patParam)* RPAREN
	|	'<@' expr '@>'
	|	'<@@' expr '@@>'
	|	NULL;

patParam:	(nonIdentPatParam
	|	longIdent+ nonIdentPatParam?) (':' type)?; */

// pats	:	pat (',' pat)*;

fieldPats
	:	fieldPat (';'fieldPat)* -> fieldPat+;
	
rules	:	VBAR? rule (VBAR rule)* -> rule+;

typeDefns
	:	typeDefn+;

primaryConstrArgs
	:	attributes? access? LPAREN tuplePat? RPAREN -> ^(NAME NEW ^(ATTRIBUTES attributes?) access? tuplePat?);

typeDefn:	TYPE typeName primaryConstrArgs? asBinding? typeDefnBody -> ^(TYPE typeName typeDefnBody primaryConstrArgs? asBinding?);

typeDefnBody
	:	'='! ((abbrevTypeDefn) => abbrevTypeDefn
	|	recordTypeDefn
	|	(enumTypeDefn) => enumTypeDefn
	|	unionTypeDefn
	|	anonTypeDefn
	|	classTypeDefn
	|	structTypeDefn
	|	interfaceTypeDefn
	|	delegateTypeDefn)
	|	typeExtension;

typeName:	attributes? access? IDENT typarDefns? -> ^(NAME IDENT ^(ATTRIBUTES attributes?) access? typarDefns?);

abbrevTypeDefn
// TODO: no constr or as
	:	type -> ^(ABBREV type);

unionTypeDefn
// TODO: no constr or as
	:	unionTypeCases typeExtensionElements? -> ^(UNION unionTypeCases typeExtensionElements?);

unionTypeCases
	:	VBAR? unionTypeCase (VBAR unionTypeCase)* -> unionTypeCase+;

unionTypeCase
	:	attributes? IDENT (OF tupleType
	|	':' uncurriedSig)? -> ^(CASE IDENT ^(ATTRIBUTES attributes?) tupleType? uncurriedSig?);

unionTypeCaseData
	:	IDENT ( -> ^(CASE IDENT)
	|	unionTypeCaseType -> ^(CASE IDENT unionTypeCaseType));

unionTypeCaseType
	:	OF^ tupleType
	|	':'^ uncurriedSig;

recordTypeDefn
// TODO: no constr or as
	:	'{' recordFields '}' typeExtensionElements? -> ^(RECORD recordFields typeExtensionElements?);
	
recordFields
	:	recordField (';' recordField)* -> ^(FIELD recordField)+;

recordField
	:	attributes? MUTABLE? access? IDENT ':' type -> attributes? IDENT type MUTABLE? access?;

anonTypeDefn
	:	BEGIN^ classTypeBody END!
//	|	{light}? LS_BEGIN classTypeBody LS_END
	;
	
classTypeDefn
	:	CLASS^ classTypeBody END!;

asBinding:	AS^ IDENT;

// TODO: switch to typeDefn after first typeDefn
classTypeBody
	:	classInheritsDecl? ((classLetBinding) => classLetBinding)* typeDefnElement*;

classInheritsDecl
	:	INHERIT^ type prefixExpr?;

classLetBindings
	:	classLetBinding+;

classLetBinding
	:	attributes? STATIC? moduleLetBinding -> ^(LET ^(ATTRIBUTES attributes?) ^(INSTANCE STATIC?) moduleLetBinding);

structTypeDefn
	:	STRUCT^ structTypeBody END!;

structTypeBody
	:	typeDefnElement+;

interfaceTypeDefn
// TODO: no constr or as
	:	INTERFACE^ interfaceTypeBody END!;

interfaceTypeBody
	:	typeDefnElement+;

exceptionDefn
	:	EXCEPTION^ ((unionTypeCaseData) => unionTypeCaseData | IDENT '='! longIdent);

enumTypeDefn
// TODO: no constr or as
	:	enumTypeCases -> ^(ENUM enumTypeCases);

enumTypeCases
	:	VBAR? enumTypeCase (VBAR enumTypeCase)* -> ^(CASE enumTypeCase)+;

enumTypeCase
	:	IDENT '='! constant;

delegateTypeDefn
// TODO: no constr or as
	:	delegateSignature;

delegateSignature
	:	DELEGATE^ OF! uncurriedSig;

typeExtension
	:	typeExtensionElements;

typeExtensionElements
	:	WITH^ typeDefnElement+ END!;

typeDefnElement
	:	memberDefn
	|	interfaceImpl;

additionalConstrDefn
	:	access? NEW pat asBinding '=' additionalConstrExpr -> pat asBinding ^(EXPR additionalConstrExpr) ^(ACCESS access?);

additionalConstrExpr
	:	(letExpr ';')* (additionalConstrInitExpr) => additionalConstrInitExpr (THEN expr)? ->
	^(PRE letExpr*) ^(CONSTRUCTION additionalConstrInitExpr) ^(THEN expr)?;

additionalConstrInitExpr
	:	'{' INHERIT type prefixExpr? (fieldBinds) => fieldBinds '}' -> ^(INHERIT type ^(EXPR prefixExpr?) fieldBinds)
	|	NEW? type prefixExpr -> ^(NEW type prefixExpr);

memberBinding
	:	((identPrefix) => identPrefix)? ((IDENT WITH) => IDENT WITH bindings | binding) ->
	^(THIS identPrefix?) ^(PROPERTY IDENT bindings)? binding?;

memberDefn
	:	attributes? ((stat=STATIC? (MEMBER memberBinding -> ^(MEMBER ^(ATTRIBUTES attributes?) memberBinding STATIC?)
	|	VAL MUTABLE? access? IDENT ':' type -> ^(VAL ^(ATTRIBUTES attributes?) ^(STATIC $stat) IDENT type MUTABLE? access?)))
	|	ABSTRACT MEMBER? access? memberSig -> ^(ABSTRACT ^(ATTRIBUTES attributes?) memberSig access?)
	|	OVERRIDE memberBinding -> ^(OVERRIDE  ^(ATTRIBUTES attributes?) memberBinding)
	|	DEFAULT memberBinding -> ^(DEFAULT ^(ATTRIBUTES attributes?) memberBinding)
	|	additionalConstrDefn -> ^(NEW ^(ATTRIBUTES attributes?) additionalConstrDefn));

memberDefnBody
	:	;

/*staticMemberBinding
	:	binding
	|	IDENT WITH bindings;*/

identPrefix
	:	IDENT '.'!;

memberSig
	: IDENT typarDefns? ':' curriedSig propSig? -> ^(MEMBER_SIG curriedSig ^(ACCESSORS propSig?) ^(TYPARS typarDefns?));

propSig	:	WITH! 
	(	keyGet (','! keySet)?
	|	keySet (','! keyGet)?);

// TODO: last should be a type
curriedSig:	argsSpec ('->' argsSpec)* -> ^(SIG ^(ARGS argsSpec)+);

uncurriedSig:	argSpec ('*' argSpec)* '->' type -> ^(USIG type argSpec+);

argsSpec:	argSpec ('*' argSpec)* -> argSpec+;

argSpec:	attributes? ((argNameSpec) => argNameSpec)? suffixType -> ^(ARG suffixType ^(ARG_NAME argNameSpec?) ^(ATTRIBUTES attributes?));

argNameSpec:	OP_QUESTION? IDENT ':' -> IDENT OP_QUESTION?;

interfaceSpec:	INTERFACE^ type;

namespaceDeclGroup
	:	NAMESPACE^ (longIdent moduleElem+
	|	GLOBAL moduleElem+);

moduleDefn:	MODULE access? IDENT '=' BEGIN moduleElem+ END -> ^(MODULE ^(ACCESS access?) IDENT moduleElem+);

moduleElem
	:	(moduleAbbrev) => moduleAbbrev
	|	typeDefn
	|	attributes? (exceptionDefn
	|	moduleLetBinding
	|	moduleDefn)
	|	importDecl
	|	compilerDirectiveDecl;

moduleLetBinding
	:	LET REC? binding (AND binding)* -> ^(LET ^(RECURSE REC?) binding*)
	|	DO^ expr;

importDecl:	OPEN^ longIdent;

moduleAbbrev:	MODULE IDENT '=' longIdent -> ^(MODULE_ABBREV IDENT longIdent);

compilerDirectiveDecl:	'#'^ IDENT STRING+;

access
	:	PRIVATE
	|	INTERNAL
	|	PUBLIC;

namespaceDeclGroupSpec
	:	NAMESPACE^ longIdent moduleSpecElements
	|	MODULE^ longIdent moduleSpecElements;

// moduleSpec:	MODULE IDENT '=' moduleSpecBody;

funcValSpec
	:	MUTABLE? curriedSig -> ^(FUNC curriedSig MUTABLE?);

moduleSpecElement
	:	VAL! ((funcValSpec) => funcValSpec
	|	binding)
	|	TYPE! typeSpecs
	|	EXCEPTION^ IDENT OF! type
	|	MODULE^ IDENT '='! (moduleSpecBody | longIdent)
	|	importDecl;

moduleSpecElements:	((moduleSpecElement) => moduleSpecElement)+;

moduleSpecBody:	BEGIN! moduleSpecElements END!
//	| {light}? LS_BEGIN moduleSpecElements LS_END
	;

typeSpec
	:	typeName typeSpecSpec -> ^(TYPE_SPEC typeName typeSpecSpec);

typeSpecSpec
	:	typeExtensionSpec
	|	'='! ((abbrevTypeSpec) => abbrevTypeSpec
	|	(enumTypeSpec) => enumTypeSpec
	|	unionTypeSpec
	|	anonTypeSpec
	|	classTypeSpec
	|	structTypeSpec
	|	interfaceTypeSpec
	|	delegateTypeSpec
	|	recordTypeSpec);

typeSpecs:	typeSpec (AND typeSpec)* -> typeSpec+;

typeSpecElement
	:	attributes? mem=memberSpec -> ^(MEMBER_SPEC $mem ^(ATTRIBUTES attributes?))
	|	INTERFACE^ type;

memberSpec
	:	access? NEW ':' uncurriedSig -> ^(NEW uncurriedSig access?)
	|	MEMBER access? memberSig -> ^(MEMBER memberSig access?)
	|	ABSTRACT access? memberSig -> ^(ABSTRACT memberSig access?)
	|	OVERRIDE^ memberSig
	|	DEFAULT^ memberSig
	|	STATIC^ MEMBER! access? memberSig;

abbrevTypeSpec:	type;

unionTypeSpec:	unionTypeCases typeExtensionElementsSpec?;

recordTypeSpec:	'{'! recordFields '}'! typeExtensionElementsSpec?;

anonTypeSpec:	BEGIN^ typeSpecElement+ END!;

classTypeSpec:	CLASS^ typeSpecElement+ END!;

structTypeSpec:	STRUCT^ typeSpecElement+ END!;

interfaceTypeSpec:	INTERFACE^ typeSpecElement+ END!;

enumTypeSpec:	enumTypeCases;

delegateTypeSpec:	delegateSignature;

typeExtensionSpec:	typeExtensionElementsSpec;

typeExtensionElementsSpec:	WITH^ typeSpecElement+ END!;

implementationFile
	:	((compilerDirectiveDecl) => compilerDirectiveDecl)* (namespaceDeclGroup+ -> ^(IMPLEMENTATION ^('#' compilerDirectiveDecl*) namespaceDeclGroup)
	|	(MODULE longIdent ~('=' | DOT)) => namedModule -> ^(IMPLEMENTATION ^('#' compilerDirectiveDecl*) namedModule)
	|	anonymousModule -> ^(IMPLEMENTATION ^('#' compilerDirectiveDecl*) anonymousModule));

scriptFile:	implementationFile;

signatureFile
// TODO: if starts with MODULE then 1st alt
	:	(MODULE | NAMESPACE) => namespaceDeclGroupSpec+
	|	moduleSpecElements;

namedModule:	MODULE^ longIdent moduleElem+;

anonymousModule:	moduleElem+ -> ^(MODULE moduleElem+);

scriptFragment:	moduleElem+ -> ^(SCRIPT_FRAG moduleElem+);

attribute:	(attributeTarget ':')? objectConstruction -> ^(ATTRIBUTE ^(ATTRIBUTE_TARGET attributeTarget?) objectConstruction);

attributeSet:	'[<' attribute (';' attribute)* '>]';

attributes:	attributeSet+;

attributeTarget:	keyAssembly | MODULE | RETURN | keyField | keyProperty | keyParam | TYPE | keyConstructor | keyEvent;

keyAssembly
	:	{input.LT(1).getText().equals("assembly")}? IDENT;

keyUnit
	:	{input.LT(1).getText().equals("unit")}? IDENT;

keyNot
	:	{input.LT(1).getText().equals("not")}? IDENT;

keyGet
	:	{input.LT(1).getText().equals("get")}? IDENT;

keySet
	:	{input.LT(1).getText().equals("set")}? IDENT;

keyField
	:	{input.LT(1).getText().equals("field")}? IDENT;

keyProperty
	:	{input.LT(1).getText().equals("property")}? IDENT;

keyParam
	:	{input.LT(1).getText().equals("param")}? IDENT;

keyConstructor
	:	{input.LT(1).getText().equals("constructor")}? IDENT;

keyEvent
	:	{input.LT(1).getText().equals("event")}? IDENT;

fragment
Whitespace
	:	(' ' | '\t')+;

fragment
NewLine	:	'\n' | '\r' '\n';

fragment
WhitespaceOrNewLine
	:	Whitespace | NewLine;
	
WHITESPACE
	:	WhitespaceOrNewLine {$channel=HIDDEN;};
	
IF_DIRECTIVE
	:	'#if' Whitespace IdentText;

ELSE_DIRECTIVE
	:	'#else';

ENDIF_DIRECTIVE
	:	'#endif';
	
fragment
DigitChar
	:	'0'..'9';
	
fragment
LetterChar	
	:	UppercaseLetter | LowercaseLetter | TitlecaseLetter | ModifierLetter | OtherLetter | LetterNumber ;

fragment
ConnectingChar
	:	ConnectorPunctuation;

fragment
CombiningChar
	:	NonSpacingMark | SpacingCombiningMark;
	
fragment
FormattingChar
	:	Format;

fragment
IdentStartChar
	:	LetterChar | UNDERSCORE;

fragment
IdentChar
	:	LetterChar | ConnectingChar | CombiningChar | FormattingChar | Digit | '\u0027';

fragment
IdentText
	:	IdentStartChar IdentChar*;

/* IDENT_KEYWORD
	:	ABSTRACT | AND | AS | ASSERT | BASE | BEGIN | CLASS
	|	DEFAULT | DELEGATE | DO | DONE | DOWNCAST | DOWNTO
	|	ELIF | ELSE | END | EXCEPTION | EXTERN | FALSE
	|	FINALLY | FOR | FUN | FUNCTION | GLOBAL | IF | IN
	|	INHERIT | INLINE | INTERFACE | INTERNAL | LAZY | LET
	|	MATCH | MEMBER | MODULE | MUTABLE | NEW
	|	NULL | OF | OPEN | OR | OVERRIDE | PRIVATE | PUBLIC
	|	REC | RETURN | SIG | STATIC | STRUCT | THEN | TO
	|	TRUE | TRY | TYPE | UPCAST | USE | VAL | VOID
	|	WHEN | WHILE | WITH | YIELD; */

RESERVED_KEYWORD
	:	'atomic' | 'break' | 'checked' | 'component' | 'const' | 'constraint'
	|	'constructor' | 'continue' | 'eager' | 'fixed' | 'fori' | 'functor'
	|	'include' | 'measure' | 'method' | 'mixin' | 'object' | 'parallel'
	|	'params' | 'process' | 'protected' | 'pure' | 'recursive' | 'sealed'
	|	'tailcall' | 'trait' | 'virtual' | 'volatile';

/* RESERVED_IDENT_FORMAT
	:	IdentText ('!' | '#'); */

IDENT	:	IdentText
	|	'``' ~('\n' | '\r' | '\t' | '`')+ | '`'~('\n' | '\r' | '\t' | '`') '``';
	
fragment
EscapeChar
	:	'\\' ('\\' | '\"' | '\u0027' | 'b' | 'n' | 'r' | 't');

fragment
NonEscapeChars
	:	'\\' ~('\\' | '\"' | '\u0027' | 'b' | 'n' | 'r' | 't');
fragment
SimpleChar
	:	~('\n' | '\t' | '\r' | '\b' | '\u0027' | '\\' | '\"');

fragment
Ugraph	:	'\\' 'u' Hex Hex Hex Hex;

fragment
ULgraph	:	'\\' 'U' Hex Hex Hex Hex Hex Hex Hex Hex;

fragment
CharChar:	SimpleChar | EscapeChar | Trigraph | Ugraph;

fragment
StringChar
	:	SimpleChar | EscapeChar | NonEscapeChars | Trigraph | Ugraph | ULgraph | NewLine;

fragment
StringElem
	:	StringChar
	|	'\u0027' NewLine Whitespace* StringElem;

CHAR	:	'\u0027' CharChar '\u0027';
STRING	:	'\"' StringChar* '\"';

fragment
VerbatimStringChar
	:	SimpleChar
	|	NonEscapeChars
	|	NewLine
	|	'\\'
	|	'""';

VERBATIM_STRING
	:	'@"' VerbatimStringChar* '"';

BYTE_CHAR
	:	'\u0027' SimpleOrEscapeChar '\u0027B';

BYTE_ARRAY
	:	'\"' StringChar* '\"B';

VERBATIM_BYTE_ARRAY
	:	'@\"' VerbatimStringChar* '\"B';

EOL_COMMENT
	:	'//' ~('\n' | '\r')* {$channel=HIDDEN;};

MULTILINE_COMMENT
	:	'(**)' {$channel=HIDDEN;}
	|	'(*' ~'*' (options {greedy=false;} : .)* '*)' {$channel=HIDDEN;};
	
fragment
SimpleOrEscapeChar
	:	EscapeChar | SimpleChar;

fragment
Trigraph:	'\\' Digit Digit Digit;

OP_MUL_NAME
	:	'(*)';

/* SYMBOLIC_KEYWORD
	:	'let!' | 'use!' | 'do!' | 'yield!' | 'return!'
	|	'->' | '<-' | '.' | ':' | LPAREN | RPAREN | '[<' | '>]' |'[|' | '|]' | '[' | ']'
	|	'{' | '}' | '\'' | '#' | ':?>' | ':>' | '..' | ':=' | ';;' | ';' | '='
	|	UNDERSCORE | '??' | OP_QUESTION | OP_MUL_NAME | '<@@' | '<@' | '@@>' | '@>'; */

/* RESERVED_SYMBOLS
	:	'~' | '`';*/

fragment
FirstOpChar
	:	'!' | '%' | '&' | '*' | '+' | '-' | '.' | '/' | '<' | '=' | '>' | '@' | '^' | '|' | '~';
fragment
OpChar	:	FirstOpChar | OP_QUESTION;

PLUS	:	'+' Whitespace;

PREFIX_PLUS
	:	// ('+' ~(OpChar | Whitespace | '.')) => '+'
	// |	
	'+'
	;

MINUS	:	'-' Whitespace;

PREFIX_MINUS
	:	// ('-' ~(OpChar | Whitespace | '.')) => '-'
	//|	
	'-'
	;

PLUS_DOT:	'+.' Whitespace;

PREFIX_PLUS_DOT
	:	// ('+.' ~(OpChar | Whitespace)) => '+.'
	//|	
	'+.'
	;

MINUS_DOT
	:	'-.' Whitespace;

PREFIX_MINUS_DOT
	:	// ('-.' ~(OpChar | Whitespace)) => '-.'
	//|	
	'-.'
	;

PERCENT	:	'%' Whitespace;

PREFIX_PERCENT
	:	// ('%' ~(OpChar | Whitespace)) => '%'
	//|	
	'%'
	;
AMP	:	'&' Whitespace;
PREFIX_AMP
	:	// ('&' ~(OpChar | Whitespace)) => '&'
	//|	
	'&'
	;

AMP_AMP	:	'&&' Whitespace;
	
PREFIX_AMP_AMP
	:	// ('&&' ~(OpChar | Whitespace)) => '&&'
	//|	
	'&&'
	;

fragment
QuoteOpLeft
	:	'<@@' | '<@';
fragment
QuoteOpRight
	:	'@@>' | '@>';

fragment
Digit	:	'0' .. '9';

fragment
Hex : ('0'..'9' | 'a'..'f' | 'A'..'F') ;

fragment
Octal	:	'0' .. '7';

fragment
Bit	:	'0' .. '1';

fragment
Int	:	Digit+;

fragment
XInt	:	Int
	|	('0' ((('x' | 'X') Hex+ )
	|	(('o' | 'O') Octal+ )
	|	(('b' | 'B') Bit+ )));
IEEE	:	Float ('f' | 'F')?
	|	XInt ('lf' | 'LF');
INTEGER	:	XInt ('y' | 'uy' | 's' | 'us' | 'l' | ('ul' | 'u') | 'L' | ('UL' | 'uL') | 'n' | 'un');

BIGNUM	:	Int ('Q' | 'R' | 'Z' | 'I' | 'N' | 'G');
DECIMAL	:	(Float|Int) ('M' | 'm');
INT	:	Int;

fragment
Float
	:   Digit+ ('.' Digit*)? ('E' | 'e') ('+' | '-')? Digit+
	|   Digit+ '.' Digit*;

fragment
Hexgraph:	'\\x' Hex Hex;

MAXINT	:	Int '.' '.';

COMMENT
	:   '//' ~('\n' | '\r')* '\r'? '\n' {$channel=HIDDEN;}
	|   '/*' ( options {greedy=false;} : . )* '*/' {$channel=HIDDEN;}
	;

HASH	:	'#';
STAR	:	'*';
COMMA	:	',';
RARROW	:	'->';
QQMARK	:	'??';
DOT_DOT	:	'..';
DOT	:	'.';
COLON_COLON
	:	'::';
COLON_GREATER
	:	':>';

COLON_QMARK_GREATER
	:	':?>';
COLON_QMARK
	:	':?';
COLON_EQ:	':=';
COLON	:	':';

SEMICOLON
	:	';';
LARROW	:	'<-';
EQ	:	'=';
LBRACK	:	'[';
LBRACK_BAR
	:	'[|';
RBRACK	:	']';
BAR_RBRACK
	:	'|]';
LBRACE	:	'{';
RBRACE	:	'}';
DOLLAR	:	'$';
// SPERCENT:	PERCENT;
// DPERCENT:	'%%';

OP_QLM	:	'?<-';

OP_LESS	:	'<' OpChar*;

OP_GREATER
	:	'>' OpChar*;

OP_NE	:	'!=' OpChar*;

OP_OR	:	'|' OpChar*;

OP_AND	:	'&' OpChar*;

OP_DOLLAR
	:	'$' OpChar*;

OP_CONCAT	:	'^' OpChar*;

OP_TILDA:	'~'+;

OP_PLUS	:	'+' OpChar*;

OP_MINUS:	'-' OpChar*;

OP_EXP	:	'**' OpChar*;

OP_MUL	:	'*' OpChar*;

OP_DIV	:	'/' OpChar*;

OP_MOD	:	'%' OpChar*;

fragment
EXPONENT : ('e' | 'E') ('+' | '-')? Digit+ ;

fragment
Space	:	' ';

fragment
Tab	:	'\t';

fragment
White	:	Space | Tab;

fragment
OperationChar
	:	'!' | '$' | '%' | '&' | '*' | '+' | '-' | '.' | '/' | '<' | '=' | '>' | '?' | '@' | '^' | '|' | '~' | ':';
	
fragment
NoOpChar:	'.' | '$';

fragment
UppercaseLetter :
	'\u0041' .. '\u005A'	|
	'\u00C0' .. '\u00D6'	|
	'\u00D8' .. '\u00DE'	|
	'\u0100'	|
	'\u0102'	|
	'\u0104'	|
	'\u0106'	|
	'\u0108'	|
	'\u010A'	|
	'\u010C'	|
	'\u010E'	|
	'\u0110'	|
	'\u0112'	|
	'\u0114'	|
	'\u0116'	|
	'\u0118'	|
	'\u011A'	|
	'\u011C'	|
	'\u011E'	|
	'\u0120'	|
	'\u0122'	|
	'\u0124'	|
	'\u0126'	|
	'\u0128'	|
	'\u012A'	|
	'\u012C'	|
	'\u012E'	|
	'\u0130'	|
	'\u0132'	|
	'\u0134'	|
	'\u0136'	|
	'\u0139'	|
	'\u013B'	|
	'\u013D'	|
	'\u013F'	|
	'\u0141'	|
	'\u0143'	|
	'\u0145'	|
	'\u0147'	|
	'\u014A'	|
	'\u014C'	|
	'\u014E'	|
	'\u0150'	|
	'\u0152'	|
	'\u0154'	|
	'\u0156'	|
	'\u0158'	|
	'\u015A'	|
	'\u015C'	|
	'\u015E'	|
	'\u0160'	|
	'\u0162'	|
	'\u0164'	|
	'\u0166'	|
	'\u0168'	|
	'\u016A'	|
	'\u016C'	|
	'\u016E'	|
	'\u0170'	|
	'\u0172'	|
	'\u0174'	|
	'\u0176'	|
	'\u0178' .. '\u0179'	|
	'\u017B'	|
	'\u017D'	|
	'\u0181' .. '\u0182'	|
	'\u0184'	|
	'\u0186' .. '\u0187'	|
	'\u0189' .. '\u018B'	|
	'\u018E' .. '\u0191'	|
	'\u0193' .. '\u0194'	|
	'\u0196' .. '\u0198'	|
	'\u019C' .. '\u019D'	|
	'\u019F' .. '\u01A0'	|
	'\u01A2'	|
	'\u01A4'	|
	'\u01A6' .. '\u01A7'	|
	'\u01A9'	|
	'\u01AC'	|
	'\u01AE' .. '\u01AF'	|
	'\u01B1' .. '\u01B3'	|
	'\u01B5'	|
	'\u01B7' .. '\u01B8'	|
	'\u01BC'	|
	'\u01C4'	|
	'\u01C7'	|
	'\u01CA'	|
	'\u01CD'	|
	'\u01CF'	|
	'\u01D1'	|
	'\u01D3'	|
	'\u01D5'	|
	'\u01D7'	|
	'\u01D9'	|
	'\u01DB'	|
	'\u01DE'	|
	'\u01E0'	|
	'\u01E2'	|
	'\u01E4'	|
	'\u01E6'	|
	'\u01E8'	|
	'\u01EA'	|
	'\u01EC'	|
	'\u01EE'	|
	'\u01F1'	|
	'\u01F4'	|
	'\u01F6' .. '\u01F8'	|
	'\u01FA'	|
	'\u01FC'	|
	'\u01FE'	|
	'\u0200'	|
	'\u0202'	|
	'\u0204'	|
	'\u0206'	|
	'\u0208'	|
	'\u020A'	|
	'\u020C'	|
	'\u020E'	|
	'\u0210'	|
	'\u0212'	|
	'\u0214'	|
	'\u0216'	|
	'\u0218'	|
	'\u021A'	|
	'\u021C'	|
	'\u021E'	|
	'\u0220'	|
	'\u0222'	|
	'\u0224'	|
	'\u0226'	|
	'\u0228'	|
	'\u022A'	|
	'\u022C'	|
	'\u022E'	|
	'\u0230'	|
	'\u0232'	|
	'\u023A' .. '\u023B'	|
	'\u023D' .. '\u023E'	|
	'\u0241'	|
	'\u0243' .. '\u0246'	|
	'\u0248'	|
	'\u024A'	|
	'\u024C'	|
	'\u024E'	|
	'\u0386'	|
	'\u0388' .. '\u038A'	|
	'\u038C'	|
	'\u038E' .. '\u038F'	|
	'\u0391' .. '\u03A1'	|
	'\u03A3' .. '\u03AB'	|
	'\u03D2' .. '\u03D4'	|
	'\u03D8'	|
	'\u03DA'	|
	'\u03DC'	|
	'\u03DE'	|
	'\u03E0'	|
	'\u03E2'	|
	'\u03E4'	|
	'\u03E6'	|
	'\u03E8'	|
	'\u03EA'	|
	'\u03EC'	|
	'\u03EE'	|
	'\u03F4'	|
	'\u03F7'	|
	'\u03F9' .. '\u03FA'	|
	'\u03FD' .. '\u042F'	|
	'\u0460'	|
	'\u0462'	|
	'\u0464'	|
	'\u0466'	|
	'\u0468'	|
	'\u046A'	|
	'\u046C'	|
	'\u046E'	|
	'\u0470'	|
	'\u0472'	|
	'\u0474'	|
	'\u0476'	|
	'\u0478'	|
	'\u047A'	|
	'\u047C'	|
	'\u047E'	|
	'\u0480'	|
	'\u048A'	|
	'\u048C'	|
	'\u048E'	|
	'\u0490'	|
	'\u0492'	|
	'\u0494'	|
	'\u0496'	|
	'\u0498'	|
	'\u049A'	|
	'\u049C'	|
	'\u049E'	|
	'\u04A0'	|
	'\u04A2'	|
	'\u04A4'	|
	'\u04A6'	|
	'\u04A8'	|
	'\u04AA'	|
	'\u04AC'	|
	'\u04AE'	|
	'\u04B0'	|
	'\u04B2'	|
	'\u04B4'	|
	'\u04B6'	|
	'\u04B8'	|
	'\u04BA'	|
	'\u04BC'	|
	'\u04BE'	|
	'\u04C0' .. '\u04C1'	|
	'\u04C3'	|
	'\u04C5'	|
	'\u04C7'	|
	'\u04C9'	|
	'\u04CB'	|
	'\u04CD'	|
	'\u04D0'	|
	'\u04D2'	|
	'\u04D4'	|
	'\u04D6'	|
	'\u04D8'	|
	'\u04DA'	|
	'\u04DC'	|
	'\u04DE'	|
	'\u04E0'	|
	'\u04E2'	|
	'\u04E4'	|
	'\u04E6'	|
	'\u04E8'	|
	'\u04EA'	|
	'\u04EC'	|
	'\u04EE'	|
	'\u04F0'	|
	'\u04F2'	|
	'\u04F4'	|
	'\u04F6'	|
	'\u04F8'	|
	'\u04FA'	|
	'\u04FC'	|
	'\u04FE'	|
	'\u0500'	|
	'\u0502'	|
	'\u0504'	|
	'\u0506'	|
	'\u0508'	|
	'\u050A'	|
	'\u050C'	|
	'\u050E'	|
	'\u0510'	|
	'\u0512'	|
	'\u0531' .. '\u0556'	|
	'\u10A0' .. '\u10C5'	|
	'\u1E00'	|
	'\u1E02'	|
	'\u1E04'	|
	'\u1E06'	|
	'\u1E08'	|
	'\u1E0A'	|
	'\u1E0C'	|
	'\u1E0E'	|
	'\u1E10'	|
	'\u1E12'	|
	'\u1E14'	|
	'\u1E16'	|
	'\u1E18'	|
	'\u1E1A'	|
	'\u1E1C'	|
	'\u1E1E'	|
	'\u1E20'	|
	'\u1E22'	|
	'\u1E24'	|
	'\u1E26'	|
	'\u1E28'	|
	'\u1E2A'	|
	'\u1E2C'	|
	'\u1E2E'	|
	'\u1E30'	|
	'\u1E32'	|
	'\u1E34'	|
	'\u1E36'	|
	'\u1E38'	|
	'\u1E3A'	|
	'\u1E3C'	|
	'\u1E3E'	|
	'\u1E40'	|
	'\u1E42'	|
	'\u1E44'	|
	'\u1E46'	|
	'\u1E48'	|
	'\u1E4A'	|
	'\u1E4C'	|
	'\u1E4E'	|
	'\u1E50'	|
	'\u1E52'	|
	'\u1E54'	|
	'\u1E56'	|
	'\u1E58'	|
	'\u1E5A'	|
	'\u1E5C'	|
	'\u1E5E'	|
	'\u1E60'	|
	'\u1E62'	|
	'\u1E64'	|
	'\u1E66'	|
	'\u1E68'	|
	'\u1E6A'	|
	'\u1E6C'	|
	'\u1E6E'	|
	'\u1E70'	|
	'\u1E72'	|
	'\u1E74'	|
	'\u1E76'	|
	'\u1E78'	|
	'\u1E7A'	|
	'\u1E7C'	|
	'\u1E7E'	|
	'\u1E80'	|
	'\u1E82'	|
	'\u1E84'	|
	'\u1E86'	|
	'\u1E88'	|
	'\u1E8A'	|
	'\u1E8C'	|
	'\u1E8E'	|
	'\u1E90'	|
	'\u1E92'	|
	'\u1E94'	|
	'\u1EA0'	|
	'\u1EA2'	|
	'\u1EA4'	|
	'\u1EA6'	|
	'\u1EA8'	|
	'\u1EAA'	|
	'\u1EAC'	|
	'\u1EAE'	|
	'\u1EB0'	|
	'\u1EB2'	|
	'\u1EB4'	|
	'\u1EB6'	|
	'\u1EB8'	|
	'\u1EBA'	|
	'\u1EBC'	|
	'\u1EBE'	|
	'\u1EC0'	|
	'\u1EC2'	|
	'\u1EC4'	|
	'\u1EC6'	|
	'\u1EC8'	|
	'\u1ECA'	|
	'\u1ECC'	|
	'\u1ECE'	|
	'\u1ED0'	|
	'\u1ED2'	|
	'\u1ED4'	|
	'\u1ED6'	|
	'\u1ED8'	|
	'\u1EDA'	|
	'\u1EDC'	|
	'\u1EDE'	|
	'\u1EE0'	|
	'\u1EE2'	|
	'\u1EE4'	|
	'\u1EE6'	|
	'\u1EE8'	|
	'\u1EEA'	|
	'\u1EEC'	|
	'\u1EEE'	|
	'\u1EF0'	|
	'\u1EF2'	|
	'\u1EF4'	|
	'\u1EF6'	|
	'\u1EF8'	|
	'\u1F08' .. '\u1F0F'	|
	'\u1F18' .. '\u1F1D'	|
	'\u1F28' .. '\u1F2F'	|
	'\u1F38' .. '\u1F3F'	|
	'\u1F48' .. '\u1F4D'	|
	'\u1F59'	|
	'\u1F5B'	|
	'\u1F5D'	|
	'\u1F5F'	|
	'\u1F68' .. '\u1F6F'	|
	'\u1FB8' .. '\u1FBB'	|
	'\u1FC8' .. '\u1FCB'	|
	'\u1FD8' .. '\u1FDB'	|
	'\u1FE8' .. '\u1FEC'	|
	'\u1FF8' .. '\u1FFB'	|
	'\u2102'	|
	'\u2107'	|
	'\u210B' .. '\u210D'	|
	'\u2110' .. '\u2112'	|
	'\u2115'	|
	'\u2119' .. '\u211D'	|
	'\u2124'	|
	'\u2126'	|
	'\u2128'	|
	'\u212A' .. '\u212D'	|
	'\u2130' .. '\u2133'	|
	'\u213E' .. '\u213F'	|
	'\u2145'	|
	'\u2183'	|
	'\u2C00' .. '\u2C2E'	|
	'\u2C60'	|
	'\u2C62' .. '\u2C64'	|
	'\u2C67'	|
	'\u2C69'	|
	'\u2C6B'	|
	'\u2C75'	|
	'\u2C80'	|
	'\u2C82'	|
	'\u2C84'	|
	'\u2C86'	|
	'\u2C88'	|
	'\u2C8A'	|
	'\u2C8C'	|
	'\u2C8E'	|
	'\u2C90'	|
	'\u2C92'	|
	'\u2C94'	|
	'\u2C96'	|
	'\u2C98'	|
	'\u2C9A'	|
	'\u2C9C'	|
	'\u2C9E'	|
	'\u2CA0'	|
	'\u2CA2'	|
	'\u2CA4'	|
	'\u2CA6'	|
	'\u2CA8'	|
	'\u2CAA'	|
	'\u2CAC'	|
	'\u2CAE'	|
	'\u2CB0'	|
	'\u2CB2'	|
	'\u2CB4'	|
	'\u2CB6'	|
	'\u2CB8'	|
	'\u2CBA'	|
	'\u2CBC'	|
	'\u2CBE'	|
	'\u2CC0'	|
	'\u2CC2'	|
	'\u2CC4'	|
	'\u2CC6'	|
	'\u2CC8'	|
	'\u2CCA'	|
	'\u2CCC'	|
	'\u2CCE'	|
	'\u2CD0'	|
	'\u2CD2'	|
	'\u2CD4'	|
	'\u2CD6'	|
	'\u2CD8'	|
	'\u2CDA'	|
	'\u2CDC'	|
	'\u2CDE'	|
	'\u2CE0'	|
	'\u2CE2'	|
	'\uFF21' .. '\uFF3A'	;

fragment
LowercaseLetter :
	'\u0061' .. '\u007A'	|
	'\u00AA'	|
	'\u00B5'	|
	'\u00BA'	|
	'\u00DF' .. '\u00F6'	|
	'\u00F8' .. '\u00FF'	|
	'\u0101'	|
	'\u0103'	|
	'\u0105'	|
	'\u0107'	|
	'\u0109'	|
	'\u010B'	|
	'\u010D'	|
	'\u010F'	|
	'\u0111'	|
	'\u0113'	|
	'\u0115'	|
	'\u0117'	|
	'\u0119'	|
	'\u011B'	|
	'\u011D'	|
	'\u011F'	|
	'\u0121'	|
	'\u0123'	|
	'\u0125'	|
	'\u0127'	|
	'\u0129'	|
	'\u012B'	|
	'\u012D'	|
	'\u012F'	|
	'\u0131'	|
	'\u0133'	|
	'\u0135'	|
	'\u0137' .. '\u0138'	|
	'\u013A'	|
	'\u013C'	|
	'\u013E'	|
	'\u0140'	|
	'\u0142'	|
	'\u0144'	|
	'\u0146'	|
	'\u0148' .. '\u0149'	|
	'\u014B'	|
	'\u014D'	|
	'\u014F'	|
	'\u0151'	|
	'\u0153'	|
	'\u0155'	|
	'\u0157'	|
	'\u0159'	|
	'\u015B'	|
	'\u015D'	|
	'\u015F'	|
	'\u0161'	|
	'\u0163'	|
	'\u0165'	|
	'\u0167'	|
	'\u0169'	|
	'\u016B'	|
	'\u016D'	|
	'\u016F'	|
	'\u0171'	|
	'\u0173'	|
	'\u0175'	|
	'\u0177'	|
	'\u017A'	|
	'\u017C'	|
	'\u017E' .. '\u0180'	|
	'\u0183'	|
	'\u0185'	|
	'\u0188'	|
	'\u018C' .. '\u018D'	|
	'\u0192'	|
	'\u0195'	|
	'\u0199' .. '\u019B'	|
	'\u019E'	|
	'\u01A1'	|
	'\u01A3'	|
	'\u01A5'	|
	'\u01A8'	|
	'\u01AA' .. '\u01AB'	|
	'\u01AD'	|
	'\u01B0'	|
	'\u01B4'	|
	'\u01B6'	|
	'\u01B9' .. '\u01BA'	|
	'\u01BD' .. '\u01BF'	|
	'\u01C6'	|
	'\u01C9'	|
	'\u01CC'	|
	'\u01CE'	|
	'\u01D0'	|
	'\u01D2'	|
	'\u01D4'	|
	'\u01D6'	|
	'\u01D8'	|
	'\u01DA'	|
	'\u01DC' .. '\u01DD'	|
	'\u01DF'	|
	'\u01E1'	|
	'\u01E3'	|
	'\u01E5'	|
	'\u01E7'	|
	'\u01E9'	|
	'\u01EB'	|
	'\u01ED'	|
	'\u01EF' .. '\u01F0'	|
	'\u01F3'	|
	'\u01F5'	|
	'\u01F9'	|
	'\u01FB'	|
	'\u01FD'	|
	'\u01FF'	|
	'\u0201'	|
	'\u0203'	|
	'\u0205'	|
	'\u0207'	|
	'\u0209'	|
	'\u020B'	|
	'\u020D'	|
	'\u020F'	|
	'\u0211'	|
	'\u0213'	|
	'\u0215'	|
	'\u0217'	|
	'\u0219'	|
	'\u021B'	|
	'\u021D'	|
	'\u021F'	|
	'\u0221'	|
	'\u0223'	|
	'\u0225'	|
	'\u0227'	|
	'\u0229'	|
	'\u022B'	|
	'\u022D'	|
	'\u022F'	|
	'\u0231'	|
	'\u0233' .. '\u0239'	|
	'\u023C'	|
	'\u023F' .. '\u0240'	|
	'\u0242'	|
	'\u0247'	|
	'\u0249'	|
	'\u024B'	|
	'\u024D'	|
	'\u024F' .. '\u0293'	|
	'\u0295' .. '\u02AF'	|
	'\u037B' .. '\u037D'	|
	'\u0390'	|
	'\u03AC' .. '\u03CE'	|
	'\u03D0' .. '\u03D1'	|
	'\u03D5' .. '\u03D7'	|
	'\u03D9'	|
	'\u03DB'	|
	'\u03DD'	|
	'\u03DF'	|
	'\u03E1'	|
	'\u03E3'	|
	'\u03E5'	|
	'\u03E7'	|
	'\u03E9'	|
	'\u03EB'	|
	'\u03ED'	|
	'\u03EF' .. '\u03F3'	|
	'\u03F5'	|
	'\u03F8'	|
	'\u03FB' .. '\u03FC'	|
	'\u0430' .. '\u045F'	|
	'\u0461'	|
	'\u0463'	|
	'\u0465'	|
	'\u0467'	|
	'\u0469'	|
	'\u046B'	|
	'\u046D'	|
	'\u046F'	|
	'\u0471'	|
	'\u0473'	|
	'\u0475'	|
	'\u0477'	|
	'\u0479'	|
	'\u047B'	|
	'\u047D'	|
	'\u047F'	|
	'\u0481'	|
	'\u048B'	|
	'\u048D'	|
	'\u048F'	|
	'\u0491'	|
	'\u0493'	|
	'\u0495'	|
	'\u0497'	|
	'\u0499'	|
	'\u049B'	|
	'\u049D'	|
	'\u049F'	|
	'\u04A1'	|
	'\u04A3'	|
	'\u04A5'	|
	'\u04A7'	|
	'\u04A9'	|
	'\u04AB'	|
	'\u04AD'	|
	'\u04AF'	|
	'\u04B1'	|
	'\u04B3'	|
	'\u04B5'	|
	'\u04B7'	|
	'\u04B9'	|
	'\u04BB'	|
	'\u04BD'	|
	'\u04BF'	|
	'\u04C2'	|
	'\u04C4'	|
	'\u04C6'	|
	'\u04C8'	|
	'\u04CA'	|
	'\u04CC'	|
	'\u04CE' .. '\u04CF'	|
	'\u04D1'	|
	'\u04D3'	|
	'\u04D5'	|
	'\u04D7'	|
	'\u04D9'	|
	'\u04DB'	|
	'\u04DD'	|
	'\u04DF'	|
	'\u04E1'	|
	'\u04E3'	|
	'\u04E5'	|
	'\u04E7'	|
	'\u04E9'	|
	'\u04EB'	|
	'\u04ED'	|
	'\u04EF'	|
	'\u04F1'	|
	'\u04F3'	|
	'\u04F5'	|
	'\u04F7'	|
	'\u04F9'	|
	'\u04FB'	|
	'\u04FD'	|
	'\u04FF'	|
	'\u0501'	|
	'\u0503'	|
	'\u0505'	|
	'\u0507'	|
	'\u0509'	|
	'\u050B'	|
	'\u050D'	|
	'\u050F'	|
	'\u0511'	|
	'\u0513'	|
	'\u0561' .. '\u0587'	|
	'\u1D00' .. '\u1D2B'	|
	'\u1D62' .. '\u1D77'	|
	'\u1D79' .. '\u1D9A'	|
	'\u1E01'	|
	'\u1E03'	|
	'\u1E05'	|
	'\u1E07'	|
	'\u1E09'	|
	'\u1E0B'	|
	'\u1E0D'	|
	'\u1E0F'	|
	'\u1E11'	|
	'\u1E13'	|
	'\u1E15'	|
	'\u1E17'	|
	'\u1E19'	|
	'\u1E1B'	|
	'\u1E1D'	|
	'\u1E1F'	|
	'\u1E21'	|
	'\u1E23'	|
	'\u1E25'	|
	'\u1E27'	|
	'\u1E29'	|
	'\u1E2B'	|
	'\u1E2D'	|
	'\u1E2F'	|
	'\u1E31'	|
	'\u1E33'	|
	'\u1E35'	|
	'\u1E37'	|
	'\u1E39'	|
	'\u1E3B'	|
	'\u1E3D'	|
	'\u1E3F'	|
	'\u1E41'	|
	'\u1E43'	|
	'\u1E45'	|
	'\u1E47'	|
	'\u1E49'	|
	'\u1E4B'	|
	'\u1E4D'	|
	'\u1E4F'	|
	'\u1E51'	|
	'\u1E53'	|
	'\u1E55'	|
	'\u1E57'	|
	'\u1E59'	|
	'\u1E5B'	|
	'\u1E5D'	|
	'\u1E5F'	|
	'\u1E61'	|
	'\u1E63'	|
	'\u1E65'	|
	'\u1E67'	|
	'\u1E69'	|
	'\u1E6B'	|
	'\u1E6D'	|
	'\u1E6F'	|
	'\u1E71'	|
	'\u1E73'	|
	'\u1E75'	|
	'\u1E77'	|
	'\u1E79'	|
	'\u1E7B'	|
	'\u1E7D'	|
	'\u1E7F'	|
	'\u1E81'	|
	'\u1E83'	|
	'\u1E85'	|
	'\u1E87'	|
	'\u1E89'	|
	'\u1E8B'	|
	'\u1E8D'	|
	'\u1E8F'	|
	'\u1E91'	|
	'\u1E93'	|
	'\u1E95' .. '\u1E9B'	|
	'\u1EA1'	|
	'\u1EA3'	|
	'\u1EA5'	|
	'\u1EA7'	|
	'\u1EA9'	|
	'\u1EAB'	|
	'\u1EAD'	|
	'\u1EAF'	|
	'\u1EB1'	|
	'\u1EB3'	|
	'\u1EB5'	|
	'\u1EB7'	|
	'\u1EB9'	|
	'\u1EBB'	|
	'\u1EBD'	|
	'\u1EBF'	|
	'\u1EC1'	|
	'\u1EC3'	|
	'\u1EC5'	|
	'\u1EC7'	|
	'\u1EC9'	|
	'\u1ECB'	|
	'\u1ECD'	|
	'\u1ECF'	|
	'\u1ED1'	|
	'\u1ED3'	|
	'\u1ED5'	|
	'\u1ED7'	|
	'\u1ED9'	|
	'\u1EDB'	|
	'\u1EDD'	|
	'\u1EDF'	|
	'\u1EE1'	|
	'\u1EE3'	|
	'\u1EE5'	|
	'\u1EE7'	|
	'\u1EE9'	|
	'\u1EEB'	|
	'\u1EED'	|
	'\u1EEF'	|
	'\u1EF1'	|
	'\u1EF3'	|
	'\u1EF5'	|
	'\u1EF7'	|
	'\u1EF9'	|
	'\u1F00' .. '\u1F07'	|
	'\u1F10' .. '\u1F15'	|
	'\u1F20' .. '\u1F27'	|
	'\u1F30' .. '\u1F37'	|
	'\u1F40' .. '\u1F45'	|
	'\u1F50' .. '\u1F57'	|
	'\u1F60' .. '\u1F67'	|
	'\u1F70' .. '\u1F7D'	|
	'\u1F80' .. '\u1F87'	|
	'\u1F90' .. '\u1F97'	|
	'\u1FA0' .. '\u1FA7'	|
	'\u1FB0' .. '\u1FB4'	|
	'\u1FB6' .. '\u1FB7'	|
	'\u1FBE'	|
	'\u1FC2' .. '\u1FC4'	|
	'\u1FC6' .. '\u1FC7'	|
	'\u1FD0' .. '\u1FD3'	|
	'\u1FD6' .. '\u1FD7'	|
	'\u1FE0' .. '\u1FE7'	|
	'\u1FF2' .. '\u1FF4'	|
	'\u1FF6' .. '\u1FF7'	|
	'\u2071'	|
	'\u207F'	|
	'\u210A'	|
	'\u210E' .. '\u210F'	|
	'\u2113'	|
	'\u212F'	|
	'\u2134'	|
	'\u2139'	|
	'\u213C' .. '\u213D'	|
	'\u2146' .. '\u2149'	|
	'\u214E'	|
	'\u2184'	|
	'\u2C30' .. '\u2C5E'	|
	'\u2C61'	|
	'\u2C65' .. '\u2C66'	|
	'\u2C68'	|
	'\u2C6A'	|
	'\u2C6C'	|
	'\u2C74'	|
	'\u2C76' .. '\u2C77'	|
	'\u2C81'	|
	'\u2C83'	|
	'\u2C85'	|
	'\u2C87'	|
	'\u2C89'	|
	'\u2C8B'	|
	'\u2C8D'	|
	'\u2C8F'	|
	'\u2C91'	|
	'\u2C93'	|
	'\u2C95'	|
	'\u2C97'	|
	'\u2C99'	|
	'\u2C9B'	|
	'\u2C9D'	|
	'\u2C9F'	|
	'\u2CA1'	|
	'\u2CA3'	|
	'\u2CA5'	|
	'\u2CA7'	|
	'\u2CA9'	|
	'\u2CAB'	|
	'\u2CAD'	|
	'\u2CAF'	|
	'\u2CB1'	|
	'\u2CB3'	|
	'\u2CB5'	|
	'\u2CB7'	|
	'\u2CB9'	|
	'\u2CBB'	|
	'\u2CBD'	|
	'\u2CBF'	|
	'\u2CC1'	|
	'\u2CC3'	|
	'\u2CC5'	|
	'\u2CC7'	|
	'\u2CC9'	|
	'\u2CCB'	|
	'\u2CCD'	|
	'\u2CCF'	|
	'\u2CD1'	|
	'\u2CD3'	|
	'\u2CD5'	|
	'\u2CD7'	|
	'\u2CD9'	|
	'\u2CDB'	|
	'\u2CDD'	|
	'\u2CDF'	|
	'\u2CE1'	|
	'\u2CE3' .. '\u2CE4'	|
	'\u2D00' .. '\u2D25'	|
	'\uFB00' .. '\uFB06'	|
	'\uFB13' .. '\uFB17'	|
	'\uFF41' .. '\uFF5A'	;

fragment
TitlecaseLetter :
	'\u01C5'	|
	'\u01C8'	|
	'\u01CB'	|
	'\u01F2'	|
	'\u1F88' .. '\u1F8F'	|
	'\u1F98' .. '\u1F9F'	|
	'\u1FA8' .. '\u1FAF'	|
	'\u1FBC'	|
	'\u1FCC'	|
	'\u1FFC'	;

fragment
ModifierLetter :
	'\u02B0' .. '\u02C1'	|
	'\u02C6' .. '\u02D1'	|
	'\u02E0' .. '\u02E4'	|
	'\u02EE'	|
	'\u037A'	|
	'\u0559'	|
	'\u0640'	|
	'\u06E5' .. '\u06E6'	|
	'\u07F4' .. '\u07F5'	|
	'\u07FA'	|
	'\u0E46'	|
	'\u0EC6'	|
	'\u10FC'	|
	'\u17D7'	|
	'\u1843'	|
	'\u1D2C' .. '\u1D61'	|
	'\u1D78'	|
	'\u1D9B' .. '\u1DBF'	|
	'\u2090' .. '\u2094'	|
	'\u2D6F'	|
	'\u3005'	|
	'\u3031' .. '\u3035'	|
	'\u303B'	|
	'\u309D' .. '\u309E'	|
	'\u30FC' .. '\u30FE'	|
	'\uA015'	|
	'\uA717' .. '\uA71A'	|
	'\uFF70'	|
	'\uFF9E' .. '\uFF9F'	;

fragment
OtherLetter :
	'\u01BB'	|
	'\u01C0' .. '\u01C3'	|
	'\u0294'	|
	'\u05D0' .. '\u05EA'	|
	'\u05F0' .. '\u05F2'	|
	'\u0621' .. '\u063A'	|
	'\u0641' .. '\u064A'	|
	'\u066E' .. '\u066F'	|
	'\u0671' .. '\u06D3'	|
	'\u06D5'	|
	'\u06EE' .. '\u06EF'	|
	'\u06FA' .. '\u06FC'	|
	'\u06FF'	|
	'\u0710'	|
	'\u0712' .. '\u072F'	|
	'\u074D' .. '\u076D'	|
	'\u0780' .. '\u07A5'	|
	'\u07B1'	|
	'\u07CA' .. '\u07EA'	|
	'\u0904' .. '\u0939'	|
	'\u093D'	|
	'\u0950'	|
	'\u0958' .. '\u0961'	|
	'\u097B' .. '\u097F'	|
	'\u0985' .. '\u098C'	|
	'\u098F' .. '\u0990'	|
	'\u0993' .. '\u09A8'	|
	'\u09AA' .. '\u09B0'	|
	'\u09B2'	|
	'\u09B6' .. '\u09B9'	|
	'\u09BD'	|
	'\u09CE'	|
	'\u09DC' .. '\u09DD'	|
	'\u09DF' .. '\u09E1'	|
	'\u09F0' .. '\u09F1'	|
	'\u0A05' .. '\u0A0A'	|
	'\u0A0F' .. '\u0A10'	|
	'\u0A13' .. '\u0A28'	|
	'\u0A2A' .. '\u0A30'	|
	'\u0A32' .. '\u0A33'	|
	'\u0A35' .. '\u0A36'	|
	'\u0A38' .. '\u0A39'	|
	'\u0A59' .. '\u0A5C'	|
	'\u0A5E'	|
	'\u0A72' .. '\u0A74'	|
	'\u0A85' .. '\u0A8D'	|
	'\u0A8F' .. '\u0A91'	|
	'\u0A93' .. '\u0AA8'	|
	'\u0AAA' .. '\u0AB0'	|
	'\u0AB2' .. '\u0AB3'	|
	'\u0AB5' .. '\u0AB9'	|
	'\u0ABD'	|
	'\u0AD0'	|
	'\u0AE0' .. '\u0AE1'	|
	'\u0B05' .. '\u0B0C'	|
	'\u0B0F' .. '\u0B10'	|
	'\u0B13' .. '\u0B28'	|
	'\u0B2A' .. '\u0B30'	|
	'\u0B32' .. '\u0B33'	|
	'\u0B35' .. '\u0B39'	|
	'\u0B3D'	|
	'\u0B5C' .. '\u0B5D'	|
	'\u0B5F' .. '\u0B61'	|
	'\u0B71'	|
	'\u0B83'	|
	'\u0B85' .. '\u0B8A'	|
	'\u0B8E' .. '\u0B90'	|
	'\u0B92' .. '\u0B95'	|
	'\u0B99' .. '\u0B9A'	|
	'\u0B9C'	|
	'\u0B9E' .. '\u0B9F'	|
	'\u0BA3' .. '\u0BA4'	|
	'\u0BA8' .. '\u0BAA'	|
	'\u0BAE' .. '\u0BB9'	|
	'\u0C05' .. '\u0C0C'	|
	'\u0C0E' .. '\u0C10'	|
	'\u0C12' .. '\u0C28'	|
	'\u0C2A' .. '\u0C33'	|
	'\u0C35' .. '\u0C39'	|
	'\u0C60' .. '\u0C61'	|
	'\u0C85' .. '\u0C8C'	|
	'\u0C8E' .. '\u0C90'	|
	'\u0C92' .. '\u0CA8'	|
	'\u0CAA' .. '\u0CB3'	|
	'\u0CB5' .. '\u0CB9'	|
	'\u0CBD'	|
	'\u0CDE'	|
	'\u0CE0' .. '\u0CE1'	|
	'\u0D05' .. '\u0D0C'	|
	'\u0D0E' .. '\u0D10'	|
	'\u0D12' .. '\u0D28'	|
	'\u0D2A' .. '\u0D39'	|
	'\u0D60' .. '\u0D61'	|
	'\u0D85' .. '\u0D96'	|
	'\u0D9A' .. '\u0DB1'	|
	'\u0DB3' .. '\u0DBB'	|
	'\u0DBD'	|
	'\u0DC0' .. '\u0DC6'	|
	'\u0E01' .. '\u0E30'	|
	'\u0E32' .. '\u0E33'	|
	'\u0E40' .. '\u0E45'	|
	'\u0E81' .. '\u0E82'	|
	'\u0E84'	|
	'\u0E87' .. '\u0E88'	|
	'\u0E8A'	|
	'\u0E8D'	|
	'\u0E94' .. '\u0E97'	|
	'\u0E99' .. '\u0E9F'	|
	'\u0EA1' .. '\u0EA3'	|
	'\u0EA5'	|
	'\u0EA7'	|
	'\u0EAA' .. '\u0EAB'	|
	'\u0EAD' .. '\u0EB0'	|
	'\u0EB2' .. '\u0EB3'	|
	'\u0EBD'	|
	'\u0EC0' .. '\u0EC4'	|
	'\u0EDC' .. '\u0EDD'	|
	'\u0F00'	|
	'\u0F40' .. '\u0F47'	|
	'\u0F49' .. '\u0F6A'	|
	'\u0F88' .. '\u0F8B'	|
	'\u1000' .. '\u1021'	|
	'\u1023' .. '\u1027'	|
	'\u1029' .. '\u102A'	|
	'\u1050' .. '\u1055'	|
	'\u10D0' .. '\u10FA'	|
	'\u1100' .. '\u1159'	|
	'\u115F' .. '\u11A2'	|
	'\u11A8' .. '\u11F9'	|
	'\u1200' .. '\u1248'	|
	'\u124A' .. '\u124D'	|
	'\u1250' .. '\u1256'	|
	'\u1258'	|
	'\u125A' .. '\u125D'	|
	'\u1260' .. '\u1288'	|
	'\u128A' .. '\u128D'	|
	'\u1290' .. '\u12B0'	|
	'\u12B2' .. '\u12B5'	|
	'\u12B8' .. '\u12BE'	|
	'\u12C0'	|
	'\u12C2' .. '\u12C5'	|
	'\u12C8' .. '\u12D6'	|
	'\u12D8' .. '\u1310'	|
	'\u1312' .. '\u1315'	|
	'\u1318' .. '\u135A'	|
	'\u1380' .. '\u138F'	|
	'\u13A0' .. '\u13F4'	|
	'\u1401' .. '\u166C'	|
	'\u166F' .. '\u1676'	|
	'\u1681' .. '\u169A'	|
	'\u16A0' .. '\u16EA'	|
	'\u1700' .. '\u170C'	|
	'\u170E' .. '\u1711'	|
	'\u1720' .. '\u1731'	|
	'\u1740' .. '\u1751'	|
	'\u1760' .. '\u176C'	|
	'\u176E' .. '\u1770'	|
	'\u1780' .. '\u17B3'	|
	'\u17DC'	|
	'\u1820' .. '\u1842'	|
	'\u1844' .. '\u1877'	|
	'\u1880' .. '\u18A8'	|
	'\u1900' .. '\u191C'	|
	'\u1950' .. '\u196D'	|
	'\u1970' .. '\u1974'	|
	'\u1980' .. '\u19A9'	|
	'\u19C1' .. '\u19C7'	|
	'\u1A00' .. '\u1A16'	|
	'\u1B05' .. '\u1B33'	|
	'\u1B45' .. '\u1B4B'	|
	'\u2135' .. '\u2138'	|
	'\u2D30' .. '\u2D65'	|
	'\u2D80' .. '\u2D96'	|
	'\u2DA0' .. '\u2DA6'	|
	'\u2DA8' .. '\u2DAE'	|
	'\u2DB0' .. '\u2DB6'	|
	'\u2DB8' .. '\u2DBE'	|
	'\u2DC0' .. '\u2DC6'	|
	'\u2DC8' .. '\u2DCE'	|
	'\u2DD0' .. '\u2DD6'	|
	'\u2DD8' .. '\u2DDE'	|
	'\u3006'	|
	'\u303C'	|
	'\u3041' .. '\u3096'	|
	'\u309F'	|
	'\u30A1' .. '\u30FA'	|
	'\u30FF'	|
	'\u3105' .. '\u312C'	|
	'\u3131' .. '\u318E'	|
	'\u31A0' .. '\u31B7'	|
	'\u31F0' .. '\u31FF'	|
	'\u3400' .. '\u4DB5'	|
	'\u4E00' .. '\u9FBB'	|
	'\uA000' .. '\uA014'	|
	'\uA016' .. '\uA48C'	|
	'\uA800' .. '\uA801'	|
	'\uA803' .. '\uA805'	|
	'\uA807' .. '\uA80A'	|
	'\uA80C' .. '\uA822'	|
	'\uA840' .. '\uA873'	|
	'\uAC00' .. '\uD7A3'	|
	'\uF900' .. '\uFA2D'	|
	'\uFA30' .. '\uFA6A'	|
	'\uFA70' .. '\uFAD9'	|
	'\uFB1D'	|
	'\uFB1F' .. '\uFB28'	|
	'\uFB2A' .. '\uFB36'	|
	'\uFB38' .. '\uFB3C'	|
	'\uFB3E'	|
	'\uFB40' .. '\uFB41'	|
	'\uFB43' .. '\uFB44'	|
	'\uFB46' .. '\uFBB1'	|
	'\uFBD3' .. '\uFD3D'	|
	'\uFD50' .. '\uFD8F'	|
	'\uFD92' .. '\uFDC7'	|
	'\uFDF0' .. '\uFDFB'	|
	'\uFE70' .. '\uFE74'	|
	'\uFE76' .. '\uFEFC'	|
	'\uFF66' .. '\uFF6F'	|
	'\uFF71' .. '\uFF9D'	|
	'\uFFA0' .. '\uFFBE'	|
	'\uFFC2' .. '\uFFC7'	|
	'\uFFCA' .. '\uFFCF'	|
	'\uFFD2' .. '\uFFD7'	|
	'\uFFDA' .. '\uFFDC'	;

fragment
NonSpacingMark :
	'\u0300' .. '\u036F'	|
	'\u0483' .. '\u0486'	|
	'\u0591' .. '\u05BD'	|
	'\u05BF'	|
	'\u05C1' .. '\u05C2'	|
	'\u05C4' .. '\u05C5'	|
	'\u05C7'	|
	'\u0610' .. '\u0615'	|
	'\u064B' .. '\u065E'	|
	'\u0670'	|
	'\u06D6' .. '\u06DC'	|
	'\u06DF' .. '\u06E4'	|
	'\u06E7' .. '\u06E8'	|
	'\u06EA' .. '\u06ED'	|
	'\u0711'	|
	'\u0730' .. '\u074A'	|
	'\u07A6' .. '\u07B0'	|
	'\u07EB' .. '\u07F3'	|
	'\u0901' .. '\u0902'	|
	'\u093C'	|
	'\u0941' .. '\u0948'	|
	'\u094D'	|
	'\u0951' .. '\u0954'	|
	'\u0962' .. '\u0963'	|
	'\u0981'	|
	'\u09BC'	|
	'\u09C1' .. '\u09C4'	|
	'\u09CD'	|
	'\u09E2' .. '\u09E3'	|
	'\u0A01' .. '\u0A02'	|
	'\u0A3C'	|
	'\u0A41' .. '\u0A42'	|
	'\u0A47' .. '\u0A48'	|
	'\u0A4B' .. '\u0A4D'	|
	'\u0A70' .. '\u0A71'	|
	'\u0A81' .. '\u0A82'	|
	'\u0ABC'	|
	'\u0AC1' .. '\u0AC5'	|
	'\u0AC7' .. '\u0AC8'	|
	'\u0ACD'	|
	'\u0AE2' .. '\u0AE3'	|
	'\u0B01'	|
	'\u0B3C'	|
	'\u0B3F'	|
	'\u0B41' .. '\u0B43'	|
	'\u0B4D'	|
	'\u0B56'	|
	'\u0B82'	|
	'\u0BC0'	|
	'\u0BCD'	|
	'\u0C3E' .. '\u0C40'	|
	'\u0C46' .. '\u0C48'	|
	'\u0C4A' .. '\u0C4D'	|
	'\u0C55' .. '\u0C56'	|
	'\u0CBC'	|
	'\u0CBF'	|
	'\u0CC6'	|
	'\u0CCC' .. '\u0CCD'	|
	'\u0CE2' .. '\u0CE3'	|
	'\u0D41' .. '\u0D43'	|
	'\u0D4D'	|
	'\u0DCA'	|
	'\u0DD2' .. '\u0DD4'	|
	'\u0DD6'	|
	'\u0E31'	|
	'\u0E34' .. '\u0E3A'	|
	'\u0E47' .. '\u0E4E'	|
	'\u0EB1'	|
	'\u0EB4' .. '\u0EB9'	|
	'\u0EBB' .. '\u0EBC'	|
	'\u0EC8' .. '\u0ECD'	|
	'\u0F18' .. '\u0F19'	|
	'\u0F35'	|
	'\u0F37'	|
	'\u0F39'	|
	'\u0F71' .. '\u0F7E'	|
	'\u0F80' .. '\u0F84'	|
	'\u0F86' .. '\u0F87'	|
	'\u0F90' .. '\u0F97'	|
	'\u0F99' .. '\u0FBC'	|
	'\u0FC6'	|
	'\u102D' .. '\u1030'	|
	'\u1032'	|
	'\u1036' .. '\u1037'	|
	'\u1039'	|
	'\u1058' .. '\u1059'	|
	'\u135F'	|
	'\u1712' .. '\u1714'	|
	'\u1732' .. '\u1734'	|
	'\u1752' .. '\u1753'	|
	'\u1772' .. '\u1773'	|
	'\u17B7' .. '\u17BD'	|
	'\u17C6'	|
	'\u17C9' .. '\u17D3'	|
	'\u17DD'	|
	'\u180B' .. '\u180D'	|
	'\u18A9'	|
	'\u1920' .. '\u1922'	|
	'\u1927' .. '\u1928'	|
	'\u1932'	|
	'\u1939' .. '\u193B'	|
	'\u1A17' .. '\u1A18'	|
	'\u1B00' .. '\u1B03'	|
	'\u1B34'	|
	'\u1B36' .. '\u1B3A'	|
	'\u1B3C'	|
	'\u1B42'	|
	'\u1B6B' .. '\u1B73'	|
	'\u1DC0' .. '\u1DCA'	|
	'\u1DFE' .. '\u1DFF'	|
	'\u20D0' .. '\u20DC'	|
	'\u20E1'	|
	'\u20E5' .. '\u20EF'	|
	'\u302A' .. '\u302F'	|
	'\u3099' .. '\u309A'	|
	'\uA806'	|
	'\uA80B'	|
	'\uA825' .. '\uA826'	|
	'\uFB1E'	|
	'\uFE00' .. '\uFE0F'	|
	'\uFE20' .. '\uFE23'	;

fragment
SpacingCombiningMark :
	'\u0903'	|
	'\u093E' .. '\u0940'	|
	'\u0949' .. '\u094C'	|
	'\u0982' .. '\u0983'	|
	'\u09BE' .. '\u09C0'	|
	'\u09C7' .. '\u09C8'	|
	'\u09CB' .. '\u09CC'	|
	'\u09D7'	|
	'\u0A03'	|
	'\u0A3E' .. '\u0A40'	|
	'\u0A83'	|
	'\u0ABE' .. '\u0AC0'	|
	'\u0AC9'	|
	'\u0ACB' .. '\u0ACC'	|
	'\u0B02' .. '\u0B03'	|
	'\u0B3E'	|
	'\u0B40'	|
	'\u0B47' .. '\u0B48'	|
	'\u0B4B' .. '\u0B4C'	|
	'\u0B57'	|
	'\u0BBE' .. '\u0BBF'	|
	'\u0BC1' .. '\u0BC2'	|
	'\u0BC6' .. '\u0BC8'	|
	'\u0BCA' .. '\u0BCC'	|
	'\u0BD7'	|
	'\u0C01' .. '\u0C03'	|
	'\u0C41' .. '\u0C44'	|
	'\u0C82' .. '\u0C83'	|
	'\u0CBE'	|
	'\u0CC0' .. '\u0CC4'	|
	'\u0CC7' .. '\u0CC8'	|
	'\u0CCA' .. '\u0CCB'	|
	'\u0CD5' .. '\u0CD6'	|
	'\u0D02' .. '\u0D03'	|
	'\u0D3E' .. '\u0D40'	|
	'\u0D46' .. '\u0D48'	|
	'\u0D4A' .. '\u0D4C'	|
	'\u0D57'	|
	'\u0D82' .. '\u0D83'	|
	'\u0DCF' .. '\u0DD1'	|
	'\u0DD8' .. '\u0DDF'	|
	'\u0DF2' .. '\u0DF3'	|
	'\u0F3E' .. '\u0F3F'	|
	'\u0F7F'	|
	'\u102C'	|
	'\u1031'	|
	'\u1038'	|
	'\u1056' .. '\u1057'	|
	'\u17B6'	|
	'\u17BE' .. '\u17C5'	|
	'\u17C7' .. '\u17C8'	|
	'\u1923' .. '\u1926'	|
	'\u1929' .. '\u192B'	|
	'\u1930' .. '\u1931'	|
	'\u1933' .. '\u1938'	|
	'\u19B0' .. '\u19C0'	|
	'\u19C8' .. '\u19C9'	|
	'\u1A19' .. '\u1A1B'	|
	'\u1B04'	|
	'\u1B35'	|
	'\u1B3B'	|
	'\u1B3D' .. '\u1B41'	|
	'\u1B43' .. '\u1B44'	|
	'\uA802'	|
	'\uA823' .. '\uA824'	|
	'\uA827'	;

fragment
EnclosingMark :
	'\u0488' .. '\u0489'	|
	'\u06DE'	|
	'\u20DD' .. '\u20E0'	|
	'\u20E2' .. '\u20E4'	;

fragment
DecimalDigitNumber :
	'\u0030' .. '\u0039'	|
	'\u0660' .. '\u0669'	|
	'\u06F0' .. '\u06F9'	|
	'\u07C0' .. '\u07C9'	|
	'\u0966' .. '\u096F'	|
	'\u09E6' .. '\u09EF'	|
	'\u0A66' .. '\u0A6F'	|
	'\u0AE6' .. '\u0AEF'	|
	'\u0B66' .. '\u0B6F'	|
	'\u0BE6' .. '\u0BEF'	|
	'\u0C66' .. '\u0C6F'	|
	'\u0CE6' .. '\u0CEF'	|
	'\u0D66' .. '\u0D6F'	|
	'\u0E50' .. '\u0E59'	|
	'\u0ED0' .. '\u0ED9'	|
	'\u0F20' .. '\u0F29'	|
	'\u1040' .. '\u1049'	|
	'\u17E0' .. '\u17E9'	|
	'\u1810' .. '\u1819'	|
	'\u1946' .. '\u194F'	|
	'\u19D0' .. '\u19D9'	|
	'\u1B50' .. '\u1B59'	|
	'\uFF10' .. '\uFF19'	;

fragment
LetterNumber :
	'\u16EE' .. '\u16F0'	|
	'\u2160' .. '\u2182'	|
	'\u3007'	|
	'\u3021' .. '\u3029'	|
	'\u3038' .. '\u303A'	;

fragment
OtherNumber :
	'\u00B2' .. '\u00B3'	|
	'\u00B9'	|
	'\u00BC' .. '\u00BE'	|
	'\u09F4' .. '\u09F9'	|
	'\u0BF0' .. '\u0BF2'	|
	'\u0F2A' .. '\u0F33'	|
	'\u1369' .. '\u137C'	|
	'\u17F0' .. '\u17F9'	|
	'\u2070'	|
	'\u2074' .. '\u2079'	|
	'\u2080' .. '\u2089'	|
	'\u2153' .. '\u215F'	|
	'\u2460' .. '\u249B'	|
	'\u24EA' .. '\u24FF'	|
	'\u2776' .. '\u2793'	|
	'\u2CFD'	|
	'\u3192' .. '\u3195'	|
	'\u3220' .. '\u3229'	|
	'\u3251' .. '\u325F'	|
	'\u3280' .. '\u3289'	|
	'\u32B1' .. '\u32BF'	;

fragment
SpaceSeparator :
	'\u0020'	|
	'\u00A0'	|
	'\u1680'	|
	'\u180E'	|
	'\u2000' .. '\u200A'	|
	'\u202F'	|
	'\u205F'	|
	'\u3000'	;

fragment
LineSeparator :
	'\u2028'	;

fragment
ParagraphSeparator :
	'\u2029'	;

fragment
Control :
	'\u0000' .. '\u001F'	|
	'\u007F' .. '\u009F'	;

fragment
Format :
	'\u0600' .. '\u0603'	|
	'\u06DD'	|
	'\u070F'	|
	'\u17B4' .. '\u17B5'	|
	'\u200B' .. '\u200F'	|
	'\u202A' .. '\u202E'	|
	'\u2060' .. '\u2063'	|
	'\u206A' .. '\u206F'	|
	'\uFEFF'	|
	'\uFFF9' .. '\uFFFB'	;

fragment
Surrogate :
	'\uD800' .. '\uDFFF'	;

fragment
PrivateUse :
	'\uE000' .. '\uF8FF'	;

fragment
ConnectorPunctuation :
	'\u005F'	|
	'\u203F' .. '\u2040'	|
	'\u2054'	|
	'\uFE33' .. '\uFE34'	|
	'\uFE4D' .. '\uFE4F'	|
	'\uFF3F'	;

fragment
DashPunctuation :
	'\u002D'	|
	'\u00AD'	|
	'\u058A'	|
	'\u1806'	|
	'\u2010' .. '\u2015'	|
	'\u2E17'	|
	'\u301C'	|
	'\u3030'	|
	'\u30A0'	|
	'\uFE31' .. '\uFE32'	|
	'\uFE58'	|
	'\uFE63'	|
	'\uFF0D'	;

fragment
OpenPunctuation :
	'\u0028'	|
	'\u005B'	|
	'\u007B'	|
	'\u0F3A'	|
	'\u0F3C'	|
	'\u169B'	|
	'\u201A'	|
	'\u201E'	|
	'\u2045'	|
	'\u207D'	|
	'\u208D'	|
	'\u2329'	|
	'\u2768'	|
	'\u276A'	|
	'\u276C'	|
	'\u276E'	|
	'\u2770'	|
	'\u2772'	|
	'\u2774'	|
	'\u27C5'	|
	'\u27E6'	|
	'\u27E8'	|
	'\u27EA'	|
	'\u2983'	|
	'\u2985'	|
	'\u2987'	|
	'\u2989'	|
	'\u298B'	|
	'\u298D'	|
	'\u298F'	|
	'\u2991'	|
	'\u2993'	|
	'\u2995'	|
	'\u2997'	|
	'\u29D8'	|
	'\u29DA'	|
	'\u29FC'	|
	'\u3008'	|
	'\u300A'	|
	'\u300C'	|
	'\u300E'	|
	'\u3010'	|
	'\u3014'	|
	'\u3016'	|
	'\u3018'	|
	'\u301A'	|
	'\u301D'	|
	'\uFD3E'	|
	'\uFE17'	|
	'\uFE35'	|
	'\uFE37'	|
	'\uFE39'	|
	'\uFE3B'	|
	'\uFE3D'	|
	'\uFE3F'	|
	'\uFE41'	|
	'\uFE43'	|
	'\uFE47'	|
	'\uFE59'	|
	'\uFE5B'	|
	'\uFE5D'	|
	'\uFF08'	|
	'\uFF3B'	|
	'\uFF5B'	|
	'\uFF5F'	|
	'\uFF62'	;

fragment
ClosePunctuation :
	'\u0029'	|
	'\u005D'	|
	'\u007D'	|
	'\u0F3B'	|
	'\u0F3D'	|
	'\u169C'	|
	'\u2046'	|
	'\u207E'	|
	'\u208E'	|
	'\u232A'	|
	'\u2769'	|
	'\u276B'	|
	'\u276D'	|
	'\u276F'	|
	'\u2771'	|
	'\u2773'	|
	'\u2775'	|
	'\u27C6'	|
	'\u27E7'	|
	'\u27E9'	|
	'\u27EB'	|
	'\u2984'	|
	'\u2986'	|
	'\u2988'	|
	'\u298A'	|
	'\u298C'	|
	'\u298E'	|
	'\u2990'	|
	'\u2992'	|
	'\u2994'	|
	'\u2996'	|
	'\u2998'	|
	'\u29D9'	|
	'\u29DB'	|
	'\u29FD'	|
	'\u3009'	|
	'\u300B'	|
	'\u300D'	|
	'\u300F'	|
	'\u3011'	|
	'\u3015'	|
	'\u3017'	|
	'\u3019'	|
	'\u301B'	|
	'\u301E' .. '\u301F'	|
	'\uFD3F'	|
	'\uFE18'	|
	'\uFE36'	|
	'\uFE38'	|
	'\uFE3A'	|
	'\uFE3C'	|
	'\uFE3E'	|
	'\uFE40'	|
	'\uFE42'	|
	'\uFE44'	|
	'\uFE48'	|
	'\uFE5A'	|
	'\uFE5C'	|
	'\uFE5E'	|
	'\uFF09'	|
	'\uFF3D'	|
	'\uFF5D'	|
	'\uFF60'	|
	'\uFF63'	;

fragment
InitialQuotePunctuation :
	'\u00AB'	|
	'\u2018'	|
	'\u201B' .. '\u201C'	|
	'\u201F'	|
	'\u2039'	|
	'\u2E02'	|
	'\u2E04'	|
	'\u2E09'	|
	'\u2E0C'	|
	'\u2E1C'	;

fragment
FinalQuotePunctuation :
	'\u00BB'	|
	'\u2019'	|
	'\u201D'	|
	'\u203A'	|
	'\u2E03'	|
	'\u2E05'	|
	'\u2E0A'	|
	'\u2E0D'	|
	'\u2E1D'	;

fragment
OtherPunctuation :
	'\u0021' .. '\u0023'	|
	'\u0025' .. '\u0027'	|
	'\u002A'	|
	'\u002C'	|
	'\u002E' .. '\u002F'	|
	'\u003A' .. '\u003B'	|
	'\u003F' .. '\u0040'	|
	'\u005C'	|
	'\u00A1'	|
	'\u00B7'	|
	'\u00BF'	|
	'\u037E'	|
	'\u0387'	|
	'\u055A' .. '\u055F'	|
	'\u0589'	|
	'\u05BE'	|
	'\u05C0'	|
	'\u05C3'	|
	'\u05C6'	|
	'\u05F3' .. '\u05F4'	|
	'\u060C' .. '\u060D'	|
	'\u061B'	|
	'\u061E' .. '\u061F'	|
	'\u066A' .. '\u066D'	|
	'\u06D4'	|
	'\u0700' .. '\u070D'	|
	'\u07F7' .. '\u07F9'	|
	'\u0964' .. '\u0965'	|
	'\u0970'	|
	'\u0DF4'	|
	'\u0E4F'	|
	'\u0E5A' .. '\u0E5B'	|
	'\u0F04' .. '\u0F12'	|
	'\u0F85'	|
	'\u0FD0' .. '\u0FD1'	|
	'\u104A' .. '\u104F'	|
	'\u10FB'	|
	'\u1361' .. '\u1368'	|
	'\u166D' .. '\u166E'	|
	'\u16EB' .. '\u16ED'	|
	'\u1735' .. '\u1736'	|
	'\u17D4' .. '\u17D6'	|
	'\u17D8' .. '\u17DA'	|
	'\u1800' .. '\u1805'	|
	'\u1807' .. '\u180A'	|
	'\u1944' .. '\u1945'	|
	'\u19DE' .. '\u19DF'	|
	'\u1A1E' .. '\u1A1F'	|
	'\u1B5A' .. '\u1B60'	|
	'\u2016' .. '\u2017'	|
	'\u2020' .. '\u2027'	|
	'\u2030' .. '\u2038'	|
	'\u203B' .. '\u203E'	|
	'\u2041' .. '\u2043'	|
	'\u2047' .. '\u2051'	|
	'\u2053'	|
	'\u2055' .. '\u205E'	|
	'\u2CF9' .. '\u2CFC'	|
	'\u2CFE' .. '\u2CFF'	|
	'\u2E00' .. '\u2E01'	|
	'\u2E06' .. '\u2E08'	|
	'\u2E0B'	|
	'\u2E0E' .. '\u2E16'	|
	'\u3001' .. '\u3003'	|
	'\u303D'	|
	'\u30FB'	|
	'\uA874' .. '\uA877'	|
	'\uFE10' .. '\uFE16'	|
	'\uFE19'	|
	'\uFE30'	|
	'\uFE45' .. '\uFE46'	|
	'\uFE49' .. '\uFE4C'	|
	'\uFE50' .. '\uFE52'	|
	'\uFE54' .. '\uFE57'	|
	'\uFE5F' .. '\uFE61'	|
	'\uFE68'	|
	'\uFE6A' .. '\uFE6B'	|
	'\uFF01' .. '\uFF03'	|
	'\uFF05' .. '\uFF07'	|
	'\uFF0A'	|
	'\uFF0C'	|
	'\uFF0E' .. '\uFF0F'	|
	'\uFF1A' .. '\uFF1B'	|
	'\uFF1F' .. '\uFF20'	|
	'\uFF3C'	|
	'\uFF61'	|
	'\uFF64' .. '\uFF65'	;

fragment
MathSymbol :
	'\u002B'	|
	'\u003C' .. '\u003E'	|
	'\u007C'	|
	'\u007E'	|
	'\u00AC'	|
	'\u00B1'	|
	'\u00D7'	|
	'\u00F7'	|
	'\u03F6'	|
	'\u2044'	|
	'\u2052'	|
	'\u207A' .. '\u207C'	|
	'\u208A' .. '\u208C'	|
	'\u2140' .. '\u2144'	|
	'\u214B'	|
	'\u2190' .. '\u2194'	|
	'\u219A' .. '\u219B'	|
	'\u21A0'	|
	'\u21A3'	|
	'\u21A6'	|
	'\u21AE'	|
	'\u21CE' .. '\u21CF'	|
	'\u21D2'	|
	'\u21D4'	|
	'\u21F4' .. '\u22FF'	|
	'\u2308' .. '\u230B'	|
	'\u2320' .. '\u2321'	|
	'\u237C'	|
	'\u239B' .. '\u23B3'	|
	'\u23DC' .. '\u23E1'	|
	'\u25B7'	|
	'\u25C1'	|
	'\u25F8' .. '\u25FF'	|
	'\u266F'	|
	'\u27C0' .. '\u27C4'	|
	'\u27C7' .. '\u27CA'	|
	'\u27D0' .. '\u27E5'	|
	'\u27F0' .. '\u27FF'	|
	'\u2900' .. '\u2982'	|
	'\u2999' .. '\u29D7'	|
	'\u29DC' .. '\u29FB'	|
	'\u29FE' .. '\u2AFF'	|
	'\uFB29'	|
	'\uFE62'	|
	'\uFE64' .. '\uFE66'	|
	'\uFF0B'	|
	'\uFF1C' .. '\uFF1E'	|
	'\uFF5C'	|
	'\uFF5E'	|
	'\uFFE2'	|
	'\uFFE9' .. '\uFFEC'	;

fragment
CurrencySymbol :
	'\u0024'	|
	'\u00A2' .. '\u00A5'	|
	'\u060B'	|
	'\u09F2' .. '\u09F3'	|
	'\u0AF1'	|
	'\u0BF9'	|
	'\u0E3F'	|
	'\u17DB'	|
	'\u20A0' .. '\u20B5'	|
	'\uFDFC'	|
	'\uFE69'	|
	'\uFF04'	|
	'\uFFE0' .. '\uFFE1'	|
	'\uFFE5' .. '\uFFE6'	;

fragment
ModifierSymbol :
	'\u005E'	|
	'\u0060'	|
	'\u00A8'	|
	'\u00AF'	|
	'\u00B4'	|
	'\u00B8'	|
	'\u02C2' .. '\u02C5'	|
	'\u02D2' .. '\u02DF'	|
	'\u02E5' .. '\u02ED'	|
	'\u02EF' .. '\u02FF'	|
	'\u0374' .. '\u0375'	|
	'\u0384' .. '\u0385'	|
	'\u1FBD'	|
	'\u1FBF' .. '\u1FC1'	|
	'\u1FCD' .. '\u1FCF'	|
	'\u1FDD' .. '\u1FDF'	|
	'\u1FED' .. '\u1FEF'	|
	'\u1FFD' .. '\u1FFE'	|
	'\u309B' .. '\u309C'	|
	'\uA700' .. '\uA716'	|
	'\uA720' .. '\uA721'	|
	'\uFF3E'	|
	'\uFF40'	|
	'\uFFE3'	;

fragment
OtherSymbol :
	'\u00A6' .. '\u00A7'	|
	'\u00A9'	|
	'\u00AE'	|
	'\u00B0'	|
	'\u00B6'	|
	'\u0482'	|
	'\u060E' .. '\u060F'	|
	'\u06E9'	|
	'\u06FD' .. '\u06FE'	|
	'\u07F6'	|
	'\u09FA'	|
	'\u0B70'	|
	'\u0BF3' .. '\u0BF8'	|
	'\u0BFA'	|
	'\u0CF1' .. '\u0CF2'	|
	'\u0F01' .. '\u0F03'	|
	'\u0F13' .. '\u0F17'	|
	'\u0F1A' .. '\u0F1F'	|
	'\u0F34'	|
	'\u0F36'	|
	'\u0F38'	|
	'\u0FBE' .. '\u0FC5'	|
	'\u0FC7' .. '\u0FCC'	|
	'\u0FCF'	|
	'\u1360'	|
	'\u1390' .. '\u1399'	|
	'\u1940'	|
	'\u19E0' .. '\u19FF'	|
	'\u1B61' .. '\u1B6A'	|
	'\u1B74' .. '\u1B7C'	|
	'\u2100' .. '\u2101'	|
	'\u2103' .. '\u2106'	|
	'\u2108' .. '\u2109'	|
	'\u2114'	|
	'\u2116' .. '\u2118'	|
	'\u211E' .. '\u2123'	|
	'\u2125'	|
	'\u2127'	|
	'\u2129'	|
	'\u212E'	|
	'\u213A' .. '\u213B'	|
	'\u214A'	|
	'\u214C' .. '\u214D'	|
	'\u2195' .. '\u2199'	|
	'\u219C' .. '\u219F'	|
	'\u21A1' .. '\u21A2'	|
	'\u21A4' .. '\u21A5'	|
	'\u21A7' .. '\u21AD'	|
	'\u21AF' .. '\u21CD'	|
	'\u21D0' .. '\u21D1'	|
	'\u21D3'	|
	'\u21D5' .. '\u21F3'	|
	'\u2300' .. '\u2307'	|
	'\u230C' .. '\u231F'	|
	'\u2322' .. '\u2328'	|
	'\u232B' .. '\u237B'	|
	'\u237D' .. '\u239A'	|
	'\u23B4' .. '\u23DB'	|
	'\u23E2' .. '\u23E7'	|
	'\u2400' .. '\u2426'	|
	'\u2440' .. '\u244A'	|
	'\u249C' .. '\u24E9'	|
	'\u2500' .. '\u25B6'	|
	'\u25B8' .. '\u25C0'	|
	'\u25C2' .. '\u25F7'	|
	'\u2600' .. '\u266E'	|
	'\u2670' .. '\u269C'	|
	'\u26A0' .. '\u26B2'	|
	'\u2701' .. '\u2704'	|
	'\u2706' .. '\u2709'	|
	'\u270C' .. '\u2727'	|
	'\u2729' .. '\u274B'	|
	'\u274D'	|
	'\u274F' .. '\u2752'	|
	'\u2756'	|
	'\u2758' .. '\u275E'	|
	'\u2761' .. '\u2767'	|
	'\u2794'	|
	'\u2798' .. '\u27AF'	|
	'\u27B1' .. '\u27BE'	|
	'\u2800' .. '\u28FF'	|
	'\u2B00' .. '\u2B1A'	|
	'\u2B20' .. '\u2B23'	|
	'\u2CE5' .. '\u2CEA'	|
	'\u2E80' .. '\u2E99'	|
	'\u2E9B' .. '\u2EF3'	|
	'\u2F00' .. '\u2FD5'	|
	'\u2FF0' .. '\u2FFB'	|
	'\u3004'	|
	'\u3012' .. '\u3013'	|
	'\u3020'	|
	'\u3036' .. '\u3037'	|
	'\u303E' .. '\u303F'	|
	'\u3190' .. '\u3191'	|
	'\u3196' .. '\u319F'	|
	'\u31C0' .. '\u31CF'	|
	'\u3200' .. '\u321E'	|
	'\u322A' .. '\u3243'	|
	'\u3250'	|
	'\u3260' .. '\u327F'	|
	'\u328A' .. '\u32B0'	|
	'\u32C0' .. '\u32FE'	|
	'\u3300' .. '\u33FF'	|
	'\u4DC0' .. '\u4DFF'	|
	'\uA490' .. '\uA4C6'	|
	'\uA828' .. '\uA82B'	|
	'\uFDFD'	|
	'\uFFE4'	|
	'\uFFE8'	|
	'\uFFED' .. '\uFFEE'	|
	'\uFFFC' .. '\uFFFD'	;

fragment
OtherNotAssigned :
	'\u0370' .. '\u0373'	|
	'\u0376' .. '\u0379'	|
	'\u037F' .. '\u0383'	|
	'\u038B'	|
	'\u038D'	|
	'\u03A2'	|
	'\u03CF'	|
	'\u0487'	|
	'\u0514' .. '\u0530'	|
	'\u0557' .. '\u0558'	|
	'\u0560'	|
	'\u0588'	|
	'\u058B' .. '\u0590'	|
	'\u05C8' .. '\u05CF'	|
	'\u05EB' .. '\u05EF'	|
	'\u05F5' .. '\u05FF'	|
	'\u0604' .. '\u060A'	|
	'\u0616' .. '\u061A'	|
	'\u061C' .. '\u061D'	|
	'\u0620'	|
	'\u063B' .. '\u063F'	|
	'\u065F'	|
	'\u070E'	|
	'\u074B' .. '\u074C'	|
	'\u076E' .. '\u077F'	|
	'\u07B2' .. '\u07BF'	|
	'\u07FB' .. '\u0900'	|
	'\u093A' .. '\u093B'	|
	'\u094E' .. '\u094F'	|
	'\u0955' .. '\u0957'	|
	'\u0971' .. '\u097A'	|
	'\u0980'	|
	'\u0984'	|
	'\u098D' .. '\u098E'	|
	'\u0991' .. '\u0992'	|
	'\u09A9'	|
	'\u09B1'	|
	'\u09B3' .. '\u09B5'	|
	'\u09BA' .. '\u09BB'	|
	'\u09C5' .. '\u09C6'	|
	'\u09C9' .. '\u09CA'	|
	'\u09CF' .. '\u09D6'	|
	'\u09D8' .. '\u09DB'	|
	'\u09DE'	|
	'\u09E4' .. '\u09E5'	|
	'\u09FB' .. '\u0A00'	|
	'\u0A04'	|
	'\u0A0B' .. '\u0A0E'	|
	'\u0A11' .. '\u0A12'	|
	'\u0A29'	|
	'\u0A31'	|
	'\u0A34'	|
	'\u0A37'	|
	'\u0A3A' .. '\u0A3B'	|
	'\u0A3D'	|
	'\u0A43' .. '\u0A46'	|
	'\u0A49' .. '\u0A4A'	|
	'\u0A4E' .. '\u0A58'	|
	'\u0A5D'	|
	'\u0A5F' .. '\u0A65'	|
	'\u0A75' .. '\u0A80'	|
	'\u0A84'	|
	'\u0A8E'	|
	'\u0A92'	|
	'\u0AA9'	|
	'\u0AB1'	|
	'\u0AB4'	|
	'\u0ABA' .. '\u0ABB'	|
	'\u0AC6'	|
	'\u0ACA'	|
	'\u0ACE' .. '\u0ACF'	|
	'\u0AD1' .. '\u0ADF'	|
	'\u0AE4' .. '\u0AE5'	|
	'\u0AF0'	|
	'\u0AF2' .. '\u0B00'	|
	'\u0B04'	|
	'\u0B0D' .. '\u0B0E'	|
	'\u0B11' .. '\u0B12'	|
	'\u0B29'	|
	'\u0B31'	|
	'\u0B34'	|
	'\u0B3A' .. '\u0B3B'	|
	'\u0B44' .. '\u0B46'	|
	'\u0B49' .. '\u0B4A'	|
	'\u0B4E' .. '\u0B55'	|
	'\u0B58' .. '\u0B5B'	|
	'\u0B5E'	|
	'\u0B62' .. '\u0B65'	|
	'\u0B72' .. '\u0B81'	|
	'\u0B84'	|
	'\u0B8B' .. '\u0B8D'	|
	'\u0B91'	|
	'\u0B96' .. '\u0B98'	|
	'\u0B9B'	|
	'\u0B9D'	|
	'\u0BA0' .. '\u0BA2'	|
	'\u0BA5' .. '\u0BA7'	|
	'\u0BAB' .. '\u0BAD'	|
	'\u0BBA' .. '\u0BBD'	|
	'\u0BC3' .. '\u0BC5'	|
	'\u0BC9'	|
	'\u0BCE' .. '\u0BD6'	|
	'\u0BD8' .. '\u0BE5'	|
	'\u0BFB' .. '\u0C00'	|
	'\u0C04'	|
	'\u0C0D'	|
	'\u0C11'	|
	'\u0C29'	|
	'\u0C34'	|
	'\u0C3A' .. '\u0C3D'	|
	'\u0C45'	|
	'\u0C49'	|
	'\u0C4E' .. '\u0C54'	|
	'\u0C57' .. '\u0C5F'	|
	'\u0C62' .. '\u0C65'	|
	'\u0C70' .. '\u0C81'	|
	'\u0C84'	|
	'\u0C8D'	|
	'\u0C91'	|
	'\u0CA9'	|
	'\u0CB4'	|
	'\u0CBA' .. '\u0CBB'	|
	'\u0CC5'	|
	'\u0CC9'	|
	'\u0CCE' .. '\u0CD4'	|
	'\u0CD7' .. '\u0CDD'	|
	'\u0CDF'	|
	'\u0CE4' .. '\u0CE5'	|
	'\u0CF0'	|
	'\u0CF3' .. '\u0D01'	|
	'\u0D04'	|
	'\u0D0D'	|
	'\u0D11'	|
	'\u0D29'	|
	'\u0D3A' .. '\u0D3D'	|
	'\u0D44' .. '\u0D45'	|
	'\u0D49'	|
	'\u0D4E' .. '\u0D56'	|
	'\u0D58' .. '\u0D5F'	|
	'\u0D62' .. '\u0D65'	|
	'\u0D70' .. '\u0D81'	|
	'\u0D84'	|
	'\u0D97' .. '\u0D99'	|
	'\u0DB2'	|
	'\u0DBC'	|
	'\u0DBE' .. '\u0DBF'	|
	'\u0DC7' .. '\u0DC9'	|
	'\u0DCB' .. '\u0DCE'	|
	'\u0DD5'	|
	'\u0DD7'	|
	'\u0DE0' .. '\u0DF1'	|
	'\u0DF5' .. '\u0E00'	|
	'\u0E3B' .. '\u0E3E'	|
	'\u0E5C' .. '\u0E80'	|
	'\u0E83'	|
	'\u0E85' .. '\u0E86'	|
	'\u0E89'	|
	'\u0E8B' .. '\u0E8C'	|
	'\u0E8E' .. '\u0E93'	|
	'\u0E98'	|
	'\u0EA0'	|
	'\u0EA4'	|
	'\u0EA6'	|
	'\u0EA8' .. '\u0EA9'	|
	'\u0EAC'	|
	'\u0EBA'	|
	'\u0EBE' .. '\u0EBF'	|
	'\u0EC5'	|
	'\u0EC7'	|
	'\u0ECE' .. '\u0ECF'	|
	'\u0EDA' .. '\u0EDB'	|
	'\u0EDE' .. '\u0EFF'	|
	'\u0F48'	|
	'\u0F6B' .. '\u0F70'	|
	'\u0F8C' .. '\u0F8F'	|
	'\u0F98'	|
	'\u0FBD'	|
	'\u0FCD' .. '\u0FCE'	|
	'\u0FD2' .. '\u0FFF'	|
	'\u1022'	|
	'\u1028'	|
	'\u102B'	|
	'\u1033' .. '\u1035'	|
	'\u103A' .. '\u103F'	|
	'\u105A' .. '\u109F'	|
	'\u10C6' .. '\u10CF'	|
	'\u10FD' .. '\u10FF'	|
	'\u115A' .. '\u115E'	|
	'\u11A3' .. '\u11A7'	|
	'\u11FA' .. '\u11FF'	|
	'\u1249'	|
	'\u124E' .. '\u124F'	|
	'\u1257'	|
	'\u1259'	|
	'\u125E' .. '\u125F'	|
	'\u1289'	|
	'\u128E' .. '\u128F'	|
	'\u12B1'	|
	'\u12B6' .. '\u12B7'	|
	'\u12BF'	|
	'\u12C1'	|
	'\u12C6' .. '\u12C7'	|
	'\u12D7'	|
	'\u1311'	|
	'\u1316' .. '\u1317'	|
	'\u135B' .. '\u135E'	|
	'\u137D' .. '\u137F'	|
	'\u139A' .. '\u139F'	|
	'\u13F5' .. '\u1400'	|
	'\u1677' .. '\u167F'	|
	'\u169D' .. '\u169F'	|
	'\u16F1' .. '\u16FF'	|
	'\u170D'	|
	'\u1715' .. '\u171F'	|
	'\u1737' .. '\u173F'	|
	'\u1754' .. '\u175F'	|
	'\u176D'	|
	'\u1771'	|
	'\u1774' .. '\u177F'	|
	'\u17DE' .. '\u17DF'	|
	'\u17EA' .. '\u17EF'	|
	'\u17FA' .. '\u17FF'	|
	'\u180F'	|
	'\u181A' .. '\u181F'	|
	'\u1878' .. '\u187F'	|
	'\u18AA' .. '\u18FF'	|
	'\u191D' .. '\u191F'	|
	'\u192C' .. '\u192F'	|
	'\u193C' .. '\u193F'	|
	'\u1941' .. '\u1943'	|
	'\u196E' .. '\u196F'	|
	'\u1975' .. '\u197F'	|
	'\u19AA' .. '\u19AF'	|
	'\u19CA' .. '\u19CF'	|
	'\u19DA' .. '\u19DD'	|
	'\u1A1C' .. '\u1A1D'	|
	'\u1A20' .. '\u1AFF'	|
	'\u1B4C' .. '\u1B4F'	|
	'\u1B7D' .. '\u1CFF'	|
	'\u1DCB' .. '\u1DFD'	|
	'\u1E9C' .. '\u1E9F'	|
	'\u1EFA' .. '\u1EFF'	|
	'\u1F16' .. '\u1F17'	|
	'\u1F1E' .. '\u1F1F'	|
	'\u1F46' .. '\u1F47'	|
	'\u1F4E' .. '\u1F4F'	|
	'\u1F58'	|
	'\u1F5A'	|
	'\u1F5C'	|
	'\u1F5E'	|
	'\u1F7E' .. '\u1F7F'	|
	'\u1FB5'	|
	'\u1FC5'	|
	'\u1FD4' .. '\u1FD5'	|
	'\u1FDC'	|
	'\u1FF0' .. '\u1FF1'	|
	'\u1FF5'	|
	'\u1FFF'	|
	'\u2064' .. '\u2069'	|
	'\u2072' .. '\u2073'	|
	'\u208F'	|
	'\u2095' .. '\u209F'	|
	'\u20B6' .. '\u20CF'	|
	'\u20F0' .. '\u20FF'	|
	'\u214F' .. '\u2152'	|
	'\u2185' .. '\u218F'	|
	'\u23E8' .. '\u23FF'	|
	'\u2427' .. '\u243F'	|
	'\u244B' .. '\u245F'	|
	'\u269D' .. '\u269F'	|
	'\u26B3' .. '\u2700'	|
	'\u2705'	|
	'\u270A' .. '\u270B'	|
	'\u2728'	|
	'\u274C'	|
	'\u274E'	|
	'\u2753' .. '\u2755'	|
	'\u2757'	|
	'\u275F' .. '\u2760'	|
	'\u2795' .. '\u2797'	|
	'\u27B0'	|
	'\u27BF'	|
	'\u27CB' .. '\u27CF'	|
	'\u27EC' .. '\u27EF'	|
	'\u2B1B' .. '\u2B1F'	|
	'\u2B24' .. '\u2BFF'	|
	'\u2C2F'	|
	'\u2C5F'	|
	'\u2C6D' .. '\u2C73'	|
	'\u2C78' .. '\u2C7F'	|
	'\u2CEB' .. '\u2CF8'	|
	'\u2D26' .. '\u2D2F'	|
	'\u2D66' .. '\u2D6E'	|
	'\u2D70' .. '\u2D7F'	|
	'\u2D97' .. '\u2D9F'	|
	'\u2DA7'	|
	'\u2DAF'	|
	'\u2DB7'	|
	'\u2DBF'	|
	'\u2DC7'	|
	'\u2DCF'	|
	'\u2DD7'	|
	'\u2DDF' .. '\u2DFF'	|
	'\u2E18' .. '\u2E1B'	|
	'\u2E1E' .. '\u2E7F'	|
	'\u2E9A'	|
	'\u2EF4' .. '\u2EFF'	|
	'\u2FD6' .. '\u2FEF'	|
	'\u2FFC' .. '\u2FFF'	|
	'\u3040'	|
	'\u3097' .. '\u3098'	|
	'\u3100' .. '\u3104'	|
	'\u312D' .. '\u3130'	|
	'\u318F'	|
	'\u31B8' .. '\u31BF'	|
	'\u31D0' .. '\u31EF'	|
	'\u321F'	|
	'\u3244' .. '\u324F'	|
	'\u32FF'	|
	'\u4DB6' .. '\u4DBF'	|
	'\u9FBC' .. '\u9FFF'	|
	'\uA48D' .. '\uA48F'	|
	'\uA4C7' .. '\uA6FF'	|
	'\uA71B' .. '\uA71F'	|
	'\uA722' .. '\uA7FF'	|
	'\uA82C' .. '\uA83F'	|
	'\uA878' .. '\uABFF'	|
	'\uD7A4' .. '\uD7FF'	|
	'\uFA2E' .. '\uFA2F'	|
	'\uFA6B' .. '\uFA6F'	|
	'\uFADA' .. '\uFAFF'	|
	'\uFB07' .. '\uFB12'	|
	'\uFB18' .. '\uFB1C'	|
	'\uFB37'	|
	'\uFB3D'	|
	'\uFB3F'	|
	'\uFB42'	|
	'\uFB45'	|
	'\uFBB2' .. '\uFBD2'	|
	'\uFD40' .. '\uFD4F'	|
	'\uFD90' .. '\uFD91'	|
	'\uFDC8' .. '\uFDEF'	|
	'\uFDFE' .. '\uFDFF'	|
	'\uFE1A' .. '\uFE1F'	|
	'\uFE24' .. '\uFE2F'	|
	'\uFE53'	|
	'\uFE67'	|
	'\uFE6C' .. '\uFE6F'	|
	'\uFE75'	|
	'\uFEFD' .. '\uFEFE'	|
	'\uFF00'	|
	'\uFFBF' .. '\uFFC1'	|
	'\uFFC8' .. '\uFFC9'	|
	'\uFFD0' .. '\uFFD1'	|
	'\uFFD8' .. '\uFFD9'	|
	'\uFFDD' .. '\uFFDF'	|
	'\uFFE7'	|
	'\uFFEF' .. '\uFFF8'	;


