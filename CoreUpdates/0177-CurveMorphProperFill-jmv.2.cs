'From Cuis 1.0 of 6 April 2009 [latest update: #169] on 9 April 2009 at 9:48:45 am'!!PolygonMorph methodsFor: 'geometry' stamp: 'jmv 4/9/2009 09:32'!closestPointTo: aPoint	| curvePoint closestPoint dist minDist |	closestPoint _ minDist _ nil.	self lineSegmentsDo:		[:p1 :p2 | 		curvePoint _ aPoint nearestPointOnLineFrom: p1 to: p2.		dist _ curvePoint dist: aPoint.		(closestPoint isNil or: [dist < minDist])			ifTrue: [closestPoint _ curvePoint.					minDist _ dist]].	^ closestPoint! !!PolygonMorph methodsFor: 'geometry' stamp: 'sw 9/14/97 18:22'!merge: aPolygon	"Expand myself to enclose the other polygon.  (Later merge overlapping or disjoint in a smart way.)  For now, the two polygons must share at least two vertices.  Shared vertices must come one after the other in each polygon.  Polygons must not overlap."	| shared mv vv hv xx |	shared _ vertices select: [:mine | 		(aPolygon vertices includes: mine)].	shared size < 2 ifTrue: [^ nil].	"not sharing a segment"	mv _ vertices asOrderedCollection.	[shared includes: mv first] whileFalse: ["rotate them"		vv _ mv removeFirst.		mv addLast: vv].	hv _ aPolygon vertices asOrderedCollection.	[mv first = hv first] whileFalse: ["rotate him until same shared vertex is first"		vv _ hv removeFirst.		hv addLast: vv].	[shared size > 2] whileTrue: [		shared _ shared asOrderedCollection.		(self mergeDropThird: mv in: hv from: shared) ifNil: [^ nil]].		"works by side effect on the lists"	(mv at: 2) = hv last ifTrue: [mv removeFirst; removeFirst.		^ self setVertices: (hv, mv) asArray].	(hv at: 2) = mv last ifTrue: [hv removeFirst; removeFirst.		^ self setVertices: (mv, hv) asArray].	(mv at: 2) = (hv at: 2) ifTrue: [hv removeFirst.  mv remove: (mv at: 2).		xx _ mv removeFirst.		^ self setVertices: (hv, (Array with: xx), mv reversed) asArray].	mv last = hv last ifTrue: [mv removeLast.  hv removeFirst.		^ self setVertices: (mv, hv reversed) asArray].	^ nil! !!PolygonMorph methodsFor: 'geometry' stamp: 'sw 9/14/97 18:22'!mergeDropThird: mv in: hv from: shared	"We are merging two polygons.  In this case, they have at least three identical shared vertices.  Make sure they are sequential in each, and drop the middle one from vertex lists mv, hv, and shared.  First vertices on lists are identical already."	"know (mv first = hv first)"	| mdrop vv |	(shared includes: (mv at: mv size - 2)) 		ifTrue: [(shared includes: (mv last)) ifTrue: [mdrop _ mv last]]		ifFalse: [(shared includes: (mv last)) ifTrue: [			(shared includes: (mv at: 2)) ifTrue: [mdrop _ mv first]]].	(shared includes: (mv at: 3)) ifTrue: [		(shared includes: (mv at: 2)) ifTrue: [mdrop _ mv at: 2]].	mdrop ifNil: [^ nil].	mv remove: mdrop.	hv remove: mdrop.	shared remove: mdrop.	[shared includes: mv first] whileFalse: ["rotate them"		vv _ mv removeFirst.		mv addLast: vv].	[mv first = hv first] whileFalse: ["rotate him until same shared vertex is first"		vv _ hv removeFirst.		hv addLast: vv].! !!PolygonMorph methodsFor: 'initialization' stamp: 'jmv 4/9/2009 09:47'!initialize"initialize the state of the receiver"	super initialize.""	vertices _ Array				with: 15 @ 10				with: 30 @ 20				with: 10 @ 30.	closed _ true.	smoothCurve _ false.	arrows _ #none.	self computeBounds! !!PolygonMorph methodsFor: 'private' stamp: 'dgd 2/22/2003 14:15'!curveBounds	| curveBounds pointAfterFirst pointBeforeLast |	smoothCurve 		ifFalse: 			[^(Rectangle encompassing: vertices) expandBy: (borderWidth + 1) // 2].	"Compute the bounds from actual curve traversal, with leeway for borderWidth.	Also note the next-to-first and next-to-last points for arrow directions."	curveState := nil.	"Force recomputation"	curveBounds := vertices first corner: vertices last.	pointAfterFirst := nil.	self lineSegmentsDo: 			[:p1 :p2 | 			pointAfterFirst isNil 				ifTrue: 					[pointAfterFirst := p2 asIntegerPoint.					curveBounds := curveBounds encompass: p1 asIntegerPoint].			curveBounds := curveBounds encompass: p2 asIntegerPoint.			pointBeforeLast := p1 asIntegerPoint].	curveState at: 2 put: pointAfterFirst.	curveState at: 3 put: pointBeforeLast.	^curveBounds expandBy: (borderWidth + 1) // 2! !!PolygonMorph methodsFor: 'private' stamp: 'jmv 4/9/2009 09:42'!filledForm	"Note: The filled form is actually 2 pixels bigger than bounds, and the point corresponding to this morphs' position is at 1@1 in the form.  This is due to the details of the fillig routines, at least one of which requires an extra 1-pixel margin around the outside.  Computation of the filled form is done only on demand."	| bb origin |	closed ifFalse: [^ filledForm _ nil].	filledForm ifNotNil: [^ filledForm].	filledForm _ ColorForm extent: bounds extent+2.	"Draw the border..."	bb _ (BitBlt current toForm: filledForm) sourceForm: nil; fillColor: Color black;			combinationRule: Form over; width: 1; height: 1.	origin _ bounds topLeft asIntegerPoint-1.	self lineSegmentsDo: [:p1 :p2 | bb drawFrom: p1 asIntegerPoint-origin										to: p2 asIntegerPoint-origin].	"Fill it in..."	filledForm _ ColorForm mappingWhiteToTransparentFrom: filledForm anyShapeFill.	(borderColor isColor and: [borderColor isTranslucentColor]) ifTrue:		["If border is stored as a form, then erase any overlap now."		filledForm copy: self borderForm boundingBox from: self borderForm			to: 1@1 rule: Form erase].	^ filledForm! !