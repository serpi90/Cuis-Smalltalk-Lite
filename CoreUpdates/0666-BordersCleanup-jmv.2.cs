'From Cuis 2.9 of 5 November 2010 [latest update: #634] on 29 November 2010 at 4:26:38 pm'!!classDefinition: #BorderedMorph category: #'Morphic-Kernel'!Morph subclass: #BorderedMorph	instanceVariableNames: 'borderWidth borderColor borderStyle '	classVariableNames: ''	poolDictionaries: ''	category: 'Morphic-Kernel'!!Canvas methodsFor: 'drawing-rectangles' stamp: 'jmv 11/29/2010 13:23'!fillRectangle: r fillStyle: aFillStyle borderWidth: bw borderStyleSymbol: aSymbol	"	Display getCanvas		fillRectangle: (10@10 extent: 300@200)		fillStyle: (SystemWindow titleGradient: 24)		borderWidth: 5		borderStyleSymbol: #raised	"		"Nicer. does frame with translucent black or white. Downside: requires proper color.	Some buttons are actually transparent (should be fixed!!), and there is a trick to grab some	opaque owner's color. And this is needed because (for instance) SystemWindow does NOT paint the inside with its color, but that color is needed to paint separators and button area. Something better is needed!!!!!!!!!!"	"	self fillRectangle: r fillStyle: aFillStyle.	self frameRectangle: r borderWidth: bw borderStyleSymbol: aSymbol	"		| c |	c _ aFillStyle asColor.	self fillRectangle: (r insetBy: bw) fillStyle: aFillStyle.	self frameRectangle: r color: c borderWidth: bw borderStyleSymbol: aSymbol! !!Canvas methodsFor: 'drawing-rectangles' stamp: 'jmv 11/29/2010 13:22'!fillRectangle: r fillStyle: aFillStyle borderWidth: bw borderStyleSymbol: aSymbol baseColorForBorder: baseColorForBorder	"Pretty ugly.#fillRectangle:fillStyle:borderWidth:borderStyleSymbol:  is much better but has trouble with silly transparent morphs	"		self fillRectangle: (r insetBy: bw) fillStyle: aFillStyle.	self frameRectangle: r color: baseColorForBorder borderWidth: bw borderStyleSymbol: aSymbol! !!Canvas methodsFor: 'drawing-rectangles' stamp: 'jmv 11/29/2010 11:58'!frameRectangle: r borderWidth: bw borderStyleSymbol: aSymbol	"	Display getCanvas		frameRectangle: (10@10 extent: 300@200)		borderWidth: 2		borderStyleSymbol: #raised	Display getCanvas fillRectangle: (10@10 extent: 300@200) color: Color white	"	| bright dark |	bright _ Color white alpha: 0.4.	dark _ Color black alpha: 0.2.	aSymbol == #raised ifTrue: [		^ self			frameRectangle: r			borderWidth: bw			topLeftColor: bright			bottomRightColor: dark ].	aSymbol == #inset ifTrue: [		^ self			frameRectangle: r			borderWidth: bw			topLeftColor: dark			bottomRightColor: bright ]! !!Canvas methodsFor: 'drawing-rectangles' stamp: 'jmv 11/29/2010 11:09'!frameRectangle: r borderWidth: borderWidth topLeftColor: topLeftColor bottomRightColor: bottomRightColor	"	Display getCanvas		frameRectangle: (10@10 extent: 300@200)		borderWidth: 20		topLeftColor: Color green		bottomRightColor: Color red	Display getCanvas fillRectangle: (10@10 extent: 300@200) color: Color white	"	| bw halfbw tl br |	bw _ borderWidth asPoint.	halfbw _ bw // 2.	tl _ r topLeft +halfbw.	br _ r bottomRight -halfbw.	self line: tl to: tl + (0@(r extent y - bw y)) width: borderWidth color: topLeftColor.	self line: br to: br - (0@(r extent y - bw y)) width: borderWidth color: bottomRightColor.	self line: tl to: tl + ((r extent x - bw x)@0) width: borderWidth color: topLeftColor.	self line: br to: br - ((r extent x - bw x)@0) width: borderWidth color: bottomRightColor.! !!Canvas methodsFor: 'drawing-rectangles' stamp: 'jmv 11/29/2010 13:36'!frameRectangle: r color: aColor borderWidth: bw borderStyleSymbol: aSymbol	"	Display getCanvas		frameRectangle: (10@10 extent: 300@200)		color: Color green		borderWidth: 2		borderStyleSymbol: #raised	Display getCanvas fillRectangle: (10@10 extent: 300@200) color: Color white	"	aSymbol == #raised ifTrue: [		^ self			frameRectangle: r			borderWidth: bw			topLeftColor: aColor quiteWhiter			bottomRightColor: aColor quiteBlacker ].	aSymbol == #inset ifTrue: [		^ self			frameRectangle: r			borderWidth: bw			topLeftColor: aColor quiteBlacker			bottomRightColor: aColor quiteWhiter ].		"Unrecognized border style. Draw some border..."	self frameRectangle: r width: bw color: aColor! !!FormCanvas methodsFor: 'drawing-rectangles' stamp: 'jmv 11/29/2010 11:19'!frameRectangle: r borderWidth: borderWidth topLeftColor: topLeftColor bottomRightColor: bottomRightColor	"	Display getCanvas		frameRectangle: (10@10 extent: 300@200)		borderWidth: 20		topLeftColor: Color green		bottomRightColor: Color red	Display getCanvas fillRectangle: (10@10 extent: 300@200) color: Color white	"	| rect w h |	self setFillColor: topLeftColor.	rect _ r translateBy: origin.	port frameRectTopLeft: rect borderWidth: borderWidth.	borderWidth isNumber		ifTrue: [w _ h _ borderWidth]		ifFalse: [w _ borderWidth x.   h _ borderWidth y].	self setFillColor: bottomRightColor.	port 		 frameRectRight: rect width: w;		 frameRectBottom: rect height: h! !!FormCanvas methodsFor: 'drawing-rectangles' stamp: 'jmv 11/29/2010 11:28'!frameRectangle: r width: borderWidth color: borderColor	"	Display getCanvas		frameRectangle: (10@10 extent: 300@200)		width: 20		color: Color red	"	| rect |	rect _ r translateBy: origin.	self setFillColor: borderColor.	port		frameRect: rect		borderWidth: borderWidth.! !!GrafPort methodsFor: 'drawing support' stamp: 'jmv 11/29/2010 11:20'!frameRectTopLeft: rect borderWidth: borderWidth	"Paint the top and left edges of a border whose rectangular area is defined by rect.	 The width of the border of each side is borderWidth."	sourceX _ 0.	sourceY _ 0.		"top"	height _ borderWidth. 	width _ rect width. 	destX _ rect left.	destY _ rect top.	self copyBits.	"left"	height _ rect height. 	width _ borderWidth. 	destY _ rect top.	destX _ rect left.	self copyBits! !!ButtonLW methodsFor: 'drawing' stamp: 'jmv 11/29/2010 16:23'!drawOn: aCanvas	| w h c |	c _ self backColor.	aCanvas		fillRectangle: bounds		fillStyle: c		borderWidth: 3		borderStyleSymbol: (pressed ifTrue: [ #inset ] ifFalse: [ #raised ])		baseColorForBorder: c.	w _ (font widthOfString: label) // 2.	h _ font ascent // 2.	pressed ifTrue: [		w _ w - 1.		h _ h - 1].	aCanvas drawString: label at: bounds center - (w@h) font: font color: self foreColor! !!BorderedMorph methodsFor: 'drawing' stamp: 'jmv 11/29/2010 13:35'!drawOn: aCanvas	borderColor class == Symbol		ifTrue: [			" This would of course be much better...			^aCanvas fillRectangle: bounds fillStyle: self fillStyle borderWidth: borderWidth borderStyleSymbol: borderColor			"			aCanvas fillRectangle: bounds fillStyle: self fillStyle borderWidth: borderWidth borderStyleSymbol: borderColor baseColorForBorder: self raisedColor ]		ifFalse: [			aCanvas fillRectangle: bounds fillStyle: self fillStyle borderWidth: borderWidth borderStyleSymbol: #simple baseColorForBorder: borderColor			"aCanvas fillRectangle: bounds fillStyle: self fillStyle borderStyle: self borderStyle" ]! !!PolygonMorph methodsFor: 'drawing' stamp: 'jmv 11/28/2010 16:45'!drawBorderOn: aCanvas usingEnds: anArray 	"Display my border on the canvas."	"NOTE: Much of this code is also copied in drawDashedBorderOn:  	(should be factored)"	| bigClipRect p1i p2i |	borderDashSpec		ifNotNil: [^ self drawDashedBorderOn: aCanvas usingEnds: anArray].	bigClipRect _ aCanvas clipRect expandBy: self borderWidth + 1 // 2.	self lineSegmentsDo: [:p1 :p2 | 		p1i _ p1 asIntegerPoint.		p2i _ p2 asIntegerPoint.		(arrows ~= #none and: [closed not])			ifTrue: ["Shorten line ends so as not to interfere with tip of arrow."					((arrows == #back								or: [arrows == #both])							and: [p1 = vertices first])						ifTrue: [p1i _ anArray first asIntegerPoint].					((arrows == #forward								or: [arrows == #both])							and: [p2 = vertices last])						ifTrue: [p2i _ anArray last asIntegerPoint]].		(closed or: ["bigClipRect intersects: (p1i rect: p2i) optimized:"			((p1i min: p2i) max: bigClipRect origin)				<= ((p1i max: p2i) min: bigClipRect corner)])				ifTrue: [					aCanvas line: p1i to: p2i width: self borderWidth color: self borderColor ]]! !!PolygonMorph methodsFor: 'drawing' stamp: 'jmv 11/29/2010 13:46'!drawDashedBorderOn: aCanvas usingEnds: anArray 	"Display my border on the canvas. NOTE: mostly copied from  	drawBorderOn:"	| lineColor bigClipRect p1i p2i segmentOffset |	(borderColor isNil 		or: [(borderColor is: #Color) and: [borderColor isTransparent]]) ifTrue: [^self].	lineColor := borderColor.	bigClipRect := aCanvas clipRect expandBy: (self borderWidth + 1) // 2.	segmentOffset := self borderDashOffset.	self lineSegmentsDo: 			[:p1 :p2 | 			p1i := p1 asIntegerPoint.			p2i := p2 asIntegerPoint.			(arrows ~= #none and: [closed not]) 				ifTrue: 					["Shorten line ends so as not to interfere with tip  					of arrow."					((arrows == #back or: [arrows == #both]) and: [p1 = vertices first]) 						ifTrue: [p1i := anArray first asIntegerPoint].					((arrows == #forward or: [arrows == #both]) and: [p2 = vertices last]) 						ifTrue: [p2i := anArray last asIntegerPoint]].			(closed or: 					["bigClipRect intersects: (p1i rect: p2i) optimized:"					((p1i min: p2i) max: bigClipRect origin) 						<= ((p1i max: p2i) min: bigClipRect corner)]) 				ifTrue: 					[					segmentOffset := aCanvas 								line: p1i								to: p2i								width: borderWidth								color: lineColor								dashLength: borderDashSpec first								secondColor: borderDashSpec third								secondDashLength: borderDashSpec second								startingOffset: segmentOffset]]! !!PolygonMorph methodsFor: 'drawing' stamp: 'jmv 11/29/2010 13:48'!drawOn: aCanvas 	"Display the receiver, a spline curve, approximated by straight line segments."	| lineColor bigClipRect brush p1i p2i |	vertices size < 1 ifTrue: [self error: 'a polygon must have at least one point'].	closed & color isTransparent not ifTrue: [		self filledForm colors: (Array with: Color transparent with: color).		aCanvas paintImage: self filledForm at: bounds topLeft-1].	lineColor _ borderColor. 	bigClipRect _ aCanvas clipRect expandBy: self borderWidth+1//2.	brush _ nil.	self lineSegmentsDo: [ :p1 :p2 |		p1i _ p1 asIntegerPoint.  p2i _ p2 asIntegerPoint.		(closed or: ["bigClipRect intersects: (p1i rect: p2i) optimized:"					((p1i min: p2i) max: bigClipRect origin) <=					((p1i max: p2i) min: bigClipRect corner)]) ifTrue: [			(borderWidth > 3 and: [borderColor is: #Color])			ifTrue: [brush ifNil: [						brush _ (ColorForm dotOfSize: borderWidth)								colors: (Array with: Color transparent with: borderColor)].					aCanvas line: p1i to: p2i brushForm: brush]			ifFalse: [aCanvas line: p1i to: p2i							width: borderWidth color: lineColor]]].	self arrowForms ifNotNil: [		self arrowForms do: [ :f |			f colors: (Array with: Color transparent with: borderColor).			aCanvas paintImage: f at: f offset]]! !!ScrollBar methodsFor: 'other events' stamp: 'jmv 11/28/2010 16:31'!mouseDownInSlider: event	interval = 1.0 ifTrue: [		"make the entire scrollable area visible if a full scrollbar is clicked on"		self setValue: 0.		self model hideOrShowScrollBars.].	slider borderColor == #raised		ifTrue: [slider borderColor: #inset].		sliderShadow color: self sliderShadowColor.	sliderShadow bounds: slider bounds.	sliderShadow show! !!ScrollBar methodsFor: 'other events' stamp: 'jmv 11/28/2010 16:31'!mouseUpInSlider: event 	slider borderColor == #inset		ifTrue: [slider borderColor: #raised].		sliderShadow hide! !!SystemWindow methodsFor: 'drawing' stamp: 'jmv 11/29/2010 13:37'!drawClassicFrameOn: aCanvas titleColor: titleColor	"Window border encompasses title area. No round corners. No title gradient."	borderColor class == Symbol		ifTrue: [			" This would of course be much better...			^aCanvas fillRectangle: bounds fillStyle: self fillStyle borderWidth: borderWidth borderStyleSymbol: borderColor			"			aCanvas fillRectangle: bounds fillStyle: self fillStyle borderWidth: borderWidth borderStyleSymbol: borderColor baseColorForBorder: self raisedColor ]		ifFalse: [			aCanvas fillRectangle: bounds fillStyle: self fillStyle borderWidth: borderWidth borderStyleSymbol: #simple baseColorForBorder: borderColor			"aCanvas fillRectangle: bounds fillStyle: self fillStyle borderStyle: self borderStyle" ].	aCanvas fillRectangle: self titleAreaInnerRect color: titleColor! !!SystemWindow methodsFor: 'drawing' stamp: 'jmv 11/29/2010 13:37'!drawWindowBodyOn: aCanvas roundCorners: doRoundCorners widgetsColor: widgetsColor	"Title area is not inside window borders"	| r bl tl tr he tw bw |	doRoundCorners		ifFalse: [			borderColor class == Symbol				ifTrue: [					" This would of course be much better...					^aCanvas fillRectangle: bounds fillStyle: self fillStyle borderWidth: borderWidth borderStyleSymbol: borderColor					"					aCanvas fillRectangle: bounds fillStyle: self fillStyle borderWidth: borderWidth borderStyleSymbol: borderColor baseColorForBorder: self raisedColor ]				ifFalse: [					aCanvas fillRectangle: bounds fillStyle: self fillStyle borderWidth: borderWidth borderStyleSymbol: #simple baseColorForBorder: borderColor					"aCanvas fillRectangle: bounds fillStyle: self fillStyle borderStyle: self borderStyle" ]]		ifTrue: [			r _ ColorTheme current roundedCornerRadius.			aCanvas image: SystemWindow roundedCornerBL multipliedBy: widgetsColor at: bounds bottomLeft - (0@r).			aCanvas image: SystemWindow roundedCornerBR multipliedBy: widgetsColor at: bounds bottomRight - (r@r) .			aCanvas fillRectangle: self innerBounds fillStyle: self fillStyle.			tl _ bounds topLeft + (0@self labelHeight).			tr _ bounds topRight + (borderWidth negated@self labelHeight).			bl _ bounds bottomLeft + (r@borderWidth negated).			he _ borderWidth@(bounds height - self labelHeight - r).			tw _ bounds width@borderWidth.			bw _ bounds width - r - r@borderWidth.			aCanvas fillRectangle: (tl extent: he) fillStyle: widgetsColor.			aCanvas fillRectangle: (tr extent: he) fillStyle: widgetsColor.			aCanvas fillRectangle: (bl extent: bw) fillStyle: widgetsColor.			aCanvas fillRectangle: (tl extent: tw) fillStyle: widgetsColor ]! !TransformMorph removeSelector: #colorForInsets!SystemWindow removeSelector: #colorForInsets!HandMorph removeSelector: #colorForInsets!BorderedMorph removeSelector: #borderStyle!BorderedMorph removeSelector: #releaseCachedState!!classDefinition: #BorderedMorph category: #'Morphic-Kernel'!Morph subclass: #BorderedMorph	instanceVariableNames: 'borderWidth borderColor'	classVariableNames: ''	poolDictionaries: ''	category: 'Morphic-Kernel'!Morph removeSelector: #colorForInsets!Morph removeSelector: #insetColor!LightWidget removeSelector: #borderStyleFor:!LightWidget removeSelector: #borderStyleWith:!LightWidget removeSelector: #pressedBorderStyleWith:!InfiniteForm removeSelector: #colorForInsets!Color removeSelector: #colorForInsets!Canvas removeSelector: #fillRectangle:color:borderStyle:!Canvas removeSelector: #fillRectangle:fillStyle:borderStyle:!Smalltalk removeClassNamed: #BorderStyle!Smalltalk removeClassNamed: #InsetBorder!Smalltalk removeClassNamed: #RaisedBorder!Smalltalk removeClassNamed: #SimpleBorder!