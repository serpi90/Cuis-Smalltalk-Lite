'From Cuis 2.9 of 5 November 2010 [latest update: #634] on 4 December 2010 at 7:45:08 pm'!!FillInTheBlankMorph methodsFor: 'initialization' stamp: 'jmv 12/4/2010 00:47'!createTextPaneExtent: answerExtent acceptBoolean: acceptBoolean	"create the textPane"	| result frame |	result := TextModelMorph				textProvider: self				textGetter: #response				textSetter: #response:				selectionGetter: #selectionInterval.	result extent: answerExtent.	result borderWidth: 1.	result hasUnacceptedEdits: true.	result acceptOnCR: acceptBoolean.	frame := LayoutFrame new.	frame leftFraction: 0.0;		 rightFraction: 1.0;		 topFraction: 0.2;		 bottomFraction: 0.7.	result layoutFrame: frame.	self addMorph: result.	^ result! !!ObjectExplorer methodsFor: 'accessing' stamp: 'jmv 12/4/2010 00:48'!explorerFor: anObject 	| window listMorph |	rootObject := anObject.	window := (SystemWindow labelled: (rootObject printStringLimitedTo: 64)) model: self.	window addMorph: (listMorph := SimpleHierarchicalListMorph 						model: self						listGetter: #getList						indexGetter: #getCurrentSelection						indexSetter: #noteNewSelection:						menuGetter: #genericMenu:						keystrokeAction: #explorerKey:from:)		frame: (0 @ 0 corner: 1 @ 0.8).	window 		addMorph: ((TextModelMorph 				textProvider: self				textGetter: nil				textSetter: nil				selectionGetter: nil) askBeforeDiscardingEdits: false)		frame: (0 @ 0.8 corner: 1 @ 1).	listMorph autoDeselect: false.	^window! !!PluggableTextModel methodsFor: 'accessing' stamp: 'jmv 12/4/2010 00:26'!textGetter	^getTextSelector! !!PluggableTextModel methodsFor: 'accessing' stamp: 'jmv 12/4/2010 00:26'!textSetter	^setTextSelector! !!PluggableTextModel methodsFor: 'pane menu' stamp: 'jmv 12/4/2010 00:31'!editorClass	^textProvider editorClass! !!PluggableTextMorph class methodsFor: 'as yet unclassified' stamp: 'jmv 12/4/2010 19:41'!model: anObject textGetter: getTextSel textSetter: setTextSel selectionGetter: getSelectionSel	|styler answer |	answer _ self new.true ifTrue:[		self  == PluggableTextMorph ifTrue: [		answer _ TextModelMorph new ]].	(Preferences syntaxHighlightingAsYouType 			and: [ anObject respondsTo: #shoutAboutToStyle:]) ifTrue: [		styler _ SHTextStylerST80 new.		styler view: answer.		answer styler: styler ].	answer		model: anObject		textGetter: getTextSel		textSetter: setTextSel		selectionGetter: getSelectionSel.	^answer! !!ProcessBrowser methodsFor: 'views' stamp: 'jmv 12/4/2010 00:48'!asPrototypeInWindow	"Create a pluggable version of me, answer a window"	| window aTextMorph |	window _ SystemWindow new model: self.	window		addMorph: ((PluggableListMorph				model: self				listGetter: #processNameList				indexGetter: #processListIndex				indexSetter: #processListIndex:				menuGetter: #processListMenu:				keystrokeAction: #processListKey:from:)				enableDragNDrop: false)		frame: (0 @ 0 extent: 0.5 @ 0.5).	window		addMorph: ((PluggableListMorph				model: self				listGetter: #stackNameList				indexGetter: #stackListIndex				indexSetter: #stackListIndex:				menuGetter: #stackListMenu:				keystrokeAction: #stackListKey:from:)				enableDragNDrop: false)		frame: (0.5 @ 0.0 extent: 0.5 @ 0.5).	aTextMorph _ TextModelMorph				textProvider: self				textGetter: #selectedMethod				textSetter: nil				selectionGetter: nil.	window		addMorph: aTextMorph		frame: (0 @ 0.5 corner: 1 @ 1).	window setLabel: 'Process Browser'.	^ window! !!ProcessBrowser methodsFor: 'views' stamp: 'jmv 12/4/2010 00:48'!openAsMorph	"Create a pluggable version of me, answer a window"	| window aTextMorph |	window _ SystemWindow new				model: self.	deferredMessageRecipient _ WorldState.	window		addMorph: ((PluggableListMorph				model: self				listGetter: #processNameList				indexGetter: #processListIndex				indexSetter: #processListIndex:				menuGetter: #processListMenu:				keystrokeAction: #processListKey:from:)				enableDragNDrop: false)		frame: (0 @ 0 extent: 0.5 @ 0.5).	window		addMorph: ((PluggableListMorph				model: self				listGetter: #stackNameList				indexGetter: #stackListIndex				indexSetter: #stackListIndex:				menuGetter: #stackListMenu:				keystrokeAction: #stackListKey:from:)				enableDragNDrop: false)		frame: (0.5 @ 0.0 extent: 0.5 @ 0.5).	aTextMorph _ TextModelMorph				textProvider: self				textGetter: #selectedMethod				textSetter: nil				selectionGetter: nil.	aTextMorph askBeforeDiscardingEdits: false.	window		addMorph: aTextMorph		frame: (0 @ 0.5 corner: 1 @ 1).	window setUpdatablePanesFrom: #(#processNameList #stackNameList ).	(window setLabel: 'Process Browser') openInWorld.	startedCPUWatcher ifTrue: [ self setUpdateCallbackAfter: 7 ].	^ window! !!SyntaxError class methodsFor: 'instance creation' stamp: 'jmv 12/2/2010 11:02'!buildMorphicViewOn: aSyntaxError	"Answer an Morphic view on the given SyntaxError."	| window |	window _ (SystemWindow labelled: 'Syntax Error') model: aSyntaxError.	window addMorph: (PluggableListMorph 			model: aSyntaxError 			listGetter: #list			indexGetter: #listIndex 			indexSetter: nil 			menuGetter: #listMenu:)		frame: (0@0 corner: 1@0.15).	window addMorph: (PluggableTextMorph 			model: aSyntaxError			textGetter: #actualContents			textSetter: #contents:notifying: 			selectionGetter: #contentsSelection)		frame: (0@0.15 corner: 1@1).	^ window openInWorldExtent: 380@220! !!TestRunner methodsFor: 'interface opening' stamp: 'jmv 12/4/2010 00:47'!buildDetailsText	detailsText _ TextModelMorph		textProvider: self		textGetter: #details		textSetter: nil.	detailsText hideScrollBarsIndefinitely.	^detailsText! !!TestRunner methodsFor: 'interface opening' stamp: 'jmv 12/4/2010 00:47'!buildPassFailText	passFailText _ TextModelMorph		textProvider: self		textGetter: #passFail		textSetter: nil.	passFailText hideScrollBarsIndefinitely.	^ passFailText! !!TextModelMorph methodsFor: 'initialization' stamp: 'jmv 12/4/2010 00:31'!model: newModel selectionGetter: getSelectionSel	editorClass _ newModel editorClass.	getTextSelector _ newModel textGetter.	setTextSelector _ newModel textSetter.	getSelectionSelector _ getSelectionSel.	self model: newModel.	model forceRefetch.	self maybeStyle.		self setSelection: self getSelection! !!TextModelMorph class methodsFor: 'instance creation' stamp: 'jmv 12/4/2010 00:18'!model: aTextModel! !!TextModelMorph class methodsFor: 'instance creation' stamp: 'jmv 12/4/2010 00:19'!textProvider: aTextProvider textGetter: getTextSel textSetter: setTextSel	^ self		textProvider: aTextProvider		textGetter: getTextSel		textSetter: setTextSel		selectionGetter: nil.! !!TextModelMorph class methodsFor: 'instance creation' stamp: 'jmv 12/4/2010 00:26'!textProvider: aTextProvider textGetter: textGetter textSetter: textSetter selectionGetter: getSelectionSel	| styler newModel answer |	answer _ self new.	(Preferences syntaxHighlightingAsYouType 			and: [ aTextProvider respondsTo: #shoutAboutToStyle:]) ifTrue: [		styler _ SHTextStylerST80 new.		styler view: answer.		answer styler: styler ].	newModel _ PluggableTextModel on: aTextProvider.	newModel getTextSelector: textGetter setTextSelector: textSetter.	aTextProvider addDependent: newModel.	answer		model: newModel		selectionGetter: getSelectionSel.	^answer! !!BrowserCommentTextMorph class methodsFor: 'instance creation' stamp: 'jmv 12/4/2010 00:44'!textProvider: aTextProvider textGetter: textGetter textSetter: textSetter selectionGetter: getSelectionSel	| newModel answer |	answer _ self new.	newModel _ PluggableTextModel on: aTextProvider.	newModel getTextSelector: textGetter setTextSelector: textSetter.	aTextProvider addDependent: newModel.	answer		model: newModel		selectionGetter: getSelectionSel.	^answer! !!CodeProvider methodsFor: 'annotation' stamp: 'jmv 12/4/2010 00:42'!addOptionalAnnotationsTo: window at: fractions plus: verticalOffset	"Add an annotation pane to the window if preferences indicate a desire for it, and return the incoming verticalOffset plus the height of the added pane, if any"	| aTextMorph divider delta |	self wantsAnnotationPane ifFalse: [^ verticalOffset].	aTextMorph _ TextModelMorph 		textProvider: self		textGetter: #annotation 		textSetter: nil.	aTextMorph		askBeforeDiscardingEdits: false;		borderWidth: 0;		hideScrollBarsIndefinitely.	divider _ BorderedSubpaneDividerMorph forBottomEdge.	delta _ self defaultAnnotationPaneHeight.	window 		addMorph: aTextMorph 		fullFrame: (LayoutFrame 				fractions: fractions 				offsets: (0@verticalOffset corner: 0@(verticalOffset + delta - 1))).	window 		addMorph: divider		fullFrame: (LayoutFrame 				fractions: fractions 				offsets: (0@(verticalOffset + delta - 1) corner: 0@(verticalOffset + delta))).	^ verticalOffset + delta! !!CodeProvider methodsFor: 'construction' stamp: 'jmv 12/4/2010 00:35'!buildMorphicCodePaneWith: editString	"Construct the pane that shows the code.	Respect the Preference for standardCodeFont."	| codePane |	codePane _ TextModelMorph		textProvider: self		textGetter: #acceptedContents		textSetter: #contents:notifying:		selectionGetter: #contentsSelection.	editString		ifNotNil: [			codePane editString: editString.			codePane hasUnacceptedEdits: true].	^ codePane! !!Browser methodsFor: 'class comment pane' stamp: 'jmv 12/4/2010 00:44'!buildMorphicCommentPane	"Construct the pane that shows the class comment.	Respect the Preference for standardCodeFont."	| commentPane |	commentPane := BrowserCommentTextMorph				textProvider: self				textGetter: #classCommentText				textSetter: #classComment:notifying:				selectionGetter: nil.	commentPane styler: nil.	^ commentPane! !!ChangeList methodsFor: 'menu actions' stamp: 'jmv 12/4/2010 00:28'!buildMorphicCodePaneWith: editString	| codePane |	codePane _ TextModelMorph		textProvider: self		textGetter: #acceptedContents 		textSetter: nil.	editString ifNotNil: [		codePane editString: editString.		codePane hasUnacceptedEdits: true	].	^codePane! !!Debugger methodsFor: 'initialize' stamp: 'jmv 12/4/2010 00:37'!openFullMorphicLabel: aLabelString	"Open a full morphic debugger with the given label"	| window aListMorph oldContextStackIndex |	oldContextStackIndex _ contextStackIndex.	self expandStack. "Sets contextStackIndex to zero."	window _ (SystemWindow labelled: aLabelString) model: self.	aListMorph _ PluggableListMorph		model: self 		listGetter: #contextStackList		indexGetter: #contextStackIndex		indexSetter: #toggleContextStackIndex:		menuGetter: #contextStackMenu:shifted:		keystrokeAction: #contextStackKey:from:.	window addMorph: aListMorph		frame: (0@0 corner: 1@0.25).	self addLowerPanesTo: window at: (0@0.25 corner: 1@0.8) with: nil.	window addMorph: (		PluggableListMorph new			doubleClickSelector: #inspectSelection;			model: self receiverInspector			listGetter: #fieldList			indexGetter: #selectionIndex 			indexSetter: #toggleIndex:			menuGetter: #fieldListMenu: 			keystrokeAction: #inspectorKey:from:)		frame: (0@0.8 corner: 0.2@1).	window addMorph: (TextModelMorph 			textProvider: self receiverInspector			textGetter: #acceptedContents 			textSetter: #accept:			selectionGetter: #contentsSelection)		frame: (0.2@0.8 corner: 0.5@1).	window addMorph: (		PluggableListMorph new			doubleClickSelector: #inspectSelection;			model: self contextVariablesInspector 			listGetter: #fieldList			indexGetter: #selectionIndex 			indexSetter: #toggleIndex:			menuGetter: #fieldListMenu: 			keystrokeAction: #inspectorKey:from:)		frame: (0.5@0.8 corner: 0.7@1).	window addMorph: (TextModelMorph 			textProvider: self contextVariablesInspector			textGetter: #acceptedContents 			textSetter: #accept:			selectionGetter: #contentsSelection)		frame: (0.7@0.8 corner: 1@1).	window openInWorld.	self toggleContextStackIndex: oldContextStackIndex.	^ window ! !!Debugger methodsFor: 'context stack menu' stamp: 'jmv 12/4/2010 00:37'!buildMorphicNotifierLabelled: label message: messageString 	| notifyPane window contentTop extentToUse |	self expandStack.	window := (SystemWindow labelled: label) model: self.	contentTop := 0.2.	extentToUse := 650 @ 320.	"nice and wide to show plenty of the error msg"	window addMorph: self buttonRowForPreDebugWindow		frame: (0 @ 0 corner: 1 @ contentTop).	messageString 		ifNil: [			notifyPane := PluggableListMorph 						model: self						listGetter: #contextStackList						indexGetter: #contextStackIndex						indexSetter: #debugAt:						menuGetter: nil						keystrokeAction: nil]		ifNotNil: [			notifyPane _ TextModelMorph				textProvider: self				textGetter: nil 				textSetter: nil.			notifyPane				editString: messageString;				askBeforeDiscardingEdits: false].	window addMorph: notifyPane frame: (0 @ contentTop corner: 1 @ 1).	^window openInWorldExtent: extentToUse! !!FileContentsBrowser methodsFor: 'creation' stamp: 'jmv 12/4/2010 00:38'!addLowerPanesTo: window at: nominalFractions with: editString	| verticalOffset column codePane infoPane infoHeight divider |	column _ AlignmentMorph proportional.	codePane _ TextModelMorph 		textProvider: self		textGetter: #acceptedContents 		textSetter: #contents:notifying:		selectionGetter: #contentsSelection.	infoPane _ TextModelMorph 		textProvider: self		textGetter: #infoViewContents 		textSetter: nil.	infoPane askBeforeDiscardingEdits: false.	verticalOffset _ 0.	infoHeight _ 20.	column 		addMorph: (codePane borderWidth: 0)		fullFrame: (			LayoutFrame 				fractions: (0@0 corner: 1@1) 				offsets: (0@verticalOffset corner: 0@infoHeight negated)		).	divider _ BorderedSubpaneDividerMorph forTopEdge.	column 		addMorph: divider		fullFrame: (			LayoutFrame 				fractions: (0@1 corner: 1@1) 				offsets: (0@infoHeight negated corner: 0@(1-infoHeight))		).	column 		addMorph: (infoPane borderWidth: 0; hideScrollBarsIndefinitely)		fullFrame: (			LayoutFrame 				fractions: (0@1 corner: 1@1) 				offsets: (0@(1-infoHeight) corner: 0@0)		).	window 		addMorph: column		frame: nominalFractions.	column on: #mouseEnter send: #paneTransition: to: window.	column on: #mouseLeave send: #paneTransition: to: window! !!FileList class methodsFor: 'instance creation' stamp: 'jmv 12/4/2010 00:42'!addVolumesAndPatternPanesTo: window at: upperFraction plus: offset forFileList: aFileList 	| column patternHeight volumeListMorph patternMorph divider dividerDelta |	column _ AlignmentMorph proportional.	patternHeight _ 25.	volumeListMorph _ (PluggableListMorph				model: aFileList				listGetter: #volumeList				indexGetter: #volumeListIndex				indexSetter: #volumeListIndex:				menuGetter: #volumeMenu:)				autoDeselect: false.	volumeListMorph enableDrag: false; enableDrop: true.	patternMorph _ TextModelMorph				textProvider: aFileList				textGetter: #pattern				textSetter: #pattern:.	patternMorph acceptOnCR: true.	patternMorph hideScrollBarsIndefinitely.	divider _ BorderedSubpaneDividerMorph new.	dividerDelta _ 0.	volumeListMorph borderColor: Color transparent.	patternMorph borderColor: Color transparent.	dividerDelta _ 3.	column		addMorph: (volumeListMorph autoDeselect: false)		fullFrame: (LayoutFrame				fractions: (0 @ 0 corner: 1 @ 1)				offsets: (0 @ 0 corner: 0 @ patternHeight negated - dividerDelta)).	column		addMorph: divider		fullFrame: (LayoutFrame				fractions: (0 @ 1 corner: 1 @ 1)				offsets: (0 @ patternHeight negated - dividerDelta corner: 0 @ patternHeight negated)).	column		addMorph: patternMorph		fullFrame: (LayoutFrame				fractions: (0 @ 1 corner: 1 @ 1)				offsets: (0 @ patternHeight negated corner: 0 @ 0)).	window		addMorph: column		fullFrame: (LayoutFrame				fractions: upperFraction				offsets: (0 @ offset corner: 0 @ 0)).	column borderWidth: 2! !!FileList class methodsFor: 'instance creation' stamp: 'jmv 12/4/2010 00:39'!openAsMorph	"Open a morphic view of a FileList on the default directory."	| dir aFileList window upperFraction offset |	dir := FileDirectory default.	aFileList := self new directory: dir.	window := (SystemWindow labelled: dir pathName) model: aFileList.	upperFraction := 0.3.	offset := 0.	self 		addVolumesAndPatternPanesTo: window		at: (0 @ 0 corner: 0.3 @ upperFraction)		plus: offset		forFileList: aFileList.	self 		addButtonsAndFileListPanesTo: window		at: (0.3 @ 0 corner: 1.0 @ upperFraction)		plus: offset		forFileList: aFileList.	window addMorph: (TextModelMorph 			textProvider: aFileList			textGetter: #acceptedContents			textSetter: #put:			selectionGetter: #contentsSelection)		frame: (0 @ 0.3 corner: 1 @ 1).	^window openInWorld! !!FileList class methodsFor: 'instance creation' stamp: 'jmv 12/4/2010 00:39'!openMorphOn: aFileStream editString: editString 	"Open a morphic view of a FileList on the given file."	| fileModel window fileContentsView |	fileModel _ FileList new setFileStream: aFileStream.	"closes the stream"	window _ (SystemWindow labelled: aFileStream fullName) model: fileModel.	window addMorph: (fileContentsView _ TextModelMorph 			textProvider: fileModel			textGetter: #acceptedContents 			textSetter: #put:			selectionGetter: #contentsSelection)		frame: (0@0 corner: 1@1).	editString ifNotNil: [		fileContentsView editString: editString.		fileContentsView hasUnacceptedEdits: true].	^ window! !!FileList2 methodsFor: 'user interface' stamp: 'jmv 12/4/2010 00:39'!morphicFileContentsPane	^TextModelMorph 		textProvider: self		textGetter: #acceptedContents 		textSetter: #put:		selectionGetter: #contentsSelection! !!FileList2 methodsFor: 'user interface' stamp: 'jmv 12/4/2010 00:42'!morphicPatternPane	^TextModelMorph 		textProvider: self		textGetter: #pattern 		textSetter: #pattern:		! !!Inspector class methodsFor: 'instance creation' stamp: 'jmv 12/4/2010 00:40'!openAsMorphOn: anObject withLabel: aLabel	" Inspector openAsMorphOn: SystemOrganization "	| window inspector |	inspector _ self inspect: anObject.	window _ (SystemWindow labelled: aLabel) model: inspector.	window addMorph: (		PluggableListMorph new			doubleClickSelector: #inspectSelection;			model: inspector 			listGetter: #fieldList			indexGetter: #selectionIndex			indexSetter: #toggleIndex:			menuGetter: #fieldListMenu:			keystrokeAction: #inspectorKey:from:)		frame: (0@0 corner: self horizontalDividerProportion @ self verticalDividerProportion).	window addMorph: (TextModelMorph			textProvider: inspector			textGetter: #acceptedContents 			textSetter: #accept:			selectionGetter: #contentsSelection)		frame: (self horizontalDividerProportion @0 corner: 1@self verticalDividerProportion).	window addMorph: ((TextModelMorph 			textProvider: inspector			textGetter: nil 			textSetter: nil			selectionGetter: #contentsSelection)					askBeforeDiscardingEdits: false)		frame: (0@self verticalDividerProportion corner: 1@1).	window setUpdatablePanesFrom: #(fieldList).	^ window! !!MessageNames methodsFor: 'initialization' stamp: 'jmv 12/4/2010 00:40'!inMorphicWindowWithInitialSearchString: initialString	"Answer a morphic window with the given initial search string, nil if none""MessageNames openMessageNames"	| window selectorListView firstDivider secondDivider horizDivider typeInPane searchButton plugTextMor |	window _ (SystemWindow labelled: 'Message Names') model: self.	firstDivider _ 0.07.	secondDivider _ 0.5.	horizDivider _ 0.5.	typeInPane _ AlignmentMorph proportional height: 14.	plugTextMor _ TextModelMorph 		textProvider: self		textGetter: #searchString 		textSetter: #searchString:notifying:.	plugTextMor setProperty: #alwaysAccept toValue: true.	plugTextMor askBeforeDiscardingEdits: false.	plugTextMor acceptOnCR: true.	plugTextMor setTextColor: Color brown.	plugTextMor hideScrollBarsIndefinitely.	plugTextMor textMorph		on: #mouseEnter send: #selectAll to: plugTextMor textMorph.	searchButton _ SimpleButtonMorph new 		target: self;		beTransparent;		label: 'Search';		actionSelector: #doSearchFrom:;		arguments: {plugTextMor}.	searchButton setBalloonText: 'Type some letters into the pane at right, and then press this Search button (or hit RETURN) and all method selectors that match what you typed will appear in the list pane below.  Click on any one of them, and all the implementors of that selector will be shown in the right-hand pane, and you can view and edit their code without leaving this tool.'.	typeInPane addInProportionalRow: { searchButton. plugTextMor. }.	initialString isEmptyOrNil ifFalse:		[plugTextMor changeText: initialString].	window addMorph: typeInPane frame: (0@0 corner: horizDivider @ firstDivider).	selectorListView _ PluggableListMorph 		model: self		listGetter: #selectorList		indexGetter: #selectorListIndex		indexSetter: #selectorListIndex:		menuGetter: #selectorListMenu:		keystrokeAction: #selectorListKey:from:.	window addMorph: selectorListView frame: (0 @ firstDivider corner: horizDivider @ secondDivider).	window addMorph: self buildMorphicMessageList frame: (horizDivider @ 0 corner: 1@ secondDivider).	self 		addLowerPanesTo: window 		at: (0 @ secondDivider corner: 1@1) 		with: nil.	initialString isEmptyOrNil ifFalse: [		self searchString: initialString notifying: nil].	^ window! !!TranscriptStream methodsFor: 'initialization' stamp: 'jmv 12/4/2010 00:51'!openAsMorphLabel: labelString 	"Build a morph viewing this transcriptStream"	| window |	window _ (SystemWindow labelled: labelString) model: self.	window addMorph: (TextModelMorph 			textProvider: self			textGetter: nil 			textSetter: nil			selectionGetter: nil)		frame: (0@0 corner: 1@1).	^ window openInWorld! !!TextProvider reorganize!('self-updating' updatePaneIfNeeded)!TextModelMorph class removeSelector: #model:textGetter:textSetter:!TextModelMorph class removeSelector: #noShoutModel:textGetter:textSetter:selectionGetter:!PluggableTextMorph class removeSelector: #model:textGetter:textSetter:!PluggableTextMorph class removeSelector: #noShoutModel:textGetter:textSetter:selectionGetter:!