using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class Spline : MonoBehaviour 
{
	/** 
	* This function calculates the closest point on the spline to a given point.
	* @param p A given point.
	* @param iterations Define how accurate the calculation will be. A value of 5 should be high enough for most purposes.
    * @return Returns the closest point to p on the spline.
	*/
	public float GetClosestPoint( Vector3 p, int iterations )
	{
		float len = Mathf.Infinity;
		float param = 0f;
		
		iterations = Mathf.Clamp( iterations, 0, 5 );
		
		for( float f = 0f; f <= 1f; f += 0.01f )
		{
			float tmpLen = ( GetPositionOnSpline( f ) - p ).sqrMagnitude;
			
			if( len > tmpLen )
			{
				len = tmpLen;
				param = f;
			}
		}
		
		for( int i = 0; i < iterations; i++ )
		{
			float divergence = 0.01f * Mathf.Pow( 10f, -(float)i );
			float divergence10 = divergence * 0.1f;
			
			for( float f = Mathf.Clamp01(param-divergence); f <= Mathf.Clamp01(param+divergence); f += divergence10 )
			{
				float tmpLen = ( GetPositionOnSpline( f ) - p ).sqrMagnitude;
				
				if( len > tmpLen )
				{
					len = tmpLen;
					param = f;
				}
			}
		}
		
		return param;
	}
	
	/** 
	* This function calculates the closest point on a specific region of the spline to an other given point. 
	* It is very useful if you want to prevent big jumps from one point on the spline to another.
	* @param p A given point.
	* @param iterations Define how accurate the calculation will be. A value of 5 should be high enough for most purposes.
	* @param lastParam A parameter that represents the middle of the specified spline region.
	* @param diff A parameter that represents the length of the specified spline region.
    * @return Returns the closest point to p on the spline.
	*/
	public float GetClosestPoint( Vector3 p, int iterations, float lastParam, float diff )
	{
		float len = Mathf.Infinity;
		float param = 0f;
		
		iterations = Mathf.Clamp( iterations, 0, 5 );
		
		for( float f = 0f; f <= 1f; f += 0.01f )
		{
			float tmpLen = ( GetPositionOnSpline( f ) - p ).magnitude;
			
			if( len > tmpLen && Mathf.Abs( f - lastParam ) < diff )
			{
				len = tmpLen;
				param = f;
			}
		}
		
		for( int i = 0; i < iterations; i++ )
		{
			float divergence = 0.01f / Mathf.Pow( 10f, (float)i );
			float divergence10 = divergence * 0.1f;
			
			for( float f = Mathf.Clamp01(param-divergence); f <= Mathf.Clamp01(param+divergence); f += divergence10 )
			{
				float tmpLen = ( GetPositionOnSpline( f ) - p ).magnitude;
				
				if( len > tmpLen && Mathf.Abs( f - lastParam ) < diff )
				{
					len = tmpLen;
					param = f;
				}
			}
		}
		
		return param;
	}
	
	/** 
	* This function calculates the shortest connecting line from a given point to the spline.
	* @param p A given point.
	* @param iterations Define how accurate the calculation will be. A value of 5 should be high enough for most purposes.
    * @return Returns the shortest connection from p to the spline.
	*/
	public Vector3 GetShortestConnection( Vector3 p, int iterations )
	{
		return GetPositionOnSpline( GetClosestPoint( p, iterations ) ) - p;
	}
}
