'From Cuis 3.3 of 2 June 2011 [latest update: #1024] on 14 June 2011 at 3:21:06 pm'!!SHParserST80 methodsFor: 'parse' stamp: 'jmv 6/14/2011 15:16'!parseBinary 	| binary type |	self parseUnary.	[ self isBinary ]		whileTrue: [			binary _ currentToken.			type _ #binary.			(binary isEmpty or: [ Symbol hasInternedAndImplemented: binary ])				ifFalse: [					type _ (Symbol thatStartsCaseSensitive: binary)						ifNil: [ #undefinedBinary]						ifNotNil: [ #incompleteBinary]].				self scanPast: type. 				self parseTerm.            	self parseUnary ]! !!SHParserST80 methodsFor: 'parse' stamp: 'jmv 6/14/2011 15:20'!parseKeyword     | keyword rangeIndices |	self parseBinary.	keyword _ ''.	rangeIndices _ #().	[    		[ self isKeyword ]        		whileTrue: [				keyword _ keyword, currentToken. 				self rangeType: #keyword.				"remember where this keyword token is in ranges"				rangeIndices _ rangeIndices copyWith: ranges size.				self scanNext.				self parseTerm.				self parseBinary ]	] ensure: [ | type |		"do this in an ensure so that it happens even if the errorBlock evaluates before getting here"		"patch up the keyword tokens, so that incomplete and undefined ones look different"		(keyword isEmpty or: [ Symbol hasInternedAndImplemented: keyword ])			ifFalse: [				type _ (Symbol thatStartsCaseSensitive: keyword)					ifNil: [ #undefinedKeyword]					ifNotNil: [ #incompleteKeyword ].				rangeIndices do: [ :i | (ranges at: i) type: type ]]]! !!SHParserST80 methodsFor: 'parse' stamp: 'jmv 6/14/2011 15:20'!parseUnary	| unary type |	[ self isName ]		whileTrue: [			unary _ currentToken.			type _ #unary.			(unary isEmpty or: [ Symbol hasInternedAndImplemented: unary ])				ifFalse:[					type _ (Symbol thatStartsCaseSensitive: unary)						ifNil: [ #undefinedUnary]						ifNotNil: [ #incompleteUnary ]].			self scanPast: type ]! !!SmalltalkEditor class methodsFor: 'keyboard shortcut tables' stamp: 'jmv 6/14/2011 14:29'!initializeCmdKeyShortcuts	"Initialize the (unshifted) command-key (or alt-key if not on Mac) shortcut table."	"NOTE: if you don't know what your keyboard generates, use Sensor test"	"SmalltalkEditor initialize"	| cmds |	self initializeBasicCmdKeyShortcuts.		cmds := #(		$i	#inspectIt:		$p	#printIt:		$s	#save:		$d	#doIt:		$j	#doAgainOnce:		$l	#cancel:		$b	#browseIt:		$n	#sendersOfIt:		$m	#implementorsOfIt:		$T	#displayIfTrue:		$I	#exploreIt:		$A	#argAdvance:		$F	#displayIfFalse:		$G	#fileItIn:		$V	#pasteInitials:		$N	#referencesToIt:	).	1 to: cmds size		by: 2		do: [ :i | cmdActions at: (cmds at: i) asciiValue + 1 put: (cmds at: i + 1)]! !!Symbol class methodsFor: 'access' stamp: 'jmv 6/14/2011 14:39'!thatStartsCaseSensitive: leadingCharacters	"Same as thatStarts:skipping: but caseSensitive"	"Note: Only answers symbols for which there is some implementor"	| size firstMatch key |	size := leadingCharacters size.	size = 0 ifTrue: [^#''].	firstMatch := leadingCharacters at: 1.	size > 1 ifTrue: [ key _ leadingCharacters copyFrom: 2 to: size ].	self allSymbolTablesDo: [ :each |			each size >= size ifTrue: [				((each at: 1) == firstMatch and: [					key == nil or: [						(each findString: key startingAt: 2 caseSensitive: true) = 2]])							ifTrue: [								(Smalltalk isThereAnImplementorOf: each) ifTrue: [									^each]]]].	^nil! !Symbol class removeSelector: #thatStartsCaseSensitive:skipping:!SmalltalkEditor removeSelector: #completeSymbol:lastOffering:!SmalltalkEditor removeSelector: #querySymbol:!SmalltalkEditor removeSelector: #undoQuery:lastOffering:!"Postscript:Leave the line above, and replace the rest of this comment by a useful one.Executable statements should follow this comment, and shouldbe separated by periods, with no exclamation points (!!).Be sure to put any further comments in double-quotes, like this one."Editor initialize!