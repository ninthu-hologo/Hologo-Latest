using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Hologo2;
using System.IO;

public class load_image : MonoBehaviour
{

    public Texture2D texture;
    // Start is called before the first frame update
    private void Start()
    {
       // LoadImage();
        imageL();
    }

    private async void imageL()
    {
        await loadImage();
    }

    private async Task loadImage ()
    {
        texture = await readWriteData.ReadImage(564, 564, "lan", "brick.jpg");
    }

   
    private async void LoadImage()
    {
        string filePath = readWriteData.GetPath("lan", "brick.jpg");
        Task<byte[]> task = new Task<byte[]>(()=>readBytes(filePath));
        task.Start();
        byte[] bytes = await task;
        texture = new Texture2D(564, 564, TextureFormat.RGB24, false);
        texture.filterMode = FilterMode.Bilinear;
        texture.LoadImage(bytes);
    }

    private byte[] readBytes(string stringPath)
    {
        byte[] bytes = File.ReadAllBytes(stringPath);
        return bytes;
    }

    private int count()
    {
        return 41;
    }

}
