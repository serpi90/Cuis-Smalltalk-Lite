'From Cuis 3.1 of 4 March 2011 [latest update: #850] on 23 March 2011 at 5:53:08 pm'!!SHParserST80 methodsFor: 'parse' stamp: 'jmv 3/23/2011 17:47'!parseBinary 	| binary type |	self parseUnary.	[ self isBinary ]		whileTrue: [			binary _ currentToken.			type _ #binary.			(binary isEmpty or: [ Symbol hasInternedAndImplemented: binary ])				ifFalse: [					type _ (Symbol thatStartsCaseSensitive: binary skipping: nil)						ifNil: [ #undefinedBinary]						ifNotNil: [ :symbol | 							(Smalltalk isThereAnImplementorOf: symbol)								ifTrue: [ #incompleteBinary ]								ifFalse: [ #undefinedBinary ]]].				self scanPast: type. 				self parseTerm.            	self parseUnary ]! !!SHParserST80 methodsFor: 'parse' stamp: 'jmv 3/23/2011 17:49'!parseKeyword     | keyword rangeIndices |    self parseBinary.	keyword := ''.	rangeIndices := #().	[    		[self isKeyword]        		whileTrue: [				keyword := keyword, currentToken. 				self rangeType: #keyword.				"remember where this keyword token is in ranges"				rangeIndices := rangeIndices copyWith: ranges size.				self scanNext.				self parseTerm.				self parseBinary ]	] ensure: [ | type |		"do this in an ensure so that it happens even if the errorBlock evaluates before getting here"		"patch up the keyword tokens, so that incomplete and undefined ones look different"		(keyword isEmpty or: [ Symbol hasInternedAndImplemented: keyword ])			ifFalse: [				type := (Symbol thatStartsCaseSensitive: keyword skipping: nil)					ifNil: [ #undefinedKeyword]					ifNotNil: [ :symbol | 							(Smalltalk isThereAnImplementorOf: symbol)								ifTrue: [ #incompleteKeyword ]								ifFalse: [ #undefinedKeyword ]].				rangeIndices do: [:i | (ranges at: i) type: type]]]! !!SHParserST80 methodsFor: 'parse' stamp: 'jmv 3/23/2011 17:49'!parseUnary	| unary type |	    [self isName]        whileTrue: [			unary := currentToken.			type := #unary.			(unary isEmpty or: [ Symbol hasInternedAndImplemented: unary ])				ifFalse:[					type := (Symbol thatStartsCaseSensitive: unary skipping: nil)						ifNil: [ #undefinedUnary]						ifNotNil: [ :symbol | 							(Smalltalk isThereAnImplementorOf: symbol)								ifTrue: [ #undefinedUnary ]								ifFalse: [ #undefinedUnary ]]].			self scanPast: type ]! !!Symbol class methodsFor: 'services' stamp: 'jmv 3/23/2011 17:44'!hasInterned: aString and: oneArgBlock 	"Answer with false if aString hasnt been interned (into a Symbol), 	otherwise supply the symbol to symBlock and return the result of the evaluation."	| symbol |	^ (symbol _ self lookup: aString)		ifNil: [ false ]		ifNotNil: [ oneArgBlock value: symbol ]! !!Symbol class methodsFor: 'services' stamp: 'jmv 3/23/2011 17:44'!hasInterned: aString ifTrue: symBlock 	"Answer with false if aString hasnt been interned (into a Symbol), 	otherwise supply the symbol to symBlock and return true."	| symbol |	^(symbol _ self lookup: aString)		ifNil: [false]		ifNotNil: [symBlock value: symbol. true]! !!Symbol class methodsFor: 'services' stamp: 'jmv 3/23/2011 17:44'!hasInternedAndImplemented: aString	"Answer with false if aString hasnt been interned (into a Symbol), 	or if there are no implemetors of the selector."	^self hasInterned: aString and: [ :symbol | Smalltalk isThereAnImplementorOf: symbol ]! !!Symbol class methodsFor: 'services' stamp: 'jmv 3/23/2011 17:44'!hasInternedAndImplementedOrReferenced: aString	"Answer with false if aString hasnt been interned (into a Symbol), 	or if there are no implemetors of the selector."	^self hasInterned: aString and: [ :symbol |		(Smalltalk isThereAnImplementorOf: symbol) or: [			Smalltalk isThereAReferenceTo: symbol ]]! !!Symbol class methodsFor: 'services' stamp: 'jmv 3/23/2011 17:44'!possibleSelectorsFor: misspelled 	"Answer an ordered collection of possible corrections	for the misspelled selector in order of likelyhood"	| numArgs candidates lookupString best binary short long first ss |	lookupString _ misspelled asLowercase. "correct uppercase selectors to lowercase"	numArgs _ lookupString numArgs.	(numArgs < 0 or: [lookupString size < 2]) ifTrue: [^ OrderedCollection new: 0].	first _ lookupString first.	short _ lookupString size - (lookupString size // 4 max: 3) max: 2.	long _ lookupString size + (lookupString size // 4 max: 3).	"First assemble candidates for detailed scoring"	candidates _ OrderedCollection new.	self allSymbolTablesDo: [:s | (((ss _ s size) >= short	"not too short"			and: [ss <= long			"not too long"					or: [(s at: 1) = first]])	"well, any length OK if starts w/same letter"			and: [s numArgs = numArgs])	"and numArgs is the same"			ifTrue: [candidates add: s]].	"Then further prune these by correctAgainst:"	best _ lookupString correctAgainst: candidates.	((misspelled last ~~ $:) and: [misspelled size > 1]) ifTrue: [		binary _ misspelled, ':'.		"try for missing colon"		Symbol hasInterned: binary ifTrue: [:him | best addFirst: him]].	^ best! !!Symbol class reorganize!('access' allSymbols findInterned: selectorsContaining: selectorsMatching: thatStarts:skipping: thatStartsCaseSensitive:skipping:)('class initialization' allSymbolTablesDo: allSymbolTablesDo:after: compactSymbolTable initialize)('instance creation' intern: internCharacter: lookup: newFrom: readFrom:)('private' rehash shutDown:)('services' hasInterned:and: hasInterned:ifTrue: hasInternedAndImplemented: hasInternedAndImplementedOrReferenced: possibleSelectorsFor:)!