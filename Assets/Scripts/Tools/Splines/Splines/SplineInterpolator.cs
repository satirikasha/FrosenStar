using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class Spline : MonoBehaviour 
{
	private Vector3 GetPositionInternal( int idxFirstPoint, float t )
	{
		if( !splineNodes[idxFirstPoint].CheckReferences( ) )
			return Vector3.zero;
		
		Vector3 P1 = splineNodes[idxFirstPoint].nodeTransform.position;
		Vector3 P2 = splineNodes[idxFirstPoint].NextNode0.nodeTransform.position;
		
		switch( interpolationMode )
		{
		case InterpolationMode.Hermite:
			{
				Vector3 T1;
				Vector3 T2;
				
				GetCatMullTangentsInternal( splineNodes[idxFirstPoint], ref P1, ref P2, out T1, out T2 );
				
				return InterpolatePosition( t, ref P1, ref P2, ref T1, ref T2 );
			}
		case InterpolationMode.Bezier:
			{
				Vector3 P3 = splineNodes[idxFirstPoint].NextNode1.nodeTransform.position;
				Vector3 P4 = splineNodes[idxFirstPoint].NextNode2.nodeTransform.position;
				
				return InterpolatePosition( t, ref P1, ref P2, ref P3, ref P4 );
			}
		default:
			{
				Vector3 P0 = splineNodes[idxFirstPoint].PrevNode0.nodeTransform.position;
				Vector3 P3 = splineNodes[idxFirstPoint].NextNode1.nodeTransform.position;
				
				return InterpolatePosition( t, ref P0, ref P1, ref P2, ref P3 );
			}
		}
	}
	
	private Vector3 GetTangentInternal( int idxFirstPoint, float t )
	{
		
		if( !splineNodes[idxFirstPoint].CheckReferences( ) )
			return Vector3.zero;
		
		Vector3 P1 = splineNodes[idxFirstPoint].nodeTransform.position;
		Vector3 P2 = splineNodes[idxFirstPoint].NextNode0.nodeTransform.position;
		
		switch( interpolationMode )
		{
		case InterpolationMode.Hermite:
			{
				Vector3 T1;
				Vector3 T2;
				
				GetCatMullTangentsInternal( splineNodes[idxFirstPoint], ref P1, ref P2, out T1, out T2 );
				
				return InterpolateTangent( t, ref P1, ref P2, ref T1, ref T2 );
			}
		case InterpolationMode.Bezier:
			{
				Vector3 P3 = splineNodes[idxFirstPoint].NextNode1.nodeTransform.position;
				Vector3 P4 = splineNodes[idxFirstPoint].NextNode2.nodeTransform.position;
				
				return InterpolateTangent( t, ref P1, ref P2, ref P3, ref P4 );
			}
		default:
			{
				Vector3 P0 = splineNodes[idxFirstPoint].PrevNode0.nodeTransform.position;
				Vector3 P3 = splineNodes[idxFirstPoint].NextNode1.nodeTransform.position;
				
				return InterpolateTangent( t, ref P0, ref P1, ref P2, ref P3 );
			}
		}
	}
	
	private Quaternion GetRotationInternal( int idxFirstPoint, float t )
	{
		if( !splineNodes[idxFirstPoint].CheckReferences( ) )
			return Quaternion.identity;
		
		Quaternion Q0 = splineNodes[idxFirstPoint].PrevNode0.nodeTransform.rotation;
		Quaternion Q1 = splineNodes[idxFirstPoint].nodeTransform.rotation;
		Quaternion Q2 = splineNodes[idxFirstPoint].NextNode0.nodeTransform.rotation;
		Quaternion Q3 = splineNodes[idxFirstPoint].NextNode1.nodeTransform.rotation;

		Quaternion T1 = GetSquadIntermediate( Q0, Q1, Q2 );
		Quaternion T2 = GetSquadIntermediate( Q1, Q2, Q3 );

		return GetQuatSquad( t, Q1, Q2, T1, T2 );
	}
	
	private Vector3 InterpolatePosition( float t, ref Vector3 P1, ref Vector3 P2, ref Vector3 P3, ref Vector3 P4 )
	{
		float t2 = t * t;
		float t3 = t2 * t;
		
		float b1;
		float b2;
		float b3;
		float b4;
		
		switch( interpolationMode )
		{
		default: 
		case InterpolationMode.Hermite:
			b1 =  2 * t3 - 3 * t2 + 0 * t + 1;
			b2 = -2 * t3 + 3 * t2 + 0 * t + 0;
			b3 =  1 * t3 - 2 * t2 + 1 * t + 0;
			b4 =  1 * t3 - 1 * t2 + 0 * t + 0;
			break;
		case InterpolationMode.Bezier:
			b1 = -1 * t3 + 3 * t2 - 3 * t + 1;
			b2 =  3 * t3 - 6 * t2 + 3 * t + 0;
			b3 = -3 * t3 + 3 * t2 + 0 * t + 0;
			b4 =  1 * t3 - 0 * t2 + 0 * t + 0;
			break;
		case InterpolationMode.BSpline:
			b1 = -1f/6f * t3 + 3f/6f * t2 - 3f/6f * t + 1f/6f;
			b2 =  3f/6f * t3 - 6f/6f * t2 + 0f/6f * t + 4f/6f;
			b3 = -3f/6f * t3 + 3f/6f * t2 + 3f/6f * t + 1f/6f;
			b4 =  1f/6f * t3 + 0f/6f * t2 + 0f/6f * t + 0f/6f;
			
			break;
		}
		
		return new Vector3( b1 * P1.x + b2 * P2.x + b3 * P3.x + b4 * P4.x, 
		                   	b1 * P1.y + b2 * P2.y + b3 * P3.y + b4 * P4.y, 
		                   	b1 * P1.z + b2 * P2.z + b3 * P3.z + b4 * P4.z );
	}
	
	private Vector3 InterpolateTangent( float t, ref Vector3 P1, ref Vector3 P2, ref Vector3 P3, ref Vector3 P4 )
	{
		float t2 = t * t;
		
		float b1;
		float b2;
		float b3;
		float b4;
		
		switch( interpolationMode )
		{
		default: 
		case InterpolationMode.Hermite:
			b1 =  6 * t2 - 6 * t + 0;
			b2 = -6 * t2 + 6 * t + 0;
			b3 =  3 * t2 - 4 * t + 1;
			b4 =  3 * t2 - 2 * t + 0;
			break;
			
		case InterpolationMode.Bezier:
			b1 = -3 * t2 + 6 * t - 3;
			b2 =  9 * t2 -12 * t + 3;
			b3 = -9 * t2 + 6 * t + 0;
			b4 =  3 * t2 - 0 * t + 0;
			break;
			
		case InterpolationMode.BSpline:
			b1 = -0.5f * t2 + 1.0f * t - 0.5f;
			b2 =  1.5f * t2 - 2.0f * t + 0.0f;
			b3 = -1.5f * t2 + 1.0f * t + 0.5f;
			b4 =  0.5f * t2 + 0.0f * t + 0.0f;
			
			break;
			
		}
		
		return new Vector3( b1 * P1.x + b2 * P2.x + b3 * P3.x + b4 * P4.x, 
		                   	b1 * P1.y + b2 * P2.y + b3 * P3.y + b4 * P4.y, 
		                   	b1 * P1.z + b2 * P2.z + b3 * P3.z + b4 * P4.z );
	}
	
	private void GetCatMullTangentsInternal( SplineNode firstNode, ref Vector3 P1, ref Vector3 P2, out Vector3 T1, out Vector3 T2 )
	{
		switch( tangentMode )
		{
		case TangentMode.UseTangents:
			T1 = firstNode.PrevNode0.nodeTransform.position;
			T2 = firstNode.NextNode1.nodeTransform.position;
			
			T1.x = (P2.x - T1.x) * tension;
			T1.y = (P2.y - T1.y) * tension;
			T1.z = (P2.z - T1.z) * tension;
			
			T2.x = (T2.x - P1.x) * tension;
			T2.y = (T2.y - P1.y) * tension;
			T2.z = (T2.z - P1.z) * tension;
			return;
		case TangentMode.UseNodeForwardVector:
			T1 = firstNode.nodeTransform.forward * tension;
			T2 = firstNode.NextNode0.nodeTransform.forward * tension;
			return;
		default:
			T1 = firstNode.PrevNode0.nodeTransform.position;
			T2 = firstNode.NextNode1.nodeTransform.position;
			
			T1.x = (P2.x - T1.x);
			T1.y = (P2.y - T1.y);
			T1.z = (P2.z - T1.z);
			
			T2.x = (T2.x - P1.x);
			T2.y = (T2.y - P1.y);
			T2.z = (T2.z - P1.z);
			
			T1.Normalize( );
			T2.Normalize( );
			
			T1.x *= tension;
			T1.y *= tension;
			T1.z *= tension;
			
			T2.x *= tension;
			T2.y *= tension;
			T2.z *= tension;
			return;
		}
	}
	
	//Approximate the length of a spline segment numerically
	private float GetSegmentLengthInternal( int idxFirstPoint )
	{
		return GetSegmentLengthInternal( idxFirstPoint, 0f, 1f, .2f );
	}
	
	private float GetSegmentLengthInternal( int idxFirstPoint, float startValue, float endValue, float step )
	{
		float result = 0f;
		
		Vector3 lastPos = GetPositionInternal( idxFirstPoint, startValue );
		
		float slightlyHigherValue = endValue+step*0.5f;
		
		for( float f = startValue + step; f < slightlyHigherValue; f += step )
		{
			Vector3 currentPos = GetPositionInternal( idxFirstPoint, f );
			
			result += Vector3.Distance( lastPos, currentPos );
			
			lastPos = currentPos;
		}
		
		return result;
	}
}
