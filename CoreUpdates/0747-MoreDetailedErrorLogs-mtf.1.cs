'From Cuis 2.9 of 5 November 2010 [latest update: #634] on 13 January 2011 at 4:10:05 pm'!!ContextPart methodsFor: 'debugger access' stamp: 'jmv 1/13/2011 16:09'!errorReportOn: strm	"Write a detailed error report on the stack (above me) on a stream.  For both the error file, and emailing a bug report.  Suppress any errors while getting printStrings.  Limit the length."	| cnt aContext startPos | 	strm print: Date today; space; print: Time now; cr.	strm cr.	strm nextPutAll: 'VM: ';		nextPutAll: Smalltalk platformName asString;		nextPutAll: ' - ';		nextPutAll: Smalltalk vmVersion asString;		cr.	strm nextPutAll: 'Image: ';		nextPutAll: Smalltalk version asString;		nextPutAll: ' [';		nextPutAll: Smalltalk lastUpdateString asString;		nextPutAll: ']';		cr.	strm cr.		"Note: The following is an open-coded version of ContextPart>>stackOfSize: since this method may be called during a low space condition and we might run out of space for allocating the full stack."	cnt _ 0.  startPos _ strm position.	aContext _ self.	[aContext notNil and: [(cnt _ cnt + 1) < 20]] whileTrue:		[aContext printDetails: strm.	"variable values"		strm cr.		aContext _ aContext sender].	strm cr; nextPutAll: '--- The full stack ---'; cr.	aContext _ self.	cnt _ 0.	[aContext == nil] whileFalse:		[cnt _ cnt + 1.		cnt = 20 ifTrue: [strm nextPutAll: ' - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -'; cr].		strm print: aContext; cr.  "just class>>selector"			strm position > (startPos+40000) ifTrue: [strm nextPutAll: '...etc...'.			^ self]. 	"exit early"		cnt > 60 ifTrue: [strm nextPutAll: '-- and more not shown --'.  ^ self].		aContext _ aContext sender].! !!ContextPart methodsFor: 'printing' stamp: 'jrd 5/6/2010 00:41'!printDetails: strm	"Put my class>>selector and arguments and temporaries on the stream.  Protect against errors during printing."	| str |	self printOn: strm.  	strm cr; tab; nextPutAll: 'Arguments and temporary variables: '; cr.	str := [self tempsAndValuesLimitedTo: 160 indent: 2] ifError: [:err :rcvr | 						'<<error during printing>>'].	strm nextPutAll: str.	strm peekLast == Character cr ifFalse: [strm cr].! !