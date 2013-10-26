'From Cuis 2.0 of 12 February 2010 [latest update: #418] on 22 February 2010 at 2:18:14 pm'!!DifferenceFinder methodsFor: 'private' stamp: 'jmv 2/22/2010 10:24'!keywordsAndBlanksFrom: aString	^Array streamContents: [:strm | | read keyword tail |		read := aString readStream.		[read atEnd] whileFalse: [			keyword := read nextKeyword.			keyword notEmpty ifTrue: [				strm nextPut: keyword ].			tail := read untilAnySatisfying: [:ch | ch isAlphaNumeric].			tail notEmpty ifTrue: [strm nextPut: tail]]]! !!DifferenceFinder methodsFor: 'private' stamp: 'jmv 2/22/2010 11:36'!maxLength	(tally width = 0 or: [ tally height = 0 ]) ifTrue: [ ^0 ].	^tally i: x size j: y size! !!DifferenceFinder class methodsFor: 'compatibility' stamp: 'jmv 2/22/2010 13:08'!displayPatchFrom: srcString to: dstString tryWords: aBoolean	| finder |	aBoolean ifTrue: [		(self wordsDisplayPatchFrom: srcString to: dstString)			ifNotNil: [ :answer | ^answer ] ].	finder _ self base: srcString case: dstString.	finder compareLines; compute.	^finder differences anyOne asText! !!DifferenceFinder class methodsFor: 'compatibility' stamp: 'jmv 2/22/2010 14:17'!wordsDisplayPatchFrom: srcString to: dstString	| finder answer src1 dst1 changedCount |	finder _ self base: srcString case: dstString.	finder compareLines; compute.	answer _ '' asText.	src1 _ '' writeStream.	dst1 _ '' writeStream.	changedCount _ 0.	finder differences sort first do: [:item :condition |		condition caseOf: {			[ #unchanged ] -> [				changedCount > 0 ifTrue: [					"If the sequence of changed lines is large, comparing words gets too slow and less useful"					changedCount > 10 ifTrue: [						^nil ].					"Compare the just ended sequence of changed lines"					finder base: src1 contents case: dst1 contents.					finder compareWords; compute.					answer _ answer append:  finder differences anyOne asText.					src1 resetToStart.					dst1 resetToStart.					changedCount _ 0.				].				"This line hasn't changed. Just add it to the result in plain text."				answer append: item ].			[ #removed ] -> [				"A removed line belongs in the source"				src1 nextPutAll: item.				changedCount _ changedCount + 1 ].			[ #inserted ] -> [				"An added line belongs in the destination"				dst1 nextPutAll: item.				changedCount _ changedCount + 1  ].			}.		].	finder base: src1 contents case: dst1 contents.	finder compareWords; compute.	answer append:  finder differences anyOne asText.	^answer! !DifferenceFinder removeSelector: #linesAreSimilar!DifferenceFinder removeSelector: #recomputeWithWords!