'From Cuis 2.7 of 3 September 2010 [latest update: #600] on 24 September 2010 at 9:02:11 am'!!classDefinition: #RunArray category: #'Collections-Arrayed'!ArrayedCollection subclass: #RunArray	instanceVariableNames: 'runs values lastIndex lastRun lastOffset canJoinBlock '	classVariableNames: ''	poolDictionaries: ''	category: 'Collections-Arrayed'!!classDefinition: #Text category: #'System-Text'!ArrayedCollection subclass: #Text	instanceVariableNames: 'string runs initialStyle '	classVariableNames: 'CanJoinBlock '	poolDictionaries: ''	category: 'System-Text'!!TextAnchor commentStamp: 'jmv 9/23/2010 13:19' prior: 0!TextAnchors support anchoring of images in text.  A TextAnchor exists as an attribute of text emphasis, and it gets control like a FontReference, through the emphasizeScanner: message.  Depending on whether its anchoredMorph is a Morph or a Form, it repositions the morph, or displays the form respectively.  The coordination between composition, display and selection can best be understood by browsing the various implementations of placeEmbeddedObject:.In the morphic world, simply embed any morph in text.  In the old world, you can create an image reference using code such as the following."A Form"('Hello', (Text string: '*' attribute: (TextAnchor new anchoredMorph: EllipseMorph new imageForm)), 'world') edit"A Morph"((Text withAll: 'foo') , (Text string: '*' asString attribute: (TextAnchor new anchoredMorph: EllipseMorph new)) , (Text withAll: 'bar')) editIn this case you select a piece of the screen, and it gets anchored to a one-character text in the editor's past buffer.  If you then paste into some other text, you will see the image as an embedded image.!!CharacterScanner methodsFor: 'scanning' stamp: 'jmv 9/23/2010 12:13'!placeEmbeddedObject	(text attributesAt: lastIndex) do: [ :attr |		attr anchoredMorph ifNotNil: [ :m |			self placeEmbeddedObject: m ]].! !!CharacterScanner methodsFor: 'scanning' stamp: 'jmv 9/23/2010 12:10'!placeEmbeddedObject: anchoredMorph	"Place the anchoredMorph or return false if it cannot be placed.	In any event, advance destX by its width."	| w |	"Workaround: The following should really use #textAnchorType"	anchoredMorph relativeTextAnchorPosition ifNotNil:[^true].	destX _ destX + (w _ anchoredMorph width).	(destX > rightMargin and: [(leftMargin + w) <= rightMargin])		ifTrue: ["Won't fit, but would on next line"				^ false].	lastIndex _ lastIndex + 1.	"Is this needed?"	false ifTrue: [		self setFont.  "Force recalculation of emphasis for next run"	].	^ true! !!CharacterScanner methodsFor: 'initialize' stamp: 'jmv 9/23/2010 08:32'!initialize	destX _ destY _ leftMargin _ rightMargin _ 0.! !!CharacterBlockScanner methodsFor: 'stop conditions' stamp: 'jmv 9/23/2010 12:14'!endOfRun	"Before arriving at the cursor location, the selection has encountered an 	end of run. Answer false if the selection continues, true otherwise. Set 	up indexes for building the appropriate CharacterBlock."	| runLength lineStop |	((characterIndex notNil and:		[runStopIndex < characterIndex and: [runStopIndex < text size]])			or:	[characterIndex == nil and: [lastIndex < line last]])		ifTrue:	["We're really at the end of a real run."				runLength _ (text runLengthFor: (lastIndex _ lastIndex + 1)).				lineStop _ characterIndex		"scanning for index"					ifNil: [ line last ].			"scanning for point"				(runStopIndex _ lastIndex + (runLength - 1)) > lineStop					ifTrue: 	[runStopIndex _ lineStop].				self setStopConditions.				self placeEmbeddedObject.				^false].	lastCharacter _ text at: lastIndex.	characterPoint _ destX @ destY.	((lastCharacter = Character space and: [alignment = CharacterScanner justifiedCode])		or: [lastCharacter = Character tab and: [lastSpaceOrTabExtent notNil]])		ifTrue: [lastCharacterExtent _ lastSpaceOrTabExtent].	characterIndex		ifNotNil: ["If scanning for an index and we've stopped on that index,				then we back destX off by the width of the character stopped on				(it will be pointing at the right side of the character) and return"				runStopIndex = characterIndex					ifTrue:	[self characterPointSetX: destX - lastCharacterExtent x.							^true].				"Otherwise the requested index was greater than the length of the				string.  Return string size + 1 as index, indicate further that off the				string by setting character to nil and the extent to 0."				lastIndex _  lastIndex + 1.				lastCharacter _ nil.				self lastCharacterExtentSetX: 0.				^true].	"Scanning for a point and either off the end of the line or off the end of the string."	runStopIndex = text size		ifTrue:	["off end of string"				lastIndex _  lastIndex + 1.				lastCharacter _ nil.				self lastCharacterExtentSetX: 0.				^true].	"just off end of line without crossing x"	lastIndex _ lastIndex + 1.	^true! !!CharacterBlockScanner methodsFor: 'scanning' stamp: 'jmv 9/23/2010 12:14'!characterBlockAtPoint: aPoint index: index in: textLine	"This method is the Morphic characterBlock finder.  It combines	MVC's characterBlockAtPoint:, -ForIndex:, and buildCharcterBlock:in:"	| runLength lineStop done stopCondition |	line _ textLine.	rightMargin _ line rightMargin.	lastIndex _ line first.	self setStopConditions.		"also sets font"	characterIndex _ index.  " == nil means scanning for point"	characterPoint _ aPoint.	(characterPoint == nil or: [characterPoint y > line bottom])		ifTrue: [characterPoint _ line bottomRight].	(text isEmpty or: [(characterPoint y < line top or: [characterPoint x < line left])				or: [characterIndex notNil and: [characterIndex < line first]]])		ifTrue:	[^ (CharacterBlock new stringIndex: line first text: text					topLeft: line leftMargin@line top extent: 0 @ "actualTextStyle initialLineGrid"line lineHeight)					textLine: line].	destX _ leftMargin _ line leftMarginForAlignment: alignment.	destY _ line top.	runLength _ text runLengthFor: line first.	lineStop _ characterIndex	"scanning for index"		ifNil: [ line last ].			"scanning for point"	runStopIndex _ lastIndex + (runLength - 1) min: lineStop.	lastCharacterExtent _ 0 @ line lineHeight.	spaceCount _ 0.	self placeEmbeddedObject.	done  _ false.	[ done ] whileFalse: [		stopCondition _ self scanCharactersFrom: lastIndex to: runStopIndex			in: text string rightX: characterPoint x			stopConditions: stopConditions kern: kern.		"see setStopConditions for stopping conditions for character block 	operations."		self lastCharacterExtentSetX: (specialWidth ifNil: [ font widthOf: (text at: lastIndex) ]).		(self perform: stopCondition) ifTrue: [			^characterIndex				ifNil: ["Result for characterBlockAtPoint: "						(CharacterBlock new stringIndex: lastIndex							text: text topLeft: characterPoint + (font descentKern @ 0)							extent: lastCharacterExtent - (font baseKern @ 0))									textLine: line]				ifNotNil: ["Result for characterBlockForIndex: "						(CharacterBlock new stringIndex: characterIndex							text: text topLeft: characterPoint + ((font descentKern - kern max: 0)@ 0)							extent: lastCharacterExtent)									textLine: line]]]! !!CharacterScanner class methodsFor: 'class initialization' stamp: 'jmv 9/22/2010 21:34'!initialize	"CharacterScanner initialize"	| stopConditions |	stopConditions _ Array new: 258.	stopConditions atAllPut: nil.	stopConditions at: Character space asciiValue + 1 put: nil.	stopConditions at: Character tab asciiValue + 1 put: #tab.	stopConditions at: Character lf asciiValue + 1 put: #lf.	stopConditions at: Character cr asciiValue + 1 put: #cr.	stopConditions at: CharacterScanner endOfRunCode put: #endOfRun.	stopConditions at: CharacterScanner crossedXCode put: #crossedX.	DefaultStopConditions _ stopConditions.! !!CompositionScanner methodsFor: 'scanning' stamp: 'jmv 9/23/2010 12:15'!composeFrom: startIndex inRectangle: lineRectangle firstLine: firstLine leftSide: leftSide rightSide: rightSide	"Answer an instance of TextLineInterval that represents the next line in the paragraph."	| runLength done stopCondition xtraSpaceBefore spaceAfterParagraph |		lastIndex _ startIndex.	"scanning sets last index"	destY _ lineRectangle top.	lineHeight _ baseline _ 0.  "Will be increased by setFont"	self setStopConditions.	"also sets font, style, etc"	"Set up margins"	leftMargin _ lineRectangle left.	leftSide ifTrue: [		leftMargin _ leftMargin +			((firstLine and: [actualTextStyle isListStyle not])				ifTrue: [actualTextStyle firstIndent]				ifFalse: [actualTextStyle restIndent])].	destX _ spaceX _ leftMargin.	rightMargin _ lineRectangle right.	rightSide ifTrue: [		rightMargin _ rightMargin - actualTextStyle rightIndent].	runLength _ text runLengthFor: startIndex.	runStopIndex _ (lastIndex _ startIndex) + (runLength - 1).	line _ (TextLine start: lastIndex stop: 0 internalSpaces: 0 paddingWidth: 0)				rectangle: lineRectangle.	line isFirstLine: firstLine.	spaceCount _ 0.	leftMargin _ destX.	line leftMargin: leftMargin.	done _ false.	xtraSpaceBefore _ firstLine		ifTrue: [ actualTextStyle paragraphSpacingBefore ]		ifFalse: [ 0 ].	spaceAfterParagraph _ actualTextStyle paragraphSpacingAfter.	self placeEmbeddedObject.	[ done ]		whileFalse: [			stopCondition _ self scanCharactersFrom: lastIndex to: runStopIndex				in: text string rightX: rightMargin stopConditions: stopConditions				kern: kern.			"See setStopConditions for stopping conditions for composing."			(self perform: stopCondition) ifTrue: [				^ line 					lineHeight: lineHeight + xtraSpaceBefore + 						(stopCondition = #cr ifTrue: [spaceAfterParagraph] ifFalse: [0]) 					baseline: baseline + xtraSpaceBefore ]]! !!CompositionScanner methodsFor: 'stop conditions' stamp: 'jmv 9/23/2010 12:15'!endOfRun	"Answer true if scanning has reached the end of the paragraph. 	Otherwise step conditions (mostly install potential new font) and answer 	false."	| runLength |	lastIndex = text size	ifTrue:	[line stop: lastIndex.			spaceX _ destX.			line paddingWidth: rightMargin - destX.			^true]	ifFalse:	[runLength _ (text runLengthFor: (lastIndex _ lastIndex + 1)).			runStopIndex _ lastIndex + (runLength - 1).			self setStopConditions.			self placeEmbeddedObject.			^false]! !!DisplayScanner methodsFor: 'scanning' stamp: 'jmv 9/23/2010 12:16'!displayLine: textLine offset: offset leftInRun: leftInRun	"The call on the primitive (scanCharactersFrom:to:in:rightX:) will be interrupted according to an array of stop conditions passed to the scanner at which time the code to handle the stop condition is run and the call on the primitive continued until a stop condition returns true (which means the line has terminated).  leftInRun is the # of characters left to scan in the current run; when 0, it is time to call setStopConditions."	| done stopCondition nowLeftInRun startIndex string lastPos priorFont |	line _ textLine.	morphicOffset _ offset.	lineY _ line top + offset y.	lineHeight _ line lineHeight.	rightMargin _ line rightMargin + offset x.	lastIndex _ line first.	leftInRun <= 0 ifTrue: [self setStopConditions].	leftMargin _ (line leftMarginForAlignment: alignment) + offset x.	destX _ runX _ leftMargin.	destY _ lineY + line baseline - font ascent.	textLine isEmptyLine ifTrue: [		textLine textStyle ifNotNil: [ :ts |			ts = actualTextStyle ifFalse: [				""				foregroundColor _ paragraphColor.				priorFont _ font.				self setActualFont: ts font.				ts color ifNotNil: [ :color | self textColor: color ].				alignment _ ts alignment.				actualTextStyle _ ts.				priorFont ifNotNil: [ destX _ destX + priorFont descentKern ].				destX _ destX - font descentKern.				kern _ 0 - font baseKern.				spaceWidth _ font widthOf: Character space.				xTable _ font xTable.				map _ font characterToGlyphMap.				stopConditions _ DefaultStopConditions.				font installOn: bitBlt foregroundColor: foregroundColor.				text ifNotNil:[destY _ lineY + line baseline - font ascent]				""			]		].		self displayBulletIfAppropriateFor: textLine offset: offset.		^leftInRun ].	self displayBulletIfAppropriateFor: textLine offset: offset.	lastIndex _ line first.	leftInRun <= 0		ifTrue: [nowLeftInRun _ text runLengthFor: lastIndex]		ifFalse: [nowLeftInRun _ leftInRun].	runStopIndex _ lastIndex + (nowLeftInRun - 1) min: line last.	spaceCount _ 0.	done _ false.	string _ text string.	self placeEmbeddedObject.	[done] whileFalse:[		startIndex _ lastIndex.		lastPos _ destX@destY.		stopCondition _ self scanCharactersFrom: lastIndex to: runStopIndex						in: string rightX: rightMargin stopConditions: stopConditions						kern: kern.		lastIndex >= startIndex ifTrue:[			font displayString: string on: bitBlt 				from: startIndex to: lastIndex at: lastPos kern: kern].		"see setStopConditions for stopping conditions for displaying."		done _ self perform: stopCondition].	^ runStopIndex - lastIndex   "Number of characters remaining in the current run"! !!DisplayScanner methodsFor: 'scanning' stamp: 'jmv 9/23/2010 12:09'!placeEmbeddedObject: anchoredMorph	anchoredMorph relativeTextAnchorPosition ifNotNil:[		anchoredMorph position: 			anchoredMorph relativeTextAnchorPosition +			(anchoredMorph owner textBounds origin x @ 0)			- (0@morphicOffset y) + (0@lineY).		^true	].	(super placeEmbeddedObject: anchoredMorph) ifFalse: [^ false].	(anchoredMorph is: #Morph) ifTrue: [		anchoredMorph position: ((destX - anchoredMorph width)@lineY) - morphicOffset	] ifFalse: [		destY _ lineY.		runX _ destX.		anchoredMorph 			displayOn: bitBlt destForm 			at: destX - anchoredMorph width @ destY			clippingBox: bitBlt clipRect			rule: Form blend			fillColor: nil	].	^ true! !!DisplayScanner methodsFor: 'stop conditions' stamp: 'jmv 9/23/2010 12:16'!endOfRun	"The end of a run in the display case either means that there is actually 	a change in the style (run code) to be associated with the string or the 	end of this line has been reached."	| runLength |	lastIndex = line last ifTrue: [^true].	runX _ destX.	runLength _ text runLengthFor: (lastIndex _ lastIndex + 1).	runStopIndex _ lastIndex + (runLength - 1) min: line last.	self setStopConditions.	self placeEmbeddedObject.	^ false! !!Morph methodsFor: 'text-anchor' stamp: 'jmv 9/23/2010 13:16'!textAnchorType	^ self		valueOfProperty: #textAnchorType		ifAbsent: [			#inline			"#document" ].! !!RunArray methodsFor: 'accessing' stamp: 'jmv 9/23/2010 12:44'!canJoinBlock: aTwoArgBlock	canJoinBlock _ aTwoArgBlock! !!RunArray methodsFor: 'adding' stamp: 'jmv 9/23/2010 12:27'!addFirst: value	"Add value as the first element of the receiver."	lastIndex _ nil.  "flush access cache"	(runs size ~= 0 and: [ self canJoin: values first and: value ])		ifTrue: [			runs at: 1 put: runs first + 1]		ifFalse: [			runs _ {1}, runs.			values _ {value}, values]! !!RunArray methodsFor: 'adding' stamp: 'jmv 9/23/2010 12:30'!addLast: value	"Add value as the last element of the receiver."	lastIndex _ nil.		"flush access cache"	(runs size ~= 0 and: [ self canJoin: values last and: value ])		ifTrue: [			runs at: runs size put: runs last + 1 ]		ifFalse: [			runs _ runs copyWith: 1.			values _ values copyWith: value ]! !!RunArray methodsFor: 'adding' stamp: 'jmv 9/23/2010 12:30'!addLast: value  times: times	"Add value as the last element of the receiver, the given number of times"	times = 0 ifTrue: [ ^self ].	lastIndex _ nil.  "flush access cache"	(runs size ~= 0 and: [ self canJoin: values last and: value ])		ifTrue: [			runs at: runs size put: runs last + times ]		ifFalse: [			runs _ runs copyWith: times.			values _ values copyWith: value ]! !!RunArray methodsFor: 'adding' stamp: 'jmv 9/23/2010 12:32'!coalesce	"Try to combine adjacent runs"	| ind |	ind _ 2.	[ ind > values size ] whileFalse: [		(self canJoin: (values at: ind-1) and: (values at: ind))			ifFalse: [ ind _ ind + 1 ]			ifTrue: [		"two are the same, combine them"				values _ values copyReplaceFrom: ind to: ind with: #().				runs at: ind-1 put: (runs at: ind-1) + (runs at: ind).				runs _ runs copyReplaceFrom: ind to: ind with: #().				"self error: 'needed to combine runs' "]]! !!RunArray methodsFor: 'copying' stamp: 'jmv 9/23/2010 12:58'!, aRunArray 	"Answer a new RunArray that is a concatenation of the receiver and	aRunArray."	| new newRuns answer |	(aRunArray isMemberOf: RunArray)		ifFalse: [			new _ self copy.			"attempt to be sociable"			aRunArray do: [:each | new addLast: each].			^new].	runs size = 0 ifTrue: [^aRunArray copy].	aRunArray runs size = 0 ifTrue: [^self copy].	(self canJoin: (values at: values size) and: (aRunArray values at: 1))		ifFalse: [ 			answer _ RunArray				runs: runs , aRunArray runs				values: values , aRunArray values.			answer canJoinBlock: canJoinBlock.			^answer ].	newRuns _ runs					copyReplaceFrom: runs size					to: runs size					with: aRunArray runs.	newRuns at: runs size put: (runs at: runs size) + (aRunArray runs at: 1).	answer _ RunArray		runs: newRuns		values: 			(values				copyReplaceFrom: values size				to: values size				with: aRunArray values).	answer canJoinBlock: canJoinBlock.	^answer! !!RunArray methodsFor: 'copying' stamp: 'jmv 9/23/2010 12:59'!copyFrom: start to: stop	| newRuns run1 run2 offset1 offset2 answer | 	stop < start ifTrue: [		answer _ RunArray new.		answer canJoinBlock: canJoinBlock.		^answer ].	self at: start setRunOffsetAndValue: [ :r :o :value1 |		run1 _ r.		offset1_ o. 		value1 ].	self at: stop setRunOffsetAndValue: [ :r :o :value2 |		run2 _ r.		offset2_ o.		value2].	run1 = run2		ifTrue: [			newRuns _ Array with: offset2 - offset1 + 1]		ifFalse: [			newRuns _ runs copyFrom: run1 to: run2.			newRuns at: 1 put: (newRuns at: 1) - offset1.			newRuns at: newRuns size put: offset2 + 1 ].	answer _ RunArray runs: newRuns values: (values copyFrom: run1 to: run2).	answer canJoinBlock: canJoinBlock.	^answer! !!RunArray methodsFor: 'private' stamp: 'jmv 9/23/2010 12:43'!canJoin: aValue and: anotherValue	^ canJoinBlock		ifNil: [ aValue = anotherValue ]		ifNotNil: [			canJoinBlock				value: aValue				value: anotherValue ].! !!Text methodsFor: 'accessing' stamp: 'jmv 9/24/2010 08:20'!replaceFrom: start to: stop with: replacement	"newSize = oldSize - (stop-start-1) + aText size"		| rep |	rep _ replacement asText.	"might be a string"	string _ string copyReplaceFrom: start to: stop with: rep string.	self privateSetRuns: (runs copyReplaceFrom: start to: stop with: rep runs).	"Ensure the ParagraphAttributes invariant for the interval that could have been affected"	self fixParagraphAttributesFrom: start to: start + replacement size - 1! !!Text methodsFor: 'converting' stamp: 'jmv 9/24/2010 08:19'!replaceFrom: start to: stop with: replacement startingAt: repStart 	"This destructively replaces elements from start to stop in the receiver starting at index, repStart, in replacementCollection. 	Do it to both the string and the runs.	The size does not change"	| rep newRepRuns |	rep _ replacement asText.	"might be a string"	string replaceFrom: start to: stop with: rep string startingAt: repStart.	newRepRuns _ rep runs copyFrom: repStart to: repStart + stop - start.	self privateSetRuns: (runs copyReplaceFrom: start to: stop with: newRepRuns).	"Ensure the ParagraphAttributes invariant for the interval that could have been affected"	self fixParagraphAttributesFrom: start to: start + replacement size - 1! !!Text methodsFor: 'converting' stamp: 'jmv 9/24/2010 08:19'!replaceFrom: start to: stop withString: replacementString attributes: attributesArray startingAt: repStart 	"This destructively replaces elements from start to stop in the receiver starting at index, repStart, in replacementCollection. 	Do it to both the string and the runs.	The size does not change"	| newRepRuns |	string replaceFrom: start to: stop with: replacementString startingAt: repStart.	newRepRuns _ RunArray new: stop-start+1 withAll: attributesArray.	self privateSetRuns: (runs copyReplaceFrom: start to: stop with: newRepRuns).	"Ensure the ParagraphAttributes invariant for the interval that could have been affected"	self fixParagraphAttributesFrom: start to: start + replacementString size - 1! !!Text methodsFor: 'emphasis' stamp: 'jmv 9/24/2010 08:19'!addAttribute: att from: requestedStart to: requestedStop	"Set the attribute for characters in the interval start to stop."	| intervalToFix start stop |	start _ requestedStart.	stop _ requestedStop.		"If att must be applied to whole paragraphs, do so."	att isParagraphAttribute ifTrue: [		intervalToFix _ self encompassParagraph: (start to: stop).		start _ intervalToFix first.		stop _ intervalToFix last ].	self privateSetRuns: (runs		copyReplaceFrom: start		to: stop		with: ((runs copyFrom: start to: stop) mapValues: [ :attributes | 			Text addAttribute: att toArray: attributes])).	runs coalesce! !!Text methodsFor: 'emphasis' stamp: 'jmv 9/24/2010 08:18'!removeAttribute: att from: requestedStart to: requestedStop	"Remove the attribute over the interval start to stop."	| intervalToFix start stop |	start _ requestedStart.	stop _ requestedStop.		"If att must be applied to whole paragraphs, do so."	att isParagraphAttribute ifTrue: [		intervalToFix _ self encompassParagraph: (start to: stop).		start _ intervalToFix first.		stop _ intervalToFix last ].	self privateSetRuns: (runs		copyReplaceFrom: start		to: stop		with: ((runs copyFrom: start to: stop) mapValues: [ :attributes | 			attributes copyWithout: att])).	runs coalesce! !!Text methodsFor: 'private' stamp: 'jmv 9/24/2010 08:20'!privateSetParagraphAttributes: paragraphAttributes from: start to: stop	self privateSetRuns: (runs		copyReplaceFrom: start		to: stop		with: ((runs copyFrom: start to: stop) mapValues: [ :attributes |			Text setParagraphAttributes: paragraphAttributes toArray: attributes]))! !!Text methodsFor: 'private' stamp: 'jmv 9/24/2010 08:55'!privateSetRuns: anArray	"Warning. No attempt is done to ensure the invariant that TextAttributes that answer true to	 #isParagraphAttribute are only applied to whole paragraphs.	Use with care. Currently only used for Shout, that seems to know what it does.	Also used for private use, replacing asignment to the ivar, to ensure that the RunArray is set to properly compare TextAttributes"	runs _ anArray.	runs canJoinBlock: CanJoinBlock! !!Text methodsFor: 'private' stamp: 'jmv 9/24/2010 08:19'!setInitialStyle: aTextStyle setString: aString setRuns: anArray	"Warning. No attempt is done to ensure the invariant that TextAttributes that answer true to	 #isParagraphAttribute are only applied to whole paragraphs.	Use with care. "	initialStyle _ aTextStyle.	string _ aString.	self privateSetRuns: anArray! !!Text class methodsFor: 'class initialization' stamp: 'jmv 9/24/2010 08:55'!initialize	"	Text initialize	"	CanJoinBlock _ [ :attributes1 :attributes2 | | s |		s _ attributes1 size.		s = attributes2 size and: [			(1 to: s) allSatisfy: [ :i |				(attributes1 at: i) canBeJoinedWith: (attributes2 at: i) ]]]! !!TextAttribute methodsFor: 'testing' stamp: 'jmv 9/22/2010 23:32'!canBeJoinedWith: aTextAttribute	"To be used for RunArray compaction"	^self = aTextAttribute! !!TextAnchor methodsFor: 'testing' stamp: 'jmv 9/22/2010 23:33'!canBeJoinedWith: aTextAttribute	"Never join in RunArray. Not even when self == aTextAttribute!!"	^false! !TextAnchor removeSelector: #=!TextAnchor removeSelector: #hash!Text initialize!!classDefinition: #Text category: #'System-Text'!ArrayedCollection subclass: #Text	instanceVariableNames: 'string runs initialStyle'	classVariableNames: 'CanJoinBlock'	poolDictionaries: ''	category: 'System-Text'!RunArray removeSelector: #beForTextAttributes!RunArray removeSelector: #canJoinSelector:!!classDefinition: #RunArray category: #'Collections-Arrayed'!ArrayedCollection subclass: #RunArray	instanceVariableNames: 'runs values lastIndex lastRun lastOffset canJoinBlock'	classVariableNames: ''	poolDictionaries: ''	category: 'Collections-Arrayed'!CharacterScanner initialize!CharacterScanner removeSelector: #embeddedObject!"Postscript:Leave the line above, and replace the rest of this comment by a useful one.Executable statements should follow this comment, and shouldbe separated by periods, with no exclamation points (!!).Be sure to put any further comments in double-quotes, like this one."CharacterScanner initialize!