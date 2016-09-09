using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


/**
* @class SplineMesh
*
* @brief This class provides functions for generating curved meshes around a Spline.
*
* This class allows you to dynamically generate curves meshes (e. g. streets, rivers, tubes, ropes, tunnels, etc).
*/ 

[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
public class SplineMesh : MonoBehaviour
{
	public Spline spline;///< Reference to the spline that defines the path.
	
	public Spline.UpdateMode uMode = Spline.UpdateMode.DontUpdate; ///< Specifies when the mesh will be updated.
	
	public float deltaSeconds = 0.1f; ///< Specifies after how much time the mesh will be updated (see UpdateMode).
	public int deltaFrames = 2; ///< Specifies after how many frames the mesh will be updated (see UpdateMode).
	
	public Mesh baseMesh; ///< Reference to the base mesh that will be created around the spline.
	public int segmentCount = 100; ///< Number of segments (base meshes) stringed together per generated mesh.
	public Vector2 xyScale = Vector2.one; ///< Mesh scale in the directions arount the spline.
	
	public Vector2 uvScale = Vector2.one; ///< Affects the calculation of texture coordinates along the streched mesh
	public bool swapUV = false; ///< Defines which UV component will be extruded.
	
	public int splineSegment = -1; ///< Index of the spline segment that will be used as control path. If set to -1, the whole spline will be used.
	
	private Mesh bentMesh = null;
	
	private float passedTime = 0f;
	
	public Mesh BentMesh { get { return bentMesh; } } ///< Returns a reference to the spline mesh.
			
	
	void Start( )
	{
		if( spline == null )
			return;
		
		spline.UpdateSplineNodes( );
		UpdateMesh( );
	}
	
	void OnEnable( )
	{
		if( spline == null )
			return;
		
		spline.UpdateSplineNodes( );
		UpdateMesh( );
	}
	
	void LateUpdate( )
	{
		switch( uMode )
		{
		case Spline.UpdateMode.EveryFrame:
			UpdateMesh( );
			break;
			
		case Spline.UpdateMode.EveryXFrames:
			if( deltaFrames <= 0 )
				deltaFrames = 1;
			
			if( Time.frameCount % deltaFrames == 0 )
				UpdateMesh( );
			
			break;
			
		case Spline.UpdateMode.EveryXSeconds:
			passedTime += Time.deltaTime;
			
			if( passedTime >= deltaSeconds )
			{
				UpdateMesh( );
				passedTime = 0f;
			}
			
			break;
		}
	}
	
	/** 
	* This function updates the spline mesh. It is called automatically once in a while, if updateMode isn't set to DontUpdate.
	*/
	public void UpdateMesh( )
	{
		Setup( );
		
		//Reset the generated meshes
		if( BentMesh )
			BentMesh.Clear( );
		
		if( baseMesh == null || spline == null || segmentCount <= 0 )
			return;
		
		
		//Gather model data
		Vector3[] verticesBase = baseMesh.vertices;
		Vector3[] normalsBase = baseMesh.normals;
		Vector4[] tangentsBase = baseMesh.tangents;
		Vector2[] uvBase = baseMesh.uv;
		
		int[] trianglesBase = baseMesh.triangles;
		
		
		//Allocate some memory for new mesh data
		Vector3[] verticesNew = new Vector3[verticesBase.Length * segmentCount];
		Vector3[] normalsNew = new Vector3[normalsBase.Length * segmentCount];
		Vector4[] tangentsNew = new Vector4[tangentsBase.Length * segmentCount];
		Vector2[] uvNew = new Vector2[uvBase.Length * segmentCount];
		
		int[] trianglesNew = new int[trianglesBase.Length * segmentCount];
		
		
		//Group front/rear vertices together 
		List<int> verticesFront = new List<int>( );
		List<int> verticesBack = new List<int>( );
		
		Vector3 centerFront = Vector3.zero;
		Vector3 centerBack = Vector3.zero;
		
		for( int i = 0; i < verticesBase.Length; i++ )
		{
			if( verticesBase[i].z > 0f )
			{
				verticesFront.Add( i );
				centerFront += verticesBase[i];
			}
			else if( verticesBase[i].z < 0f )
			{
				verticesBack.Add( i );
				centerBack += verticesBase[i];
			}
		}
		
		centerFront /= verticesFront.Count;
		centerBack /= verticesBack.Count;
		
		
		if( splineSegment >= 0 && splineSegment < spline.SegmentCount )
		{
			SplineSegment currentSegment = spline.SplineSegments[splineSegment];
			
			int vIndex = 0;
			
			for( int segment = 0; segment < segmentCount; segment++ )
			{
				float param0 = (float) segment / segmentCount;
				float param1 = (float) (segment+1) / segmentCount;
				
				if( param1 == 1f ) param1 -= 0.00001f;
				
				param0 = currentSegment.ConvertSegmentToSplineParamter( param0 );
				param1 = currentSegment.ConvertSegmentToSplineParamter( param1 );
				
				CalculateBentMesh( ref vIndex, verticesFront, verticesBack, ref centerFront, ref centerBack, param0, param1, 
					verticesBase, normalsBase, tangentsBase, uvBase, verticesNew, normalsNew, tangentsNew, uvNew );
				
				for( int i = 0; i < trianglesBase.Length; i++ )
					trianglesNew[i+(segment*trianglesBase.Length)] = trianglesBase[i] + (verticesBase.Length * segment);
			}
				
			BentMesh.vertices = verticesNew;
			BentMesh.uv = uvNew;
			
			if( normalsBase.Length > 0 )
				BentMesh.normals = normalsNew;
			
			if( tangentsBase.Length > 0 )
				BentMesh.tangents = tangentsNew;
			
			BentMesh.triangles = trianglesNew;
		}
		else
		{
			int vIndex = 0;
				
			for( int segment = 0; segment < segmentCount; segment++ )
			{
				float param0 = (float) segment / segmentCount;
				float param1 = (float) (segment+1) / segmentCount;
				
				if( param1 == 1f ) param1 -= 0.00001f;
				
				CalculateBentMesh( ref vIndex, verticesFront, verticesBack, ref centerFront, ref centerBack, param0, param1, 
					verticesBase, normalsBase, tangentsBase, uvBase, verticesNew, normalsNew, tangentsNew, uvNew );
				
				for( int i = 0; i < trianglesBase.Length; i++ )
					trianglesNew[i+(segment*trianglesBase.Length)] = trianglesBase[i] + (verticesBase.Length * segment);
			}
			
			BentMesh.vertices = verticesNew;
			BentMesh.uv = uvNew;
			
			if( normalsBase.Length > 0 )
				BentMesh.normals = normalsNew;
			
			if( tangentsBase.Length > 0 )
				BentMesh.tangents = tangentsNew;
			
			BentMesh.triangles = trianglesNew;
		}
	}
	
	private void Setup( )
	{
		if( spline == null )
			return;
		
		if( bentMesh == null )
		{
			bentMesh = new Mesh( );
		
			bentMesh.name = "BentMesh";
			bentMesh.hideFlags = HideFlags.HideAndDontSave;
		}
		
		MeshFilter meshFilter = GetComponent<MeshFilter>( );
		
		if( meshFilter.sharedMesh != BentMesh )
			meshFilter.sharedMesh = BentMesh;
		
		
		
		MeshCollider meshCollider = GetComponent<MeshCollider>( );
		
		if( meshCollider != null )
		{
			meshCollider.sharedMesh = null;
			meshCollider.sharedMesh = BentMesh;
		}
	}
	
	private void CalculateBentMesh( ref int vIndex, List<int> verticesFront, List<int> verticesBack, ref Vector3 centerFront, ref Vector3 centerBack,
		float param0, float param1, 
		Vector3[] verticesBase, Vector3[] normalsBase, Vector4[] tangentsBase, Vector2[] uvBase,
		Vector3[] verticesNew, Vector3[] normalsNew, Vector4[] tangentsNew, Vector2[] uvNew )
	{
		Vector3 pos0 = spline.transform.InverseTransformPoint(spline.GetPositionOnSpline( param0 ));
		Vector3 pos1 = spline.transform.InverseTransformPoint(spline.GetPositionOnSpline( param1 ));
		
		Quaternion rot0 = spline.GetOrientationOnSpline( param0 ) * Quaternion.Inverse( spline.transform.localRotation );
		Quaternion rot1 = spline.GetOrientationOnSpline( param1 ) * Quaternion.Inverse( spline.transform.localRotation );
		
		for( int i = 0; i < verticesBase.Length; i++ )
		{
			Vector3 tmpVert;
			Vector3 tmpUV;
			
			Vector3 tmpNormal;
			Vector3 tmpTangent;
			
			tmpVert = verticesBase[i];
			tmpUV = uvBase[i];
			
			if( normalsBase.Length > 0 )
				tmpNormal = normalsBase[i];
			else
				tmpNormal = Vector3.zero;
			
			if( tangentsBase.Length > 0 )
				tmpTangent = tangentsBase[i];
			else
				tmpTangent = Vector3.zero;
			
			if( verticesBack.Contains( i ) )
			{
				tmpVert -= centerBack;
				
				tmpVert.Scale( new Vector3( xyScale[0], xyScale[1], 1f ) );
				
				tmpVert = rot0 * tmpVert;
				tmpVert += pos0;
				
				tmpNormal = rot0 * tmpNormal;
				tmpTangent = rot0 * tmpTangent;
				
				if( !swapUV )
					tmpUV.y = param0;
				else
					tmpUV.x = param0;
			}
			else if( verticesFront.Contains( i ) )
			{
				tmpVert -= centerFront;
				
				tmpVert.Scale( new Vector3( xyScale[0], xyScale[1], 1f ) );
				
				tmpVert = rot1 * tmpVert;
				tmpVert += pos1;
				
				tmpNormal = rot1 * tmpNormal;
				tmpTangent = rot1 * tmpTangent;
				
				if( !swapUV )
					tmpUV.y = param1;
				else
					tmpUV.x = param1;
			}
			
			verticesNew[vIndex] = tmpVert;
			uvNew[vIndex] = Vector2.Scale( tmpUV, uvScale );
			
			if( normalsBase.Length > 0 )
				normalsNew[vIndex] = tmpNormal;
			
			if( tangentsBase.Length > 0 )
				tangentsNew[vIndex] = tmpTangent;
			
			vIndex++;
		}
	}
	
}
