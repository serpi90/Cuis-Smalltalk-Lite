'From Cuis 1.0 of 4 September 2009 [latest update: #290] on 2 October 2009 at 9:55:18 am'!!BorderedMorph methodsFor: 'accessing' stamp: 'jmv 10/2/2009 09:54'!borderWidth: anInteger	borderWidth = anInteger ifFalse: [		borderColor ifNil: [borderColor _ Color black].		borderWidth _ anInteger max: 0.		self changed ]! !!DamageRecorder methodsFor: 'recording' stamp: 'jmv 10/2/2009 09:49'!recordInvalidRect: newRect	"Record the given rectangle in my damage list, a list of rectangular areas of the display that should be redraw on the next display cycle."	"Details: Damaged rectangles are often identical or overlap significantly. In these cases, we merge them to reduce the number of damage rectangles that must be processed when the display is updated. Moreover, above a certain threshold, we ignore the individual rectangles completely, and simply do a complete repaint on the next cycle."	| mergeRect a |	totalRepaint ifTrue: [^ self].  "planning full repaint; don't bother collecting damage"	invalidRects do: [ :rect |		(newRect origin = rect origin and: [ newRect corner = rect corner ])			ifTrue: [				^ self ].		((a _ (rect intersect: newRect) area) > 40			and: ["Avoid combining a vertical and horizontal rects.				  Can make a big diff and we only test when likely."				  a > (newRect area // 4) or: [ a > (rect area // 4)]])			ifTrue: [				"merge rectangle in place (see note below) if there is significant overlap"				rect					setOrigin: (rect origin min: newRect origin) truncated					corner: (rect corner max: newRect corner) truncated.				^ self]].	invalidRects size >= 15 ifTrue: [		"if there are too many separate areas, merge them all"		mergeRect _ Rectangle merging: invalidRects.		self reset.		invalidRects addLast: mergeRect].	"add the given rectangle to the damage list"	"Note: We make a deep copy of all rectangles added to the damage list,		since rectangles in this list may be extended in place."	invalidRects addLast:		(newRect topLeft truncated corner: newRect bottomRight truncated)! !!MenuMorph methodsFor: 'control' stamp: 'jmv 10/2/2009 09:53'!popUpAt: aPoint forHand: hand in: aWorld allowKeyboard: aBoolean 	"Present this menu at the given point under control of the given hand."	| evt |	self items isEmpty ifTrue: [^self].	MenuIcons decorateMenu: self.	(self submorphs select: [:m | m isKindOf: UpdatingMenuItemMorph]) 		do: [:m | m updateContents].	self 		positionAt: aPoint		relativeTo: (selectedItem ifNil: [self items first])		inWorld: aWorld.	aWorld addMorphFront: self.	"Acquire focus for valid pop up behavior"	hand newMouseFocus: self.	aBoolean ifTrue: [hand newKeyboardFocus: self].	evt := hand lastEvent.	(evt isKeyboard or: [evt isMouse and: [evt anyButtonPressed not]]) 		ifTrue: [			"Select first item if button not down"			self moveSelectionDown: 1 event: evt]! !!TextMorph methodsFor: 'accessing' stamp: 'jmv 10/2/2009 09:55'!backgroundColor: newColor	backgroundColor = newColor ifFalse: [		backgroundColor _ newColor.		self changed ]! !!TransformMorph methodsFor: 'accessing' stamp: 'jmv 10/2/2009 09:52'!offset: newOffset	| o |	o _ newOffset - self innerBounds topLeft.	transform offset = o ifFalse: [		transform _ transform withOffset: o.		self changed ]! !