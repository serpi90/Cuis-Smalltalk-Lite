'From Cuis 2.9 of 5 November 2010 [latest update: #634] on 6 December 2010 at 6:35:26 pm'!!classDefinition: #LimitedHeightTextMorph category: #'Morphic-Windows'!TextModelMorph subclass: #LimitedHeightTextMorph	instanceVariableNames: 'maxHeight'	classVariableNames: ''	poolDictionaries: ''	category: 'Morphic-Windows'!!LimitedHeightTextMorph commentStamp: '<historical>' prior: 0!A TextMorph that will expand and contract vertically to adjust for the contents, but limited to a specific max height. If contents are larger, a scrollbar will be used.LimitedHeightTextMorph new	maxHeight: 48;	model: (TextModel new contents: 'This is some text to test the morph.');	openInWorld!!BareTextMorph methodsFor: 'private' stamp: 'jmv 12/6/2010 18:31'!fit	"Adjust my bounds to fit the text.	Required after the text changes,	or if wrapFlag is true and the user attempts to change the extent."	| newExtent para |	newExtent := (self paragraph extent max: 9 @ self text initialStyle initialLineGrid) + (0 @ 2).	newExtent ~= bounds extent 		ifTrue: [			para := paragraph.	"Save para (layoutChanged smashes it)"			self basicExtent: newExtent.			paragraph := para].	"These statements should be pushed back into senders"	self paragraph positionWhenComposed: self position.	self changed.	"Too conservative: only paragraph composition					should cause invalidation."	editView innerHeight: newExtent y! !!BareTextMorph methodsFor: 'private' stamp: 'jmv 12/6/2010 18:34'!installEditorToReplace: priorEditor 	"Install an editor for my paragraph.  This constitutes 'hasFocus'.	If priorEditor is not nil, then initialize the new editor from its state.	We may want to rework this so it actually uses the prior editor."	| stateArray |	priorEditor ifNotNil: [ stateArray _ priorEditor stateArray] .	editor := self editorClass new morph: self.	editor model: model.	editor changeParagraph: self paragraph.	stateArray ifNotNil: [ editor stateArrayPut: stateArray ].	self selectionChanged.	^editor! !!Editor methodsFor: 'accessing-selection' stamp: 'jmv 11/4/2008 13:24'!selectionInterval	"Answer the interval that is currently selected."	^self startIndex to: self stopIndex - 1 ! !!EntryField2LW methodsFor: 'accessing' stamp: 'jmv 12/6/2010 18:21'!installEditorToReplace: priorEditor	"Install an editor for my contents.  This constitutes 'hasFocus'.	If priorEditor is not nil, then initialize the new editor from its state.	We may want to rework this so it actually uses the prior editor."	| stateArray |	priorEditor ifNotNil: [stateArray := priorEditor stateArray].	editor := self editorClass new morph: self.	editor changeString: contents.	stateArray ifNotNil: [editor stateArrayPut: stateArray].	self changed.	^editor! !!OneLineEditorMorph methodsFor: 'private' stamp: 'jmv 12/6/2010 18:21'!installEditorToReplace: priorEditor	"Install an editor for my contents.  This constitutes 'hasFocus'.	If priorEditor is not nil, then initialize the new editor from its state.	We may want to rework this so it actually uses the prior editor."	| stateArray |	priorEditor ifNotNil: [stateArray := priorEditor stateArray].	editor := SimpleEditor new morph: self.	editor changeString: contents.	stateArray ifNotNil: [editor stateArrayPut: stateArray].	self changed.	^editor! !!SimpleEditor methodsFor: 'initialize-release' stamp: 'jmv 12/6/2010 18:23'!stateArray	"nothing if not built yet"	selectionShowing ifNil: [ ^nil ].		^ {	self selectionInterval.		self startOfTyping}! !!TextEditor methodsFor: 'initialize-release' stamp: 'jmv 12/6/2010 18:23'!stateArray	"nothing if not built yet"	selectionShowing ifNil: [ ^nil ].	^ {		self selectionInterval.		self startOfTyping.		emphasisHere}! !!TextModelMorph methodsFor: 'geometry' stamp: 'jmv 12/6/2010 15:45'!extent: newExtent 	super extent: newExtent.	textMorph ifNotNil: [		textMorph extent = self viewableBounds extent ifFalse: ["			textMorph extent print.			self viewableBounds extent print."			textMorph extent: self viewableBounds extent ]].	self setScrollDeltas! !!TextModelMorph methodsFor: 'geometry' stamp: 'jmv 12/6/2010 18:30'!innerHeight: aNumber	"Adjust height and scrollbar to the new contents height.	Nothing to do here: TextModelMorph height does not depend on contents height."! !!TextModelMorph methodsFor: 'geometry' stamp: 'jmv 12/6/2010 15:39'!resetExtent	"Reset the extent while maintaining the current selection."	textMorph ifNotNil: [		self extent: self extent ]! !!LimitedHeightTextMorph methodsFor: 'accessing' stamp: 'jmv 12/6/2010 18:28'!maxHeight: aNumber	maxHeight _ aNumber! !!LimitedHeightTextMorph methodsFor: 'geometry' stamp: 'jmv 12/6/2010 18:30'!innerHeight: aNumber	"Adjust height and scrollbar to the new contents height."	self height: (aNumber + 10 min: maxHeight)! !!LimitedHeightTextMorph reorganize!('accessing' maxHeight:)('geometry' innerHeight:)!