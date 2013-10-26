'From Squeak3.7 of ''4 September 2004'' [latest update: #5989] on 30 January 2009 at 3:48:56 pm'!!classDefinition: #ProgressInitiationException category: #'System-Exceptions Kernel'!Exception subclass: #ProgressInitiationException	instanceVariableNames: 'workBlock maxVal minVal aPoint progressTitle currentVal'	classVariableNames: ''	poolDictionaries: ''	category: 'System-Exceptions Kernel'!!DisplayText methodsFor: 'private' stamp: 'jmv 1/30/2009 13:04'!composeForm
	"For the TT strings in MVC widgets in a Morphic world such as a progress bar, the form is created by Morphic machinery.	jmv- NO, let's do it in the old, fast way!!"
	"
	| canvas tmpText |
	tmpText := OldTextMorph new contentsAsIs: text deepCopy.
			foreColor 
				ifNotNil: [tmpText text addAttribute: (TextColor color: foreColor)].
			backColor ifNotNil: [tmpText backgroundColor: backColor].
			tmpText setTextStyle: textStyle.
			canvas := FormCanvas on: (Form extent: tmpText extent depth: 32).
			tmpText drawOn: canvas.
			form := canvas form .	"	form _ self asParagraph asForm! !!Paragraph methodsFor: 'converting' stamp: 'jmv 1/30/2009 13:02'!asForm	"Answer a Form made up of the bits that represent the receiver's displayable text."	| theForm |	theForm _ (Form extent: compositionRectangle extent depth: 32)		offset: offset.	self displayOn: theForm		at: 0@0		clippingBox: theForm boundingBox		rule: Form over		fillColor: nil.	^ theForm"Example:| p |p _ 'Abc' asParagraph.p foregroundColor: Color red backgroundColor: Color black.p asForm displayOn: Display at: 30@30 rule: Form over"! !!Preferences class methodsFor: 'themes' stamp: 'jmv 1/30/2009 11:50'!slowMachine1

	self setPreferencesFrom:

	#(
		(optionalButtons false)
		(smartUpdating false)		(subPixelRenderFonts false)	)! !!ProgressInitiationException methodsFor: 'as yet unclassified' stamp: 'jmv 1/30/2009 15:41'!defaultAction	| delta savedArea captionText textFrame barFrame outerFrame result range lastW w |	barFrame _ aPoint - (75@10) corner: aPoint + (75@10).	captionText _ DisplayText text: progressTitle asText.	captionText		foregroundColor: Color black		backgroundColor: Color white.	textFrame _ captionText boundingBox insetBy: -4.	textFrame _ textFrame align: textFrame bottomCenter					with: barFrame topCenter + (0@2).	outerFrame _ barFrame merge: textFrame.	delta _ outerFrame amountToTranslateWithin: Display boundingBox.	barFrame _ barFrame translateBy: delta.	textFrame _ textFrame translateBy: delta.	outerFrame _ outerFrame translateBy: delta.	savedArea _ Form fromDisplay: outerFrame.	Display fillBlack: barFrame; fillWhite: (barFrame insetBy: 2).	Display fillBlack: textFrame; fillWhite: (textFrame insetBy: 2).	captionText displayOn: Display at: textFrame topLeft + (4@4).	range _ maxVal = minVal ifTrue: [1] ifFalse: [maxVal - minVal].  "Avoid div by 0"	lastW _ 0.	result _ workBlock value:  "Supply the bar-update block for evaluation in the work block"		[ :barVal |		barVal notNil			ifTrue: [ currentVal _ barVal ]			ifFalse: [						currentVal _ currentVal + 1.				currentVal >= maxVal					ifTrue: [ currentVal _ minVal ]].		w _ ((barFrame width-4) asFloat * ((currentVal-minVal) asFloat / range min: 1.0)) asInteger.		w < lastW ifTrue: [			Display fillWhite: (barFrame insetBy: 2)].		w ~= lastW ifTrue: [			Display fillGray: (barFrame topLeft + (2@2) extent: w@16).			lastW _ w]].	savedArea displayOn: Display at: outerFrame topLeft.	self resume: result! !!ProgressInitiationException methodsFor: 'as yet unclassified' stamp: 'jmv 1/30/2009 15:24'!display: argString at: argPoint from: argMinVal to: argMaxVal during: argWorkBlock	progressTitle _ argString.	aPoint _ argPoint.	minVal _ argMinVal.	maxVal _ argMaxVal.	currentVal _ minVal.	workBlock _ argWorkBlock.	^self signal! !!String methodsFor: 'displaying' stamp: 'jmv 1/30/2009 15:41'!displayProgressAt: aPoint from: minVal to: maxVal during: workBlock 	"Display this string as a caption over a progress bar while workBlock is evaluated.EXAMPLE (Select next 6 lines and Do It)'Now here''s some Real Progress'	displayProgressAt: Sensor cursorPoint	from: 0 to: 10	during: [:bar |	1 to: 10 do: [:x | bar value: x.			(Delay forMilliseconds: 100) wait]].	'Now here''s some Real Progress'	displayProgressAt: Sensor cursorPoint	from: 0 to: 10	during: [:bar |	1 to: 30 do: [:x | bar value: x \\ 11.			(Delay forMilliseconds: 100) wait]].'Now here''s some Real Progress'	displayProgressAt: Sensor cursorPoint	from: 0 to: 10	during: [:bar |	1 to: 30 do: [:x | bar value: nil.			(Delay forMilliseconds: 200) wait]].HOW IT WORKS (Try this in any other language :-)Since your code (the last 2 lines in the above example) is in a block,this method gets control to display its heading before, and clean up the screen after, its execution.The key, though, is that the block is supplied with an argument,named 'bar' in the example, which will update the bar image every it is sent the message value: x, where x is in the from:to: range.The use of ProgressInitiationException allows for avoiding actualprogress display, by catching the exception."	^ProgressInitiationException 		display: self		at: aPoint 		from: minVal 		to: maxVal 		during: workBlock! !!String methodsFor: 'displaying' stamp: 'jmv 1/30/2009 12:02'!newdisplayProgressAt: aPoint from: minVal to: maxVal during: workBlock 	"Display this string as a caption over a progress bar while workBlock is evaluated.EXAMPLE (Select next 6 lines and Do It)'Now here''s some Real Progress'	displayProgressAt: Sensor cursorPoint	from: 0 to: 10	during: [:bar |	1 to: 10 do: [:x | bar value: x.			(Delay forMilliseconds: 500) wait]].HOW IT WORKS (Try this in any other language :-)Since your code (the last 2 lines in the above example) is in a block,this method gets control to display its heading before, and clean up the screen after, its execution.The key, though, is that the block is supplied with an argument,named 'bar' in the example, which will update the bar image every it is sent the message value: x, where x is in the from:to: range."	^ProgressInitiationException 		display: self		at: aPoint 		from: minVal 		to: maxVal 		during: workBlock! !