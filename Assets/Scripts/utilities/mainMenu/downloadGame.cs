using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using UnityEngine.Networking;

public class downloadGame : MonoBehaviour
{
    [Header("downloading vars")]
    bool hasDownloaded = false;
    bool downloadStarted = false;
    
    public void dowoloadLogo(string url, Image spirte)
    {
        StartCoroutine(DownloadLogo(url, spirte));
    }
     IEnumerator DownloadLogo(string downloadUrl, Image wheretoputTheTexture)
    {

        UnityEngine.Debug.Log("Downloading image");

        UnityEngine.Debug.Log(downloadUrl);
        

        Texture2D downloadedTexture = null;

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(downloadUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            UnityEngine.Debug.Log("cant connect oof");
            UnityEngine.Debug.Log(request.error);
            
        }
        else
        {
            downloadedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            UnityEngine.Debug.Log("image downladed");
           


        }

        if (downloadedTexture != null)
        {
            wheretoputTheTexture.sprite = Sprite.Create(downloadedTexture, new Rect(0f, 0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100f);
        }

        else
        {
            UnityEngine.Debug.Log("texuture is null");
            
        }


    }

    public void DownloadGame(UnityWebRequest uwr, string gName, string pathExtractTo, Image sliderImage, Text gnameTexe, downloadableGameInfoContainer infoContainer)
    {
        StartCoroutine(downloadFile_Coroutine(gnameTexe, gName, uwr, sliderImage, pathExtractTo, infoContainer));
    }

    IEnumerator downloadFile_Coroutine(Text gnameText, string gName, UnityWebRequest uwr, Image sliderImage, string pathExtractTo, downloadableGameInfoContainer infoContainer)
    {
        gnameText.text = "Downloading the module...";
        //var uwr = new UnityWebRequest("https://drive.google.com/u/0/uc?id=1g1xFDKdRILlgGE5pyaSf2dONFcWusZxE&export=download", UnityWebRequest.kHttpVerbGET);
        string path = Path.Combine(Application.dataPath, gName + ".zip");
        uwr.downloadHandler = new DownloadHandlerFile(path);

        infoContainer.downloadStarted = true;


        yield return uwr.SendWebRequest();

        UnityEngine.Debug.Log(uwr.downloadProgress == 1.0f);
        while (uwr.downloadProgress != 1.0f)
        {
            yield return new WaitForSeconds(1);
            UnityEngine.Debug.Log(uwr.downloadProgress);
        }

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            UnityEngine.Debug.LogError(uwr.error);
        }

        else
        {
            UnityEngine.Debug.Log("File successfully downloaded and saved to " + path);

            afterFinsihDownloading(sliderImage, gnameText, gName, pathExtractTo);

        }

        PlayerPrefs.SetString(gName, "true");
        infoContainer.downloadStarted = false;


    }




    void afterFinsihDownloading(Image sliderImage, Text gnameText, string gName, string  gLocalPath)
    {
        //extracting file
        ZipFile.ExtractToDirectory(Path.Combine(Application.dataPath, gName + ".zip"), gLocalPath);
        hasDownloaded = true;
        sliderImage.color = new Color(0, 0, 0, 0);
        gnameText.text = gName;
        //gnameText.fontSize = 10;
    }
}
