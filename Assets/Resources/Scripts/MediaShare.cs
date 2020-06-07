using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class MediaShare : MonoBehaviour {

    public int resWidth = 2550;
    public int resHeight = 3300;
    private string filename;

    private bool takeHiResShot = false;

    public void TakeHiResShot() {
        StartCoroutine(ScreenShot());
    }

    //void LateUpdate() {
    //    if (takeHiResShot) {
    //        GetComponent<Camera>().enabled = true;
    //        GetComponent<BloomOptimized>().enabled = true;
    //        GetComponent<DepthOfField>().enabled = true;
    //        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
    //        GetComponent<Camera>().targetTexture = rt;
    //        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
    //        GetComponent<Camera>().Render();
    //        RenderTexture.active = rt;
    //        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
    //        GetComponent<Camera>().targetTexture = null;
    //        RenderTexture.active = null; // JC: added to avoid errors
    //        Destroy(rt);
    //        GetComponent<Camera>().enabled = false;
    //        GetComponent<BloomOptimized>().enabled = false;
    //        GetComponent<DepthOfField>().enabled = false;
    //        byte[] bytes = screenShot.EncodeToPNG();
    //        string filename = ScreenShotName(resWidth, resHeight);
    //        System.IO.File.WriteAllBytes(filename, bytes);
    //        Debug.Log(string.Format("Took screenshot to: {0}", filename));
    //        takeHiResShot = false;
    //    }
    //}

    IEnumerator ScreenShot() {
        takeHiResShot = true;
        //while (takeHiResShot) {
        while (false) {
            GetComponent<Camera>().enabled = true;
            GetComponent<BloomOptimized>().enabled = true;
            GetComponent<DepthOfField>().enabled = true;
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            GetComponent<Camera>().targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            GetComponent<Camera>().Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            screenShot.Apply();
            GetComponent<Camera>().targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            gameObject.SetActive(false);
            GetComponent<Camera>().enabled = false;
            GetComponent<BloomOptimized>().enabled = false;
            GetComponent<DepthOfField>().enabled = false;
            byte[] data = screenShot.EncodeToPNG();
            filename = Path.Combine(Application.persistentDataPath,
                                       System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");
            File.WriteAllBytes(filename, data);
            //System.IO.File.WriteAllBytes(filename, bytes);                    //IO HALT
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            takeHiResShot = false;
            yield return null;
        }
    }

    public void Share() {
        string body = "Body of message to be shared";
        string subject = "Subject of message";
        
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + filename);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
        intentObject.Call<AndroidJavaObject>("setType", "image/png");
        AndroidJavaClass unity = new AndroidJavaClass("com.finter.sisyphean");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        currentActivity.Call("startActivity", intentObject);

        // run intent from the current Activity
        currentActivity.Call("startActivity", intentObject);
    }
}
