'From Cuis 1.0 of 21 May 2009 [latest update: #204] on 15 June 2009 at 9:26:14 pm'!!CPUWatcher methodsFor: 'startup-shutdown' stamp: 'jmv 6/15/2009 21:19'!monitorProcessPeriod: secs sampleRate: msecs	self stopMonitoring.	watcher _ [ [ | promise |		promise _ Processor tallyCPUUsageFor: secs every: msecs.		tally _ promise value.		promise _ nil.		self findThePig.	] repeat ] newProcess.	watcher priority: Processor highestPriority.	watcher resume.	Processor yield ! !!InputSensor methodsFor: 'user interrupts' stamp: 'jmv 6/15/2009 21:21'!installInterruptWatcher	"Initialize the interrupt watcher process. Terminate the old process if any."	"Sensor installInterruptWatcher"	InterruptWatcherProcess ifNotNil: [InterruptWatcherProcess terminate].	InterruptSemaphore _ Semaphore new.	InterruptWatcherProcess _ [self userInterruptWatcher] newProcess.	InterruptWatcherProcess priority: Processor lowIOPriority.	InterruptWatcherProcess resume.	self primInterruptSemaphore: InterruptSemaphore! !!EventSensor methodsFor: 'private' stamp: 'jmv 6/15/2009 21:20'!installEventTickler	"Initialize the interrupt watcher process. Terminate the old process if any."	"Sensor installEventTickler"	EventTicklerProcess ifNotNil: [ EventTicklerProcess terminate ].	EventTicklerProcess _ [ self eventTickler ] newProcess.	EventTicklerProcess priority: Processor lowIOPriority.	EventTicklerProcess resume! !!ProcessBrowser methodsFor: 'updating' stamp: 'jmv 6/15/2009 21:15'!startAutoUpdate	self isAutoUpdatingPaused ifTrue: [ ^autoUpdateProcess resume ].	self isAutoUpdating		ifFalse: [			| delay | 			delay _ Delay forSeconds: 2.			autoUpdateProcess _ [					[ self hasView ]						whileTrue: [delay wait.							deferredMessageRecipient ifNotNil: [								deferredMessageRecipient addDeferredUIMessage: [self updateProcessList]]							ifNil: [ self updateProcessList ]].					autoUpdateProcess _ nil			] newProcess.			autoUpdateProcess resume.		].	self updateProcessList! !!SemaphoreTest methodsFor: 'testing' stamp: 'jmv 6/15/2009 21:24'!testSemaAfterCriticalWait	"self run: #testSemaAfterCriticalWait"	"This tests whether a semaphore that has just left the wait in Semaphore>>critical:	leaves it with signaling the associated semaphore."	| s p |	s := Semaphore new.	p := [ s critical: []] newProcess.	p priority: Processor activePriority-1.	p resume.	"wait until p entered the critical section"	[ p suspendingList == s ] whileFalse: [ (Delay forMilliseconds: 10) wait ].	"Now that p entered it, signal the semaphore. p now 'owns' the semaphore	but since we are running at higher priority than p it will not get to do	anything."	s signal.	p terminate.	self assert: [ (s instVarNamed: #excessSignals) = 1 ]! !!SemaphoreTest methodsFor: 'testing' stamp: 'jmv 6/15/2009 21:17'!testSemaInCriticalWait	"self run: #testSemaInCriticalWait"	"This tests whether a semaphore that has entered the wait in Semaphore>>critical:	leaves it without signaling the associated semaphore."	| s p |	s := Semaphore new.	p := [s critical:[]] newProcess.	p resume.	Processor yield.	self assert:[p suspendingList == s].	p terminate.	self assert:[(s instVarNamed: #excessSignals) = 0]! !!SemaphoreTest methodsFor: 'testing' stamp: 'jmv 6/15/2009 21:17'!testWaitAndWaitTimeoutTogether	| semaphore value waitProcess waitTimeoutProcess |	semaphore := Semaphore new.		waitProcess := [semaphore wait. value := #wait] newProcess.	waitProcess resume.	waitTimeoutProcess := [semaphore waitTimeoutMSecs: 50. value := #waitTimeout] newProcess.	waitTimeoutProcess resume.	"Wait for the timeout to happen"	(Delay forMilliseconds: 100) wait.	"The waitTimeoutProcess should already have timed out.  This should release the waitProcess"	semaphore signal.	[waitProcess isTerminated and: [waitTimeoutProcess isTerminated]]		whileFalse: [(Delay forMilliseconds: 100) wait].	self assert: value = #wait.	! !!SystemDictionary methodsFor: 'miscellaneous' stamp: 'jmv 6/15/2009 21:18'!handleUserInterrupt	Preferences cmdDotEnabled ifTrue: [		[ProjectX currentInterruptNameX: 'User Interrupt'] fork]! !!WeakArray class methodsFor: 'private' stamp: 'jmv 6/15/2009 21:25'!restartFinalizationProcess	"kill any old process, just in case"	FinalizationProcess		ifNotNil: [ 			FinalizationProcess terminate.			FinalizationProcess := nil ].	"Check if Finalization is supported by this VM"	IsFinalizationSupported := nil.	self isFinalizationSupported		ifFalse: [^ self].	FinalizationSemaphore := Smalltalk specialObjectsArray at: 42.	FinalizationDependents ifNil: [FinalizationDependents := WeakArray new: 10].	FinalizationLock := Semaphore forMutualExclusion.	FinalizationProcess := [ self finalizationProcess ] newProcess.	FinalizationProcess priority: Processor userInterruptPriority.	FinalizationProcess resume! !