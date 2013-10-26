'From Cuis 4.0 of 16 November 2011 [latest update: #1144] on 29 December 2011 at 11:10:17 am'!!classDefinition: #MessageNamesWindow category: #'Morphic-Tools'!MessageSetWindow subclass: #MessageNamesWindow	instanceVariableNames: 'textMorph '	classVariableNames: ''	poolDictionaries: ''	category: 'Morphic-Tools'!!MessageNames methodsFor: 'initialization' stamp: 'jmv 12/29/2011 11:04'!                      contentsSelection	"Return the interval of text in the search pane to select when I set the pane's contents"	^ 1 to: 500 		"all of it"! !!SystemWindow methodsFor: 'top window' stamp: 'jmv 12/29/2011 11:00'!            activateAndSendTopToBack: aBoolean	"Bring me to the front and make me able to respond to mouse and keyboard"	| oldTop |	self owner 		ifNil: [^self	"avoid spurious activate when drop in trash"].	oldTop _TopWindow.	TopWindow _ self.	oldTop ifNotNil: [		oldTop passivate.		aBoolean ifTrue: [			oldTop owner addMorphBack: oldTop ]].	self owner firstSubmorph == self 		ifFalse: [			"Bring me to the top if not already"			self owner addMorphFront: self].	self redrawNeeded.	"Set keyboard focus"	self submorphToFocusKeyboard ifNotNil: [ :m |		self world ifNotNil: [ :w | w activeHand newKeyboardFocus: m ]]! !!SystemWindow methodsFor: 'top window' stamp: 'jmv 12/29/2011 11:00'!        submorphToFocusKeyboard	| candidate |	"Set keyboard focus"	candidate _ self		nextMorphThat: [ :m |  m handlesKeyboard ].	candidate isReallyVisible ifFalse: [		candidate _ candidate			nextMorphThat: [ :m |  m handlesKeyboard and: [ m isReallyVisible ]]].	^candidate handlesKeyboard ifTrue: [ candidate ]! !!MessageNamesWindow methodsFor: 'GUI building' stamp: 'jmv 12/29/2011 11:10'!                      buildMorphicWindow	"Answer a morphic window with the given initial search string, nil if none""MessageNames openMessageNames"	| selectorListView firstRow searchButton secondRow |	textMorph _ TextModelMorph		textProvider: model		textGetter: #searchString 		textSetter: #searchString:		selectionGetter: #contentsSelection.	textMorph setProperty: #alwaysAccept toValue: true.	textMorph askBeforeDiscardingEdits: false.	textMorph acceptOnCR: true.	textMorph setTextColor: Color brown.	textMorph hideScrollBarsIndefinitely	textMorph styler: nil.	searchButton _ PluggableButtonMorph new 		model: textMorph;		label: 'Search';		action: #accept.	searchButton setBalloonText: 'Type some letters into the pane at right, and then press this Search button (or hit RETURN) and all method selectors that match what you typed will appear in the list pane below.  Click on any one of them, and all the implementors of that selector will be shown in the right-hand pane, and you can view and edit their code without leaving this tool.'.	firstRow _ LayoutMorph newRow.	firstRow		addMorph: searchButton proportionalWidth: 0.25;		addMorph: textMorph proportionalWidth: 0.75.	selectorListView _ PluggableListMorph		model: model		listGetter: #selectorList		indexGetter: #selectorListIndex		indexSetter: #selectorListIndex:		mainView: self		menuGetter: #selectorListMenu		keystrokeAction: #selectorListKey:from:.	secondRow _  LayoutMorph newRow.	secondRow		addMorph: selectorListView proportionalWidth: 0.5;		addAdjusterAndMorph: self buildMorphicMessageList proportionalWidth: 0.5.	self layoutMorph		addMorph: firstRow fixedHeight: self defaultButtonPaneHeight+4;		addAdjusterAndMorph: secondRow proportionalHeight: 0.5;		addAdjusterAndMorph: self buildLowerPanes proportionalHeight: 0.5.	model changed: #editSelection! !!MessageNamesWindow methodsFor: 'GUI building' stamp: 'jmv 12/29/2011 11:05'!                            submorphToFocusKeyboard	^textMorph textMorph! !!classDefinition: #MessageNamesWindow category: #'Morphic-Tools'!MessageSetWindow subclass: #MessageNamesWindow	instanceVariableNames: 'textMorph'	classVariableNames: ''	poolDictionaries: ''	category: 'Morphic-Tools'!