// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.iOS;
// using System.IO;
// using System;
// # UNITY_IOS
// using Photos;


// public class ScreenCap : MonoBehaviour
// {
//     [SerializeField]
//     private Camera cam;

//     [SerializeField]
//     string saveLocation;

//     [SerializeField]
//     private Canvas canvas;


//     IEnumerator _capture(string filename)
//     {
// 	//initiate
//         RenderTexture texture = new RenderTexture(Screen.width, Screen.height, 24);
//         cam.targetTexture = texture;

//         cam.Render();

//         Texture2D texture_holder = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

//         canvas.enabled = false; //disable the canvas to hide UI elements
	
// 	//capture
//         RenderTexture.active = texture;
//         texture_holder.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

//         texture_holder.Apply();
	
// 	//save
//         byte[] bytes = texture_holder.EncodeToJPG();
//         string savePath = Path.Combine(Application.persistentDataPath, saveLocation, filename);
//         string directoryPath = Path.GetDirectoryName(savePath);

//          NativeGallery.SaveImageToGallery(bytes, "HologoScreenshots", filename, (success, path) =>
//         {
//             if (success)
//             {
//                 Debug.Log("Screenshot saved to Photos library.");
//             }
//             else
//             {
//                 Debug.LogError("Failed to save screenshot to Photos library.");
//             }
//         });
// if (!Directory.Exists(directoryPath))
// {
//     Directory.CreateDirectory(directoryPath);
// }
//         File.WriteAllBytes(savePath, bytes);

// 	//cleanup
//         canvas.enabled = true; //enable the canvas back
//         cam.targetTexture = null;
//         RenderTexture.active = null;
//         Destroy(texture);
        
//         yield return null;
//     }

//     public void capture()
//     {
//         string _filename = "HologoCapture" + DateTime.Now.ToString(); //set up a unique name to avoid overriding

//         StartCoroutine(_capture(_filename));
//     }
// }
