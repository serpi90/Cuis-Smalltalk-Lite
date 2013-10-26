'From Cuis 2.3 of 22 March 2010 [latest update: #472] on 31 May 2010 at 2:20:43 pm'!!ContextPart methodsFor: 'private' stamp: 'jmv 5/28/2010 15:00'!tryNamedPrimitiveIn: aCompiledMethod for: aReceiver withArgs: arguments	"Hack. Attempt to execute the named primitive from the given compiled method"	| selector theMethod spec |	arguments size > 8 ifTrue:[^PrimitiveFailToken].	selector _ #(		tryNamedPrimitive 		tryNamedPrimitive: 		tryNamedPrimitive:with: 		tryNamedPrimitive:with:with: 		tryNamedPrimitive:with:with:with:		tryNamedPrimitive:with:with:with:with:		tryNamedPrimitive:with:with:with:with:with:		tryNamedPrimitive:with:with:with:with:with:with:		tryNamedPrimitive:with:with:with:with:with:with:with:) at: arguments size+1.	theMethod _ aReceiver class lookupSelector: selector.	theMethod ifNil: [ ^PrimitiveFailToken].	spec _ theMethod literalAt: 1.	spec replaceFrom: 1 to: spec size with: (aCompiledMethod literalAt: 1) startingAt: 1.	theMethod flushCache.	selector flushCache.	^aReceiver perform: selector withArguments: arguments! !!Decompiler methodsFor: 'public access' stamp: 'ar 5/27/2010 21:35'!decompileBlock: aBlock 	"Decompile aBlock, returning the result as a BlockNode.  	Show temp names from source if available."	"Decompiler new decompileBlock: [3 + 4]"	| startpc end homeClass blockNode methodNode home source |	(home := aBlock home) ifNil: [^ nil].	method := home method.	(homeClass := home methodClass) == #unknown ifTrue: [^ nil].	constructor := self constructorForMethod: aBlock method.	self withTempNames: (method tempNamesString ifNil:[	method fileIndex ~~ 0 ifTrue: "got any source code?"		[source := [method getSourceFromFile]						on: Error						do: [:ex | ^ nil].		 methodNode := [homeClass compilerClass new								parse: source								in: homeClass								notifying: nil]							on: SyntaxErrorNotification							do: [:ex | ^ nil].		methodNode schematicTempNamesString]]).	self initSymbols: homeClass.	startpc := aBlock startpc.	end := aBlock isClosure				ifTrue: [(method at: startpc - 2) * 256					  + (method at: startpc - 1) + startpc - 1]				ifFalse:					[(method at: startpc - 2) \\ 16 - 4 * 256					+ (method at: startpc - 1) + startpc - 1].	stack := OrderedCollection new: method frameSize.	caseExits := OrderedCollection new.	statements := OrderedCollection new: 20.	super		method: method		pc: (aBlock isClosure ifTrue: [startpc - 4] ifFalse: [startpc - 5]).	aBlock isClosure ifTrue:		[numLocalTemps := #decompileBlock: "Get pushClosureCopy... to hack fake temps for copied values"].	blockNode := self blockTo: end.	stack isEmpty ifFalse: [self error: 'stack not empty'].	^blockNode statements first! !!LiteralVariableNode methodsFor: 'code generation (new scheme)' stamp: 'eem 5/20/2010 08:55'!sizeCodeForStorePop: encoder	self reserve: encoder.	^(key isVariableBinding and: [key isSpecialWriteBinding])		ifTrue: [(self sizeCodeForStore: encoder) + encoder sizePop]		ifFalse: [encoder sizeStorePopLiteralVar: index]! !!LiteralVariableNode methodsFor: 'testing' stamp: 'eem 5/20/2010 09:12'!assignmentCheck: encoder at: location	^(key isVariableBinding and: [key canAssign not])		ifTrue: [location]		ifFalse: [-1]! !!LookupKey methodsFor: 'accessing' stamp: 'ajh 9/12/2002 12:04'!canAssign	^ true! !!Parser methodsFor: 'primitives' stamp: 'jmv 5/28/2010 14:53'!externalFunctionDeclaration	"Parse the function declaration for a call to an external library."	| descriptorClass callType modifier retType externalName args argType module fn |	descriptorClass _ Smalltalk at: #ExternalFunction ifAbsent: nil.	descriptorClass == nil ifTrue:[^false].	callType _ descriptorClass callingConventionFor: here.	descriptorClass		ifNil: [^ false].	[modifier _ descriptorClass callingConventionModifierFor: token.	 modifier notNil] whileTrue:		[self advance.		 callType _ callType bitOr: modifier].	"Parse return type"	self advance.	retType _ self externalType: descriptorClass.	retType == nil ifTrue:[^self expected:'return type'].	"Parse function name or index"	externalName _ here.	(self match: #string) 		ifTrue:[externalName _ externalName asSymbol]		ifFalse:[(self match:#number) ifFalse:[^self expected:'function name or index']].	(self matchToken: #'(') ifFalse:[^self expected:'argument list'].	args _ WriteStream on: Array new.	[here == #')'] whileFalse:[		argType _ self externalType: descriptorClass.		argType == nil ifTrue:[^self expected:'argument'].		argType isVoid & argType isPointerType not ifFalse:[args nextPut: argType].	].	(args position = self properties selector numArgs) ifFalse:[		^self expected: 'Matching number of arguments'	].	(self matchToken: #')') ifFalse:[^self expected:')'].	(self matchToken: 'module:') ifTrue:[		module _ here.		(self match: #string) ifFalse:[^self expected: 'String'].		module _ module asSymbol].	Smalltalk at: #ExternalLibraryFunction ifPresent:[:xfn|		fn _ xfn name: externalName 				module: module 				callType: callType				returnType: retType				argumentTypes: args contents.		self allocateLiteral: fn.	].	(self matchToken: 'error:')		ifTrue:			[| errorCodeVariable |			 errorCodeVariable _ here.			(hereType == #string			 or: [hereType == #word]) ifFalse:[^self expected: 'error code (a variable or string)'].			 self advance.			 self addPragma: (Pragma keyword: #primitive:error: arguments: (Array with: 120 with: errorCodeVariable)).			 fn ifNotNil: [fn setErrorCodeName: errorCodeVariable]]		ifFalse:			[self addPragma: (Pragma keyword: #primitive: arguments: #(120))].	^true! !!ReadOnlyVariableBinding methodsFor: 'accessing' stamp: 'ajh 9/12/2002 12:06'!canAssign	^ false! !