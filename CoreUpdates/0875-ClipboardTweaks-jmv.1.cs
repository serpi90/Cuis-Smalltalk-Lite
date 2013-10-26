'From Cuis 3.1 of 4 March 2011 [latest update: #850] on 17 March 2011 at 9:28:38 am'!!Clipboard methodsFor: 'accessing' stamp: 'jmv 3/17/2011 09:20'!retrieveObject	"Answer whatever was last stored in the clipboard"	| stringOrNil |	"If the OS clipboard has the id for our contents, or the same characters, then answer the richer Smalltalk object.	Note: if the (extended) clipboard contains a serialized object, it shouldn't contain an id, so	it is deserialized even if ivar contents contains the object. This is done to guarantee consistency with pasting	from another Cuis image."	stringOrNil _ self retrieveIdOrStringFromOS.	(stringOrNil = (self idFor: contents) or: [ stringOrNil = contents asString])		ifTrue: [			"We copy the object, because the result of each paste operation could be modified independently of the others afterwards			(and the same clipboard contents might be pasted many times)"			^contents copyForClipboard ].	"If we have the ExtendedClipboardInterface, try to get an RTF or Form"	Smalltalk at: #ExtendedClipboardInterface ifPresent: [ :clipboardInterface |		clipboardInterface current retrieveObject ifNotNil: [ :object | ^object ]].	"Otherwise answer the string brought by clipboard primitives"	^stringOrNil! !!Clipboard methodsFor: 'private' stamp: 'jmv 3/17/2011 09:16'!retrieveIdOrStringFromOS	"Use a specific content type if ExtendedClipboard is active.	Otherwise, use regular clipboard primitives"	| primitiveFormat |	Smalltalk at: #ExtendedClipboardInterface ifPresent: [ :clipboardInterface |		"Answer nil if no id was stored"		^ clipboardInterface current retrieveId ].			primitiveFormat _ self primitiveClipboardString.	"Clipboard primitives answer an empty string if there is no string in OS clipboard.	We prefer nil"	primitiveFormat isEmpty ifTrue: [ ^nil ].	"The VM uses UTF-8 for clipboard"	^primitiveFormat utf8ToISO8859s15 withSqueakLineEndings! !