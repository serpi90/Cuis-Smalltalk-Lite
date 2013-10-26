'From Squeak3.10.2bc-beta of 16 December 2008 [latest update: #7179] on 24 March 2009 at 11:24:57 pm'!"Change Set:		DelayWaitTimeoutDate:			23 March 2009Author:			Andreas RaabProvides a light-weight, correct implementation of waitTimeoutMSecs:"!Delay subclass: #DelayWaitTimeout	instanceVariableNames: 'process expired'	classVariableNames: ''	poolDictionaries: ''	category: 'Kernel-Processes'!!DelayWaitTimeout commentStamp: '<historical>' prior: 0!DelayWaitTimeout is a special kind of Delay used in waitTimeoutMSecs: to avoid signaling the underlying semaphore when the wait times out.!!DelayWaitTimeout methodsFor: 'testing' stamp: 'ar 3/23/2009 16:37'!isExpired	"Did this timeout fire before the associated semaphore was signaled?"	^expired! !!DelayWaitTimeout methodsFor: 'private' stamp: 'ar 3/23/2009 16:38'!setDelay: anInteger forSemaphore: aSemaphore	super setDelay: anInteger forSemaphore: aSemaphore.	process := Processor activeProcess.	expired := false.! !!DelayWaitTimeout methodsFor: 'signaling' stamp: 'ar 3/24/2009 23:24'!signalWaitingProcess	"Release the given process from the semaphore it is waiting on.	This method relies on running at highest priority so that it cannot be preempted	by the process being released."	beingWaitedOn := false.	"Release the process but only if it is still waiting on its original list"	process suspendingList == delaySemaphore ifTrue:[		expired := true.		process suspend; resume.	].! !!Semaphore methodsFor: 'communication' stamp: 'ar 3/23/2009 17:02'!waitTimeoutMSecs: anInteger	"Wait on this semaphore for up to the given number of milliseconds, then timeout. 	Return true if the deadline expired, false otherwise."	| d |	d := DelayWaitTimeout timeoutSemaphore: self afterMSecs: (anInteger max: 0).	[self wait] ensure:[d unschedule].	^d isExpired! !!SemaphoreTest methodsFor: 'testing' stamp: 'jf 12/2/2003 19:31'!testWaitAndWaitTimeoutTogether	| semaphore value waitProcess waitTimeoutProcess |	semaphore := Semaphore new.		waitProcess := [semaphore wait. value := #wait] fork.	waitTimeoutProcess := [semaphore waitTimeoutMSecs: 50. value := #waitTimeout] fork.	"Wait for the timeout to happen"	(Delay forMilliseconds: 100) wait.	"The waitTimeoutProcess should already have timed out.  This should release the waitProcess"	semaphore signal.	[waitProcess isTerminated and: [waitTimeoutProcess isTerminated]]		whileFalse: [(Delay forMilliseconds: 100) wait].	self assert: value = #wait.	! !!SemaphoreTest methodsFor: 'testing' stamp: 'ar 3/23/2009 17:01'!testWaitTimeoutMSecs	"Ensure that waitTimeoutMSecs behaves properly"	"Ensure that a timed out waitTimeoutMSecs: returns true from the wait"	self assert: (Semaphore new waitTimeoutMSecs: 50) == true.	"Ensure that a signaled waitTimeoutMSecs: returns false from the wait"	self assert: (Semaphore new signal waitTimeoutMSecs: 50) == false.! !