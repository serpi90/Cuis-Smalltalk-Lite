'From Cuis 3.2 of 12 April 2011 [latest update: #914] on 26 May 2011 at 8:44:26 am'!!classDefinition: #CuisSourceFileArrayTest category: #'Tests-Files'!TestCase subclass: #CuisSourceFileArrayTest	instanceVariableNames: ''	classVariableNames: ''	poolDictionaries: ''	category: 'Tests-Files'!!CuisSourceFileArrayTest commentStamp: '<historical>' prior: 0!This test documents the source pointer address conversion methods for StandardSourceFileArray.The available address space for source pointers in CompiledMethod is 16r1000000 through 16r4FFFFFF. StandardSourceFileArray maps positions in the sources file to address range 16r1000000 through 16r1FFFFFF and 16r3000000 through 16r3FFFFFF, and positions in the changes file to address range 16r2000000 through 16r2FFFFFF and 16r4000000 through 16r4FFFFFF. This permits a maximum file size of 16r2000000 (32MB) for both the sources file and the changes file. !!ClassDescription methodsFor: 'fileIn/Out' stamp: 'jmv 5/22/2011 22:42'!printMethodChunk: selector withPreamble: doPreamble on: outStream moveSource: moveSource toFile: fileIndex	"Copy the source code for the method associated with selector onto the fileStream.  If moveSource true, then also set the source code pointer of the method."	| preamble method oldPos newPos sourceFile endPos |	doPreamble 		ifTrue: [preamble _ self name , ' methodsFor: ' ,					(self organization categoryOfElement: selector) asString printString]		ifFalse: [preamble _ ''].	method _ self methodDict at: selector ifAbsent: [		outStream nextPutAll: selector; cr.		outStream tab; nextPutAll: '** ERROR!!  THIS SCRIPT IS MISSING ** '; cr; cr.		outStream nextPutAll: '  '.		^ outStream].	((method fileIndex = 0		or: [(SourceFiles at: method fileIndex) == nil])		or: [(oldPos _ method filePosition) = 0])	ifTrue: [		"The source code is not accessible.  We must decompile..."		preamble size > 0 ifTrue: [outStream cr; nextPut: $!!; nextChunkPut: preamble; cr].		outStream nextChunkPut: method decompileString]	ifFalse: [		sourceFile _ SourceFiles at: method fileIndex.		preamble size > 0			ifTrue:    "Copy the preamble"				[outStream copyPreamble: preamble from: sourceFile at: oldPos]			ifFalse:				[sourceFile position: oldPos].		"Copy the method chunk"		outStream padTo: SourceFiles pointerScaleForWriting put: $ .		newPos _ outStream position.		outStream copyMethodChunkFrom: sourceFile.		sourceFile skipSeparators.      "The following chunk may have ]style["		sourceFile peek == $] ifTrue: [			outStream cr; copyMethodChunkFrom: sourceFile].		moveSource ifTrue: [    "Set the new method source pointer"			endPos _ outStream position.			method checkOKToAdd: endPos - newPos at: newPos in: method fileIndex.			method setSourcePosition: newPos inFile: fileIndex]].	preamble size > 0 ifTrue: [outStream nextChunkPut: ' '].	^ outStream cr! !!CompiledMethod methodsFor: 'source code management' stamp: 'jmv 5/22/2011 21:58'!putSource: sourceStr fromParseNode: methodNode inFile: fileIndex withPreamble: preambleBlock	"Store the source code for the receiver on an external file.	If no sources are available, i.e., SourceFile is nil, then store	temp names for decompilation at the end of the method.	If the fileIndex is 1, print on *.sources;  if it is 2, print on *.changes,	in each case, storing a 4-byte source code pointer at the method end."	| file remoteString  |	(SourceFiles == nil or: [(file _ SourceFiles at: fileIndex) == nil]) ifTrue:		[^ self become: (self copyWithTempsFromMethodNode: methodNode)].	Smalltalk assureStartupStampLogged.	file setToEnd.	preambleBlock value: file.  "Write the preamble"	remoteString _ RemoteString newString: sourceStr onFileNumber: fileIndex toFile: file.	file nextChunkPut: ' '.	InMidstOfFileinNotification signal ifFalse: [file flush].	self checkOKToAdd: sourceStr size at: remoteString position in: fileIndex.	self setSourcePosition: remoteString position inFile: fileIndex! !!CuisSourceFileArray methodsFor: 'sourcePointer conversion' stamp: 'jmv 5/23/2011 00:01'!sourcePointerFromFileIndex: index andPosition: position	"Return a sourcePointer encoding the given file index and position"	| answer changesFlag |	((index between: 1 and: 2) and: [position >= 0])		ifFalse: [self error: 'invalid source code pointer'].	changesFlag _ index = 2 ifTrue: [ 16r2000000 ] ifFalse: [ 0 ].	answer _ (position // pointerScale bitOr: changesFlag) + 16r1000000.	^answer! !!CuisSourceFileArrayTest methodsFor: 'testing' stamp: 'jmv 5/22/2011 23:57'!testAddressRange	"Test source pointer to file position address translation across the full address range"		| sf |	sf := CuisSourceFileArray new.	(16r1000000 to: 16r4FFFFFF by: 811) do: [:e | | i a p |		i := sf fileIndexFromSourcePointer: e.		p := sf filePositionFromSourcePointer: e.		a := sf sourcePointerFromFileIndex: i andPosition: p.		self assert: a = e]! !!CuisSourceFileArrayTest methodsFor: 'testing' stamp: 'jmv 5/23/2011 00:08'!testChangesFileAddressRange	"Test file position to source pointer address translation for the changes file"		| sf a e |	sf := CuisSourceFileArray new.	(0 to: 16r1FFFFFF by: 811) do: [:ee | | a2 i p |		e _ ee // 32 * 32.		a := sf sourcePointerFromFileIndex: 2 andPosition: e.		i := sf fileIndexFromSourcePointer: a.		self assert: i = 2.		p := sf filePositionFromSourcePointer: a.		self assert: p = e.		a2 := sf sourcePointerFromFileIndex: 2 andPosition: p.		self assert: a2 = a].	(0 to: 16rFFFFFF by: 811) do: [:ee |		e _ ee // 32 * 32.		a := sf sourcePointerFromFileIndex: 2 andPosition: e.		self assert: (a between: 16r3000000 and: 16r3FFFFFF)].	(16r1000000 to: 16r1FFFFFF by: 811) do: [:ee |		e _ ee // 32 * 32.		a := sf sourcePointerFromFileIndex: 2 andPosition: e.		self assert: (a between: 16r3000000 and: 16r4FFFFFF)]! !!CuisSourceFileArrayTest methodsFor: 'testing' stamp: 'jmv 5/23/2011 00:06'!testFileIndexFromSourcePointer	"Test derivation of file index for sources or changes file from source pointers"	| sf |	sf := CuisSourceFileArray new.	"sources file mapping"	self assert: 1 = (sf fileIndexFromSourcePointer: 16r1000000).	self assert: 1 = (sf fileIndexFromSourcePointer: 16r1000013).	self assert: 1 = (sf fileIndexFromSourcePointer: 16r1FFFFFF).	self assert: 1 = (sf fileIndexFromSourcePointer: 16r2000000).	self assert: 1 = (sf fileIndexFromSourcePointer: 16r2000013).	self assert: 1 = (sf fileIndexFromSourcePointer: 16r2FFFFFF).	(16r1000000 to: 16r1FFFFFF by: 811) do: [:e | self assert: 1 = (sf fileIndexFromSourcePointer: e)].	(16r2000000 to: 16r2FFFFFF by: 811) do: [:e | self assert: 1 = (sf fileIndexFromSourcePointer: e)].	"changes file mapping"	self assert: 2 = (sf fileIndexFromSourcePointer: 16r3000000).	self assert: 2 = (sf fileIndexFromSourcePointer: 16r3000013).	self assert: 2 = (sf fileIndexFromSourcePointer: 16r3FFFFFF).	self assert: 2 = (sf fileIndexFromSourcePointer: 16r4000000).	self assert: 2 = (sf fileIndexFromSourcePointer: 16r4000013).	self assert: 2 = (sf fileIndexFromSourcePointer: 16r4FFFFFF).	(16r3000000 to: 16r3FFFFFF by: 811) do: [:e | self assert: 2 = (sf fileIndexFromSourcePointer: e)].	(16r4000000 to: 16r4FFFFFF by: 811) do: [:e | self assert: 2 = (sf fileIndexFromSourcePointer: e)]! !!CuisSourceFileArrayTest methodsFor: 'testing' stamp: 'jmv 5/23/2011 00:09'!testFilePositionFromSourcePointer	"Test derivation of file position for sources or changes file from source pointers"	| sf |	sf := CuisSourceFileArray new.	"sources file"	self assert: 0 = (sf filePositionFromSourcePointer: 16r1000000).	"changes file"	self assert: 0 = (sf filePositionFromSourcePointer: 16r3000000).! !!CuisSourceFileArrayTest methodsFor: 'testing' stamp: 'jmv 5/23/2011 00:10'!testSourcePointerFromFileIndexAndPosition	"Test valid input ranges"	| sf |	sf := CuisSourceFileArray new.	self should: [sf sourcePointerFromFileIndex: 0 andPosition: 0] raise: Error.	self shouldnt: [sf sourcePointerFromFileIndex: 1 andPosition: 0] raise: Error.	self shouldnt: [sf sourcePointerFromFileIndex: 2 andPosition: 0] raise: Error.	self should: [sf sourcePointerFromFileIndex: 0 andPosition: 3] raise: Error.	self should: [sf sourcePointerFromFileIndex: 1 andPosition: -1] raise: Error.	self shouldnt: [sf sourcePointerFromFileIndex: 1 andPosition: 16r1FFFFFF] raise: Error.	self shouldnt: [sf sourcePointerFromFileIndex: 1 andPosition: 16r2000000] raise: Error.	self should: [sf sourcePointerFromFileIndex: 3 andPosition: 0] raise: Error.	self should: [sf sourcePointerFromFileIndex: 4 andPosition: 0] raise: Error.		self assert: 16r1000000 = (sf sourcePointerFromFileIndex: 1 andPosition: 0).	self assert: 16r3000000 = (sf sourcePointerFromFileIndex: 2 andPosition: 0).! !!CuisSourceFileArrayTest methodsFor: 'testing' stamp: 'jmv 5/23/2011 00:11'!testSourcesFileAddressRange	"Test file position to source pointer address translation for the sources file"		| sf a e |	sf := CuisSourceFileArray new.	(0 to: 16r1FFFFFF by: 811) do: [:ee | | a2 p i |		e _ ee // 32 * 32.		a := sf sourcePointerFromFileIndex: 1 andPosition: e.		i := sf fileIndexFromSourcePointer: a.		self assert: i = 1.		p := sf filePositionFromSourcePointer: a.		self assert: p = e.		a2 := sf sourcePointerFromFileIndex: 1 andPosition: p.		self assert: a2 = a].	(0 to: 16rFFFFFF by: 811) do: [:ee |		e _ ee // 32 * 32.		a := sf sourcePointerFromFileIndex: 1 andPosition: e.		self assert: (a between: 16r1000000 and: 16r1FFFFFF)].	(16r1000000 to: 16r1FFFFFF by: 811) do: [:ee |		e _ ee // 32 * 32.		a := sf sourcePointerFromFileIndex: 1 andPosition: e.		self assert: (a between: 16r1000000 and: 16r2FFFFFF)]! !!RemoteString methodsFor: 'private' stamp: 'jmv 5/22/2011 22:42'!string: aStringOrText onFileNumber: fileNumber toFile: aFileStream	"Store this as the receiver's text if source files exist."	| position |	aFileStream padTo: SourceFiles pointerScaleForWriting put: $ .	position _ aFileStream position.	self fileNumber: fileNumber position: position.	aFileStream nextChunkPut: aStringOrText asString! !CompiledMethod removeSelector: #checkOKToAdd:at:!