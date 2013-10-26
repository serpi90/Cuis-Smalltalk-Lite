'From Cuis 3.2 of 12 April 2011 [latest update: #914] on 27 April 2011 at 7:54:35 am'!!Editor methodsFor: 'menu messages' stamp: 'jmv 4/27/2011 07:50'!wordSelectAndEmptyCheck: returnBlock	"Ensure selecting the entire current word; if after that's done the selection is still empty, then evaluate the returnBlock, which will typically consist of '[^ self]' in the caller -- check senders of this method to understand this."	self selectWord.  "Select exactly a whole word"	self hasSelection ifFalse: [morph flash.  ^ returnBlock value]! !!SmalltalkEditor methodsFor: 'menu messages' stamp: 'jmv 4/27/2011 07:48'!browseClassFromIt	"Launch a hierarchy browser for the class indicated by the current selection.  If multiple classes matching the selection exist, let the user choose among them."	| aClass |	self wordSelectAndEmptyCheck: [^ self].	aClass _ Utilities classFromPattern: (self selection string copyWithout: Character cr) withCaption: 'choose a class to browse...'.	aClass ifNil: [^ morph flash].	HierarchyBrowserWindow		onClass: aClass		selector: nil! !!SmalltalkEditor methodsFor: 'menu messages' stamp: 'jmv 4/27/2011 07:48'!browseIt	"Launch a browser for the current selection, if appropriate"	| aSymbol anEntry browser |	Preferences alternativeBrowseIt ifTrue: [^ self browseClassFromIt].	self wordSelectAndEmptyCheck: [^ self].	(aSymbol _ self selectedSymbol) ifNil: [^ morph flash].	aSymbol first isUppercase		ifTrue: [			anEntry _ (Smalltalk				at: aSymbol				ifAbsent: [					Smalltalk browseAllImplementorsOf: aSymbol.					^ nil]).			anEntry ifNil: [^ morph flash].			(anEntry isKindOf: Class)				ifFalse:	[anEntry _ anEntry class].			browser _ Browser new.			browser setClass: anEntry selector: nil.			BrowserWindow open: browser label:'System Browser' ]		ifFalse:			[Smalltalk browseAllImplementorsOf: aSymbol]! !!SmalltalkEditor methodsFor: 'menu messages' stamp: 'jmv 4/27/2011 07:54'!referencesToIt	"Open a references browser on the selected symbol: a variable name or class name"	| symbol provider environment reference |	self selectWord.	"look for exactly a whole word"	symbol _ self selectedSymbol		ifNil: [ ^ morph flash ].	"convenient access to class variables, including those in SharedPools"	provider _ self codeProvider.	environment _ ((provider respondsTo: #selectedClassOrMetaClass)		ifTrue: [ provider selectedClassOrMetaClass ])			ifNil: [ Smalltalk ].	reference _ (environment bindingOf: symbol) ifNil: [ ^ morph flash ].	Smalltalk browseAllCallsOn: reference! !