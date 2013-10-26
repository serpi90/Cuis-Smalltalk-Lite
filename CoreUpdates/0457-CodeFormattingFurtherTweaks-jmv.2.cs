'From Cuis 2.0 of 24 February 2010 [latest update: #440] on 3 March 2010 at 9:16:23 am'!!BlockNode methodsFor: 'printing' stamp: 'jmv 3/3/2010 09:09'!printOn: aStream indent: level 	| separateLines |	aStream		 nextPut:  $[ ;		 space.	self		printArgumentsOn:  aStream		indent:  level.	separateLines _ (self		printTemporaries: temporaries		on:  aStream		doPrior:  [ ] ) or: [ arguments size > 0 ].	separateLines ifTrue: 		[ "If args+temps > 0 and statements > 1 (or just one complex statement), put all statements on separate lines"		(statements size >  1 or: [ statements size = 1 and: [statements first isComplex]])			ifTrue:  [ aStream crtab:  level ] 			ifFalse:  [ aStream space] ] .	self		printStatementsOn:  aStream		indent:  level.	aStream		 space ;		 nextPut:  $]! !!BraceNode methodsFor: 'printing' stamp: 'jmv 3/3/2010 08:58'!printOn: aStream indent: level	aStream nextPut: ${.	1 to: elements size do: 		[ :i | 		(elements at: i) printOn: aStream indent: level.		i < elements size ifTrue: [aStream nextPutAll: '. ']].	aStream nextPut: $}	" Alternative implementation: Each element in its own line, indented as a rectangular block	aStream		 crtab: level+1.	aStream nextPut: ${.	1		to: elements size		do: 			[ : i | 			(elements at: i)				printOn: aStream				indent: level.			i < elements size ifTrue: 				[ aStream					 nextPutAll: '. ' ;					 crtab: level+1 ] ].	aStream nextPut: $}	"! !!Preferences class methodsFor: 'standard queries' stamp: 'jmv 3/3/2010 09:14'!syntaxHighlightingAsYouTypeLeftArrowAssignment	^ self		valueOfFlag: #syntaxHighlightingAsYouTypeLeftArrowAssignment		ifAbsent: [ true ]! !!SHTextStylerST80 class methodsFor: 'preferences' stamp: 'jmv 3/3/2010 08:44'!initializePreferences		(Preferences preferenceAt: #syntaxHighlightingAsYouType) ifNil:[		Preferences 			disable: #browseWithPrettyPrint.		Preferences 			addPreference: #syntaxHighlightingAsYouType			 categories: #( browsing)			default: true 			balloonHelp: 'Enable, or disable, Shout - Syntax Highlighting As You Type. When enabled, code in Browsers and Workspaces is styled to reveal its syntactic structure. When the code is changed (by typing some characters, for example), the styling is changed so that it remains in sync with the modified code'].	(Preferences preferenceAt: #syntaxHighlightingAsYouTypeAnsiAssignment) ifNil:[		Preferences 			addPreference: #syntaxHighlightingAsYouTypeAnsiAssignment			 categories: #( browsing)			default: false 			balloonHelp: 'If true, and syntaxHighlightingAsYouType is enabled,  all left arrow assignments ( _ ) will be converted to the ANSI format ( := ) when a method is selected in a Browser. Whilst editing a method, this setting has no effect - both the left arrow and the ansi format may be used'.		(Preferences preferenceAt: #syntaxHighlightingAsYouTypeAnsiAssignment)			changeInformee: self			changeSelector: #ansiAssignmentPreferenceChanged].			(Preferences preferenceAt: #syntaxHighlightingAsYouTypeLeftArrowAssignment) ifNil:[		Preferences 			addPreference: #syntaxHighlightingAsYouTypeLeftArrowAssignment		 	categories: #( browsing)			default: true 			balloonHelp: 'If true, and syntaxHighlightingAsYouType is enabled,  all ANSI format assignments ( := ) will be converted to left arrows ( _ ) when a method is selected in a Browser. Whilst editing a method, this setting has no effect - both the left arrow and the ansi format may be used'.		(Preferences preferenceAt: #syntaxHighlightingAsYouTypeLeftArrowAssignment)			changeInformee: self 			changeSelector: #leftArrowAssignmentPreferenceChanged ].							! !"Postscript:Leave the line above, and replace the rest of this comment by a useful one.Executable statements should follow this comment, and shouldbe separated by periods, with no exclamation points (!!).Be sure to put any further comments in double-quotes, like this one."Preferences removePreference: #syntaxHighlightingAsYouTypeLeftArrowAssignment.SHTextStylerST80 initializePreferences.!