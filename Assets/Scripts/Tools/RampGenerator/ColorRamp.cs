using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "ColorRamp", menuName = "ColorRamp", order = 5)]
public class ColorRamp : ScriptableObject {

    public int Resolution = 256;
    public Gradient Gradient;
   
    void OnValidate() {
        //Generate();
    }

    public void Generate() {
        var texPath = AssetDatabase.GetAssetPath(this);
        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);
        if(texture == null) {
            texture = CreateTexture();
            AssetDatabase.AddObjectToAsset(texture, this);
        }
       
        texture.SetPixels(GetPixels(Resolution));
        texture.Apply();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private Color[] GetPixels(int resolution) {
        var result = new Color[resolution];
        for (int i = 0; i < resolution; i++) {
            result[i] = Gradient.Evaluate((float)i / (float)resolution);
        }
        return result;
    }

    private Texture2D CreateTexture() {
        var texture = new Texture2D(Resolution, 1, TextureFormat.ARGB32, true);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.name = this.name + "Result";
        return texture;
    }
}
