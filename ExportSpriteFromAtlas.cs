using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExportSpriteFromAtlas
{
    public static Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();

    private static string fileName;

    [MenuItem("Assets/Export Sprite From Atlas", false, 0)]
    public static void SpriteFromAtlas()
    {
        //init
        spriteDic = new Dictionary<string, Sprite>();
        
        fileName = Selection.activeObject.name;
        LoadSprite(fileName);
    }

    //Loads the provided sprites
    public static void LoadSprite(string spriteBaseName)
    {
        Sprite[] allSprites = Resources.LoadAll<Sprite>(spriteBaseName);

        if (allSprites == null || allSprites.Length <= 0)
        {
            Debug.LogError("The Provided Base-Atlas Sprite `" + spriteBaseName + "` does not exist!");
            return;
        }

        for (int i = 0; i < allSprites.Length; i++)
        {
            spriteDic.Add(allSprites[i].name, allSprites[i]);
            MakeFile(allSprites[i]);
        }
    }

    public static void MakeFile(Sprite sprite)
    {
        try
        {
            Debug.Log(string.Format("{0} : {1}", sprite.name, sprite.rect.ToString()));
            Rect rect = sprite.rect;
            Texture2D mainTex = sprite.texture;
            
            Texture2D tex = new Texture2D((int)rect.width, (int)rect.height, sprite.texture.format, false);
            
            Color[] c = mainTex.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
            tex.SetPixels(c);
            tex.Apply();
            var bytes = tex.EncodeToPNG();
            string savePath = string.Format("{0}/{1}.png", Application.persistentDataPath, "[" + fileName + "]" + sprite.name);
            Object.DestroyImmediate(tex, true);
            System.IO.File.WriteAllBytes(savePath, bytes);
            Debug.Log("MakeFile : " + savePath + sprite.name);
        }
#pragma warning disable CS0168 // Variable is declared but never used
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
        catch (System.Exception ex)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore CS0168 // Variable is declared but never used
        {

        }

    }
}