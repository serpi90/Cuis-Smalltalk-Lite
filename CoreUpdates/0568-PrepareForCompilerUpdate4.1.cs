'From Cuis 2.6 of 10 August 2010 [latest update: #540] on 29 August 2010 at 8:45:42 am'!!classDefinition: #MethodNode category: #'Compiler-ParseNodes'!ParseNode subclass: #MethodNode	instanceVariableNames: 'selectorOrFalse precedence arguments block primitive encoder temporaries properties sourceText localsPool locationCounter '	classVariableNames: ''	poolDictionaries: ''	category: 'Compiler-ParseNodes'!!BytecodeAgnosticMethodNode methodsFor: 'code generation (closures)' stamp: 'jmv 8/29/2010 08:45'!addLocalsToPool: locals "<Set of: TempVariableNode>"	zzlocalsPool ifNil: [ zzlocalsPool _ localsPool ].	zzlocalsPool ifNil: [		zzlocalsPool := IdentitySet new].	zzlocalsPool addAll: locals.	localsPool _ zzlocalsPool! !!BytecodeAgnosticMethodNode methodsFor: 'code generation (closures)' stamp: 'jmv 8/29/2010 08:44'!locationCounter	zzlocationCounter ifNil: [ zzlocationCounter _ locationCounter ].	^zzlocationCounter! !!BytecodeAgnosticMethodNode methodsFor: 'code generation (closures)' stamp: 'jmv 8/29/2010 08:44'!noteBlockEntry: aBlock	"Evaluate aBlock with the numbering for the block entry."	zzlocationCounter ifNil: [ zzlocationCounter _ locationCounter ].	zzlocationCounter ifNil: [		zzlocationCounter := -1].	aBlock value: zzlocationCounter + 1.	zzlocationCounter := zzlocationCounter + 2.	locationCounter _ zzlocationCounter! !!BytecodeAgnosticMethodNode methodsFor: 'code generation (closures)' stamp: 'jmv 8/29/2010 08:44'!noteBlockExit: aBlock	"Evaluate aBlock with the numbering for the block exit."	zzlocationCounter ifNil: [ zzlocationCounter _ locationCounter ].	aBlock value: zzlocationCounter + 1.	zzlocationCounter := zzlocationCounter + 2.	locationCounter _ zzlocationCounter! !!BytecodeAgnosticMethodNode methodsFor: 'code generation (closures)' stamp: 'jmv 8/29/2010 08:45'!referencedValuesWithinBlockExtent: anInterval 	zzlocalsPool ifNil: [ zzlocalsPool _ localsPool ].	^(zzlocalsPool select:		[:temp|		 temp isReferencedWithinBlockExtent: anInterval]) collect:			[:temp|			temp isRemote ifTrue: [temp remoteNode] ifFalse: [temp]]! !!classDefinition: #MethodNode category: #'Compiler-ParseNodes'!ParseNode subclass: #MethodNode	instanceVariableNames: 'selectorOrFalse precedence arguments block primitive encoder temporaries properties sourceText locationCounter localsPool'	classVariableNames: ''	poolDictionaries: ''	category: 'Compiler-ParseNodes'!