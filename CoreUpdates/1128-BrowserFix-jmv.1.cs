'From Cuis 3.3 of 2 June 2011 [latest update: #1024] on 4 November 2011 at 10:36:10 pm'!!Browser methodsFor: 'accessing' stamp: 'jmv 11/4/2011 22:36'!         contents: input notifying: aController	"The retrieved information has changed and its source must now be	 updated. The information can be a variety of things, depending on	 the list selections (such as templates for class or message definition,	 methods) or the user menu commands (such as definition, comment,	 hierarchy).  Answer the result of updating the source."	| aString aText theClass |	aString _ input asString.	aText _ input asText.	editSelection == #editSystemCategories ifTrue: [ ^ self changeSystemCategories: aString ].	editSelection == #editClass | (editSelection == #newClass) ifTrue: [ ^ self defineClass: aString notifying: aController ].	editSelection == #editComment		ifTrue: [			theClass _ self selectedClass.			theClass				ifNil: [					self inform: 'You must select a classbefore giving it a comment.'.					^ false].			theClass comment: aText stamp: Utilities changeStamp.			self changed: #classCommentText.			^ true].	editSelection == #hierarchy ifTrue: [ ^ true ].	editSelection == #editMessageCategories ifTrue: [ ^ self changeMessageCategories: aString ].	editSelection == #editMessage | (editSelection == #newMessage)		ifTrue: [			^ self okayToAccept				ifFalse:[ false ]				ifTrue: [					(self compileMessage: aText notifying: aController)						ifTrue: [ self changed: #annotation ];						yourself ]].	editSelection == #none		ifTrue: [			self inform: 'This text cannot be acceptedin this part of the browser.'.			^ false].	self error: 'unacceptable accept'! !