'From Cuis 1.0 of 2 October 2009 [latest update: #316] on 8 October 2009 at 8:26:55 pm'!!LightWidget methodsFor: 'geometry testing' stamp: 'jmv 10/8/2009 08:52'!containsPoint: aPoint	| shadow |		self isParallelRectangle ifTrue: [		^bounds containsPoint: aPoint ].	"Ojo!! esto anda solo si clippeamos!! porque la sombra incluye a los submorphs no clippeados.	Ver en Morph y en M3Morph hecho mejor..."	shadow _ self shadowForm.	^(shadow pixelValueAt: aPoint - shadow offset) > 0! !!Morph methodsFor: 'drawing' stamp: 'jmv 10/7/2009 23:34'!areasRemainingToFill: aRectangle	(self isOrthoRectangularMorph and: [ self isOpaqueMorph ]) ifTrue: [		^aRectangle areasOutside: self bounds ].	^ Array with: aRectangle! !!Morph methodsFor: 'drawing' stamp: 'jmv 10/7/2009 23:07'!ownShadowForm	"Return a form representing the 'shadow' of the receiver, without including submorphs 	regardless of clipping"	| canvas bnds |	bnds _ self bounds.	canvas _ (Display defaultCanvasClass extent: bnds extent depth: 1)				asShadowDrawingCanvas: Color black. "Color black represents one for 1bpp"	canvas translateBy: bnds topLeft negated		during: [ :tempCanvas | tempCanvas drawMorph: self].	^ canvas form offset: bnds topLeft! !!Morph methodsFor: 'drawing' stamp: 'jmv 10/7/2009 23:07'!shadowForm	"Return a form representing the 'shadow' of the receiver - e.g., all pixels that are occupied by the receiver are one, all others are zero."	| canvas bnds |	bnds _ self fullBounds.	canvas _ (Display defaultCanvasClass extent: bnds extent depth: 1)				asShadowDrawingCanvas: Color black. "Color black represents one for 1bpp"	canvas translateBy: bnds topLeft negated		during: [ :tempCanvas | tempCanvas fullDrawMorph: self ].	^ canvas form offset: bnds topLeft! !!Morph methodsFor: 'geometry testing' stamp: 'jmv 10/8/2009 20:20'!containsPoint: aPoint	| shadow |	"Most morphs answer true to to #isOrthoRectangularMorph, or redefine this method..."	self isOrthoRectangularMorph ifTrue: [		^ self bounds containsPoint: aPoint ].		"...But for those who not, provide correct albeit expensive behavior."	shadow _ self ownShadowForm.	^(shadow pixelValueAt: aPoint - shadow offset) > 0! !!Morph methodsFor: 'geometry testing' stamp: 'jmv 10/8/2009 20:26'!fullContainsPoint: aPoint"	This alternative implementation is included in this comment because it could be useful someday.	If we start to rely heavily on the use of #ownShadowForm in #containsPoint, this could be cheaper.		| shadow |	self clipSubmorphs		ifTrue: [ ^self containsPoint: aPoint ]		ifFalse: [			(self fullBounds containsPoint: aPoint) ifFalse: [^ false].			(self containsPoint: aPoint) ifTrue: [^ true].			shadow _ self shadowForm.			^(shadow pixelValueAt: aPoint - shadow offset) > 0 ]"		(self fullBounds containsPoint: aPoint) ifFalse: [ ^ false ].  "quick elimination"	(self containsPoint: aPoint) ifTrue: [ ^ true ].  "quick acceptance"	submorphs do: [:m | (m fullContainsPoint: aPoint) ifTrue: [ ^ true ]].	^ false! !!Morph methodsFor: 'geometry testing' stamp: 'jmv 10/8/2009 20:10'!isOrthoRectangularMorph	"Answer true if I fill my bounds. I.e. I am a rectangle aligned with Display borders and	specified by my bounds instance variable.	If true, #containsPoint: can simply check #bounds."	^true! !!Morph methodsFor: 'testing' stamp: 'jmv 10/8/2009 20:14'!isOpaqueMorph	"Just answer false in the general case, to simplify submorphs.	See the implementation and comment in BorderedMorph. and see also senders.	If the answer is true, there is an optimization in world draw"	^false! !!BorderedMorph methodsFor: 'testing' stamp: 'jmv 10/8/2009 20:13'!isOpaqueMorph	"Any submorph that answers true to #isOrthoRectangularMorph (to optimize #containsPoint:)	but is not an opaque rectangle covering bounds MUST answer false to this message"	(color isColor and: [color isTranslucent]) ifTrue: [		^false ].	^(self borderWidth > 0			and: [self borderColor isColor					and: [self borderColor isTranslucent]]) not! !!EllipseMorph methodsFor: 'geometry testing' stamp: 'jmv 10/7/2009 23:21'!isOrthoRectangularMorph	^false! !!HaloMorph methodsFor: 'events-processing' stamp: 'jmv 10/7/2009 22:45'!containsPoint: aPoint event: anEvent	"Blue buttons are handled by the halo"	(anEvent isMouse and:[anEvent isMouseDown and:[anEvent blueButtonPressed]])		ifFalse:[^super containsPoint: aPoint event: anEvent].	^self fullBounds containsPoint: anEvent position! !!HaloMorph methodsFor: 'geometry testing' stamp: 'jmv 10/8/2009 09:02'!isOrthoRectangularMorph	^false! !!HandMorph methodsFor: 'drawing' stamp: 'jmv 10/7/2009 23:07'!shadowForm	"Return a 1-bit shadow of my submorphs.  Assumes submorphs is not empty"	| bnds canvas |	bnds _ Rectangle merging: (submorphs collect: [:m | m fullBounds]).	canvas _ (Display defaultCanvasClass extent: bnds extent depth: 1) 		asShadowDrawingCanvas: Color black.	canvas translateBy: bnds topLeft negated		during: [ :tempCanvas | self drawSubmorphsOn: tempCanvas ].	^ canvas form offset: bnds topLeft! !!ImageMorph methodsFor: 'testing' stamp: 'jmv 10/8/2009 20:15'!isOpaqueMorph	"Cheap optimization here"	^self isOpaque! !!NewHandleMorph methodsFor: 'all' stamp: 'jmv 10/8/2009 09:02'!followHand: aHand forEachPointDo: block1 lastPointDo: block2 withCursor: aCursor	hand _ aHand.	hand showTemporaryCursor: aCursor "hotSpotOffset: aCursor offset negated".	borderWidth _ 0.	color _ Color white alpha: 0.01. "If transparent, it can not be grabbed by the mouse!!"	pointBlock _ block1.	lastPointBlock _ block2.	self position: hand lastEvent cursorPoint - (self extent // 2)! !!PolygonMorph methodsFor: 'geometry testing' stamp: 'jmv 10/7/2009 23:21'!isOrthoRectangularMorph	^false! !!SketchMorph methodsFor: 'geometry testing' stamp: 'jmv 5/23/2009 00:03'!containsPoint: aPoint	^ (self bounds containsPoint: aPoint) and:	  [(originalForm isTransparentAt: aPoint - bounds origin) not]! !!TextMorph methodsFor: 'testing' stamp: 'jmv 10/7/2009 23:32'!isOpaqueMorph	"Overridden from BorderedMorph to test backgroundColor instead of (text) color."	(backgroundColor isNil or: [backgroundColor isTranslucent])		ifTrue: [^ false].	^(borderWidth > 0 and: [borderColor isColor and: [borderColor isTranslucent]]) not! !TextMorph removeSelector: #areasRemainingToFill:!SystemWindow removeSelector: #areasRemainingToFill:!MenuItemMorph removeSelector: #isOrthoRectangularMorph!!MenuItemMorph reorganize!('accessing' arguments arguments: contentString contentString: contents: contents:withMarkers: contents:withMarkers:inverse: hasIcon hasIconOrMarker hasMarker hasSubMenu icon icon: isEnabled isEnabled: selector selector: subMenu subMenu: target target:)('copying' veryDeepFixupWith: veryDeepInner:)('drawing' drawOn:)('events' activateOwnerMenu: activateSubmenu: deselectTimeOut: handleMouseUp: handlesMouseDown: handlesMouseOver: handlesMouseOverDragging: invokeWithEvent: mouseDown: mouseEnter: mouseEnterDragging: mouseLeave: mouseLeaveDragging: mouseUp:)('grabbing' aboutToBeGrabbedBy: duplicateMorph:)('initialization' defaultBounds deleteIfPopUp: initialize)('layout' minItemWidth)('meta actions')('selecting' deselect: isSelected: select:)('private' offImage onImage)('testing')!StringMorph removeSelector: #isOpaqueMorph!StringMorph removeSelector: #isOrthoRectangularMorph!!StringMorph reorganize!('accessing' contents contents: contentsClipped: fitContents font font:emphasis: fontToUse interimContents: measureContents minimumWidth setWidth: userString valueFromContents)('drawing' drawOn:)('editing' acceptContents acceptValue: cancelEdits doneWithEdits launchMiniEditor: lostFocusWithoutAccepting)('event handling' handlesMouseDown: hasFocus mouseDown: userEditsAllowed)('font' emphasis:)('halos and balloon help' addOptionalHandlesTo:box:)('initialization' defaultColor initWithContents:font:emphasis: initialize)('menu' addCustomMenuItems:hand: changeEmphasis changeFont)('printing' boundsForBalloon font: printOn:)('testing')!SketchMorph removeSelector: #isOpaqueMorph!!SketchMorph reorganize!('accessing' form form:)('drawing' drawOn:)('geometry' extent:)('geometry testing' containsPoint:)('initialization' initialize initializeWith:)('layout' layoutChanged)('menus' addFillStyleMenuItems:hand:)('testing')!PolygonMorph removeSelector: #areasRemainingToFill:!PasteUpMorph removeSelector: #fullContainsPoint:!OneLineEditorMorph removeSelector: #isOpaqueMorph!OneLineEditorMorph removeSelector: #isOrthoRectangularMorph!MinimalStringMorph removeSelector: #isOpaqueMorph!MinimalStringMorph removeSelector: #isOrthoRectangularMorph!!MinimalStringMorph reorganize!('initialization' defaultColor initWithContents:font:emphasis: initialize)('drawing' drawOn:)('accessing' contents: fitContents fontToUse measureContents minimumWidth)('testing')!LazyListMorph removeSelector: #isOpaqueMorph!!LazyListMorph reorganize!('initialization' initialize listSource:)('list management' drawBoundsForRow: listChanged rowAtLocation: selectRow: selectedRow selectedRow:)('drawing' adjustHeight adjustWidth bottomVisibleRowForCanvas: colorForRow: display:atRow:on: drawBackgroundForMulti:on: drawOn: drawSelectionOn: font font: highlightPotentialDropRow:on: topVisibleRowForCanvas:)('list access' getListItem: getListSize item:)('scroll range' hTotalScrollRange widthToDisplayItem:)('private' noSelection)('testing')!!ImageMorph reorganize!('accessing' borderWidth: color: form image: isOpaque isOpaque:)('drawing' drawOn:)('geometry' extent:)('initialization' initialize)('menu' changeOpacity opacityString)('menu commands' grabFromScreen readFromFile)('menus' addCustomMenuItems:hand:)('testing' isOpaqueMorph)!HandMorph removeSelector: #isOpaqueMorph!HandMorph removeSelector: #isOrthoRectangularMorph!!HandMorph reorganize!('accessing' colorForInsets lastEvent mouseOverHandler showTemporaryCursor: targetOffset userInitials userPicture userPicture:)('balloon help' balloonHelp balloonHelp: deleteBalloonTarget: removePendingBalloonFor: spawnBalloonFor: triggerBalloonFor:after:)('caching' releaseCachedState)('change reporting' invalidRect:from:)('classification' isHandMorph)('copying' veryDeepCopyWith:)('cursor' cursorBounds showTemporaryCursor:hotSpotOffset: temporaryCursor)('double click support' resetClickState waitForClicksOrDrag:event: waitForClicksOrDrag:event:clkSel:dblClkSel:dblClkTimeoutSel:dragSel: waitForClicksOrDragOrSimulatedYellow:event:clkSel:dblClkSel:dblClkTimeoutSel:dragSel: waitForSimulatedYellow:event:)('drawing' drawOn: fullDrawOn: hasChanged hasUserInformation needsToBeDrawn nonCachingFullDrawOn: restoreSavedPatchOn: savePatchFrom: shadowForm)('drop shadows' shadowOffset)('event handling' cursorPoint flushEvents noticeMouseOver:event: processEvents)('events-processing' handleEvent: isCapturingGesturePoints)('focus handling' keyboardFocus keyboardFocusNext keyboardFocusPrevious mouseFocus newKeyboardFocus: newMouseFocus: newMouseFocus:event: releaseAllFoci releaseKeyboardFocus releaseKeyboardFocus: releaseMouseFocus releaseMouseFocus:)('geometry' position position: userInitials:andPicture:)('grabbing/dropping' attachMorph: dropMorph:event: dropMorphs: grabMorph:from:)('halo handling' halo: obtainHalo: releaseHalo: removeHaloFromClick:on:)('halos and balloon help' halo)('initialization' initForEvents initialize interrupted)('layout' fullBounds)('listeners' addEventListener: addKeyboardListener: addListener:to: addMouseListener: eventListeners eventListeners: keyboardListeners keyboardListeners: mouseListeners mouseListeners: removeEventListener: removeKeyboardListener: removeListener:from: removeMouseListener:)('meta-actions' copyToPasteBuffer: grabMorph:)('objects from disk' objectForDataStream:)('paste buffer' objectToPaste pasteBuffer pasteBuffer: pasteMorph)('updating' changed)('private events' generateKeyboardEvent: generateMouseEvent: mouseTrailFrom: moveToEvent: sendEvent: sendFocusEvent:to:in: sendKeyboardEvent: sendListenEvent:to: sendMouseEvent:)('testing')!HaloMorph removeSelector: #isOpaqueMorph!EllipseMorph removeSelector: #areasRemainingToFill:!BorderedMorph removeSelector: #areasRemainingToFill:!