using UnityEngine;
using System.Collections;
using UnityEditor;

public class ModelPreprocessor : AssetPostprocessor {
    
    void OnPreprocessModel() {
        var modelImporter = assetImporter as ModelImporter;
        modelImporter.importMaterials = false;
    }

}
