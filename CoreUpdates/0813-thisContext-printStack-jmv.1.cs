'From Cuis 3.0 of 31 January 2011 [latest update: #790] on 23 February 2011 at 4:04:35 pm'!!BlockClosure methodsFor: 'printing' stamp: 'jmv 2/23/2011 16:04'!printStack: depth	self print.	depth > 0 ifTrue: [		self sender ifNotNil: [ :s | s printStack: depth-1 ]]! !!ContextPart methodsFor: 'printing' stamp: 'jmv 2/23/2011 16:04'!printStack: depth	self print.	depth > 0 ifTrue: [		self sender ifNotNil: [ :s | s printStack: depth-1 ]]! !