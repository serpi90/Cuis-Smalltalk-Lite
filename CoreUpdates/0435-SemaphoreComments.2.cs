'From Cuis 2.0 of 12 February 2010 [latest update: #418] on 23 February 2010 at 2:28:04 pm'!!Semaphore methodsFor: 'mutual exclusion' stamp: 'jmv 2/23/2010 14:27'!critical: mutuallyExcludedBlock	"Evaluate mutuallyExcludedBlock only if the receiver is not currently in	the process of running the critical: message. If the receiver is, evaluate	mutuallyExcludedBlock after the other critical: message is finished."		| caught |	"We need to catch eventual interruptions very carefully. 	The naive approach of just doing, e.g.,:		self wait.		aBlock ensure:[self signal].	will fail if the active process gets terminated while in the wait.	However, the equally naive:		[self wait.		aBlock value] ensure:[self signal].	will fail too, since the active process may get interrupted while	entering the ensured block and leave the semaphore signaled twice.	To avoid both problems we make use of the fact that interrupts only	occur on sends (or backward jumps) and use an assignment (bytecode)	right before we go into the wait primitive (which is not a real send and	therefore not interruptable either)."	caught := false.	^[		caught := true.		self wait.		mutuallyExcludedBlock value	] ensure: [ caught ifTrue: [self signal] ]! !!Semaphore methodsFor: 'mutual exclusion' stamp: 'jmv 2/23/2010 11:52'!critical: mutuallyExcludedBlock ifLocked: alternativeBlock	"Evaluate mutuallyExcludedBlock only if the receiver is not currently in 	the process of running the critical: message. If the receiver is, evaluate 	mutuallyExcludedBlock after the other critical: message is finished."		"Note: The following is tricky and depends on the fact that the VM will not switch between processes while executing byte codes (process switches happen only in real sends). The following test is written carefully so that it will result in bytecodes only."	excessSignals == 0 ifTrue:[		"If we come here, then the semaphore was locked when the test executed. 		Evaluate the alternative block and answer its result."		^alternativeBlock value 	].	^self critical: mutuallyExcludedBlock! !