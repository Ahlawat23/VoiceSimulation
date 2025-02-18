using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using SimpleJSON;



public class tabPanelmanager : MonoBehaviour
{
    [Header("Prefabs and transoform")]
    public GameObject dgPrefab;
    public Transform phoenticsSpawnPoint;
    public Transform syllableSpawnPoint;
    public Transform wordSpawnPoint;
    public List<downloadableGameInfoContainer> allButtonList = new List<downloadableGameInfoContainer>();

    private void OnDisable()
    {
        destroyAllButtons();
    }

    public void fetchGamesOfCotegrie(string cotegrie)
    {
        StartCoroutine(getallGameLinks_coroutine(cotegrie));   
    }

    IEnumerator getallGameLinks_coroutine(string cotegrie)
    {
        //cotegrie can be HASI, PCT, CFANV, LL
        Debug.Log("getting all the game links");
        
        string uri = "http://vsadmin-env.eba-7n23mnc3.ap-south-1.elasticbeanstalk.com/gamelist?gamecategories=" + cotegrie;
        Debug.Log(uri);

        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {

                Debug.Log(request.error);
                Debug.Log(request.downloadHandler.text);

            }
            else
            {

                Debug.Log(request.downloadHandler.text);
                //status":true," +
                //    ""msg":"game details"," +
                //    ""data":[{" +
                //    ""_id":"6377b97748b6fb9ceab72f68"," +
                //    ""gamecategories":"HASI"," +
                //    ""gamename":"level1Duck"," +
                //    ""gamedescription":"new game for student"," +
                //    ""gamefile":"https://doc-games.s3.amazonaws.com/gameslist/level1Duck.zip"}

                 JSONNode node = JSON.Parse(request.downloadHandler.text);
                //Debug.Log(node["data"][0]["_id"]);

                int i = 0;
                while (node["data"][i] != null)
                {
                    createGameButton(node["data"][i]["gametype"], 
                        node["data"][i]["_id"], 
                        node["data"][i]["gamename"],
                        node["data"][i]["gamefile"], 
                        node["data"][i]["gamedescription"],
                        node["data"][i]["gameimage"]);
                    i++;
                }
            }
        }
    }

   

    void createGameButton(string type, string gid, string gname, string glink, string gDescription, string gLogoLink )
    {
        downloadableGameInfoContainer container = Instantiate(dgPrefab, spawnAcordingToType(type)).GetComponent<downloadableGameInfoContainer>();
        container.gId = gid;
        container.gName = gname;
        container.gLink = glink;
        container.gdescription = gDescription;
        container.gLogoLink = gLogoLink;
        

        container.setInfo();
        container.gameObject.SetActive(false);
        container.gameObject.SetActive(true);
        allButtonList.Add(container);
    }

    void destroyAllButtons()
    {
        for (int i = 0; i < allButtonList.Count; i++)
        {
            Destroy(allButtonList[i].gameObject);
        }
        allButtonList.Clear();
    }

    Transform spawnAcordingToType(string type)
    {
        switch (type)
        {
            case "phoentics":
                return phoenticsSpawnPoint;
            case "syllables":
                return syllableSpawnPoint;
            case "words":
                return wordSpawnPoint;
            default:
                return phoenticsSpawnPoint;
        }

    }
}
