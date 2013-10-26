'From Cuis 2.4 of 17 June 2010 [latest update: #509] on 27 July 2010 at 10:24:53 pm'!!Object methodsFor: 'error handling' stamp: 'jmv 7/27/2010 22:24'!doesNotUnderstand: aMessage 	 "Handle the fact that there was an attempt to send the given	  message to the receiver but the receiver does not understand	  this message (typically sent from the machine when a message	 is sent to the receiver and no method is defined for that selector).		Question: Why is this method different from the one inherited from ProtoObject?	Answer (eem):		This is intentional.  Martin's reply is one half of the issue, that you want		to be able to proceed after defining a method in  the debugger.  The other		half is that you want to be able to catch doesNotUnderstand: in an exception		handler and proceed with a result, e.g.	[nil zork]		on: MessageNotUnderstood		do: [:ex|			ex message selector == #zork ifTrue:				[ex resume: #ok].			ex pass]		evaluates to #ok.			jmv adds:		The real difference is what happens if the exception is eventually handled by the default handler		(i.e. the debugger is opened). I that case, don't allow the user to proceed.	"	"Testing: 		(3 activeProcess)	"	| exception resumeValue |	(exception _ MessageNotUnderstood new)		message: aMessage;		receiver: self.	resumeValue _ exception signal.	^exception reachedDefaultHandler		ifTrue: [ aMessage sentTo: self ]		ifFalse: [ resumeValue ]! !