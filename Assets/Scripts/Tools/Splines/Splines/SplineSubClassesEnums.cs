using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class Spline : MonoBehaviour
{
	/**
	* @enum ControlNodeMode
	* Defines whether to use children gameobjects as control nodes or the gameobjects that are assigned to Spline.splineNodesTransform.
	*/ 
	public enum ControlNodeMode
	{
		UseChildren, ///< Use children of the gameobject the spline-component is assigned to as control nodes.
		UseArray ///< Use the elements of the manually defined transform array (Spline.splineNodesTransform) as control nodes.
	}
	
	/**
	* @enum TangentMode
	* Specifies how tangents of control points should be calculated. Note that this will only affect Hermite-Splines.
	*/ 
	public enum TangentMode 
	{ 
		UseNormalizedTangents, ///< Use the normalized vector that connects the two adjacent control nodes as tangent (see UseTangents).
		UseTangents, ///< Use the vector that connects the two adjacent control nodes as tangent.
		UseNodeForwardVector ///< Use the forward vector which depends on the control node's rotation.
	}
	
	/**
	* @enum RotationMode
	* Specifies how to interpolate rotations over the spline.
	*/ 
	public enum RotationMode 
	{ 
		None, ///< No rotation (Quaternion.identity).
		Node, ///< Interpolate the control nodes' orientation.
		Tangent ///< Use the tangent to calculate the rotation on the spline.
	}
	
	/**
	* @enum InterpolationMode
	* Tells the plugin what type of curve you want to generate.
	*/ 
	public enum InterpolationMode 
	{
		Hermite, ///< Hermite Spline
		Bezier, ///< Bézier Spline
		BSpline ///< B-Spline
	}
	
	/**
	* @enum InterpolationMode
	* Tells the plugin when to update and recalculate the spline.
	*/ 
	public enum UpdateMode
	{
		DontUpdate, ///< Keep the spline static. It will only be updated when the Component becomes enables (OnEnable( )).
		EveryFrame, ///< Updates the spline every frame.
		EveryXFrames, ///< Updates the spline every x frames.
		EveryXSeconds ///< Updates the spline every x seconds.
	}
}

/**
* @class SplineNode
*
* @brief This class represents a control node of the Spline.
*
* This class stores data about the position and orientation of the control node.
* It also stores the spline parameter that is associated to the control node and the normalized distance (on the spline) between this and the next adjacent control node.
*  
*/ 
public class SplineNode
{
	public Transform nodeTransform; ///< Reference to a Transform in the scene.
	public float posInSpline = 0f; ///< Normalized position on the spline (parameter from 0 to 1).
	public float length = 0f; ///< Normalized distance to the next adjacent node.
	
	public SplineNode[] adjacentNodes; ///< References to 4 adjacent nodes (previous node and 3 next nodes). You better use the accessors to read and change values in this array. ;)
	public SplineNode this[ int idx ] { get{ return adjacentNodes[idx]; } set{ if( value != null ) adjacentNodes[idx] = value; } } ///< Accessor to the adjacentNodes array.
	
	public SplineNode PrevNode0 { get{ return adjacentNodes[0]; } set{ if( value != null ) adjacentNodes[0] = value; } } ///< The previous adjacent node.
	public SplineNode NextNode0 { get{ return adjacentNodes[1]; } set{ if( value != null ) adjacentNodes[1] = value; } } ///< The 1st next adjacent node. 
	public SplineNode NextNode1 { get{ return adjacentNodes[2]; } set{ if( value != null ) adjacentNodes[2] = value; } } ///< The 2nd next adjacent node.
	public SplineNode NextNode2 { get{ return adjacentNodes[3]; } set{ if( value != null ) adjacentNodes[3] = value; } } ///< The 3rd next adjacent node.
	
	public Vector3 Position { get{ return nodeTransform.position; } set{ nodeTransform.position = value; } } ///< Quick access to the control node's position.
	public Quaternion Rotation { get{ return nodeTransform.rotation; } set{ nodeTransform.rotation = value; } } ///< Quick access to the control node's orientation.
	
	public SplineNode( Transform controlPoint )
	{
		adjacentNodes = new SplineNode[4];
		
		nodeTransform = controlPoint;
	}
	
	/** 
	* This function checks the references to adjacent nodes for null references.
    * @return Returns true if any null references are found.
	*/
	public bool CheckReferences( )
	{
		return( nodeTransform != null && adjacentNodes[0] != null && adjacentNodes[1] != null && adjacentNodes[2] != null && adjacentNodes[3] != null );
	}
}

/**
* @class SplineSegment
*
* @brief This class represents a pair of two control nodes that define a segment on the Spline.
*
* A spline segment is represented by two adjacent control nodes. This stores two references to such nodes and provides
* useful functions that allow you to convert a parameter (0..1) that represents a point on the segment to a normalized 
* spline parameter that represents the same point on the spline. 
* This class becomes quite useful when dealing with Bézier curves!
*/ 
public class SplineSegment
{
	private readonly Spline parentSpline;
	private readonly SplineNode startNode;
	private readonly SplineNode endNode; 
	
	public Spline ParentSpline { get { return parentSpline; } }  ///< Returns a reference to the containing spline.
	public SplineNode StartNode { get { return startNode; } } ///< Returns a reference to the spline segment's start point.
	public SplineNode EndNode { get { return endNode; } } ///< Returns a reference to the spline segment's end point.
	
	public float Length { get { return startNode.length * parentSpline.Length; } } ///< Returns the actual length of the spline segment.
	public float NormalizedLength { get { return startNode.length; } }  ///< Returns the normlaized length of the segment in the spline.
	
	/** 
	* Constructor
    * @param pSpline The spline that contains the segment.
    * @param sNode The segment's start node.
    * @param eNode The segment's end node.
	*/
	public SplineSegment( Spline pSpline, SplineNode sNode, SplineNode eNode )
	{
		if( (sNode.NextNode0 == eNode || sNode.NextNode2 == eNode) && pSpline != null )
		{
			parentSpline = pSpline;
			
			startNode = sNode;
			endNode = eNode;
		}
		else
		{
			parentSpline = null;
			
			startNode = null;
			endNode = null; 
		}
	}
	
	//Convert a parameter [0..1] representing a point on the segment to a 
	//normalized parameter [0..1] representing a point on the spline
	
	/** 
	* This method converts a parameter [0..1] representing a point on the segment to a normalized parameter [0..1] representing a point on the whole spline.
    * @param param The normalized segment parameter.
    * @return Returns a normalized spline parameter.
	*/
	public float ConvertSegmentToSplineParamter( float param )
	{
		return startNode.posInSpline + param * startNode.length;
	}
	
	/** 
	* This method converts a parameter [0..1] representing a point on the whole spline to a normalized parameter [0..1] representing a point on the segment.
    * @param param The normalized spline parameter.
    * @return Returns a normalized segment parameter.
	*/
	public float ConvertSplineToSegmentParamter( float param )
	{
		if( param < startNode.posInSpline )
			return 0;
		
		if( param >= endNode.posInSpline )
			return 1;
		
		return ( param - startNode.posInSpline ) / startNode.length;
	}
	
	/** 
	* This method clamps a normalized spline parameter to spline parameters defining the segment. The returned parameter will only represent points on the segment.
    * @param param A normalized spline parameter.
    * @return Returns a clamped spline parameter that will only represent points on the segment.
	*/
	public float ClampParameterToSegment( float param )
	{
		if( param < startNode.posInSpline )
			return startNode.posInSpline;
		
		if( param >= endNode.posInSpline )
			return endNode.posInSpline;
		
		return param;
	}
}
