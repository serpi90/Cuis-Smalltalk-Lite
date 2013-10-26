'From Cuis 1.0 of 16 July 2009 [latest update: #228] on 27 July 2009 at 8:52:42 pm'!!classDefinition: #AbstractFont category: #'Graphics-Text'!Object subclass: #AbstractFont	instanceVariableNames: ''	classVariableNames: 'AvailableFonts DefaultFont '	poolDictionaries: ''	category: 'Graphics-Text'!!classDefinition: #TextStyle category: #'Graphics-Text'!Object subclass: #TextStyle	instanceVariableNames: 'fontArray lineGrid baseline alignment firstIndent restIndent rightIndent tabsArray marginTabsArray leading defaultFontIndex font '	classVariableNames: 'AvailableTextStyles DefaultTextStyle '	poolDictionaries: ''	category: 'Graphics-Text'!!AbstractFont methodsFor: 'accessing' stamp: 'jmv 7/27/2009 17:31'!textStyle	^ TextStyle withFont: self! !!AbstractFont class methodsFor: 'instance accessing' stamp: 'jmv 7/27/2009 17:06'!default	^DefaultFont! !!AbstractFont class methodsFor: 'instance accessing' stamp: 'jmv 7/27/2009 17:06'!default: aFont	DefaultFont _ aFont! !!AbstractFont class methodsFor: 'instance accessing' stamp: 'jmv 7/27/2009 17:37'!familyName: aString pointSize: aNumber	"	AbstractFont familyName: 'DejaVu' pointSize: 12	"	| familyDictionary |	familyDictionary _ AvailableFonts at: aString ifAbsent: [^nil].	^familyDictionary at: aNumber ifAbsent: [nil]! !!AbstractFont class methodsFor: 'instance accessing' stamp: 'jmv 7/27/2009 18:47'!familyNames	"	AbstractFont familyNames	"	^AvailableFonts keys asArray sort! !!AbstractFont class methodsFor: 'instance accessing' stamp: 'jmv 7/27/2009 18:55'!pointSizesFor: aString	"	AbstractFont pointSizesFor: 'DejaVu'	"	| familyDictionary |	familyDictionary _ AvailableFonts at: aString ifAbsent: [^#()].	^familyDictionary keys asArray sort! !!AbstractFont class methodsFor: 'class initialization' stamp: 'jmv 7/27/2009 17:16'!initialize	"AvailableFonts is a dictionary whose keys are family names, such as 'DejaVu' and values are family dictionaries	family dictionaries have keys that are integers (point sizes such as 10 or 12) and values instances of the Font hierarcy		Fonts with emphasis (such as bold or italic) are derivative fonts of the one found in the family dictionary"		AvailableFonts _ Dictionary new! !!AbstractFont class methodsFor: 'user interface' stamp: 'jmv 7/27/2009 19:26'!fontMenuForFamily: familyName target: target selector: selector highlight: currentSize 	"Offer a font menu for the given font family. If one is selected, pass that font to target with a call to selector. The fonts will be displayed in that font."	| menu  font |	menu := MenuMorph entitled: familyName.	(self pointSizesFor: familyName) do: [ :p |			font := self familyName: familyName pointSize: p.			menu 				add: p asString , ' Point'				target: target				selector: selector				argument: font.			menu lastItem font: font.			p = currentSize ifTrue: [menu lastItem color: Color blue darker]].	^menu! !!AbstractFont class methodsFor: 'user interface' stamp: 'jmv 7/27/2009 19:28'!promptForFont: aPrompt andSendTo: aTarget withSelector: aSelector highlight: currentFont	"Morphic Only!! prompt for a font and if one is provided, send it to aTarget using a message with selector aSelector."	"	AbstractFont promptForFont: 'Choose system font:' andSendTo: Preferences withSelector: #setSystemFontTo: highlight: nil	"	| menu subMenu currentSize currentFamilyName |	currentFamilyName _ currentFont ifNotNil: [ currentFont familyName ].	menu _ MenuMorph entitled: aPrompt.	AbstractFont familyNames do: [ :familyName |		currentSize _  (familyName = currentFamilyName ifTrue: [ currentFont pointSize ]).		subMenu _ self fontMenuForFamily: familyName target: aTarget selector: aSelector highlight: currentSize.		menu add: familyName subMenu: subMenu.		familyName = currentFamilyName ifTrue: [menu lastItem color: Color blue darker]].	menu popUpInWorld: self currentWorld! !!Preferences class methodsFor: 'fonts' stamp: 'jmv 7/27/2009 17:38'!setDefaultFonts: defaultFontsSpec			| font |	defaultFontsSpec do: [ :triplet |		font _ AbstractFont familyName: triplet second pointSize: triplet third.		triplet size > 3 ifTrue: [			font _ font emphasized: triplet fourth ].		self			perform: triplet first			with: font]! !!Preferences class methodsFor: 'fonts' stamp: 'jmv 7/27/2009 20:47'!setSystemFontTo: aFont	"Establish the default text font and style"	| aStyle newDefaultStyle |	aFont ifNil: [^ self].	StrikeFont default: aFont.	aStyle _ aFont textStyle ifNil: [^ self].	newDefaultStyle _ aStyle copy.	newDefaultStyle defaultFontIndex: (aStyle fontIndexOf: aFont).	TextStyle default: newDefaultStyle! !!StrikeFont class methodsFor: 'instance creation' stamp: 'jmv 7/27/2009 17:46'!installDejaVu"StrikeFont installDejaVu"	| baseF base boldF italicF bold italic sizes boldItalicF boldItalic data form dejaVuDict |	sizes _ 5 to: 24.	baseF _ sizes collect: [ :s |		'AAFonts/DejaVu Sans Book ', s printString -> ('DejaVu ', s printString) ].	boldF _ sizes collect: [ :s |		'AAFonts/DejaVu Sans Bold ', s printString -> ('DejaVu ', s printString, 'B') ].	italicF _ sizes collect: [ :s |		'AAFonts/DejaVu Sans Oblique ', s printString -> ('DejaVu ', s printString, 'I') ].	boldItalicF _ sizes collect: [ :s |		'AAFonts/DejaVu Sans Bold Oblique ', s printString -> ('DejaVu ', s printString, 'BI') ].			base := OrderedCollection new.	bold := OrderedCollection new.	italic := OrderedCollection new.	boldItalic := OrderedCollection new.		baseF withIndexDo: [:keyValue :index |		form _ Form fromFileNamed:keyValue key, '.bmp'.		data _ (FileStream oldFileNamed: keyValue key, '.txt') contentsOfEntireFile substrings.		base add: ((StrikeFont new buildFromForm: form data: data name: keyValue value)			pointSize: keyValue value substrings last asNumber)].	boldF withIndexDo: [:keyValue :index |		form _ Form fromFileNamed:keyValue key, '.bmp'.		data _ (FileStream oldFileNamed: keyValue key, '.txt') contentsOfEntireFile substrings.		bold add: ((StrikeFont new buildFromForm: form data: data name: keyValue value) 			emphasis: 1;			pointSize: keyValue value substrings last asNumber)].	italicF withIndexDo: [:keyValue :index |		form _ Form fromFileNamed:keyValue key, '.bmp'.		data _ (FileStream oldFileNamed: keyValue key, '.txt') contentsOfEntireFile substrings.		italic add: ((StrikeFont new buildFromForm: form data: data name: keyValue value)			emphasis:2;			pointSize: keyValue value substrings last asNumber)].	boldItalicF withIndexDo: [:keyValue :index |		form _ Form fromFileNamed:keyValue key, '.bmp'.		data _ (FileStream oldFileNamed: keyValue key, '.txt') contentsOfEntireFile substrings.		boldItalic add: ((StrikeFont new buildFromForm: form data: data name: keyValue value)			emphasis:3;			pointSize: keyValue value substrings last asNumber)].				1 to: base size do: [ :i |		(base at: i) derivativeFont: (bold at: i) at: 1.		(base at: i) derivativeFont: (italic at: i) at: 2.		(base at: i) derivativeFont: (boldItalic at: i) at: 3 ].	dejaVuDict _ Dictionary new.	sizes withIndexDo: [ :s :i |		dejaVuDict at: s put: (base at: i) ].	AvailableFonts at: 'DejaVu' put: dejaVuDict.	Preferences restoreDefaultFonts! !!StrikeFont class methodsFor: 'removing' stamp: 'jmv 7/27/2009 19:04'!removeForPDA"StrikeFont removeForPDA"	| familyDict |	familyDict _ AvailableFonts at: 'DejaVu'.	familyDict keys copy do: [ :k |		(#(5 6 7 8 9) includes: k) 			ifTrue: [				(familyDict at: k) derivativeFont: nil at: 0 ]			ifFalse: [				familyDict removeKey: k ]].		Preferences setDefaultFonts: #(		(setSystemFontTo: 'DejaVu' 8)		(setListFontTo: 'DejaVu' 6)		(setMenuFontTo: 'DejaVu' 7)		(setWindowTitleFontTo: 'DejaVu' 9)		(setBalloonHelpFontTo: 'DejaVu' 7)		(setCodeFontTo: 'DejaVu' 7)		(setButtonFontTo: 'DejaVu' 7))! !!StrikeFont class methodsFor: 'removing' stamp: 'jmv 7/27/2009 19:05'!removeMostFonts"StrikeFont removeMostFonts"	| familyDict |	familyDict _ AvailableFonts at: 'DejaVu'.	familyDict keys copy do: [ :k |		(#(8 10 12 14 16 18 20) includes: k) 			ifTrue: [				(familyDict at: k) derivativeFont: nil at: 0 ]			ifFalse: [				familyDict removeKey: k ]].		Preferences setDefaultFonts: #(		(setSystemFontTo: 'DejaVu' 10)		(setListFontTo: 'DejaVu' 10)		(setMenuFontTo: 'DejaVu' 10)		(setWindowTitleFontTo: 'DejaVu' 12)		(setBalloonHelpFontTo: 'DejaVu' 8)		(setCodeFontTo: 'DejaVu' 10)		(setButtonFontTo: 'DejaVu' 10))! !!StrikeFont class methodsFor: 'removing' stamp: 'jmv 7/27/2009 19:07'!removeSomeFonts"StrikeFont removeSomeFonts"	| familyDict |	familyDict _ AvailableFonts at: 'DejaVu'.	familyDict keys copy do: [ :k |		(#(5 6 7 8 9 10 11 12 14 16 18 20 24) includes: k)			ifTrue: [ (familyDict at: k) derivativeFont: nil at: 3 ].		(#(5 6 7 8 11 12 14 16 20 24) includes: k)			ifTrue: [ (familyDict at: k) derivativeFont: nil at: 0 ].		(#(5 6 7 8 9 10 11 12 14 16 18 20 24) includes: k) 			ifFalse: [ familyDict removeKey: k ]].		Preferences setDefaultFonts: #(		(setSystemFontTo: 'DejaVu' 9)		(setListFontTo: 'DejaVu' 9)		(setMenuFontTo: 'DejaVu' 10)		(setWindowTitleFontTo: 'DejaVu' 12)		(setBalloonHelpFontTo: 'DejaVu' 8)		(setCodeFontTo: 'DejaVu' 9)		(setButtonFontTo: 'DejaVu' 9))! !!TextStyle methodsFor: 'accessing' stamp: 'jmv 7/27/2009 20:50'!font	^font! !!TextStyle methodsFor: 'accessing' stamp: 'jmv 7/27/2009 20:46'!font: aFont	font _ aFont.fontArray _ Array with: aFont.defaultFontIndex _ 1.	lineGrid _ aFont height + leading.	baseline _ aFont ascent + leading.	alignment _ 0.	firstIndent _ 0.	restIndent _ 0.	rightIndent _ 0.	tabsArray _ TextStyle defaultTabsArray.	marginTabsArray _ TextStyle defaultMarginTabsArray! !!TextStyle methodsFor: 'fonts and font indexes' stamp: 'jmv 7/27/2009 20:38'!consistOnlyOf: aFont	fontArray _ Array with: aFont.	defaultFontIndex _ 1.	self temporaryFix.! !!TextStyle methodsFor: 'private' stamp: 'jmv 7/27/2009 20:45'!newFontArray: anArray	"Currently there is no supporting protocol for changing these arrays. If an editor wishes to implement margin setting, then a copy of the default should be stored with these instance variables.  	, Make size depend on first font."	fontArray _ anArray.defaultFontIndex _ 1.	lineGrid _ (fontArray at: 1) height + leading.	"For whole family"	baseline _ (fontArray at: 1) ascent + leading.	alignment _ 0.	firstIndent _ 0.	restIndent _ 0.	rightIndent _ 0.	tabsArray _ TextStyle defaultTabsArray.	marginTabsArray _ TextStyle defaultMarginTabsArray.	self temporaryFix."TextStyle allInstancesDo: [:ts | ts newFontArray: TextStyle default fontArray]."! !!TextStyle methodsFor: 'as yet unclassified' stamp: 'jmv 7/27/2009 14:12'!temporaryFix	font _ self defaultFont! !!TextStyle class methodsFor: 'instance creation' stamp: 'jmv 7/27/2009 17:30'!withFont: aFont	^self new font: aFont! !TextStyle class removeSelector: #fontSizesFor:!TextStyle class removeSelector: #fontWidthsFor:!TextStyle class removeSelector: #temporaryInitialize!TextStyle removeSelector: #fontAt:put:!TextStyle removeSelector: #fontNames!TextStyle removeSelector: #fontNamesAndSizes!!classDefinition: #TextStyle category: #'Graphics-Text'!Object subclass: #TextStyle	instanceVariableNames: 'font fontArray lineGrid baseline alignment firstIndent restIndent rightIndent tabsArray marginTabsArray leading defaultFontIndex'	classVariableNames: 'AvailableTextStyles DefaultTextStyle'	poolDictionaries: ''	category: 'Graphics-Text'!StrikeFont removeSelector: #textStyle!AbstractFont initialize!!classDefinition: #AbstractFont category: #'Graphics-Text'!Object subclass: #AbstractFont	instanceVariableNames: ''	classVariableNames: 'AvailableFonts DefaultFont'	poolDictionaries: ''	category: 'Graphics-Text'!"Postscript:Leave the line above, and replace the rest of this comment by a useful one.Executable statements should follow this comment, and shouldbe separated by periods, with no exclamation points (!!).Be sure to put any further comments in double-quotes, like this one."TextStyle allInstancesDo: [ :style | style temporaryFix ].AbstractFont initialize.PopUpMenu inform: 'Install new fonts prior to loading next change set!! (StrikeFont installDejaVu)'!