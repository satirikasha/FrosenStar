using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class Spline : MonoBehaviour
{
	void OnDrawGizmos( )
	{
		UpdateSplineNodes( );
		
		if( splineNodes == null )
			return;
		
		DrawSplineGizmo( new Color( 0.5f, 0.5f, 0.5f, 0.5f ) );
		
		Plane screen = new Plane( );
		Gizmos.color = new Color( 1f, 1f, 1f, 0.5f );
		
		screen.SetNormalAndPosition( Camera.current.transform.forward, Camera.current.transform.position );
		
		foreach( SplineNode node in splineNodes )
		{
			float sizeMultiplier = 0f;
			
			if( Camera.current.orthographic )
				sizeMultiplier = Camera.current.orthographicSize * 2.5f;
			else
				screen.Raycast( new Ray( node.Position, Camera.current.transform.forward ), out sizeMultiplier );
			
			Gizmos.DrawSphere( node.Position, sizeMultiplier * 0.015f );
		}
	}
	
	void OnDrawGizmosSelected( )
	{
		UpdateSplineNodes( );
		
		if( splineNodes == null )
			return;
		
		DrawSplineGizmo( new Color( 1f, 0.5f, 0f, 1f ) );
		
		Plane screen = new Plane( );
		Gizmos.color = new Color( 1f, 0.5f, 0f, 0.75f );
		
		screen.SetNormalAndPosition( Camera.current.transform.forward, Camera.current.transform.position );
		
		foreach( SplineNode node in splineNodes )
		{
			float sizeMultiplier = 0f;
			
			if( Camera.current.orthographic )
				sizeMultiplier = Camera.current.orthographicSize * 2.5f;
			else
				screen.Raycast( new Ray( node.Position, Camera.current.transform.forward ), out sizeMultiplier );
			
			Gizmos.DrawSphere( node.Position, sizeMultiplier * 0.0075f );
		}
	}
	
	void DrawSplineGizmo( Color curveColor )
	{
		int step = 1;
		
		switch( interpolationMode )
		{
		case InterpolationMode.BSpline:
			Gizmos.color = new Color( curveColor.r, curveColor.g, curveColor.b, curveColor.a * 0.25f );
			
			for( int i = 0; i < ControlSegmentCount; i++ )
				Gizmos.DrawLine( splineNodes[i].Position, splineNodes[i].NextNode0.Position );
			
			goto default;
			
		case InterpolationMode.Bezier:
			Gizmos.color = new Color( curveColor.r, curveColor.g, curveColor.b, curveColor.a * 0.25f );
			
			for( int i = 0; i < ControlSegmentCount; i++ )
				Gizmos.DrawLine( splineNodes[i].Position, splineNodes[i].NextNode0.Position );
			
			step = 3;
			
			goto default;
		
		case InterpolationMode.Hermite:
		default:
			Gizmos.color = curveColor;
			
			for( int i = 0; i < ControlSegmentCount; i+=step )
			{
				Vector3 lastPos = GetPositionInternal( i, 0 );
				
				for( float f = 0.05f; f < 1.05f; f += 0.05f )
				{
					Vector3 curPos = GetPositionInternal( i, f );
					
					Gizmos.DrawLine( lastPos, curPos );
					
					lastPos = curPos;
				}
			}
			
			break;
		}
	}
}
