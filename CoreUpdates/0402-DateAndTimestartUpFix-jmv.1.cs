'From Cuis 1.0 of 28 November 2009 [latest update: #337] on 1 February 2010 at 3:50:20 pm'!!DateAndTime class methodsFor: 'initialize-release' stamp: 'jmv 2/1/2010 15:50'!startUp: resuming 	resuming ifFalse: [^ self].		"jmv: This used to run forked. It meant that during startup process, DateAndTime could fail."	self initializeOffsets	! !