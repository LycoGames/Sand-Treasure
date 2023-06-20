#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;

enum SaveFormat
{
    Triangles,
}

enum SaveResolution
{
    Full = 0,
    Half,
    Quarter,
    Eighth,
    Sixteenth
}

enum SizeOfTexture
{
    Texture512x512,
    Texture1024x1024,
    Texture2048x2048
}

class ExportTerrain : EditorWindow
{
    SaveFormat saveFormat = SaveFormat.Triangles;
    SaveResolution saveResolution = SaveResolution.Half;

    static TerrainData terrain;
    static Vector3 terrainPos;

    int tCount;
    int counter;
    int totalCount;
    int progressUpdateInterval = 10000;

    //luna
    int sizeMultiplier = 2;
    string pathToSaveTexture = "";
    int tillingSizeOfTextures = 20;

    private Texture2D firstTexture;
    private Texture2D secondTexture;
    private Texture2D thirdTexture;
    private Texture2D fourthTexture;

    SizeOfTexture textureResolution = SizeOfTexture.Texture1024x1024;

    [MenuItem("Tools/Luna Export to .obj Format")]
    static void Init()
    {
        terrain = null;
        Terrain terrainObject = Selection.activeObject as Terrain;
        if (!terrainObject)
        {
            terrainObject = Terrain.activeTerrain;
        }

        if (terrainObject)
        {
            terrain = terrainObject.terrainData;
            terrainPos = terrainObject.transform.position;
        }

        EditorWindow.GetWindow<ExportTerrain>().Show();
    }

    void OnGUI()
    {
        if (!terrain)
        {
            GUILayout.Label("No terrain found.\nPlease make sure there is 1 active terrain object in the scene");
            if (GUILayout.Button("Refresh"))
            {
                Init();
            }

            if (GUILayout.Button("Cancel"))
            {
                EditorWindow.GetWindow<ExportTerrain>().Close();
            }

            return;
        }

      //  saveFormat = (SaveFormat)EditorGUILayout.EnumPopup("Export format", saveFormat);

        saveResolution = (SaveResolution)EditorGUILayout.EnumPopup("Resolution", saveResolution);

        textureResolution = (SizeOfTexture)EditorGUILayout.EnumPopup("Terrain texture resolution", textureResolution);

        tillingSizeOfTextures = EditorGUILayout.IntField("Tilling of the texture", tillingSizeOfTextures);

        int w = terrain.heightmapResolution;
        int h = terrain.heightmapResolution;

        int tRes = (int)Mathf.Pow(2, (int)saveResolution);

        w = (w - 1) / tRes + 1;
        h = (h - 1) / tRes + 1;
        Vector3[] tVertices = new Vector3[w * h];

        int[] tPolys;

        if (saveFormat == SaveFormat.Triangles)
        {
            tPolys = new int[(w - 1) * (h - 1) * 6];
        }
        else
        {
            tPolys = new int[(w - 1) * (h - 1) * 4];
        }


        totalCount =
            (tVertices.Length * 2 + (saveFormat == SaveFormat.Triangles ? tPolys.Length / 3 : tPolys.Length / 4));

        float totalSize = terrain.size.x * terrain.size.z;

        if (totalSize >= 2000000)
        {
            GUILayout.Label("Your terrain is most likely too big. Consider a smaller terrain.\nContinue only if you know what you're doing and need a large terrain for your playable.");
        }
        
        if(tVertices.Length > 65000)
        {
            GUILayout.Label("Vertices are over 65k. Luna/WebGL 1.0 does not support this.\nLower the resolution to export the terrain\nRecommended is Quarter or Eight (Also for performance reasons)");
        }
        else
        {
            if (GUILayout.Button("Export"))
            {
                Export();
            }
        }

    }

    void DirectorySanityCheck()
    {
        pathToSaveTexture = Path.Combine(Application.dataPath, "Luna Terrain Exporter", "Exported files");

        if (!Directory.Exists(pathToSaveTexture))
        {
            Directory.CreateDirectory(pathToSaveTexture);
        }

        var directories = Directory.GetDirectories(pathToSaveTexture).Length;
        Debug.Log(directories);
        if (directories == 0)
        {
            Directory.CreateDirectory(pathToSaveTexture + "/" + 0);
            pathToSaveTexture = pathToSaveTexture + "/" + 0;
        }
        else
        {
            directories++;
            for (int i = 0; i < directories; i++)
            {
                Debug.Log(Directory.Exists(pathToSaveTexture + "/" + i));
                Debug.Log(pathToSaveTexture + i);
                if (!Directory.Exists(pathToSaveTexture + "/" + i))
                {
                    Directory.CreateDirectory(pathToSaveTexture + "/" + i);
                    pathToSaveTexture = pathToSaveTexture + "/" + i;
                }
            }
        }
    }

    void Export()
    {
        DirectorySanityCheck();
        string fileName = pathToSaveTexture + "/ExportedTerrain.obj";
        int w = terrain.heightmapResolution;
        int h = terrain.heightmapResolution;
        Vector3 meshScale = terrain.size;
        int tRes = (int)Mathf.Pow(2, (int)saveResolution);
        meshScale = new Vector3(meshScale.x / (w - 1) * tRes, meshScale.y, meshScale.z / (h - 1) * tRes);
        Vector2 uvScale = new Vector2(1.0f / (w - 1), 1.0f / (h - 1));
        float[,] tData = terrain.GetHeights(0, 0, w, h);

        w = (w - 1) / tRes + 1;
        h = (h - 1) / tRes + 1;
        Vector3[] tVertices = new Vector3[w * h];
        Vector2[] tUV = new Vector2[w * h];

        int[] tPolys;

        if (saveFormat == SaveFormat.Triangles)
        {
            tPolys = new int[(w - 1) * (h - 1) * 6];
        }
        else
        {
            tPolys = new int[(w - 1) * (h - 1) * 4];
        }


        // Build vertices and UVs
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                tVertices[y * w + x] =
                    Vector3.Scale(meshScale, new Vector3(-y, tData[x * tRes, y * tRes], x)) + terrainPos;
                tUV[y * w + x] = Vector2.Scale(new Vector2(x * tRes, y * tRes), uvScale);
            }
        }

        int index = 0;
        if (saveFormat == SaveFormat.Triangles)
        {
            // Build triangle indices: 3 indices into vertex array for each triangle
            for (int y = 0; y < h - 1; y++)
            {
                for (int x = 0; x < w - 1; x++)
                {
                    // For each grid cell output two triangles
                    tPolys[index++] = (y * w) + x;
                    tPolys[index++] = ((y + 1) * w) + x;
                    tPolys[index++] = (y * w) + x + 1;

                    tPolys[index++] = ((y + 1) * w) + x;
                    tPolys[index++] = ((y + 1) * w) + x + 1;
                    tPolys[index++] = (y * w) + x + 1;
                }
            }
        }
        else
        {
            // Build quad indices: 4 indices into vertex array for each quad
            for (int y = 0; y < h - 1; y++)
            {
                for (int x = 0; x < w - 1; x++)
                {
                    // For each grid cell output one quad
                    tPolys[index++] = (y * w) + x;
                    tPolys[index++] = ((y + 1) * w) + x;
                    tPolys[index++] = ((y + 1) * w) + x + 1;
                    tPolys[index++] = (y * w) + x + 1;
                }
            }
        }

        // Export to .obj
        StreamWriter sw = new StreamWriter(fileName);
        try
        {
            sw.WriteLine("# Unity terrain OBJ File");

            // Write vertices
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            counter = tCount = 0;
            totalCount =
                (tVertices.Length * 2 + (saveFormat == SaveFormat.Triangles ? tPolys.Length / 3 : tPolys.Length / 4)) /
                progressUpdateInterval;
            for (int i = 0; i < tVertices.Length; i++)
            {
                UpdateProgress();
                StringBuilder sb = new StringBuilder("v ", 20);
                // StringBuilder stuff is done this way because it's faster than using the "{0} {1} {2}"etc. format
                // Which is important when you're exporting huge terrains.
                sb.Append(tVertices[i].x.ToString()).Append(" ").Append(tVertices[i].y.ToString()).Append(" ")
                    .Append(tVertices[i].z.ToString());
                sw.WriteLine(sb);
            }

            // Write UVs
            for (int i = 0; i < tUV.Length; i++)
            {
                UpdateProgress();
                StringBuilder sb = new StringBuilder("vt ", 22);
                sb.Append(tUV[i].x.ToString()).Append(" ").Append(tUV[i].y.ToString());
                sw.WriteLine(sb);
            }

            if (saveFormat == SaveFormat.Triangles)
            {
                // Write triangles
                for (int i = 0; i < tPolys.Length; i += 3)
                {
                    UpdateProgress();
                    StringBuilder sb = new StringBuilder("f ", 43);
                    sb.Append(tPolys[i] + 1).Append("/").Append(tPolys[i] + 1).Append(" ").Append(tPolys[i + 1] + 1)
                        .Append("/").Append(tPolys[i + 1] + 1).Append(" ").Append(tPolys[i + 2] + 1).Append("/")
                        .Append(tPolys[i + 2] + 1);
                    sw.WriteLine(sb);
                }
            }
            else
            {
                // Write quads
                for (int i = 0; i < tPolys.Length; i += 4)
                {
                    UpdateProgress();
                    StringBuilder sb = new StringBuilder("f ", 57);
                    sb.Append(tPolys[i] + 1).Append("/").Append(tPolys[i] + 1).Append(" ").Append(tPolys[i + 1] + 1)
                        .Append("/").Append(tPolys[i + 1] + 1).Append(" ").Append(tPolys[i + 2] + 1).Append("/")
                        .Append(tPolys[i + 2] + 1).Append(" ").Append(tPolys[i + 3] + 1).Append("/")
                        .Append(tPolys[i + 3] + 1);
                    sw.WriteLine(sb);
                }
            }
        }
        catch (Exception err)
        {
            Debug.Log("Error saving file: " + err.Message);
        }

        sw.Close();

        GetTextures();

        terrain = null;
        EditorUtility.DisplayProgressBar("Saving file to disc.", "This might take a while...", 1f);
        EditorWindow.GetWindow<ExportTerrain>().Close();
        EditorUtility.ClearProgressBar();
    }

    void UpdateProgress()
    {
        if (counter++ == progressUpdateInterval)
        {
            counter = 0;
            EditorUtility.DisplayProgressBar("Saving...", "", Mathf.InverseLerp(0, totalCount, ++tCount));
        }
    }

    //Luna
    void GetTextures()
    {
        sizeMultiplier = GetSizeMultiplier();
        if(terrain.alphamapTextures == null || terrain.alphamapTextureCount == 0)
        {
            return;
        }
        var width = terrain.alphamapTextures[0].width * sizeMultiplier;
        var height = terrain.alphamapTextures[0].height * sizeMultiplier;

        AssignTextures();

        Texture2D generatedTexture = new Texture2D(width, height);
        Color red = new Color();
        Color green = new Color();
        Color blue = new Color();
        Color alpha = new Color();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float alphaR = 0, alphaG = 0, alphaB = 0, alphaA = 0;
                if (firstTexture != null)
                {
                    alphaR = GetColorAtPixelFromTerrain(x / sizeMultiplier, y / sizeMultiplier).r;
                    red = firstTexture.GetPixel(x * tillingSizeOfTextures, y * tillingSizeOfTextures);
                    red.a = alphaR;
                }

                if (secondTexture != null)
                {
                    alphaG = GetColorAtPixelFromTerrain(x / sizeMultiplier, y / sizeMultiplier).g;
                    green = secondTexture.GetPixel(x * tillingSizeOfTextures, y * tillingSizeOfTextures);
                    green.a = alphaG;
                }

                if (thirdTexture != null)
                {
                    alphaB = GetColorAtPixelFromTerrain(x / sizeMultiplier, y / sizeMultiplier).b;
                    blue = thirdTexture.GetPixel(x * tillingSizeOfTextures, y * tillingSizeOfTextures);
                    blue.a = alphaB;
                }
                if (fourthTexture != null)
                {
                    alphaA = GetColorAtPixelFromTerrain(x / sizeMultiplier, y / sizeMultiplier).a;
                    alpha = fourthTexture.GetPixel(x * tillingSizeOfTextures, y * tillingSizeOfTextures);
                    alpha.a = alphaA;
                }

                Color avarage = new Color(
                    (red.r * alphaR) + (green.r * alphaG) + (blue.r * alphaB) + (alpha.r * alphaA),
                    (red.g * alphaR) + (green.g * alphaG + (blue.g * alphaB) + (alpha.g * alphaA)),
                    (red.b * alphaR) + (green.b * alphaG) + (blue.b * alphaB) + (alpha.b * alphaA));

                generatedTexture.SetPixel(x, y, avarage);
            }
        }

        generatedTexture.Apply();

        WriteTextureFile(generatedTexture);
        AssetDatabase.Refresh();
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>("Luna Terrain Exporter/Exported files/");
    }

    void AssignTextures()
    {
        int numberOfTextures = terrain.terrainLayers.Length;
        for (int i = 0; i < numberOfTextures; i++)
        {
            if (i == 0)
            {
                firstTexture = terrain.terrainLayers[i].diffuseTexture;
            }

            if (i == 1)
            {
                secondTexture = terrain.terrainLayers[i].diffuseTexture;

            }
            if (i == 2)
            {
                thirdTexture = terrain.terrainLayers[i].diffuseTexture;
            }

            if (i == 3)
            {
                fourthTexture = terrain.terrainLayers[i].diffuseTexture;
            }
        }

    }

    void WriteTextureFile(Texture2D generatedTexture)
    {

        //Check if directory excists

        if (Directory.Exists(pathToSaveTexture))
        {
            File.WriteAllBytes(pathToSaveTexture + "/ExportedTerrainTexture.png", generatedTexture.EncodeToPNG());
        }
        else
        {
            Directory.CreateDirectory(pathToSaveTexture);
            File.WriteAllBytes(pathToSaveTexture + "/ExportedTerrainTexture.png", generatedTexture.EncodeToPNG());
        }
    }

    Color GetColorAtPixelFromTerrain(int width, int height)
    {
        return terrain.alphamapTextures[0].GetPixel(height, width);
    }

    Color GetColorAtPixelFromTerrainSecond(int width, int height)
    {
        return terrain.alphamapTextures[1].GetPixel(height, width);
    }

    int GetSizeMultiplier()
    {
        switch (textureResolution)
        {
            case SizeOfTexture.Texture512x512:
                return 1;
            case SizeOfTexture.Texture1024x1024:
                return 2;
            case SizeOfTexture.Texture2048x2048:
                return 4;
        }

        return 0;
    }
}
#endif