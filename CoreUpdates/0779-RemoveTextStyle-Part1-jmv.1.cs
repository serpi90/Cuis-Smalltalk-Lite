'From Cuis 3.0 of 18 January 2011 [latest update: #768] on 24 January 2011 at 3:28:05 pm'!!classDefinition: #PopUpMenu category: #'Tools-Menus'!Object subclass: #PopUpMenu	instanceVariableNames: 'labelString lineArray '	classVariableNames: 'MenuStyle '	poolDictionaries: ''	category: 'Tools-Menus'!!CharacterScanner methodsFor: 'stop conditions' stamp: 'jmv 3/21/2009 15:26'!tab	^self subclassResponsibility! !!CharacterScanner methodsFor: 'stop conditions' stamp: 'jmv 1/24/2011 15:22'!tabDestX	"This is the basic method of adjusting destX for a tab."	^actualTextStyle		ifNotNil: [			actualTextStyle				nextTabXFrom: destX				leftMargin: leftMargin				rightMargin: rightMargin ]		ifNil: [			destX + 24 min: rightMargin ]! !!CharacterBlockScanner methodsFor: 'stop conditions' stamp: 'jmv 1/24/2011 15:24'!tab	| currentX |	currentX _ self tabDestX.	lastSpaceOrTabExtent _ lastCharacterExtent copy.	self lastSpaceOrTabExtentSetX: (currentX - destX max: 0).	currentX >= characterPoint x		ifTrue: 			[lastCharacterExtent _ lastSpaceOrTabExtent copy.			^ self crossedX].	destX _ currentX.	lastIndex _ lastIndex + 1.	^false! !!CompositionScanner methodsFor: 'scanning' stamp: 'jmv 1/24/2011 15:17'!composeFrom: startIndex inRectangle: lineRectangle firstLine: firstLine leftSide: leftSide rightSide: rightSide	"Answer an instance of TextLineInterval that represents the next line in the paragraph."	| runLength done stopCondition xtraSpaceBefore spaceAfterParagraph |		lastIndex _ startIndex.	"scanning sets last index"	destY _ lineRectangle top.	lineHeight _ baseline _ 0.  "Will be increased by setFont"	self setStopConditions.	"also sets font, style, etc"	"Set up margins"	leftMargin _ lineRectangle left.	rightMargin _ lineRectangle right.	xtraSpaceBefore _ 0.	spaceAfterParagraph _ 0.	actualTextStyle ifNotNil: [		leftSide ifTrue: [			leftMargin _ leftMargin +				((firstLine and: [ actualTextStyle isListStyle not ])					ifTrue: [ actualTextStyle firstIndent ]					ifFalse: [ actualTextStyle restIndent ])].		rightSide ifTrue: [			rightMargin _ rightMargin - actualTextStyle rightIndent].		firstLine ifTrue: [ xtraSpaceBefore _ actualTextStyle paragraphSpacingBefore ].		spaceAfterParagraph _ actualTextStyle paragraphSpacingAfter ].	destX _ spaceX _ leftMargin.	runLength _ text runLengthFor: startIndex.	runStopIndex _ (lastIndex _ startIndex) + (runLength - 1).	line _ (TextLine start: lastIndex stop: 0 internalSpaces: 0 paddingWidth: 0)				rectangle: lineRectangle.	line isFirstLine: firstLine.	spaceCount _ 0.	leftMargin _ destX.	line leftMargin: leftMargin.	done _ false.	self placeEmbeddedObject.	[ done ]		whileFalse: [			stopCondition _ self scanCharactersFrom: lastIndex to: runStopIndex				in: text string rightX: rightMargin stopConditions: stopConditions				kern: kern.			"See setStopConditions for stopping conditions for composing."			(self perform: stopCondition) ifTrue: [				^ line 					lineHeight: lineHeight + xtraSpaceBefore + 						(stopCondition = #cr ifTrue: [spaceAfterParagraph] ifFalse: [0]) 					baseline: baseline + xtraSpaceBefore ]]! !!CompositionScanner methodsFor: 'stop conditions' stamp: 'jmv 1/24/2011 15:24'!tab	"Advance destination x according to tab settings in the current	textStyle. Answer whether the character has crossed the right edge of 	the composition rectangle of the paragraph."	destX _ self tabDestX.	destX > rightMargin ifTrue:	[^self crossedX].	lastIndex _ lastIndex + 1.	^false! !!DisplayScanner methodsFor: 'scanning' stamp: 'jmv 1/24/2011 15:26'!displayBulletIfAppropriateFor: textLine offset: offset	| paragraphEnd count pattern |	actualTextStyle ifNotNil: [		(textLine isFirstLine and: [ actualTextStyle isListStyle ]) ifTrue: [			pattern _ actualTextStyle listBulletPattern.			"Count how many paragraphs before this one already used the pattern"			count _ 0.			paragraphEnd _ textLine first-1.			[			paragraphEnd > 0 and: [ (text textStyleAt: paragraphEnd) listBulletPattern = pattern ]] whileTrue: [				count _ count + 1.				paragraphEnd _ text string endOfParagraphBefore: paragraphEnd ].			"Our number in the list, is one more than the count of previous contiguous paragraphs with this pattern"			self				displayBulletOffset: offset				number: count + 1]]! !!DisplayScanner methodsFor: 'stop conditions' stamp: 'jmv 1/24/2011 15:23'!tab	destX _ self tabDestX.	lastIndex _ lastIndex + 1.	^ false! !!LightWidget methodsFor: 'halos and balloon help' stamp: 'jmv 1/24/2011 15:04'!balloonHelpTextForHandle: aHandle	"Answer a string providing balloon help for the given halo handle"	|  itsSelector |	itsSelector _ aHandle eventHandler firstMouseSelector.	#(		(chooseEmphasisOrAlignment			'Emphasis & alignment')		(chooseFont								'Change font')		(dismiss									'Remove')		(doDebug:with:							'Debug')		(doDirection:with:						'Choose forward direction')		(doDup:with:								'Duplicate')		(doMenu:with:							'Menu')		(doGrab:with:								'Pick up')		(doRecolor:with:							'Change color')		(maybeDoDup:with:						'Duplicate')		(mouseDownInCollapseHandle:with:	'Collapse')		(mouseDownOnHelpHandle:			'Help')		(startDrag:with:							'Move')		(startGrow:with:							'Change size') 		(startRot:with:							'Rotate')) 	do: [ :pair |		itsSelector == pair first ifTrue: [^ pair last]].	(itsSelector == #mouseDownInDimissHandle:with:) ifTrue: [		'Remove from screen'].	^ 'unknown halo handle'! !!Morph methodsFor: 'halos and balloon help' stamp: 'jmv 1/24/2011 15:04'!balloonHelpTextForHandle: aHandle	"Answer a string providing balloon help for the given halo handle"	|  itsSelector |	itsSelector _ aHandle eventHandler firstMouseSelector.	#(		(chooseEmphasisOrAlignment			'Emphasis & alignment')		(chooseFont								'Change font')		(dismiss									'Remove')		(doDebug:with:							'Debug')		(doDirection:with:						'Choose forward direction')		(doDup:with:								'Duplicate')		(doMenu:with:							'Menu')		(doGrab:with:								'Pick up')		(doRecolor:with:							'Change color')		(maybeDoDup:with:						'Duplicate')		(mouseDownInCollapseHandle:with:	'Collapse')		(mouseDownOnHelpHandle:			'Help')		(startDrag:with:							'Move')		(startGrow:with:							'Change size') 		(startRot:with:							'Rotate')) 	do:		[ :pair | itsSelector == pair first ifTrue: [^ pair last]].	(itsSelector == #mouseDownInDimissHandle:with:) ifTrue: [			'Remove from screen'].	^ 'unknown halo handle'! !!PopUpMenu methodsFor: 'accessing' stamp: 'jmv 1/24/2011 14:54'!frameHeight	"Designed to avoid the entire frame computation (includes MVC form),	since the menu may well end up being displayed in Morphic anyway."	| nItems |	nItems _ 1 + (labelString occurrencesOf: Character cr).	^ (nItems * Preferences standardMenuFont height) + 4 "border width"! !!PopUpMenu methodsFor: 'basic control sequence' stamp: 'jmv 1/24/2011 14:57'!startUpSegmented: segmentHeight withCaption: captionOrNil at: location allowKeyboard: aBoolean	"This menu is too big to fit comfortably on the screen.	Break it up into smaller chunks, and manage the relative indices.	Inspired by a special-case solution by Reinier van Loon.  The boolean parameter indicates whether the menu should be given keyboard focus (if in morphic)""(PopUpMenu labels: (String streamContents: [:s | 1 to: 100 do: [:i | s print: i; cr]. s skip: -1])		lines: (5 to: 100 by: 5)) startUpWithCaption: 'Give it a whirl...'."	| nLines nLinesPer allLabels from to subset subLines index |	allLabels := labelString findTokens: Character cr asString.	nLines _ allLabels size.	lineArray ifNil: [lineArray _ Array new].	nLinesPer _ segmentHeight // Preferences standardMenuFont height - 3.	from := 1.	[ true ] whileTrue:		[to := (from + nLinesPer) min: nLines.		subset := allLabels copyFrom: from to: to.		subset add: (to = nLines ifTrue: ['start over...'] ifFalse: ['more...'])			before: subset first.		subLines _ lineArray select: [:n | n >= from] thenCollect: [:n | n - (from-1) + 1].		subLines _ (Array with: 1) , subLines.		index := (PopUpMenu labels: subset asStringWithCr lines: subLines)					startUpWithCaption: captionOrNil at: location allowKeyboard: aBoolean.		index = 1			ifTrue: [from := to + 1.					from > nLines ifTrue: [ from := 1 ]]			ifFalse: [index = 0 ifTrue: [^ 0].					^ from + index - 2]]! !!PopUpMenu class methodsFor: 'class initialization' stamp: 'jmv 1/24/2011 14:59'!initialize! !!Preferences class methodsFor: 'fonts' stamp: 'jmv 1/24/2011 14:59'!setMenuFontTo: aFont	Parameters at: #standardMenuFont put: aFont! !!Preferences class methodsFor: 'fonts' stamp: 'jmv 1/24/2011 14:51'!setSystemFontTo: aFont	"Establish the default text font and style"	aFont ifNil: [^ self].	StrikeFont default: aFont! !!Preferences class methodsFor: 'halos' stamp: 'jmv 1/24/2011 15:02'!iconicHaloSpecifications	"Answer an array that characterizes the locations, colors, icons, and selectors of the halo handles that may be used in the iconic halo scheme"	"Preferences resetHaloSpecifications"^ #(	"selector						horiz				vert					color info						icon key	 ---------						------				-----------			-------------------------------		---------------"	(addCollapseHandle:		left				topCenter		(tan)							collapseIcon)	(addDebugHandle:			right				topCenter		(blue veryMuchLighter)		debugIcon)	(addDismissHandle:			left				top				(red muchLighter)			dismissIcon)	(addRotateHandle:			left				bottom			(blue)							rotateIcon)	(addMenuHandle:			leftCenter		top				(red)							menuIcon)	(addGrabHandle:				center			top				(black)							grabIcon)	(addDragHandle:				rightCenter		top				(brown)						dragIcon)	(addDupHandle:				right				top				(green)						duplicateIcon)		(addHelpHandle:				center			bottom			(lightBlue)					helpIcon)	(addGrowHandle:			right				bottom			(yellow)						scaleIcon)	(addFontSizeHandle:		leftCenter		bottom			(lightGreen)					fontSizeIcon)	(addFontEmphHandle:		rightCenter		bottom			(lightBrown darker)			fontEmphIcon)	(addRecolorHandle:			right				bottomCenter	(magenta darker)			recolorIcon))! !!SelectionMorph methodsFor: 'halos and balloon help' stamp: 'jmv 1/24/2011 15:02'!addHandlesTo: aHaloMorph box: box	| onlyThese |	aHaloMorph haloBox: box.	onlyThese _ #(addDismissHandle: addMenuHandle: addGrabHandle: addDragHandle: addDupHandle: addHelpHandle: addGrowHandle: addFontSizeHandle: addFontEmphHandle: addRecolorHandle:).	Preferences haloSpecifications do: [ :aSpec |		(onlyThese includes: aSpec addHandleSelector) ifTrue: [				aHaloMorph perform: aSpec addHandleSelector with: aSpec]].	aHaloMorph innerTarget addOptionalHandlesTo: aHaloMorph box: box! !!TextEditor class methodsFor: 'keyboard shortcut tables' stamp: 'jmv 1/24/2011 15:05'!initializeBasicCmdKeyShortcuts	"Initialize the (unshifted) command-key (or alt-key) shortcut table."	"NOTE: if you don't know what your keyboard generates, use Sensor test"	"TextEditor initialize"	| cmdMap cmds |	cmdMap := Array new: 256 withAll: #noop:.		"use temp in case of a crash"	cmdMap at: 32 + 1 put: #selectWord:.			"space bar key"			'([{''"<' do: [:char | cmdMap at: char asciiValue + 1 put: #enclose:].		"arranged in QWERTY keyboard order"	cmds _ #(		$w #backWord:		$e #exchange:		$y #swapChars:		$a #selectAll:		$f #find:		$g #findAgain:		$h #setSearchString:		$z #undo:		$x #cut:		$c #copySelection:		$v #paste:		$R	#indent:		$Y	#makeUppercase:		$U	#changeLfToCr:		$S	#search:		$D	#duplicate:		$H	#cursorTopHome:		$J	#doAgainMany:		$L	#outdent:		$Z	#makeCapitalized:		$X	#makeLowercase:		$C	#compareToClipboard:		$M	#selectCurrentTypeIn:	).	1 to: cmds size		by: 2		do: [ :i | cmdMap at: (cmds at: i) asciiValue + 1 put: (cmds at: i + 1)].			cmdActions _ cmdMap! !!TextEditor class methodsFor: 'keyboard shortcut tables' stamp: 'jmv 1/24/2011 15:06'!initializeCmdKeyShortcuts	"Initialize the (unshifted) command-key (or alt-key if not on Mac) shortcut table."	"NOTE: if you don't know what your keyboard generates, use Sensor test"	"	Editor initialize	"	| cmds |	self initializeBasicCmdKeyShortcuts.				'0123456'		do: [ :char | cmdActions at: char asciiValue + 1 put: #changeEmphasis:].		cmds := #(		$8	#offerColorMenu:		$k	#offerFontMenu:		$u	#align:	).	1 to: cmds size		by: 2		do: [ :i | cmdActions at: (cmds at: i) asciiValue + 1 put: (cmds at: i + 1)]! !!TextEditor class methodsFor: 'keyboard shortcut tables' stamp: 'jmv 1/24/2011 15:05'!initializeYellowButtonMenu	"Initialize the yellow button pop-up menu and corresponding messages."	"TextEditor initialize"	yellowButtonMenu _ SelectionMenu fromArray: {		{'find...(f)' translated.				#find}.		{'find again (g)' translated.			#findAgain}.		{'set search string (h)' translated.	#setSearchString}.		#-.		{'do again (j)' translated.			#again}.		{'undo (z)' translated.				#undo}.		#-.		{'copy (c)' translated.				#copySelection}.		{'cut (x)' translated.				#cut}.		{'paste (v)' translated.				#paste}.		{'paste...' translated.				#pasteRecent}.		#-.		{'set font... (k)' translated.			#offerFontMenu}.		{'set alignment...' translated.		#chooseAlignment}.		"		#-.		{'more...' translated.					#shiftedTextPaneMenuRequest}.		"	}! !!Transcripter methodsFor: 'accessing' stamp: 'jmv 1/24/2011 15:09'!endEntry	| c d cb |	c _ self contents.	Display extent ~= DisplayScreen actualScreenSize ifTrue: [		"Handle case of user resizing physical window"		DisplayScreen startUp.		frame _ frame intersect: Display boundingBox.		^ self clear; show: c].	para		model: (TextModel new actualContents: c asText)		in: ((frame insetBy: 4) withHeight: 9999).	para positionWhenComposed: 0@0.	d _ para extent y - frame height.	d > 0 ifTrue: [		"Scroll up to keep all contents visible"		cb _ para characterBlockAtPoint:			para compositionRectangle topLeft + (0@(d+StrikeFont default height)).		self on: (c copyFrom: cb stringIndex to: c size).		readLimit_ position_ collection size.		^ self endEntry].	Display fill: (frame insetBy: -2) fillColor: self black;			fill: frame fillColor: self white.	Display getCanvas		paragraph: para 		bounds: (0@0 extent: Display extent) 		color: Color black! !TextStyle class removeSelector: #availableTextStyleNames!TextModelMorph removeSelector: #changeStyle!TextEditor removeSelector: #changeStyle!TextEditor removeSelector: #changeStyle:!Text removeSelector: #initialStyle:!PopUpMenu initialize!PopUpMenu class removeSelector: #setMenuFontTo:!PopUpMenu removeSelector: #rescan!!classDefinition: #PopUpMenu category: #'Tools-Menus'!Object subclass: #PopUpMenu	instanceVariableNames: 'labelString lineArray'	classVariableNames: ''	poolDictionaries: ''	category: 'Tools-Menus'!HaloMorph class removeSelector: #fontStyleIcon!HaloMorph removeSelector: #addFontStyleHandle:!BareTextMorph removeSelector: #chooseStyle!CharacterScanner removeSelector: #plainTab!