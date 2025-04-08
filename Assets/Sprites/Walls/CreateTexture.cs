using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTexture : MonoBehaviour
{

    public RenderTexture renderTexture;

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(1024, 1024, TextureFormat.RGBAFloat, false, true);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    private IEnumerator SavePicture()
    {
        yield return new WaitForSeconds(2f);
        Texture2D texture2D = toTexture2D(renderTexture);
        byte[] bytes = texture2D.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + "/Sprites/Walls/texture.png", bytes);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SavePicture());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
