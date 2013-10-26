'From Cuis 2.6 of 10 August 2010 [latest update: #540] on 28 August 2010 at 10:08:11 pm'!!classDefinition: #BytecodeAgnosticMethodNode category: #'Compiler-ParseNodes'!MethodNode subclass: #BytecodeAgnosticMethodNode	instanceVariableNames: 'locationCounter localsPool zzlocationCounter zzlocalsPool '	classVariableNames: ''	poolDictionaries: ''	category: 'Compiler-ParseNodes'!!BytecodeAgnosticMethodNode methodsFor: 'code generation (closures)' stamp: 'jmv 8/28/2010 22:07'!addLocalsToPool: locals "<Set of: TempVariableNode>"	localsPool ifNil: [		localsPool := IdentitySet new].	localsPool addAll: locals.	zzlocalsPool _ localsPool! !!BytecodeAgnosticMethodNode methodsFor: 'code generation (closures)' stamp: 'jmv 8/28/2010 22:07'!noteBlockEntry: aBlock	"Evaluate aBlock with the numbering for the block entry."	locationCounter ifNil: [		locationCounter := -1].	aBlock value: locationCounter + 1.	locationCounter := locationCounter + 2.	zzlocationCounter := locationCounter! !!BytecodeAgnosticMethodNode methodsFor: 'code generation (closures)' stamp: 'jmv 8/28/2010 22:07'!noteBlockExit: aBlock	"Evaluate aBlock with the numbering for the block exit."	locationCounter ifNil: [ locationCounter _ zzlocationCounter ].	aBlock value: locationCounter + 1.	locationCounter := locationCounter + 2.	zzlocationCounter := locationCounter	! !!classDefinition: #BytecodeAgnosticMethodNode category: #'Compiler-ParseNodes'!MethodNode subclass: #BytecodeAgnosticMethodNode	instanceVariableNames: 'locationCounter localsPool zzlocationCounter zzlocalsPool'	classVariableNames: ''	poolDictionaries: ''	category: 'Compiler-ParseNodes'!