'From Cuis 2.3 of 22 March 2010 [latest update: #472] on 24 March 2010 at 9:47:16 pm'!!Collection methodsFor: 'enumerating' stamp: 'jmv 3/24/2010 21:45'!count: aBlock	"Evaluate aBlock with each of the receiver's elements as the argument.  Return the number that answered true."	^self inject: 0 into: [ :prevValue :each |		(aBlock value: each) ifTrue: [ prevValue + 1 ] ifFalse: [ prevValue ] ]! !