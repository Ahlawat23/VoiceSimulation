using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeManager : MonoBehaviour
{
    public tabPanelmanager _tabManager;
    public GameObject tabPanel;
    public void fetchHASI()
    {
        tabPanel.SetActive(true);
        _tabManager.fetchGamesOfCotegrie("HASI");
    }

    public void fetchCFANV()
    {
        tabPanel.SetActive(true);
        _tabManager.fetchGamesOfCotegrie("CFANV");
    }

    public void fetchPCT()
    {
        tabPanel.SetActive(true);
        _tabManager.fetchGamesOfCotegrie("PCT");
    }

    public void fetchLL()
    {
        tabPanel.SetActive(true);
        _tabManager.fetchGamesOfCotegrie("LL");
    }
}
