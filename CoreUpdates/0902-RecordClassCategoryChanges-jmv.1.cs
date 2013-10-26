'From Cuis 3.1 of 4 March 2011 [latest update: #850] on 5 April 2011 at 2:28:34 pm'!!ChangeSet methodsFor: 'change logging' stamp: 'jmv 4/5/2011 14:24'!changeClassCategory: class	"Remember that a class definition has been changed. 	Based on #changeClass:from:, but simplified knowing that only the class category actually changed."	class wantsChangeSetLogging ifFalse: [^ self].	self atClass: class add: #change.	self addCoherency: class name! !!ChangeSet methodsFor: 'change logging' stamp: 'jmv 4/5/2011 14:27'!event: anEvent	"Hook for SystemChangeNotifier"	(anEvent isRemoved and: [anEvent itemKind = SystemChangeNotifier classKind]) 		ifTrue: [self noteRemovalOf: anEvent item].	(anEvent isAdded and: [anEvent itemKind = SystemChangeNotifier classKind]) 		ifTrue: [self addClass: anEvent item].	(anEvent isModified and: [anEvent itemKind = SystemChangeNotifier classKind])		ifTrue: [anEvent anyChanges ifTrue: [self changeClass: anEvent item from: anEvent oldItem]].	(anEvent isCommented and: [anEvent itemKind = SystemChangeNotifier classKind])		ifTrue: [self commentClass: anEvent item].	(anEvent isAdded and: [anEvent itemKind = SystemChangeNotifier methodKind])		ifTrue: [self noteNewMethod: anEvent item forClass: anEvent itemClass selector: anEvent itemSelector priorMethod: nil].	(anEvent isModified and: [anEvent itemKind = SystemChangeNotifier methodKind])		ifTrue: [self noteNewMethod: anEvent item forClass: anEvent itemClass selector: anEvent itemSelector priorMethod: anEvent oldItem].	(anEvent isRemoved and: [anEvent itemKind = SystemChangeNotifier methodKind])		ifTrue: [self removeSelector: anEvent itemSelector class: anEvent itemClass priorMethod: anEvent item lastMethodInfo: {anEvent item sourcePointer. anEvent itemProtocol}].	(anEvent isRenamed and: [anEvent itemKind = SystemChangeNotifier classKind])		ifTrue: [self renameClass: anEvent item as: anEvent newName].	(anEvent isReorganized and: [anEvent itemKind = SystemChangeNotifier classKind])		ifTrue: [self reorganizeClass: anEvent item].	(anEvent isRecategorized and: [anEvent itemKind = SystemChangeNotifier methodKind])		ifTrue: [self reorganizeClass: anEvent itemClass].	(anEvent isRecategorized and: [anEvent itemKind = SystemChangeNotifier classKind])		ifTrue: [self changeClassCategory: anEvent itemClass].! !