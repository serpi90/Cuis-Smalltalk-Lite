'From Cuis 2.0 of 24 February 2010 [latest update: #440] on 2 March 2010 at 11:08:23 am'!!Behavior methodsFor: 'accessing method dictionary' stamp: 'jmv 3/2/2010 10:44'!compiledMethodAt: selector ifAbsent: aBlock	"Answer the compiled method associated with the argument, selector (a Symbol), a message selector in the receiver's method dictionary. If the selector is not in the dictionary, return the value of aBlock"	^ self methodDict at: selector ifAbsent: aBlock! !!FileDirectory methodsFor: 'file status' stamp: 'jmv 3/2/2010 10:49'!entryAt: fileName ifAbsent: aBlock	"Find the entry with local name fileName and answer it.	If not found, answer the result of evaluating aBlock."	| comparisonBlock |	comparisonBlock _ self isCaseSensitive		ifTrue: [[:entry | (entry at: 1) = fileName]]		ifFalse: [[:entry | (entry at: 1) sameAs: fileName]].	^ self entries detect: comparisonBlock ifNone: aBlock! !!LimitedWriteStream methodsFor: 'accessing' stamp: 'jmv 3/2/2010 10:54'!nextPut: anObject 	"Ensure that the limit is not exceeded"	^position >= limit		ifTrue: [limitBlock value]		ifFalse: [super nextPut: anObject]! !!Preferences class methodsFor: 'parameters' stamp: 'jmv 3/2/2010 10:50'!parameterAt: aKey ifAbsent: aBlock	"Answer the parameter saved at the given key; if there is no such key in the Parameters dictionary, evaluate aBlock"	^ Parameters at: aKey ifAbsent: aBlock! !!Preferences class methodsFor: 'preference-object access' stamp: 'jmv 3/2/2010 10:50'!preferenceAt: aSymbol ifAbsent: aBlock	"Answer the Preference object at the given symbol, or the value of aBlock if not present"	^ DictionaryOfPreferences at: aSymbol ifAbsent: aBlock! !!Semaphore methodsFor: 'mutual exclusion' stamp: 'jmv 3/2/2010 11:06'!critical: mutuallyExcludedBlock ifError: errorBlock	"Evaluate mutuallyExcludedBlock only if the receiver is not currently in 	the process of running the critical: message. If the receiver is, evaluate 	mutuallyExcludedBlock after the other critical: message is finished."	| blockValue hasError errMsg errRcvr |	hasError := false.	self critical:[		blockValue := mutuallyExcludedBlock ifError: [ :msg :rcvr |			hasError := true.			errMsg := msg.			errRcvr := rcvr		].	].	hasError ifTrue: [ ^errorBlock value: errMsg value: errRcvr].	^blockValue! !!TimeProfileBrowser methodsFor: 'private' stamp: 'jmv 3/2/2010 10:43'!messageListKey: aChar from: view 	"Respond to a Command key. Cmd-D means re-run block."	aChar == $d ifTrue: [^Cursor execute showWhile: block ].	^super messageListKey: aChar from: view! !!Utilities class methodsFor: 'user interface' stamp: 'jmv 3/2/2010 10:50'!informUser: aString during: aBlock 	"Display a message above (or below if insufficient room) the cursor during execution of the given block."	"Utilities informUser: 'Just a sec!!' during: [(Delay forSeconds: 1) wait]"	(MVCMenuMorph from: (SelectionMenu labels: '') title: aString) 				displayAt: Sensor cursorPoint				during: aBlock! !