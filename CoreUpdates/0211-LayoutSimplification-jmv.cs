'From Cuis 1.0 of 21 May 2009 [latest update: #204] on 28 May 2009 at 3:03:51 pm'!!AlignmentMorph methodsFor: 'initialization' stamp: 'jmv 5/28/2009 14:23'!initialize	"initialize the state of the receiver"	super initialize.	self		extent: 1@1;		clipSubmorphs: true! !!AlignmentMorph methodsFor: 'construction' stamp: 'jmv 5/28/2009 13:50'!addInProportionalRow: morphs	"Honors proportions between current width of morphs"		self addInRow: morphs widthProportionalTo: (morphs collect: [ :m | m width])! !!AlignmentMorph methodsFor: 'construction' stamp: 'jmv 5/28/2009 13:51'!addInRow: morphs	"All morphs are made equal width"		self addInRow: morphs widthProportionalTo: (morphs collect: [ :m | 1])! !!AlignmentMorph methodsFor: 'construction' stamp: 'jmv 5/28/2009 13:49'!addInRow: morphs atFractions: fractions	"Please ensure:	fractions size = (morphs size + 1)	fractions first = 0.0	fractions last = 1.0	inset can be a Number, Point or Rectangle. If it is a rectangle, 		right and bottom should be negative"	morphs doWithIndex: [ :morph :i |		self			addMorph: morph			fullFrame: (LayoutFrame				fractions: ((fractions at: i)@0 corner: (fractions at: i + 1)@1)) ]! !!AlignmentMorph methodsFor: 'construction' stamp: 'jmv 5/28/2009 13:50'!addInRow: morphs widthProportionalTo: widths	"Widths can be in any arbitrary unit. The actual widths will be proportional to them."	| fractions totalWidth prev |	totalWidth _ widths sum * 1.0.	fractions _ OrderedCollection new.	prev _ 0.0.	widths do: [ :w |		fractions add: prev.		prev _ w / totalWidth + prev ].	fractions add: 1.0.	self addInRow: morphs atFractions: fractions! !!CodeHolder methodsFor: 'controls' stamp: 'jmv 5/28/2009 14:23'!addOptionalButtonsTo: window at: fractions plus: verticalOffset	"If the receiver wishes it, add a button pane to the window, and answer the verticalOffset plus the height added"	| delta buttons divider |	self wantsOptionalButtons ifFalse: [^verticalOffset].	delta _ self defaultButtonPaneHeight.	buttons _ self optionalButtonRow 		color: (Display depth <= 8 ifTrue: [Color transparent] ifFalse: [self class windowColor duller paler]).	Preferences alternativeWindowLook ifTrue:[		buttons submorphsDo: [ :m | m borderColor: #raised].	].	divider _ BorderedSubpaneDividerMorph forBottomEdge.	Preferences alternativeWindowLook ifTrue: [		divider extent: 4@4; color: Color transparent; borderColor: #raised; borderWidth: 2.	].	window 		addMorph: buttons		fullFrame: (LayoutFrame 				fractions: fractions 				offsets: (0@verticalOffset corner: 0@(verticalOffset + delta - 1))).	window 		addMorph: divider		fullFrame: (LayoutFrame 				fractions: fractions 				offsets: (0@(verticalOffset + delta - 1) corner: 0@(verticalOffset + delta))).	^ verticalOffset + delta! !!CodeHolder methodsFor: 'controls' stamp: 'jmv 5/28/2009 14:38'!optionalButtonRow	"Answer a row of control buttons"	| row button aLabel buttons widths |	buttons _ OrderedCollection new.	widths _ OrderedCollection new.	Preferences menuButtonInToolPane ifTrue: [		buttons add: self menuButton.		widths add: 4 ].	self optionalButtonTuples do: [ :tuple | 		widths add: tuple first.		button _ PluggableButtonMorph 					on: self					getState: nil					action: tuple third.		button			clipSubmorphs: true;			onColor: Color transparent offColor: Color transparent.		aLabel := Preferences abbreviatedBrowserButtons 			ifTrue: [self abbreviatedWordingFor: tuple third]			ifFalse: [nil].		button label: (aLabel ifNil: [tuple second asString]).		tuple size > 3 ifTrue: [button setBalloonText: tuple fourth].		tuple size > 4 ifTrue: [button triggerOnMouseDown: tuple fifth].		buttons add: button ].	row _ AlignmentMorph proportional.	row addInRow: buttons widthProportionalTo: widths.	^row! !!CodeHolder methodsFor: 'diffs' stamp: 'jmv 5/28/2009 14:54'!prettyDiffButton	"Return a checkbox that lets the user decide whether prettyDiffs should be shown or not"	|  outerButton button |	outerButton _ AlignmentMorph proportional.	outerButton borderWidth: 2; borderColor: #raised.	outerButton color:  Color transparent.	button _ UpdatingThreePhaseButtonMorph checkBox.	button		target: self;		actionSelector: #togglePrettyDiffing;		getSelector: #showingPrettyDiffs.	outerButton 		addMorph: button			fullFrame: (LayoutFrame fractions: (0@0 corner: 0@1) offsets: (2@3 corner: 18@0));		addMorph: (StringMorph contents: 'prettyDiffs') lock			fullFrame: (LayoutFrame fractions: (0@0 corner: 1@1) offsets: (18@2 corner: 0@0)).	(self isKindOf: VersionsBrowser)		ifTrue:			[outerButton setBalloonText: 'If checked, then pretty-printed code differences from the previous version, if any, will be shown.']		ifFalse:			[outerButton setBalloonText: 'If checked, then pretty-printed code differences between the file-based method and the in-memory version, if any, will be shown.'].	^ outerButton! !!CodeHolder methodsFor: 'diffs' stamp: 'jmv 5/28/2009 14:54'!regularDiffButton	"Return a checkbox that lets the user decide whether regular diffs should be shown or not"	|  outerButton button |	outerButton _ AlignmentMorph proportional.	outerButton borderWidth: 2; borderColor: #raised.	outerButton color:  Color transparent.	button _ UpdatingThreePhaseButtonMorph checkBox.	button		target: self;		actionSelector: #toggleRegularDiffing;		getSelector: #showingRegularDiffs.	outerButton 		addMorph: button			fullFrame: (LayoutFrame fractions: (0@0 corner: 0@1) offsets: (2@3 corner: 18@0));		addMorph: (StringMorph contents: 'diffs') lock			fullFrame: (LayoutFrame fractions: (0@0 corner: 1@1) offsets: (18@2 corner: 0@0)).	outerButton setBalloonText: 'If checked, then code differences from the previous version, if any, will be shown.'.	^ outerButton! !!CodeHolder methodsFor: 'misc' stamp: 'jmv 5/28/2009 14:38'!menuButton	"Answer a button that brings up a menu.  Useful when adding new features, but at present is between uses"	| aButton |	aButton _ IconicButton new target: self;		clipSubmorphs: true;		labelGraphic: (ScriptingSystem formAtKey: #TinyMenu);		color: Color transparent; 		actWhen: #buttonDown;		actionSelector: #offerMenu;		yourself.	aButton setBalloonText: 'click here to get a menu with further options'.	^ aButton! !!Browser methodsFor: 'initialize-release' stamp: 'jmv 5/28/2009 13:53'!buildMorphicSwitches	| instanceSwitch commentSwitch classSwitch row aColor |	instanceSwitch := PluggableButtonMorph 				on: self				getState: #instanceMessagesIndicated				action: #indicateInstanceMessages.	instanceSwitch		label: 'instance';		askBeforeChanging: true;		borderWidth: 0.	commentSwitch := PluggableButtonMorph 				on: self				getState: #classCommentIndicated				action: #plusButtonHit.	commentSwitch		label: '?' asText allBold;		askBeforeChanging: true;		setBalloonText: 'class comment';		borderWidth: 0.	classSwitch := PluggableButtonMorph 				on: self				getState: #classMessagesIndicated				action: #indicateClassMessages.	classSwitch		label: 'class';		askBeforeChanging: true;		borderWidth: 0.	row _ AlignmentMorph proportional.	row		addInRow: {instanceSwitch. commentSwitch. classSwitch}		atFractions: #(0.0 0.5 0.65 1.0).	aColor := Color colorFrom: self class windowColor.	row color: aColor duller.	"ensure matching button divider color. (see #paneColor)"	Preferences alternativeWindowLook ifTrue: [aColor := aColor muchLighter].	{ 		instanceSwitch.		commentSwitch.		classSwitch} do: 				[:m | 				m					color: aColor;					onColor: aColor twiceDarker offColor: aColor].	^row! !!Browser methodsFor: 'initialize-release' stamp: 'jmv 5/28/2009 14:14'!morphicClassColumn	| column switchHeight divider |	column _ AlignmentMorph proportional.	switchHeight _ Preferences standardDefaultTextFont height + 4.	column 		addMorph: self buildMorphicSwitches		fullFrame: (LayoutFrame fractions: (0 @ 1 corner: 1 @ 1)				offsets: (0 @ (1 - switchHeight) corner: 0 @ 0)).	divider _ BorderedSubpaneDividerMorph forTopEdge.	Preferences alternativeWindowLook 		ifTrue: [			divider				extent: 4 @ 4;				color: Color transparent;				borderColor: #raised;				borderWidth: 2].	column addMorph: divider		fullFrame: (LayoutFrame fractions: (0 @ 1 corner: 1 @ 1)				offsets: (0 @ switchHeight negated corner: 0 @ (1 - switchHeight))).	column addMorph: self buildMorphicClassList 		fullFrame: (LayoutFrame fractions: (0 @ 0 corner: 1 @ 1)				offsets: (0 @ 0 corner: 0 @ switchHeight negated)).	^column! !!Browser methodsFor: 'initialize-release' stamp: 'jmv 5/28/2009 14:15'!openAsMorphClassEditing: editString 	"Create a pluggable version a Browser on just a single class."	| window dragNDropFlag hSepFrac switchHeight mySingletonClassList switches c |	window := SystemWindow new model: self.	dragNDropFlag := false.	hSepFrac := 0.3.	switchHeight _ Preferences standardDefaultTextFont height + 10.	mySingletonClassList := PluggableListMorph 				on: self				list: #classListSingleton				selected: #indexIsOne				changeSelected: #indexIsOne:				menu: #classListMenu:shifted:				keystroke: #classListKey:from:.	mySingletonClassList enableDragNDrop: dragNDropFlag.	self 		addLowerPanesTo: window		at: (0 @ hSepFrac corner: 1 @ 1)		with: editString.	window addMorph: mySingletonClassList		fullFrame: (LayoutFrame fractions: (0 @ 0 corner: 0.5 @ 0)				offsets: (0 @ 0 corner: 0 @ switchHeight)).	switches _ self buildMorphicSwitches.	c _ switches color.	window addMorph: switches		fullFrame: (LayoutFrame fractions: (0.5 @ 0 corner: 1.0 @ 0)				offsets: (0 @ 0 corner: 0 @ switchHeight)).	switches color: c.	window addMorph: self buildMorphicMessageCatList		fullFrame: (LayoutFrame fractions: (0 @ 0 corner: 0.5 @ hSepFrac)				offsets: (0 @ switchHeight corner: 0 @ 0)).	window addMorph: self buildMorphicMessageList		fullFrame: (LayoutFrame fractions: (0.5 @ 0 corner: 1.0 @ hSepFrac)				offsets: (0 @ switchHeight corner: 0 @ 0)).	window setUpdatablePanesFrom: #(#messageCategoryList #messageList).	^window! !!ChangeList methodsFor: 'menu actions' stamp: 'jmv 5/28/2009 14:09'!optionalButtonRow	"Answer a row of buttons to occur in a tool pane"	| row button buttons widths |	buttons _ OrderedCollection new.	widths _ OrderedCollection new.	self buttonSpecs do: [ :tuple | 		widths add: tuple first.		button _ PluggableButtonMorph 					on: self					getState: nil					action: tuple third.		button			clipSubmorphs: true;			label: tuple second asString;			askBeforeChanging: true;			onColor: Color transparent offColor: Color transparent.		buttons add: button.		button setBalloonText: tuple fourth].	buttons add: self regularDiffButton.	widths add: 9.	self wantsPrettyDiffOption ifTrue: [		buttons add:  self prettyDiffButton.		widths add: 16 ].	row _ AlignmentMorph proportional.	row addInRow: buttons widthProportionalTo: widths.	^row! !!Debugger methodsFor: 'initialize' stamp: 'jmv 5/28/2009 14:48'!buttonRowForPreDebugWindow: aDebugWindow	| row aButton quads buttons |	buttons _ OrderedCollection new.	quads _ OrderedCollection withAll: self preDebugButtonQuads.	(self interruptedContext selector == #doesNotUnderstand:) ifTrue: [		quads add: { 'Create'. #createMethod. #magenta. 'create the missing method' }	].	quads do: [ :quad |		aButton _ SimpleButtonMorph new target: aDebugWindow.		aButton clipSubmorphs: true; color: Color transparent.		aButton actionSelector: quad second.		aButton label: quad first.		aButton color: (Color colorFrom: quad third) muchLighter.		aButton setBalloonText: quad fourth.		Preferences alternativeWindowLook			ifTrue: [aButton borderWidth: 2; borderColor: #raised].		buttons add: aButton].		row _ AlignmentMorph proportional.	row addInRow: buttons.	^row! !!Debugger methodsFor: 'initialize' stamp: 'jmv 5/28/2009 14:10'!customButtonRow	"Answer a button pane affording the user one-touch access to certain functions; the pane is given the formal name 'customButtonPane' by which it can be retrieved by code wishing to send messages to widgets residing on the pane"	| aButton aLabel buttons row |	buttons _ OrderedCollection new.	self customButtonSpecs do: [ :tuple | 		aButton := PluggableButtonMorph 					on: self					getState: nil					action: tuple second.		aButton			clipSubmorphs: true;			onColor: Color transparent offColor: Color transparent.		(#(#proceed #restart #send #doStep #stepIntoBlock #fullStack #where) 			includes: tuple second) ifTrue: [aButton askBeforeChanging: true].		aLabel := Preferences abbreviatedBrowserButtons 					ifTrue: [self abbreviatedWordingFor: tuple second]					ifFalse: [nil].		aButton label: (aLabel ifNil: [tuple first asString]).		tuple size > 2 ifTrue: [aButton setBalloonText: tuple third].		Preferences alternativeWindowLook ifTrue: [			aButton				borderColor: #raised].		buttons add: aButton].			row _ AlignmentMorph proportional.	row addInProportionalRow: buttons.	^row! !!Debugger methodsFor: 'controls' stamp: 'jmv 5/28/2009 14:43'!addOptionalButtonsTo: window at: fractions plus: verticalOffset	"Add button panes to the window.  A row of custom debugger-specific buttons (Proceed, Restart, etc.) is always added, and if optionalButtons is in force, then the standard code-tool buttons are also added.  Answer the verticalOffset plus the height added."	| delta buttons divider anOffset |	anOffset _ (Preferences optionalButtons and: [Preferences extraDebuggerButtons | true])		ifTrue:			[super addOptionalButtonsTo: window at: fractions plus: verticalOffset]		ifFalse:			[verticalOffset].	delta _ self defaultButtonPaneHeight.	buttons _ self customButtonRow.	buttons color: (Display depth <= 8 ifTrue: [Color transparent] ifFalse: [self class windowColor duller paler]).	Preferences alternativeWindowLook ifTrue: [		buttons submorphsDo:[:m | m borderColor: #raised]].	divider _ BorderedSubpaneDividerMorph forBottomEdge.	Preferences alternativeWindowLook ifTrue:		[divider extent: 4@4; color: Color transparent; borderColor: #raised; borderWidth: 2].	window 		addMorph: buttons		fullFrame: (LayoutFrame 				fractions: fractions 				offsets: (0@anOffset corner: 0@(anOffset + delta - 1))).	window 		addMorph: divider		fullFrame: (LayoutFrame 				fractions: fractions 				offsets: (0@(anOffset + delta - 1) corner: 0@(anOffset + delta))).	^ anOffset + delta! !!FileList class methodsFor: 'instance creation' stamp: 'jmv 5/28/2009 14:48'!addButtonsAndFileListPanesTo: window at: upperFraction plus: offset forFileList: aFileList 	| fileListMorph column buttonHeight fileListTop divider dividerDelta buttons |	fileListMorph _ PluggableListMorph				on: aFileList				list: #fileList				selected: #fileListIndex				changeSelected: #fileListIndex:				menu: #fileListMenu:.	fileListMorph enableDrag: true; enableDrop: false.	aFileList wantsOptionalButtons		ifTrue: [			buttons _ aFileList optionalButtonRow.			divider _ BorderedSubpaneDividerMorph forBottomEdge.			dividerDelta _ 0.			Preferences alternativeWindowLook				ifTrue: [					buttons color: Color transparent.					buttons						submorphsDo: [:m | 							m borderColor: #raised].						divider extent: 4 @ 4;						 color: Color transparent;						 borderColor: #raised;						 borderWidth: 2.					fileListMorph borderColor: Color transparent.					dividerDelta _ 3].			column _ AlignmentMorph proportional.			buttonHeight _ self defaultButtonPaneHeight.			column				addMorph: buttons				fullFrame: (LayoutFrame						fractions: (0 @ 0 corner: 1 @ 0)						offsets: (0 @ 0 corner: 0 @ buttonHeight)).			column				addMorph: divider				fullFrame: (LayoutFrame						fractions: (0 @ 0 corner: 1 @ 0)						offsets: (0 @ buttonHeight corner: 0 @ buttonHeight + dividerDelta)).			column				addMorph: fileListMorph				fullFrame: (LayoutFrame						fractions: (0 @ 0 corner: 1 @ 1)						offsets: (0 @ buttonHeight + dividerDelta corner: 0 @ 0)).			window				addMorph: column				fullFrame: (LayoutFrame						fractions: upperFraction						offsets: (0 @ offset corner: 0 @ 0)).			Preferences alternativeWindowLook				ifTrue: [column borderWidth: 2]				ifFalse: [column borderWidth: 0]]		ifFalse: [			fileListTop _ 0.			window				addMorph: fileListMorph				frame: (0.3 @ fileListTop corner: 1 @ 0.3)].! !!FileList2 class methodsFor: 'utility' stamp: 'jmv 5/28/2009 14:00'!textRow: aString 	^AlignmentMorph proportional		addMorph:			((TextMorph new contents: aString)				color: Color blue; 				lock; 				autoFitOnOff; 				wrapFlag: true; 				centered)		fullFrame: (LayoutFrame fractions: (0 @ 0 corner: 1 @ 1))! !!LayoutFrame methodsFor: 'initialization' stamp: 'jmv 5/28/2009 14:01'!fractions: fractionsOrNil	| fractions |	fractions _ fractionsOrNil ifNil: [0@0 extent: 0@0].	topFraction _ fractions top.	leftFraction _ fractions left.	bottomFraction _ fractions bottom.	rightFraction _ fractions right.	leftOffset _ topOffset _ 0.	rightOffset _ bottomOffset _ 0! !!LayoutFrame class methodsFor: 'as yet unclassified' stamp: 'jmv 5/28/2009 14:02'!fractions: fractionsOrNil	^self new fractions: fractionsOrNil! !!PluggableButtonMorph methodsFor: 'initialization' stamp: 'jmv 5/28/2009 13:02'!defaultBorderWidth	"answer the default border width for the receiver"	^ 2! !!SimpleButtonMorph methodsFor: 'initialization' stamp: 'jmv 5/28/2009 14:56'!initializeAllButLabel	super initialize.	self color: (Color r: 0.4 g: 0.8 b: 0.6).	self borderColor: self color darker.	target _ nil.	actionSelector _ #flash.	arguments _ EmptyArray.	actWhen _ #buttonUp! !!SimpleServiceEntry methodsFor: 'performing service' stamp: 'jmv 5/28/2009 14:10'!buttonToTriggerIn: aFileList 	"Answer a button that will trigger the receiver service in a file list"	| aButton |	aButton := PluggableButtonMorph 				on: self				getState: nil				action: #performServiceFor:.	aButton arguments: { 				aFileList}.	aButton		color: Color transparent;		label: self buttonLabel;		askBeforeChanging: true;		onColor: Color transparent offColor: Color transparent.	aButton color: aFileList class windowColor duller paler.	aButton setBalloonText: self description.	Preferences alternativeWindowLook 		ifTrue: [			aButton				borderColor: #raised].	^aButton! !!SystemWindow methodsFor: 'initialization' stamp: 'jmv 5/28/2009 14:56'!createBox	"create a button with default to be used in the label area"	| box |	box := IconicButton new.	box		color: Color transparent;		target: self.		Preferences alternativeWindowLook		ifTrue: [ box borderColor: #raised ].			^box! !!TestRunner methodsFor: 'interface opening' stamp: 'jmv 5/28/2009 15:01'!buildTestsList	| column offset buttonRow |	column _ AlignmentMorph proportional.	testsList _ PluggableListMorphOfMany				on: self				list: #tests				primarySelection: #selectedSuite				changePrimarySelection: #selectedSuite:				listSelection: #listSelectionAt:				changeListSelection: #listSelectionAt:put:				menu: #listMenu:shifted:.	testsList autoDeselect: false.	offset _ 0.	self wantsOptionalButtons		ifTrue: [offset _ TextStyle default lineGrid + 16 ].	column		addMorph: testsList		fullFrame: (LayoutFrame				fractions: (0 @ 0 corner: 1 @ 1)				offsets: (0 @ 0 corner: 0 @ offset negated)).	self wantsOptionalButtons		ifTrue: [			buttonRow _ self optionalButtonRow.			buttonRow				color: (Display depth <= 8						ifTrue: [Color transparent]						ifFalse: [Color gray alpha: 0.2]).			Preferences alternativeWindowLook				ifTrue: [					buttonRow color: Color transparent.					buttonRow						submorphsDo: [:m |							m  borderColor: #raised]].			column				addMorph: buttonRow				fullFrame: (LayoutFrame						fractions: (0 @ 1 corner: 1 @ 1)						offsets: (0 @ (offset - 1) negated corner: 0 @ 0))].	^ column! !!TestRunner methodsFor: 'interface opening' stamp: 'jmv 5/28/2009 15:00'!buildUpperControls	| refreshButton filterButton stopButton runOneButton runButton row bWidth listsMorph |	row _ AlignmentMorph proportional.	row		color: (Display depth <= 8				ifTrue: [Color transparent]				ifFalse: [Color gray alpha: 0.2]);		 clipSubmorphs: true.	refreshButton _ self buildRefreshButton.	filterButton _ self buildFilterButton.	stopButton _ self buildStopButton.	runOneButton _ self buildRunOneButton.	runButton _ self buildRunButton.	listsMorph _ self buildTestsList.	bWidth _ 90.	row		addMorph: refreshButton		fullFrame: (LayoutFrame				fractions: (0 @ 0 corner: 0 @ 0.33)				offsets: (4 @ 2 corner: bWidth - 4 @ -2)).	row		addMorph: filterButton		fullFrame: (LayoutFrame				fractions: (0 @ 0.33 corner: 0 @ 0.66)				offsets: (4 @ 2 corner: bWidth - 4 @ -2)).	row		addMorph: stopButton		fullFrame: (LayoutFrame				fractions: (0 @ 0.66 corner: 0 @ 1)				offsets: (4 @ 2 corner: bWidth - 4 @ -2)).	row		addMorph: listsMorph		fullFrame: (LayoutFrame				fractions: (0 @ 0 corner: 1 @ 1)				offsets: (bWidth  @ 0 corner: bWidth negated @ 0)).	row		addMorph: runOneButton		fullFrame: (LayoutFrame				fractions: (1 @ 0 corner: 1 @ 0.5)				offsets: (bWidth negated + 4 @ 2 corner: -4 @ -2)).	row		addMorph: runButton		fullFrame: (LayoutFrame				fractions: (1 @ 0.5 corner: 1 @ 1)				offsets: (bWidth negated + 4 @ 2 corner: -4 @ -2)).	Preferences alternativeWindowLook		ifTrue: [			row color: Color transparent.			row				submorphsDo: [:m |					m borderColor: #raised]].	^ row! !!TestRunner methodsFor: 'interface opening' stamp: 'jmv 5/28/2009 14:03'!morphicWindow	"TestRunner new openAsMorph"	| upperRow lowerPanes fracYRatio divider window |	window _ SystemWindow labelled: self windowLabel.	window model: self.	upperRow _ self buildUpperControls.	lowerPanes _ self buildLowerPanes.	fracYRatio _ 0.25.	window		addMorph: upperRow		fullFrame: (LayoutFrame fractions: (0 @ 0 extent: 1 @ fracYRatio)).	divider _ BorderedSubpaneDividerMorph forBottomEdge.	Preferences alternativeWindowLook		ifTrue: [divider				 color: Color transparent;				 borderColor: #raised;				 borderWidth: 1].	window		addMorph: divider		fullFrame: (LayoutFrame				fractions: (0 @ fracYRatio corner: 1 @ fracYRatio)				offsets: (0 @ 0 corner: 0 @ 2)).	window		addMorph: lowerPanes		fullFrame: (LayoutFrame fractions: (0 @ fracYRatio extent: 1 @ (1 - fracYRatio))).	self refreshWindow.	window extent: 460 @ 400.	^window! !!TestRunner methodsFor: 'interface opening' stamp: 'jmv 5/28/2009 14:11'!optionalButtonRow	| row btn buttons widths |	buttons _ OrderedCollection new.	widths _ OrderedCollection new.	self optionalButtonTuples do: [ :tuple | 		widths add: tuple first.		btn _ PluggableButtonMorph 			on: self			getState: nil			action: tuple third.		btn			clipSubmorphs: true;			onColor: Color transparent offColor: Color transparent;			label: tuple second.		buttons add: btn].	row _ AlignmentMorph proportional.	row addInRow: buttons widthProportionalTo: widths.	^row! !!TestRunner methodsFor: 'menus' stamp: 'jmv 5/28/2009 15:02'!installProgressWatcher	| win host |	win _ self dependents first.	host _ win submorphs first.	progress _ ProgressMorph label: 'Test progress'.	progress		position: host position;		extent: host extent;		color: Color transparent.	win		addMorph: progress 		frame: (0.0 @ 0.7 extent: 1.0 @ 0.3).! !AlignmentMorph removeSelector: #addInRow:atFractions:inset:!AlignmentMorph removeSelector: #addInRow:widthProportionalTo:inset:!