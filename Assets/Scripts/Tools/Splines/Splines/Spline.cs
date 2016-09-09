using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
* @class Spline
*
* @brief The Spline class represents three-dimensional curves.
*
* It provides the most important functions that are necessary to create, calculate and render Splines.
* The class derives from MonoBehaviour so it can be attached to GameObjects and used like any other self-written script.
*/ 

public partial class Spline : MonoBehaviour
{
	//General settings
	public InterpolationMode interpolationMode = InterpolationMode.Hermite; ///< Specifies what kind of curve interpolation to use.
	public ControlNodeMode nodeMode = ControlNodeMode.UseChildren; ///< Defines where to search for control nodes.
	public RotationMode rotationMode = RotationMode.Tangent; ///< Specifies how to calculate rotations on the spline.
	public TangentMode tangentMode = TangentMode.UseTangents; ///< Specifies how tangents are calculated.
	public UpdateMode updateMode = UpdateMode.EveryFrame; ///< Specifies when the spline will be updated.
	
	public float deltaSeconds = 0.1f; ///< Specifies after how much time the spline will be updated (see UpdateMode).
	public int deltaFrames = 2; ///< Specifies after how many frames the spline will be updated (see UpdateMode).
	
	public Vector3 tanUpVector = Vector3.up; ///< Up-Vector used for calculation of rotations (only needed for RotationMode.Tangent).
	public float tension = 0.5f; ///< Curve Tension (only has an effect on Hermite splines).
	public bool autoClose = false; ///< If set to true the spline start and end points of the spline will be connected. (Note that Bézier-Curves can't be auto-closed!)
	
	public int interpolationAccuracy = 1; ///< Defines how accurate the constancy of velocity will be.
	
	public Transform[] splineNodesTransform; ///< An array of transforms holding data for control nodes (only needed for ControlNodeMode.UseArray).
	
	public float[] subSegmentLength;
	public float[] subSegmentPosition;
	
	public float splineLength = 0f;
	private float passedTime = 0f;
	
	public SplineNode[] splineNodes;
	
	
	//Member Accessors
	public SplineNode[] SplineNodes 
	{ 
		get{ return splineNodes; } 
	} ///< Returns the array containing the spline's control nodes.
	
	public SplineNode this[ int idx ] 
	{ 
		get{ return splineNodes[idx]; } 
		set{ if( value != null ) splineNodes[idx] = value; } 
	} ///< Indexer that accesses the spline's control nodes. 
	
	public float Length 
	{ 
		get { return splineLength; } 
	} ///< Returns the actual length of the spline.
	
	public bool AutoClose
	{ 
		get{ return (autoClose && interpolationMode != InterpolationMode.Bezier); } 
	} ///< Returns true if spline is auto-closed. If spline is a Bézier-Curve, false will always be returned.
	
	public int SegmentCount
	{ 
		get 
		{
			if( interpolationMode != InterpolationMode.Bezier )
			{
				if( AutoClose ) 
					return splineNodes.Length; 
				else 
					return splineNodes.Length - 1; 
			}
			
			return (splineNodes.Length-1) / 3;
		} 
	} ///< Returns the number of spline segments. (Note that a spline segment of a Bézier-Curve is defined by four control nodes!)
	
	public int ControlSegmentCount
	{ 
		get { if( AutoClose ) return splineNodes.Length; else return splineNodes.Length - 1; } 
	} ///< Returns the number of control nodes. 
	
	public Transform[] SplineNodeTransforms
	{
		get
		{
			if( nodeMode != ControlNodeMode.UseArray )
			{
				List<Transform> tmpArray = new List<Transform>( );
				
				foreach( SplineControlNode item in GetComponentsInChildren<SplineControlNode>( ) )
					tmpArray.Add( item.GetTransform );
				
				tmpArray.Remove( transform );
				
				tmpArray.Sort( delegate( Transform a, Transform b )
								{ return a.name.CompareTo( b.name ); } );
				
				return tmpArray.ToArray( );
			}
			
			return splineNodesTransform;
		}
	} ///< Returns an array of transforms holding data for control nodes.
	
	public SplineSegment[] SplineSegments
	{
		get
		{
			SplineSegment[] sSegments = new SplineSegment[SegmentCount];
			
			if( interpolationMode != InterpolationMode.Bezier )
			{
				for( int i = 0; i < sSegments.Length; i++ )
					sSegments[i] = new SplineSegment( this, splineNodes[i], splineNodes[i].NextNode0 );
			}
			else
			{
				for( int i = 0; i < sSegments.Length; i++ )
					sSegments[i] = new SplineSegment( this, splineNodes[i*3], splineNodes[i*3].NextNode0 );
			}
			
			return sSegments;
		}
	} ///< Returns an array containing all the spline's segments. 
	
	private bool IsBezier
	{
		get{ return interpolationMode == InterpolationMode.Bezier; } 
	}
	
	private int Step
	{
		get{ if( interpolationMode == InterpolationMode.Bezier ) return 3; else return 1; }
	}
	
	//Member functions
	void OnEnable( ) 
	{
		UpdateSplineNodes( );
	}
	
	void LateUpdate( ) 
	{
		switch( updateMode )
		{
		case UpdateMode.EveryFrame:
			UpdateSplineNodes( );
			break;
			
		case UpdateMode.EveryXFrames:
			if( deltaFrames <= 0 )
				deltaFrames = 1;
			
			if( Time.frameCount % deltaFrames == 0 )
				UpdateSplineNodes( );
			
			break;
			
		case UpdateMode.EveryXSeconds:
			passedTime += Time.deltaTime;
			
			if( passedTime >= deltaSeconds )
			{
				UpdateSplineNodes( );
				passedTime = 0f;
			}
			
			break;
		}
	}
	
	/** 
	* This function updates the spline. It is called automatically once in a while, if updateMode isn't set to DontUpdate.
	*/
	public void UpdateSplineNodes( )
	{
		SetupSplineNodes( SplineNodeTransforms );
	}
	
	/** 
	* This function returns a point on the spline for a parameter between 0 and 1
	* @param param A normalized spline parameter ([0..1]).
    * @return Returns a point on the spline.
	*/
	public Vector3 GetPositionOnSpline( float param )
	{
		if( splineNodes == null )
			return Vector3.zero;
		
		float normalizedParam; 
		
		int normalizedIndex;
		
		RecalculateParameter( param, out normalizedIndex, out normalizedParam );
		
		return GetPositionInternal( normalizedIndex, normalizedParam );
	}
	
	/** 
	* This function returns a tangent to the spline for a parameter between 0 and 1
	* @param param A normalized spline parameter ([0..1]).
    * @return Returns a tangent to the spline.
	*/
	public Vector3 GetTangentToSpline( float param )
	{
		if( splineNodes == null )
			return Vector3.zero;
		
		float normalizedParam;
		
		int normalizedIndex;
		
		RecalculateParameter( param, out normalizedIndex, out normalizedParam );
		
		return GetTangentInternal( normalizedIndex, normalizedParam );
	}
	
	/** 
	* This function returns a rotation on the spline for a parameter between 0 and 1
	* @param param A normalized spline parameter ([0..1]).
    * @return Returns a rotation on the spline.
	*/
	public Quaternion GetOrientationOnSpline( float param )
	{
		if( splineNodes == null )
			return Quaternion.identity;
		
		switch( rotationMode )
		{
		case RotationMode.Tangent:
			Vector3 tan = GetTangentToSpline( param );
			
			if( tan.x == 0 && tan.y == 0 & tan.z == 0 )
				return Quaternion.identity;
			
			return Quaternion.LookRotation( tan, tanUpVector );
			
		case RotationMode.Node:
			float normalizedParam;
		
			int normalizedIndex;
			
			RecalculateParameter( param, out normalizedIndex, out normalizedParam );
		
			return GetRotationInternal( normalizedIndex, normalizedParam );
			
		default:
			return Quaternion.identity;
		}
		
	}
	
	/** 
	* This function returns a spline segment that contains the point on the spline defined by a normalized parameter.
	* @param param A normalized spline parameter ([0..1]).
    * @return Returns a spline segment containing the point corresponding to param.
	*/
	public SplineSegment GetSplineSegment( float param )
	{
		if( interpolationMode == InterpolationMode.Bezier )
		{
			param = Mathf.Clamp( param, 0, 1f );
			
			if( param == 1f )
				return new SplineSegment( this, splineNodes[ControlSegmentCount-1], splineNodes[ControlSegmentCount-1].NextNode0 );
			
			for( int i = 0; i < ControlSegmentCount; i+=3 )
				if( param - splineNodes[i].posInSpline < splineNodes[i].length )
					return new SplineSegment( this, splineNodes[i], splineNodes[i].NextNode2 );
		}
		else
		{
			if( AutoClose )
				param = Mathf.Repeat( param, 1f );
			else
				param = Mathf.Clamp( param, 0, 1f );
			
			if( param == 1f ) //Only applies if not auto closed
				return new SplineSegment( this, splineNodes[ControlSegmentCount-1], splineNodes[ControlSegmentCount-1].NextNode0 );
			
			for( int i = 0; i < ControlSegmentCount; i++ )
				if( param - splineNodes[i].posInSpline < splineNodes[i].length )
					return new SplineSegment( this, splineNodes[i], splineNodes[i].NextNode0 );
		}
	
		return null;
	}
	
	/** 
	* This function converts a normalized spline parameter to the actual distance to the spline's start point.
	* @param param A normalized spline parameter ([0..1]).
    * @return Returns the actual distance from the start point to the point defined by param.
	*/
	public float ConvertNormalizedParameterToDistance( float param )
	{
		return splineLength * param;
	}
	
	/** 
	* This function converts an actual distance from the spline's start point to normalized spline parameter.
	* @param param A specific distance on the spline (must be less or equal to the spline length).
    * @return Returns a normalized spline parameter based on the distance from the splines start point.
	*/
	public float ConvertDistanceToNormalizedParameter( float param )
	{
		if( splineLength <= 0f || param <= 0f )
			return 0f;
		else if( param > splineLength )
			return 1f;
		else
			return param/splineLength;
	}
	
	//Recalculate the parameter for constant-velocity interpolation
	private void RecalculateParameter( float param, out int normalizedIndex, out float normalizedParam )
	{
		param = Mathf.Clamp01( param );
		
		normalizedIndex = 0;
		normalizedParam = 0;
		
		if( param == 0f )
			return;
		
		if( param == 1f )
		{
			if( interpolationMode == InterpolationMode.Bezier )
				normalizedIndex = ControlSegmentCount - 4;
			else
				normalizedIndex = ControlSegmentCount - 1;
			
			normalizedParam = 1;
			return;
		}
		
		float invertedAccuracy = 1f/interpolationAccuracy;
		
		for( int i = subSegmentPosition.Length - 1; i >= 0; i-- )
		{
			if( subSegmentPosition[i] < param )
			{
				int floorIndex = (i - (i % (interpolationAccuracy)));
				
				normalizedIndex = floorIndex * Step / interpolationAccuracy;
				normalizedParam = invertedAccuracy * (i-floorIndex + (param - subSegmentPosition[i]) / subSegmentLength[i]);
				
				return;
			}
		}
	}
	
	//Setup Spline Nodes
	private void SetupSplineNodes( Transform[] transformNodes )
	{
		splineNodes = null; 
		
		//Return if there are no nodes
		if( transformNodes.Length <= 0 ) 
			return;
		
		int effectiveNodes = transformNodes.Length;
		
		if( interpolationMode == InterpolationMode.Bezier )
		{
			if( transformNodes.Length < 7 )
				effectiveNodes -= (transformNodes.Length) % 4;
			else
				effectiveNodes -= (transformNodes.Length - 4) % 3;
			
			if( effectiveNodes < 4 )
				return;
		}
		else
		{
			if( effectiveNodes < 2 )
				return;
		}
		
		SplineNode[] newNodes = new SplineNode[effectiveNodes];
		
		for( int i = 0; i < effectiveNodes; i++ )
		{
			if( transformNodes[i] == null )
				return;
			
			newNodes[i] = new SplineNode( transformNodes[i] );
		}
		
		for( int i = 0; i < effectiveNodes; i++ )
		{
			if( transformNodes[i] == null ) 
				return;
			
			int idxPrevNode0 = i - 1;
			int idxNextNode0 = i + 1;
			int idxNextNode1 = i + 2;
			int idxNextNode2 = i + 3;
			
			if( AutoClose )
			{
				if( idxPrevNode0 < 0 )
					idxPrevNode0 = effectiveNodes - 1;
				
				idxNextNode0 %= effectiveNodes;
				idxNextNode1 %= effectiveNodes;
				idxNextNode2 %= effectiveNodes;
			}
			else
			{
				idxPrevNode0 = Mathf.Max( idxPrevNode0, 0 );
				idxNextNode0 = Mathf.Min( idxNextNode0, effectiveNodes-1 );
				idxNextNode1 = Mathf.Min( idxNextNode1, effectiveNodes-1 );
				idxNextNode2 = Mathf.Min( idxNextNode2, effectiveNodes-1 );
			}
			
			//Setup adjacent nodes
			newNodes[i][0] = newNodes[idxPrevNode0];
			newNodes[i][1] = newNodes[idxNextNode0];
			newNodes[i][2] = newNodes[idxNextNode1];
			newNodes[i][3] = newNodes[idxNextNode2];
		}
		
		splineNodes = newNodes;
		
		ReparametrizeCurve( );
	}
	
	private void ReparametrizeCurve( )
	{
		splineLength = 0f;
	
		subSegmentLength = new float[SegmentCount * interpolationAccuracy];
		subSegmentPosition = new float[SegmentCount * interpolationAccuracy];
		
		for( int i = 0; i < SegmentCount * interpolationAccuracy; i++ )
		{
			subSegmentLength[i] = 0f;
			subSegmentPosition[i] = 0f;
		}
		
		if( splineNodes == null )
			return;
		
		for( int i = 0; i < SegmentCount; i++ )
		{
			for( int j = 1; j <= interpolationAccuracy; j++ )
			{
				int index = i*interpolationAccuracy+j - 1;
				
				float invertedAccuracy = 1f / interpolationAccuracy;
				
				subSegmentLength[index] = GetSegmentLengthInternal( i * Step, invertedAccuracy*(j-1), invertedAccuracy*j, 0.2f * invertedAccuracy );
				
				splineLength += subSegmentLength[index];
			}
		}
		
		for( int i = 0; i < SegmentCount; i++ )
		{
			for( int j = 1; j <= interpolationAccuracy; j++ )
			{
				int index = i*interpolationAccuracy+j;
				
				subSegmentLength[index-1] /= splineLength;
				
				if( index == subSegmentPosition.Length )
					break;
				
				subSegmentPosition[index] = subSegmentPosition[index-1] + subSegmentLength[index-1];
			}
		}
		
		for( int i = 0; i < subSegmentLength.Length; i++ )
			splineNodes[((i - (i % interpolationAccuracy))/interpolationAccuracy)*Step].length += subSegmentLength[i];
		
		for( int i = 0; i < splineNodes.Length-Step; i+=Step )
			splineNodes[i+Step].posInSpline = splineNodes[i].posInSpline + splineNodes[i].length;
		
		if( IsBezier )
		{	
			for( int i = 0; i < splineNodes.Length-Step; i+=Step )
			{
				splineNodes[i+1].posInSpline = splineNodes[i].posInSpline;
				splineNodes[i+2].posInSpline = splineNodes[i].posInSpline;
			}
		}
		
		if( !AutoClose )
			splineNodes[splineNodes.Length-1].posInSpline = 1f;
		
	}
	
	
}
