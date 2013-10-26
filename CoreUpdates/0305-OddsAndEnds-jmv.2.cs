'From Cuis 1.0 of 4 September 2009 [latest update: #290] on 18 September 2009 at 2:03:21 pm'!!FileContentsBrowser methodsFor: 'creation' stamp: 'jmv 9/18/2009 13:54'!openAsMorph	"Create a pluggable version of all the views for a Browser, including views and controllers."	| window aListExtent next mySingletonList |	window _ SystemWindow new model: self.	self packages size = 1		ifTrue: [			aListExtent _ 0.333333 @ 0.34.			self systemCategoryListIndex: 1.			mySingletonList _ PluggableListMorph 					model: self 					listGetter: #systemCategorySingleton					indexGetter: #indexIsOne 					indexSetter: #indexIsOne:					menuGetter: #packageListMenu:					keystrokeAction: #packageListKey:from:.			mySingletonList hideScrollBarsIndefinitely.			window addMorph: mySingletonList frame: (0@0 extent: 1.0@0.06).			next := 0@0.06]		ifFalse: [			aListExtent _ 0.25 @ 0.4.			window addMorph: (PluggableListMorph 					model: self 					listGetter: #systemCategoryList					indexGetter: #systemCategoryListIndex 					indexSetter: #systemCategoryListIndex:					menuGetter: #packageListMenu:					keystrokeAction: #packageListKey:from:)				frame: (0@0 extent: aListExtent).			next := aListExtent x @ 0].	self addClassAndSwitchesTo: window at: (next extent: aListExtent) plus: 0.	next := next + (aListExtent x @ 0).	window addMorph: (PluggableListMorph 			model: self 			listGetter: #messageCategoryList			indexGetter: #messageCategoryListIndex 			indexSetter: #messageCategoryListIndex:			menuGetter: #messageCategoryMenu:)		frame: (next extent: aListExtent).	next := next + (aListExtent x @ 0).	window addMorph: (PluggableListMorph 			model: self 			listGetter: #messageList			indexGetter: #messageListIndex 			indexSetter: #messageListIndex:			menuGetter: #messageListMenu:			keystrokeAction: #messageListKey:from:)		frame: (next extent: aListExtent).	self addLowerPanesTo: window at: (0@0.4 corner: 1@1) with: nil.	^ window! !!PluggableTextMorph methodsFor: 'notifications' stamp: 'jmv 9/18/2009 14:02'!possiblyChanged	"A hook for notificating possible interested parties	Not used in base system"! !!TextEditor methodsFor: 'events' stamp: 'jmv 9/17/2009 16:21'!mouseDown: evt 	"An attempt to break up the old processRedButton code into threee phases"	| clickPoint b |	oldInterval _ self selectionInterval.	clickPoint _ evt cursorPoint.	b _ paragraph characterBlockAtPoint: clickPoint.	(paragraph clickAt: clickPoint for: model controller: self) ifTrue: [		self markBlock: b.		self pointBlock: b.		morph possiblyChanged.		evt hand releaseKeyboardFocus: self.		^ self ].		evt shiftPressed		ifFalse: [			self closeTypeIn.			self markBlock: b.			self pointBlock: b.			morph possiblyChanged ]! !!TextEditor methodsFor: 'initialize-release' stamp: 'jmv 9/17/2009 16:06'!stateArray	^ {		self selectionInterval.		self startOfTyping.		emphasisHere}! !!TextEditor methodsFor: 'initialize-release' stamp: 'jmv 9/17/2009 16:06'!stateArrayPut: stateArray	| sel |	sel _ stateArray at: 1.	self selectFrom: sel first to: sel last.	beginTypeInBlock _ stateArray at: 2.	emphasisHere _ stateArray at: 3! !!TextMorph methodsFor: 'private' stamp: 'jmv 9/17/2009 11:35'!updateFromParagraph	"A change has taken place in my paragraph, as a result of editing and I must be updated.  If a line break causes recomposition of the current paragraph, or it the selection has entered a different paragraph, then the current editor will be released, and must be reinstalled with the resulting new paragraph, while retaining any editor state, such as selection, undo state, and current typing emphasis."	"removed multiple lined paragraph support (predecessor and successor)"	| newStyle sel oldEditor |	paragraph ifNil: [ ^self ].	wrapFlag ifNil: [ wrapFlag := true ].	editor ifNotNil: [		oldEditor := editor.		sel := editor selectionInterval.		editor storeSelectionInParagraph].	text := paragraph text.	paragraph text initialStyle = text initialStyle 		ifTrue: [self fit]		ifFalse: [			newStyle := paragraph text initialStyle.			text initialStyle: newStyle.			self				releaseParagraph;					"Force recomposition"				fit.									"and propagate the change"			editor ifNotNil: [self installEditorToReplace: editor]].	self layoutChanged.	sel ifNotNil: [		editor ifNil: [			"Reinstate selection after, eg, style change"			self installEditorToReplace: oldEditor]]! !!TextMorphForEditView methodsFor: 'private' stamp: 'jmv 9/17/2009 16:20'!updateFromParagraph	super updateFromParagraph.	editView setScrollDeltas.	self possiblyChanged! !!TextMorphForEditView methodsFor: 'as yet unclassified' stamp: 'jmv 9/17/2009 16:20'!possiblyChanged	editView possiblyChanged! !!PluggableTextMorph reorganize!('accessing' editorClass getTextSelector wrapFlag:)('dependents access' canDiscardEdits hasUnacceptedEdits)('drawing' drawOn: wantsFrameAdornments)('dropping/grabbing' wantsDroppedMorph:event:)('editor access' handleEdit: scrollSelectionIntoView scrollSelectionIntoView: selectAll setTextMorphToSelectAllOnMouseEnter)('event handling' handlesKeyboard keyStroke: mouseEnter: mouseLeave:)('geometry' extent: resetExtent scrollDeltaHeight)('initialization' acceptOnCR: editString: initialize on:editorClass:text:accept:readSelection:menu:)('interactive error protocol' correctFrom:to:with: correctSelectionWithString: deselect nextTokenFrom:direction: notify:at:in: selectFrom:to: selectInvisiblyFrom:to: selectionInterval)('layout' acceptDroppingMorph:event:)('menu commands' accept again browseChangeSetsWithSelector browseIt cancel changeStyle chooseAlignment classCommentsContainingIt classNamesContainingIt copySelection cut debugIt doIt explain exploreIt fileItIn find findAgain implementorsOfIt inspectIt methodNamesContainingIt methodSourceContainingIt methodStringsContainingit offerFontMenu paste pasteRecent prettyPrint printIt referencesToIt removeCharacterStyle saveContentsInFile selectCharacterStyle selectCurrentStyle sendersOfIt setSearchString spawn undo yellowButtonActivity)('model access' basicSetText: getSelection getText selectionInterval: setSelection: setText: setTextColor: text)('scroll bar events' scrollBarMenuButtonPressed: yellowButtonActivity:)('transcript' appendEntry bsText changeText: replaceSelectionWith:)('unaccepted edits' askBeforeDiscardingEdits: hasEditingConflicts hasEditingConflicts: hasUnacceptedEdits: promptForCancel)('updating' update:)('scrolling' hUnadjustedScrollRange)('focus handling' focusText)('menu' getMenu:)('shout' okToStyle styler: stylerStyled: stylerStyledInBackground:)('notifications' possiblyChanged)!