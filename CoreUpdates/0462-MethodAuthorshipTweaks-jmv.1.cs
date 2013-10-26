'From Cuis 2.0 of 24 February 2010 [latest update: #440] on 10 March 2010 at 2:13:07 pm'!!BlockContext methodsFor: 'accessing' stamp: 'jmv 3/10/2010 14:08'!argumentCount	"Added for ANSI compatibility."	^ self numArgs! !!CodeHolder methodsFor: '*Shout-Styling' stamp: 'jmv 3/10/2010 14:10'!isModeStyleable	"determine if Shout can style in the current mode"	^ self showingSource or: [self showingPrettyPrint]! !!Browser methodsFor: '*Shout-Styling' stamp: 'jmv 3/10/2010 14:10'!shoutAboutToStyle: aSHTextStyler	"This is a notification that aSHTextStyler is about to re-style its text.	Set the classOrMetaClass in aSHTextStyler, so that identifiers	will be resolved correctly.	Answer true to allow styling to proceed, or false to veto the styling"	| type |		self isModeStyleable ifFalse: [^false].	type _ self editSelection.	(#(newMessage editMessage editClass newClass) includes: type) ifFalse:[^false].	aSHTextStyler classOrMetaClass: (type = #editClass ifFalse:[self selectedClassOrMetaClass]).	^true! !!ChangeSorter methodsFor: '*Shout-Styling' stamp: 'jmv 3/10/2010 14:10'!shoutAboutToStyle: aSHTextStyler	"This is a notification that aSHTextStyler is about to re-style its text.	Set the classOrMetaClass in aSHTextStyler, so that identifiers	will be resolved correctly.	Answer true to allow styling to proceed, or false to veto the styling"	self isModeStyleable ifFalse: [^false].	self currentSelector ifNil: [^false].	aSHTextStyler classOrMetaClass: self selectedClassOrMetaClass.	^true! !!Debugger methodsFor: '*Shout-Styling' stamp: 'jmv 3/10/2010 14:10'!shoutAboutToStyle: aSHTextStyler	"This is a notification that aSHTextStyler is about to re-style its text.	Set the classOrMetaClass in aSHTextStyler, so that identifiers	will be resolved correctly.	Answer true to allow styling to proceed, or false to veto the styling"		self isModeStyleable ifFalse: [^false].	aSHTextStyler 		classOrMetaClass: self selectedClassOrMetaClass;		sourceMap: self debuggerMap.	^true! !!FileContentsBrowser methodsFor: '*Shout-Styling' stamp: 'jmv 3/10/2010 14:10'!shoutAboutToStyle: aSHTextStyler	"This is a notification that aSHTextStyler is about to re-style its text.	Set the classOrMetaClass in aSHTextStyler, so that identifiers	will be resolved correctly.	Answer true to allow styling to proceed, or false to veto the styling"	self isModeStyleable ifFalse: [^false].	aSHTextStyler classOrMetaClass: self selectedClassOrMetaClass.	^true! !!MessageSet methodsFor: '*Shout-Styling' stamp: 'jmv 3/10/2010 14:10'!shoutAboutToStyle: aSHTextStyler	"This is a notification that aSHTextStyler is about to re-style its text.	Set the classOrMetaClass in aSHTextStyler, so that identifiers	will be resolved correctly.	Answer true to allow styling to proceed, or false to veto the styling"		self isModeStyleable ifFalse: [^false].	aSHTextStyler classOrMetaClass: self selectedClassOrMetaClass.	^true! !!SystemDictionary methodsFor: 'code authors' stamp: 'jmv 3/10/2010 13:54'!missingAuthorsWithMethods	"	Smalltalk missingAuthorsWithMethods	"	| author signatories answer startDate |	answer _ Dictionary new.	signatories _ self okContributors.	startDate _ self relicenseEffortStartDate.	Smalltalk allBehaviorsDo: [ :behavior |		behavior methodsDo: [ :compiledMethod |			author _ compiledMethod author.			(author notEmpty and: [ (signatories includes: author) not ]) ifTrue: [							compiledMethod dateMethodLastSubmitted < startDate ifTrue: [					(answer at: author ifAbsentPut: [OrderedCollection new])						add: {compiledMethod methodClass. compiledMethod selector}]]]].	^answer! !!SystemDictionary methodsFor: 'code authors' stamp: 'jmv 3/10/2010 13:44'!newContributors"This is a list of all Squeak contributors who started after the relicensing effort was started and therefore know that the license is under MIT.Nov 26th  2008---initials         name"^ #('aw'               'Alessandro Warth''be'			'Bernd Eckardt''cjs'			'(from trac)''eem'		'Eliot Emilio Miranda''jf'			'Jan Fietz''lg'               'Luke Gorrie''jl'			'Jens Linke''JSM'	   'John S Mcintosh''thf'			'an Impara employee''sjg'			'Simon Guest''kph'		'Keith Hodges''kks'            'kks''meta-auto'	'generated code''programmatic'     'generated code''wbk'		'Bryce Kampjes''mtf'         'Matthew Fulmer''rej'			'Ralph Johnson''test'			'Keith Hodges in mc1.5''xxx'			'Keith Hodges in mc1.5''NorbertHartl'			'Norbert Hartl''gvc' 					'Gary Chambers')! !!SystemDictionary methodsFor: 'code authors' stamp: 'jmv 3/10/2010 14:12'!relicenseEffortStartDate	"Contributions done after this day are assumed to be MIT"	"In the Squeak Board meeting held on 6-Dec-2006 it was decided and announced the request that all further contributions must be under the MIT license"		^'06-Dec-2006' asDate! !!SystemDictionary methodsFor: 'code authors' stamp: 'jmv 3/10/2010 13:44'!returnedSignatories"This is a list of all Squeak contributors who have returned a signed distribution agreement.Nov 25th  2008jmv - Taken from Yoshiki Ohshima's squeak4.0-relicensingTools3.zip---initials         name"^ #('AB'               'Alexandre Bergel''ab'               'Avi Bryant or Alexandre Bergel''abc' 			'Colin Putney''ACG'              'Andrew C. Greenberg''acg'              'Andrew C. Greenberg''ack'              'Alan Kay''ads'              'Adam Spitz''AFi'              'Alain Fischer''ajh'              'Anthony Hannan''aka'              'Mark Guzdial''al'               'Adrian Lienhard or Alexander Lazarevic''AlexandreBergel'       'Alexandre Bergel''aoy'              'Andres Otaduy''apb'              'Andrew P. Black''apl'              'Alain Plantec''ar'               'Andreas Raab''???'               'Andreas Raab''asm'              'Alejandro Magistrello''avi'              'Avi Bryant''ba'			'Bob Arning''bf'               'Bert Freudenberg''BEO'              'Bruce O''Neel ''beo'              'Bruce O''Neel ''BG'               'Boris Gaertner''bg'               'Boris Gaertner''bh'               'Bob Hartwig''BJP'              'Bijan Parsia''BJP9/9/1998'              'Bijan Parsia''bk'              'Bolot Kerimbaev''bkv'              'Brent Vukmer''bob'			'Bob Arning''bolot'            'Bolot Kerimbaev''bootstrap'        'Pavel Krivanek''BP'               'Brent Pinkney''bp'               'Brent Pinkney or Bernhard Pieber''brp'              'Brent Pinkney''brp`'              'Brent Pinkney''btr'              'Brian Rice''bvs'              'Ben Schroeder''cbc'              'Chris Cunningham''ccn'              'Chris Norton''ccn+ceg'          'Chris Norton and Carl Gundel''ceg'       'Carl Gundel''CdG'              'Cees de Groot''cds'              'C. David Shaffer''chronograph'               'Tetsuya Hayashi''cmm'              'Chris Muller''crl'              'Craig Latta''cwp'              'Colin Putney''damiencassou'  'Damien Cassou''daf'              'Dave Faught''dao'              'danil osipchuk''das'              'David A Smith''DAS'              'David A Smith''dc'               'Damien Cassou''dd'               'Dominique Dutoit''de'               'Scott Wallace''dew'              'Doug Way''DF'               'Diego Fernandez''dgd'              'Diego Gomez Deck''dhhi'             'Dan Ingalls''di'               'Dan Ingalls''di'               'Dan Ingalls''6/5/97'               'Dan Ingalls''6/6/97'               'Dan Ingalls''6/7/97'               'Dan Ingalls''6/8/97'               'Dan Ingalls''6/9/97'               'Dan Ingalls''6/10/97'               'Dan Ingalls''6/11/97'               'Dan Ingalls''6/13/97'               'Dan Ingalls''6/18/97'               'Dan Ingalls''djp'              'David J. Pennell''DM'               'Duane Maxwell''dm'               'Duane Maxwell/EntryPoint''DSM'              'Duane Maxwell''DSM10/15/1999'              'Duane Maxwell''drm'              'Delbert Murphy''dtl'              'Dave Lewis''dv'              'Daniel Vainsencher''dvf'              'Daniel Vainsencher''dvf6/10/2000'              'Daniel Vainsencher''eat'              'Eric Arseneau Tremblay''edc'              'Edgar DeCleene or Marcus Denker''efc'              'Eddie Cottongim''efo'              'Emilio Oca''em'               'Ernest Micklei?''emm'              'Ernest Micklei''es'               'Enrico Spinielli''fbs'              'Frank Shearar''FBS'              'Frank Shearar''fbs'              'Frank Shearar''fc'               'Frank Caggiano''fcs'              'Frank Sergeant''fm'               'Florin Mateoc''gh'               'Goran Krampe (nee Hultgren)''gk'               'Goran Krampe (nee Hultgren)''gm'               'German Morales''go'               'Georg Gollmann''gsa'              'German Arduino''HEG'              'Henrik Ekenberg''HilaireFernandes' 'Hilaire Fernandes''hk'               'Herbert Konig''hmm'              'Hans-Martin Mosner''hpt'              'Hernan Tylim''huma'             'Lyndon Tremblay''ich.'             'Yuji Ichikawa''ikp'              'Ian Piumarta''jaf'              'Jan Fietz''jam'              'Javier Musa''jb'              'Jim Benson''jcg'              'Joshua Gargus''jdf'              'David Farber''jdr'              'Javier Diaz-Reinoso''je'               'Joern Eyrich''je77'             'Jochen Rick''JF'               'Julian Fitzell''jf'               'Julian Fitzell''jhm'              'John Maloney''jm'              'John Maloney''jm'              'John Maloney''jla'              'Jerry Archibald''jlb'              'Jim Benson''jmb'              'Hans Baveco''JMM'              'John McIntosh''JMV'              'Juan Manuel Vuletich''jmv'              'Juan Manuel Vuletich''jon'              'Jon Hylands''JP'               'Joseph Pelrine''jp'               'Joseph Pelrine''jrm'              'John-Reed Maffeo''jrp'              'John Pierce''jsp'              'Jeff Pierce''JW'               'Jesse Welton''jws'			'John Sarkela''JWS'		'John Sarkela''ka'               'Kazuhiro Abe''kfr'              'Karl Ramberg''KLC'              'Ken Causey''klc'              'Ken Causey''KR'               'korakurider''KTT'              'Kurt Thams''kwl'              'Klaus D. Witzel''ky'               'Koji Yokokawa''laza'             'Alexander Lazarevic''LB'               'Leo Burd''LC'               'Leandro Caniglia''lc'               'Leandro Caniglia''LEG'              'Gerald Leeb''len'              'Luciano Esteban Notarfrancesco''lr'               'Lukas Renggli''lrs'              'Lorenzo Schiavina''ls'               'Lex Spoon''LS'               'Lex Spoon''m3r'              'Maurice Rabb''MAL'              'Michael Latta''mas'              'Mark Schwenk''m'                'Marcus Denker''MD'               'Marcus Denker''md'               'Marcus Denker''md\'              'Marcus Denker''mdr'              'Mike Rutenberg''mga'              'Markus Galli''miki'             'Mikael Kindborg''mikki'            'Mikael Kindborg''mir'              'Michael Rueger''mist'             'Michal Starke''MJG'              'Mark Guzdial''mjg'              'Mark Guzdial''mjg8/31/1998'              'Mark Guzdial''mjg9/9/1998'              'Mark Guzdial''mjg9/23/1998'              'Mark Guzdial''mjr'              'Mike Roberts''mjt'              'Mike Thomas''mk'               'Matej Kosik''mlr'              'Michael Rueger''MPW'              'Marcel Weiher''mpw'              'Marcel Weiher''mrm'              'Martin McClure''MPH'              'Michael Hewner''MU'               'Masashi Umezawa''mu'               'Masashi Umezawa''mw'               'Martin Wirblat''nb'               'Naala Brewer''nice'             'Nicolas Cellier''nk'               'Ned Konz''nop'              'Jay Carlson''Noury'            'Noury Bouraqadi''NS'               'Nathanael Schaerli''panda'            'Michael Rueger''PHK'              'Peter Keeler''pk'               'Pavel Krivanek''pm'               'Patrick Mauritz''pnm'              'Paul McDonough''pnm8/23/2000'              'Paul McDonough''RAA'              'Bob Arning''RAA3/28/2000'              'Bob Arning''r++'              'Gerardo Richarte''raa'              'Bob Arning''raok'             'Richard A. O''Keefe''rbb'              'Brian Brown''rca'              'Russell Allen''reThink'          'Paul McDonough''rew'              'Roger Whitney''rhi'              'Robert Hirschfeld''rh'              'Robert Hirschfeld''Rik'              'Rik Fischer SmOOdy''rk'               'Ram Krishnan''rkris'               'Ram Krishnan''RJT'              'Ron Teitelbaum''RM'			'Rick McGeer''rr'               'Romain Robbes''rw'               'Roel Wuyts''rw'              'Robert Withers''rww'              'Robert Withers''sbw'              'Stephan B. Wessels''SD'               'Stephane Ducasse''sd'               'Stephane Ducasse''stephaneducassse'               'Stephane Ducasse''stephane.ducasse' 	'Stephane Ducasse''sge'              'Steve Elkins''shrink'           'Pavel Krivanek''slg'              'Steve Gilbert''sm'               'Simon Michael''sma'              'Stefan Matthias Aust''sn'               'Suslov Nikolay''spfa'             'Stephane Rollandin''sps'              'Steven Swerling''sqr'              'Andres Valloud''SqR'              'Andres Valloud''SqR!!!!'            'Andres Valloud''SqR!!!!!!!!'         'Andres Valloud''sr'               'Stephan Rudlof''ssa'              'Sam S. Adams''Sames'            'Samuel S. Shuster''SSS'              'Samuel S. Shuster''st'               'Samuel Tardieu''stephaneducasse'  'Stephane Ducasse''stp'              'Stephen Travis Pope''sumim'            'Masato Sumi''svp'              'Stephen Vincent Pair''sw'               'Scott Wallace''sws'              'Scott Wallace''T2'               'Toshiyuki Takeda''tak'              'Takashi Yamamiya''tao'              'Tim Olson''tb'               'Todd Blanchard or Torsten Bergman''TBn'              'Torsten Bergmann''tbn'              'Torsten Bergmann''tetha'            'Tetsuya Hayashi''tfei'             'The Fourth Estate, Inc.''th'               'Torge Husfeldt''ti'               'Tobias Isenberg''TJ'               'TJ Leone''tk'               'Ted Kaehler''tk9/13/97'               'Ted Kaehler''tk12/6/2004'               'Ted Kaehler''tk11/29/2004'               'Ted Kaehler''tk11/26/2004'               'Ted Kaehler''tk'               'Thomas Kowark''tlk'              'Tom Koenig''TN'               'korakurider''tonyg'		'Tony Garnock-Jones''TPR'              'Tim Rowledge''tpr'              'Tim Rowledge''TAG'              'Travis Griggs''TRee'             'Trygve Reenskaug''ts'          'Shortsleeved''Tsutomu'          'Tsutomu Hiroshima''tween'            'Andy Tween''vb'               'Vassili Bykov''vbdew'               'Vassili Bykov and Doug Way''vj'               'Vladimir Janousek''wiz'              'Jerome Peace''wod'              'Bill Dargel''ykoubo'           'Koji Yokokawa''yo'               'Yoshiki Ohshima''ward'             'Ward Cunningham''zz'               'Serge Stinckwich')! !ParseNode removeSelector: #shortPrintOn:!CodeHolder removeSelector: #shoutIsModeStyleable!