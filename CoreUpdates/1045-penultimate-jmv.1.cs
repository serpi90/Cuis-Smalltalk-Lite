'From Cuis 3.3 of 2 June 2011 [latest update: #1024] on 28 June 2011 at 9:10:23 am'!!Collection methodsFor: 'private' stamp: 'jmv 6/28/2011 09:09'!errorCollectionToSmall	self error: 'this collection is too small'! !!SequenceableCollection methodsFor: 'accessing' stamp: 'jmv 6/28/2011 09:10'!penultimate	"Answer the penultimate element of the receiver.	Raise an error if the collection is empty or has just one element."	| size |	(size _ self size) < 2 ifTrue: [self errorCollectionToSmall].	^ self at: size-1! !