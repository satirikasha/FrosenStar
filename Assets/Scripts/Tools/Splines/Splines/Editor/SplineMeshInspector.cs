using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SplineMesh))]
public class SplineMeshInspector : Editor
{
	private Spline.UpdateMode uMode;
	
	private int splineSegment;
	private int segmentCount;
	private int deltaFrames;
	private float deltaSeconds;
	
	private bool swapUV;
	private bool splitMesh;
	
	private Vector2 xyScale;
	private Vector2 uvScale;
	
	private Spline spline;
	
	private Mesh baseMesh;
	
	public override void OnInspectorGUI( )
	{
		SplineMesh mesh = (SplineMesh) target;
		
		EditorGUILayout.BeginVertical( );
		
			EditorGUILayout.Space( );
			spline = (Spline) EditorGUILayout.ObjectField( "   Spline", mesh.spline, typeof( Spline ), true ); 
			baseMesh = (Mesh) EditorGUILayout.ObjectField( "   Base Mesh", mesh.baseMesh, typeof( Mesh ), false ); 
			EditorGUILayout.Space( );
		
			uMode = (Spline.UpdateMode) EditorGUILayout.EnumPopup( "   Update Mode", mesh.uMode );
			
			if( uMode == Spline.UpdateMode.EveryXFrames )
				deltaFrames = EditorGUILayout.IntField( "   Delta Frames", mesh.deltaFrames );
			else if( uMode == Spline.UpdateMode.EveryXSeconds )
				deltaSeconds = EditorGUILayout.FloatField( "   Delta Seconds", mesh.deltaSeconds );
		
			
			segmentCount = Mathf.Max( EditorGUILayout.IntField( "   Segment Count", mesh.segmentCount ), 1 );
			
			EditorGUILayout.BeginHorizontal( );
				EditorGUILayout.PrefixLabel( "   Scale" );
				xyScale.x = EditorGUILayout.FloatField( mesh.xyScale.x, GUILayout.MinWidth( 10 ) );
				xyScale.y = EditorGUILayout.FloatField( mesh.xyScale.y, GUILayout.MinWidth( 10 ) );
			EditorGUILayout.EndHorizontal( );
		
			EditorGUILayout.BeginHorizontal( );
				EditorGUILayout.PrefixLabel( "   UV-Scale" );
				uvScale.x = EditorGUILayout.FloatField( mesh.uvScale.x, GUILayout.MinWidth( 10 ) );
				uvScale.y = EditorGUILayout.FloatField( mesh.uvScale.y, GUILayout.MinWidth( 10 ) );
			EditorGUILayout.EndHorizontal( );
		
			swapUV = EditorGUILayout.Toggle( "   Swap UV", mesh.swapUV );
			splitMesh = EditorGUILayout.Toggle( "   Split Mesh", (mesh.splineSegment != -1) );
		
			if( splitMesh && spline != null )
				splineSegment = Mathf.Clamp( EditorGUILayout.IntField( "   Segment Index", mesh.splineSegment ), 0, spline.SegmentCount - 1 );
			
			EditorGUILayout.Space( );
			
		EditorGUILayout.EndVertical( );
		
		if( GUI.changed )
		{
			Undo.RegisterUndo( target, "Change Spline Mesh Settings" );
			EditorUtility.SetDirty( target );
			
			if( baseMesh == null )
				Debug.LogWarning( "There is no base mesh assigned to your spline mesh! Check the inspector to assign it!", mesh.gameObject );
			
			mesh.uMode = uMode;
			mesh.spline = spline;
			mesh.swapUV = swapUV;
			mesh.xyScale = xyScale;
			mesh.uvScale = uvScale;
			mesh.baseMesh = baseMesh;
			mesh.deltaFrames = deltaFrames;
			mesh.deltaSeconds = deltaSeconds;
			mesh.segmentCount = segmentCount;
			
			if( splitMesh )
				mesh.splineSegment = splineSegment;
			else
				mesh.splineSegment = -1;
			
			mesh.UpdateMesh( );
		}
	}
	
}
