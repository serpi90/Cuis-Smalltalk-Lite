'From Cuis 2.9 of 5 November 2010 [latest update: #634] on 19 December 2010 at 10:59:51 am'!!BareTextMorph methodsFor: 'event handling' stamp: 'jmv 12/19/2010 10:59'!keyboardFocusChange: aBoolean		"The message is sent to a morph when its keyboard focus changes.	The given argument indicates that the receiver is gaining (versus losing) the keyboard focus.	In this case, all we need to do is to redraw border feedback"	paragraph ifNotNil: [ paragraph focused: aBoolean ].	aBoolean		ifTrue: [			"A hand is wanting to send us characters..."			editor ifNil: [ self editor ].	"Forces install"			self startBlinking ]		ifFalse: [ self stopBlinking ].	"Selection might be shown differently when focused"	"If (as usual) we are part of a TextModelMorph, invalidate it, to update the keyboard focus indicator"	owner ifNotNil: [ owner owner ifNotNil: [ :grandPa | ^ grandPa changed ]].	self changed! !