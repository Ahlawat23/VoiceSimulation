using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using UnityEngine.Networking;
public class downloadableGameInfoContainer : MonoBehaviour
{
    [Header("strings")]
    public string gId;
    public string gName;
    public string gLink;
    public string gCotegrie;
    public string gdescription;
    public string gType;
    public string gLocalPath;
    public string gLogoLink;

    [Header("text elements")]
    public Text gnameText;
    public Image gLogo;
    public Text debugText;

    [Header("downloading vars")]
    public bool hasDownloaded = false;
    public bool downloadStarted = false;
    UnityWebRequest uwr;

    [Header("dowload Progree")]
    public Image sliderImage;
   
    private void Update()
    {
        showDownloadProgress(uwr);
    }

    public void setInfo()
    {
        uwr = new UnityWebRequest(gLink, UnityWebRequest.kHttpVerbGET);
        gnameText.text = gName;
        setGameLocalPath();
        menuManager.instance._gameDownloader.dowoloadLogo(gLogoLink, gLogo);
    }
    
    public void setGameLocalPath()
    {
        gLocalPath = Application.dataPath;
        string newGameData = null;
        //remove till this string
        int pos = gLocalPath.IndexOf("/Stack_Data");
        if (pos >= 0)
        {
            // String after founder  
            newGameData = gLocalPath.Remove(pos);


        }
        gLocalPath = newGameData;
        gLocalPath += PermanentData.ALL_SIMULATIONS_PATH + "/";

        if (!File.Exists(gLocalPath))
            Directory.CreateDirectory(gLocalPath);

    }


    public void buttonClick()
    {
      
        if (!isDownloaded())
            menuManager.instance._gameDownloader.DownloadGame(uwr, gName, gLocalPath, sliderImage, gnameText, this);
        else
            playGame();

    }

    bool isDownloaded()
    {
        if (File.Exists(gLocalPath + gName + "/" + PermanentData.SIMULATION_EXE_NAME))
            return true;

        return false;
    }

    void showDownloadProgress(UnityWebRequest uwr)
    {
        if (downloadStarted)
            sliderImage.fillAmount = uwr.downloadProgress;
    }

    void playGame()
    {
        runTheExe();
    }

    public void runTheExe()
    {
        Process p = new Process();
        p.StartInfo.UseShellExecute = true;
        p.StartInfo.FileName = gLocalPath + gName + "/" + PermanentData.SIMULATION_EXE_NAME;
        p.Start();
    }
   
}

