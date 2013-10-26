'From Cuis 1.0 of 4 September 2009 [latest update: #290] on 18 September 2009 at 10:54:04 am'!!classDefinition: #TextMorph category: #'Morphic-Text Support'!BorderedMorph subclass: #TextMorph	instanceVariableNames: 'text wrapFlag paragraph editor container backgroundColor margins '	classVariableNames: ''	poolDictionaries: ''	category: 'Morphic-Text Support'!!ImageSegment methodsFor: 'testing' stamp: 'jmv 9/12/2009 23:03'!findRogueRootsPrep	"Part of the tool to track down unwanted pointers into the segment.  Break all owner pointers in submorphs, scripts, and viewers in flaps."| wld morphs |wld _ arrayOfRoots detect: [:obj | 	obj isMorph ifTrue: [obj isWorldMorph] ifFalse: [false]] ifNone: [nil].wld ifNil: [wld _ arrayOfRoots detect: [:obj | obj isMorph] 				ifNone: [^ self error: 'can''t find a root morph']].morphs _ IdentitySet new: 400.wld allMorphsDo: [ :m |  morphs add: m ].morphs do: [:mm | 	"break the back pointers"	mm isInMemory ifTrue: [	(mm respondsTo: #target) ifTrue: [		mm nearestOwnerThat: [:ow | ow == mm target 			ifTrue: [mm target: nil. true]			ifFalse: [false]]].	(mm respondsTo: #arguments) ifTrue: [		mm arguments do: [:arg | arg ifNotNil: [			mm nearestOwnerThat: [:ow | ow == arg				ifTrue: [mm arguments at: (mm arguments indexOf: arg) put: nil. true]				ifFalse: [false]]]]].	mm eventHandler ifNotNil: ["recipients point back up"		(morphs includesAllOf: (mm eventHandler allRecipients)) ifTrue: [			mm eventHandler: nil]].	"temporary, until using Model for PartsBin"	(mm isMorphicModel) ifTrue: [		(mm model isMorphicModel) ifTrue: [			mm model breakDependents]]]].(Smalltalk includesKey: #Owners) ifTrue: [Smalltalk at: #Owners put: nil].	"in case findOwnerMap: is commented out""self findOwnerMap: morphs."morphs do: [:mm | 	"break the back pointers"	mm isInMemory ifTrue: [mm privateOwner: nil]]."more in extensions?"! !!Morph methodsFor: 'accessing' stamp: 'dgd 3/7/2003 15:24'!raisedColor	"Return the color to be used for shading raised borders. The 	default is my own color, but it might want to be, eg, my 	owner's color. Whoever's color ends up prevailing, the color 	itself gets the last chance to determine, so that when, for 	example, an InfiniteForm serves as the color, callers won't choke 	on some non-Color object being returned"	(color isColor			and: [color isTransparent					and: [owner notNil]])		ifTrue: [^ owner raisedColor].	^ color asColor raisedColor! !!Morph methodsFor: 'drawing' stamp: 'jmv 9/12/2009 21:10'!clipSubmorphs: aBoolean	"Drawing specific. If this property is set, clip the receiver's submorphs to the receiver's clipping bounds."	self invalidRect: self fullBounds.	aBoolean		ifTrue: [ self setProperty: #clipSubmorphs toValue: true ]		ifFalse: [ self removeProperty: #clipSubmorphs ].	self invalidRect: self fullBounds! !!Morph methodsFor: 'layout' stamp: 'jmv 9/12/2009 20:18'!computeFullBounds	"Private. Compute the actual full bounds of the receiver"	(submorphs isEmpty or: [ self clipSubmorphs ]) ifTrue: [ ^self bounds ].	^ self bounds quickMerge: self submorphBounds! !!Morph methodsFor: 'layout' stamp: 'jmv 9/12/2009 23:13'!doLayoutIn: layoutBounds 	"Compute a new layout based on the given layout bounds."	"Note: Testing for #bounds or #layoutBounds would be sufficient to	figure out if we need an invalidation afterwards but #bounds	is what we need for all leaf nodes so we use that."	submorphs isEmpty ifTrue: [ ^fullBounds _ bounds].	self submorphsDo: [ :m | m layoutProportionallyIn: layoutBounds ].	fullBounds _ self computeFullBounds! !!Morph methodsFor: 'layout' stamp: 'jmv 9/12/2009 19:21'!submorphBounds	"Private. Compute the actual full bounds of the receiver"	^submorphs inject: nil into: [ :prevBox :m |		m visible			ifTrue: [ m fullBounds quickMerge: prevBox ]			ifFalse: [ prevBox ] ]! !!Morph methodsFor: 'private' stamp: 'jmv 9/14/2009 17:40'!privateMoveBy: delta 	"Private!! Use 'position:' instead."	bounds _ bounds translateBy: delta.	fullBounds		ifNotNil: [fullBounds _ fullBounds translateBy: delta]! !!BorderedMorph methodsFor: 'menu' stamp: 'jmv 9/12/2009 23:23'!changeBorderWidth: evt 	| handle origin aHand newWidth |	aHand := evt ifNil: [self primaryHand] ifNotNil: [evt hand].	origin := aHand position.	handle := HandleMorph new forEachPointDo: 					[:newPoint | 					handle removeAllMorphs.					handle addMorph: (LineMorph 								from: origin								to: newPoint								color: Color black								width: 1).					newWidth := (newPoint - origin) r asInteger // 5.					self borderWidth: newWidth]				lastPointDo: 					[:newPoint | 					handle deleteBalloon.					self halo ifNotNil: [ :halo | halo addHandles]].	aHand attachMorph: handle.	handle 		showBalloon: 'Move cursor farther fromthis point to increase border width.Click when done.'		hand: evt hand.	handle startStepping! !!BalloonMorph class methodsFor: 'instance creation' stamp: 'jmv 9/12/2009 22:22'!string: str for: morph corner: cornerName 	"Make up and return a balloon for morph. Find the quadrant that 	clips the text the least, using cornerName as a tie-breaker. tk 9/12/97"	| tm vertices instance |	tm _ self getTextMorph: str for: morph.	vertices _ self getVertices: tm bounds.	vertices _ self				getBestLocation: vertices				for: morph				corner: cornerName.	instance _ self new		color: morph balloonColor;		 setVertices: vertices;		 addMorph: tm;		 setTarget: morph.	tm center: instance adjustedCenter.	^instance! !!BalloonMorph class methodsFor: 'private' stamp: 'jmv 9/12/2009 22:24'!getTextMorph: aStringOrMorph for: balloonOwner	"Construct text morph."	| m text fontToUse |	aStringOrMorph isMorph		ifTrue: [ m _ aStringOrMorph ]		ifFalse: [			text _ Text string: aStringOrMorph attribute: TextAlignment centered.			(fontToUse _ balloonOwner balloonFont)				ifNotNil: [					text initialFont: fontToUse ].			m _ TextMorph new contents: text].	^ m! !!ColorPickerMorph methodsFor: 'initialization' stamp: 'jmv 9/12/2009 23:22'!initializeModal: beModal 	"Initialize the receiver.  If beModal is true, it will be a modal color picker, else not"	isModal := beModal.	self removeAllMorphs.	isModal ifFalse: [		self addMorph: (SimpleButtonMorph new			borderWidth: 0;			label: 'x' font: nil;			color: Color transparent;			actionSelector: #delete;			target: self;			position: self topLeft - (0 @ 1);			extent: 10 @ 12;			setBalloonText: 'dismiss color picker')].	self addMorph: ((Morph newBounds: (DragBox translateBy: self topLeft))				color: Color transparent;				setBalloonText: 'put me somewhere').	self 		addMorph: ((Morph newBounds: (RevertBox translateBy: self topLeft))				color: Color transparent;				setBalloonText: 'restore original color').	self 		addMorph: ((Morph newBounds: (FeedbackBox translateBy: self topLeft))				color: Color transparent;				setBalloonText: 'shows selected color').	self 		addMorph: ((Morph newBounds: (TransparentBox translateBy: self topLeft))				color: Color transparent;				setBalloonText: 'adjust translucency').	self buildChartForm.	selectedColor ifNil: [selectedColor := Color white].	sourceHand := nil.	deleteOnMouseUp := false.	updateContinuously := true! !!Preferences class methodsFor: 'reacting to change' stamp: 'jmv 9/14/2009 18:27'!setNotificationParametersForStandardPreferences	"Set up the notification parameters for the standard preferences that require need them.  When adding new Preferences that require use of the notification mechanism, users declare the notifcation info as part of the call that adds the preference, or afterwards -- the two relevant methods for doing that are: 	Preferences.addPreference:categories:default:balloonHelp:projectLocal:changeInformee:changeSelector:   and	Preference changeInformee:changeSelector:"		"Preferences setNotificationParametersForStandardPreferences"	| aPreference |	#(		(smartUpdating			smartUpdatingChanged)	)  do:			[:pair |				aPreference _ self preferenceAt: pair first.				aPreference changeInformee: self changeSelector: pair second]! !!Rectangle methodsFor: 'rectangle functions' stamp: 'jmv 9/12/2009 19:22'!quickMerge: aRectangle 	"Answer the receiver if it encloses the given rectangle or the merge of the two rectangles if it doesn't. THis method is an optimization to reduce extra rectangle creations."	| useRcvr rOrigin rCorner minX maxX minY maxY |	aRectangle ifNil: [ ^self ].	useRcvr _ true.	rOrigin _ aRectangle topLeft.	rCorner _ aRectangle bottomRight.	minX _ rOrigin x < origin x ifTrue: [ useRcvr _ false. rOrigin x ] ifFalse: [ origin x ].	maxX _ rCorner x > corner x ifTrue: [ useRcvr _ false. rCorner x ] ifFalse: [ corner x ].	minY _ rOrigin y < origin y ifTrue: [ useRcvr _ false. rOrigin y ] ifFalse: [ origin y ].	maxY _ rCorner y > corner y ifTrue:  [useRcvr _ false. rCorner y ] ifFalse: [ corner y ].	^useRcvr		ifTrue: [ self ]		ifFalse: [ Rectangle origin: minX@minY corner: maxX@maxY ].! !!StringMorph methodsFor: 'accessing' stamp: 'jmv 9/14/2009 17:37'!contents: newContents	contents _ newContents isText		ifTrue: [			emphasis _ newContents emphasisAt: 1.			 newContents string ]		ifFalse: [			contents = newContents ifTrue: [ ^self ].	"no substantive change"			newContents].	self fitContents! !!StringMorph methodsFor: 'initialization' stamp: 'jmv 9/12/2009 21:08'!initialize	super initialize.	font _ nil.	emphasis _ 0.	hasFocus _ false.	self contents: 'String Morph'! !!SystemWindow methodsFor: 'drawing' stamp: 'ar 8/16/2001 12:47'!raisedColor	^self paneColor raisedColor! !!SystemWindow methodsFor: 'top window' stamp: 'jmv 9/12/2009 22:26'!activateAndSendTopToBack: aBoolean	"Bring me to the front and make me able to respond to mouse and keyboard"	| oldTop |	self owner 		ifNil: [^self	"avoid spurious activate when drop in trash"].	oldTop := TopWindow.	TopWindow := self.	oldTop ifNotNil: [		oldTop passivate.		aBoolean ifTrue: [			oldTop owner addMorphBack: oldTop ]].	self owner firstSubmorph == self 		ifFalse: [			"Bring me (with any flex) to the top if not already"			self owner addMorphFront: self].	self invalidateTitleArea.	self isCollapsed 		ifFalse: [			model modelWakeUpIn: self]! !!TextMorph methodsFor: 'accessing' stamp: 'jmv 9/12/2009 23:16'!contentsAsIs: stringOrText	"Accept new text contents with line breaks only as in the text.	Fit my width and height to the result."	wrapFlag _ false.	self newContents: stringOrText! !!TextMorph methodsFor: 'copying' stamp: 'jmv 9/12/2009 23:23'!veryDeepInner: deepCopier 	"Copy all of my instance variables. Some need to be not copied at all, but shared.	Warning!!!! Every instance variable defined in this class must be handled.	We must also implement veryDeepFixupWith:.  See DeepCopier class comment."	super veryDeepInner: deepCopier.	text _ text veryDeepCopyWith: deepCopier.	wrapFlag _ wrapFlag veryDeepCopyWith: deepCopier.	paragraph _ paragraph veryDeepCopyWith: deepCopier.	editor _ editor veryDeepCopyWith: deepCopier.	backgroundColor _ backgroundColor veryDeepCopyWith: deepCopier! !!TextMorph methodsFor: 'geometry' stamp: 'jmv 9/12/2009 23:30'!container	"Return the container for composing this text.  There are 2 cases:	1.  container is nil, and wrap is true -- grow downward as necessary,	2.  container is nil, and wrap is false -- grow in 2D as necessary."	wrapFlag ifTrue: [		^ self innerBounds withHeight: 9999999].	^ self innerBounds topLeft extent: 9999999@9999999! !!TextMorph methodsFor: 'geometry' stamp: 'jmv 9/12/2009 23:07'!extent: aPoint 	| newExtent priorEditor |	priorEditor _ editor.	self isAutoFit		ifTrue: [wrapFlag ifFalse: [^ self].  "full autofit can't change"				newExtent _ aPoint truncated max: self minimumExtent.				newExtent x = self extent x ifTrue: [^ self].  "No change of wrap width"				self releaseParagraph.  "invalidate the paragraph cache"				super extent: newExtent.				priorEditor					ifNil: [self fit]  "since the width has changed..."					ifNotNil: [self installEditorToReplace: priorEditor]]		ifFalse: [super extent: (aPoint truncated max: self minimumExtent).				wrapFlag ifFalse: [^ self].  "no effect on composition"				self composeToBounds]! !!TextMorph methodsFor: 'geometry' stamp: 'jmv 9/12/2009 23:24'!minimumExtent	borderWidth ifNil: [^ 9@16].	^(9@(text initialStyle initialLineGrid+2)) + (borderWidth*2)! !!TextMorph methodsFor: 'menu' stamp: 'jmv 9/12/2009 23:19'!addCustomMenuItems: aCustomMenu hand: aHandMorph 	"Add text-related menu items to the menu"	super addCustomMenuItems: aCustomMenu hand: aHandMorph.	aCustomMenu 		addUpdating: #autoFitString		target: self		action: #autoFitOnOff.	aCustomMenu 		addUpdating: #wrapString		target: self		action: #wrapOnOff.	Preferences simpleMenus		ifFalse: [			aCustomMenu add: 'code pane menu...' translated				action: #yellowButtonActivity.			aCustomMenu add: 'code pane shift menu....' translated				action: #shiftedYellowButtonActivity]! !!TextMorph methodsFor: 'private' stamp: 'jmv 9/12/2009 23:16'!composeToBounds	"Compose my text to fit my bounds.	If any text lies outside my bounds, it will be clipped, or	if I have successors, it will be shown in the successors."	self releaseParagraph.	self paragraph positionWhenComposed: self position! !!TextMorph methodsFor: 'private' stamp: 'jmv 9/12/2009 23:24'!fit	"Adjust my bounds to fit the text.  Should be a no-op if autoFit is not specified.	Required after the text changes,	or if wrapFlag is true and the user attempts to change the extent."	| newExtent para |	self isAutoFit 		ifTrue: [			newExtent := (self paragraph extent max: 9 @ text initialStyle initialLineGrid) + (0 @ 2).			newExtent := newExtent + (2 * borderWidth).			newExtent ~= bounds extent 				ifTrue: [					para := paragraph.	"Save para (layoutChanged smashes it)"					super extent: newExtent.					paragraph := para].			].	"These statements should be pushed back into senders"	self paragraph positionWhenComposed: self position.	self changed	"Too conservative: only paragraph composition					should cause invalidation."! !!TextMorph methodsFor: 'private' stamp: 'jmv 9/12/2009 23:07'!releaseParagraph	"Paragraph instantiation is lazy -- it will be created only when needed"	self releaseEditor.	paragraph ifNotNil: [		paragraph _ nil]! !!TextMorph methodsFor: 'private' stamp: 'jmv 9/12/2009 23:03'!text: t wrap: wrap color: c	"Private -- for use only in morphic duplication"	text _ t.	wrapFlag _ wrap.	color _ c.	paragraph _ editor _ nil! !!TransformMorph methodsFor: 'layout' stamp: 'jmv 9/12/2009 23:08'!fullBounds	"Overridden to clip submorph hit detection to my bounds."	"It might be better to override doLayoutIn:, and remove this method"	fullBounds ifNotNil: [ ^fullBounds ].	fullBounds _ bounds.	^fullBounds! !!TransformMorph methodsFor: 'layout' stamp: 'jmv 9/12/2009 20:11'!submorphBounds	"Answer, in owner coordinates, the bounds of my visible submorphs, or my bounds"	| box |	box _ super submorphBounds.	^box ifNotNil: [ (transform localBoundsToGlobal: box) truncated ]! !TransformMorph removeSelector: #angle!TransformMorph removeSelector: #localVisibleSubmorphBounds!TransformMorph removeSelector: #scale!TextMorph removeSelector: #bounds!TextMorph removeSelector: #changeMargins:!TextMorph removeSelector: #compositionRectangle!TextMorph removeSelector: #containsPoint:!TextMorph removeSelector: #fillingOnOff!TextMorph removeSelector: #goBehind!TextMorph removeSelector: #margins!TextMorph removeSelector: #margins:!TextMorph removeSelector: #occlusionsOnOff!TextMorph removeSelector: #ownerChanged!TextMorph removeSelector: #privateOwner:!TextMorph removeSelector: #releaseParagraphReally!TextMorph removeSelector: #setContainer:!!classDefinition: #TextMorph category: #'Morphic-Text Support'!BorderedMorph subclass: #TextMorph	instanceVariableNames: 'text wrapFlag paragraph editor backgroundColor'	classVariableNames: ''	poolDictionaries: ''	category: 'Morphic-Text Support'!SystemWindow removeSelector: #positionSubmorphs!Symbol removeSelector: #isOrientedFill!StringMorph removeSelector: #fullBounds!Preferences class removeSelector: #annotationPanesChanged!Preferences class removeSelector: #optionalButtonsChanged!Pen class removeSelector: #feltTip:cellSize:!MorphicTransform removeSelector: #angle!MorphicTransform removeSelector: #asMatrixTransform2x3!MorphicTransform removeSelector: #composedWith:!MorphicTransform removeSelector: #invertRect:!MorphicTransform removeSelector: #scale!MorphicTransform removeSelector: #setIdentiy!MorphicTransform removeSelector: #transformBoundsRect:!Morph removeSelector: #adhereToEdge:!Morph removeSelector: #adjustedCenter!Morph removeSelector: #adjustedCenter:!Morph removeSelector: #ownerChanged!Morph removeSelector: #positionSubmorphs!Morph removeSelector: #privateFullBounds!Morph removeSelector: #setCenteredBalloonText:!Morph removeSelector: #setToAdhereToEdge:!Morph removeSelector: #snapToEdgeIfAppropriate!LightWidget removeSelector: #ownerChanged!InfiniteForm removeSelector: #isOrientedFill!InfiniteForm removeSelector: #origin:!IdentityTransform removeSelector: #angle!IdentityTransform removeSelector: #asMatrixTransform2x3!IdentityTransform removeSelector: #composedWith:!IdentityTransform removeSelector: #composedWithGlobal:!FormCanvas removeSelector: #privateWarp:transform:at:sourceRect:cellSize:!FormCanvas removeSelector: #warpImage:transform:at:sourceRect:cellSize:!ColorForm removeSelector: #scaledToSize:!Form removeSelector: #displayInterpolatedIn:on:!Form removeSelector: #displayInterpolatedOn:!Form removeSelector: #displayScaledOn:!Form removeSelector: #isBltAccelerated:for:!Form removeSelector: #isFillAccelerated:for:!Form removeSelector: #rotateBy:centerAt:!Form removeSelector: #rotateBy:magnify:smoothing:!Form removeSelector: #scaledToSize:!Envelope removeSelector: #scale!CompositeTransform removeSelector: #angle!CompositeTransform removeSelector: #asCompositeTransform!CompositeTransform removeSelector: #asMatrixTransform2x3!CompositeTransform removeSelector: #composedWith:!CompositeTransform removeSelector: #isCompositeTransform!CompositeTransform removeSelector: #scale!DisplayTransform removeSelector: #asCompositeTransform!DisplayTransform removeSelector: #asMatrixTransform2x3!DisplayTransform removeSelector: #composedWithGlobal:!DisplayTransform removeSelector: #isCompositeTransform!DisplayTransform removeSelector: #isMatrixTransform2x3!Color removeSelector: #isOrientedFill!Canvas removeSelector: #image:at:rule:!Canvas removeSelector: #imageWithOpaqueWhite:at:!Canvas removeSelector: #transform2By:clippingTo:during:smoothing:!Canvas removeSelector: #transformBy:clippingTo:during:!Canvas removeSelector: #warpImage:transform:!Canvas removeSelector: #warpImage:transform:at:!Canvas removeSelector: #warpImage:transform:at:sourceRect:cellSize:!BitBlt class removeSelector: #antiAliasDemo!BitBlt class removeSelector: #benchmark!BitBlt class removeSelector: #benchmark2!BitBlt class removeSelector: #benchmark3!Smalltalk removeClassNamed: #MatrixTransform2x3!Smalltalk removeClassNamed: #TextContainer!"Postscript:Leave the line above, and replace the rest of this comment by a useful one.Executable statements should follow this comment, and shouldbe separated by periods, with no exclamation points (!!).Be sure to put any further comments in double-quotes, like this one."Preferences setNotificationParametersForStandardPreferences!