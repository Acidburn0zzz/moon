/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * animation2.h: Animation engine, contains declarations for 2.0 classes.
 *
 * Contact:
 *   Moonlight List (moonlight-list@lists.ximian.com)
 *
 * Copyright 2007 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 * 
 */

#ifndef MOON_ANIMATION2_H
#define MOON_ANIMATION2_H

/* @Version=2,Namespace=System.Windows.Media.Animation */
/* @IncludeInKinds */
class ObjectKeyFrame : public KeyFrame /* The managed class derives directly from DependencyObject */ {
 protected:
	virtual ~ObjectKeyFrame ();
	
 public:
	/* @GenerateCBinding,GeneratePInvoke,ManagedAccess=Protected */
	ObjectKeyFrame ();
	
	/* BROKEN BROKEN BROKEN! */
	/* @PropertyType=DependencyObject,ManagedPropertyType=object,GenerateAccessors */
	static DependencyProperty *ValueProperty;
	/* @PropertyType=KeyTime,Nullable,ManagedPropertyType=KeyTime,GenerateAccessors */
	static DependencyProperty *KeyTimeProperty;

	// Property accessors
	DependencyObject *GetValue ();
	void    SetValue (DependencyObject *pv);
	void    SetValue (DependencyObject v);

	virtual KeyTime *GetKeyTime ();
	virtual void SetKeyTime (KeyTime keytime);
	virtual void SetKeyTime (KeyTime *keytime);
};


/* @Version=2,Namespace=System.Windows.Media.Animation */
/* @IncludeInKinds */
class DiscreteObjectKeyFrame : public ObjectKeyFrame {
 protected:
	virtual ~DiscreteObjectKeyFrame ();
	
 public:
	/* @GenerateCBinding,GeneratePInvoke */
	DiscreteObjectKeyFrame ();
	
	virtual Value *InterpolateValue (Value *baseValue, double keyFrameProgress);
};


/* @Version=2,Namespace=System.Windows.Media.Animation */
/* @IncludeInKinds */
class ObjectKeyFrameCollection : public KeyFrameCollection {
 protected:
	virtual ~ObjectKeyFrameCollection ();

 public:
	/* @GenerateCBinding,GeneratePInvoke */
	ObjectKeyFrameCollection ();

	virtual Type::Kind GetElementType() { return Type::OBJECTKEYFRAME; }
};


/* @Version=2 */
/* @Namespace=System.Windows.Media.Animation */
/* @ContentProperty="KeyFrames" */
/* @IncludeInKinds */
class ObjectAnimationUsingKeyFrames : public /*Object*/Animation {
 protected:
	virtual ~ObjectAnimationUsingKeyFrames ();

 public:
 	/* @PropertyType=ObjectKeyFrameCollection,ManagedFieldAccess=Internal,ManagedSetterAccess=Internal,GenerateAccessors */
	static DependencyProperty *KeyFramesProperty;
	
	/* @GenerateCBinding,GeneratePInvoke */
	ObjectAnimationUsingKeyFrames ();

	void AddKeyFrame (ObjectKeyFrame *frame);
	void RemoveKeyFrame (ObjectKeyFrame *frame);

	// Property accessors
	ObjectKeyFrameCollection *GetKeyFrames ();
	void SetKeyFrames (ObjectKeyFrameCollection* value);

	virtual Value *GetCurrentValue (Value *defaultOriginValue, Value *defaultDestinationValue,
					AnimationClock* animationClock);

	virtual void Resolve ();

	virtual Duration GetNaturalDurationCore (Clock* clock);
	virtual bool Validate ();

	virtual Type::Kind GetValueKind () { return Type::INVALID; };
};

#endif /* MOON_ANIMATION2_H */
