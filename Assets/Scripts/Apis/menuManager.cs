using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public enum startMenuState
{
     DoctorLogin,
     Loading,
     OpeningMenu,
     PatientDesk,
     newPatientRegistration,
     searchPatient, 
     models, 
     tabPanel,
     roster, 
     notes,
     profile

}



public class menuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject canvas;
    public GameObject doctorLoginPanel;
    public GameObject loadingPanel;
    public GameObject openingMenuPanel;
    public GameObject newPatientRegisterPanel;
    public GameObject searchPatientPanel;
    public GameObject patientDeskPanel;
    public GameObject modelsPanel;
    public GameObject tabPanel;
    public GameObject rosterPanel;
    public GameObject notesPanel;
    public GameObject profilePanel;
   

    [Header("ErrorBox")]
    public errorBox _errorBox;

    [Header("otherScripts")]
    public patientDataManager _patientDataManager;
    public MenuApiManager _menuApiManager;
    public welcomeChat _welChat;
    public downloadGame _gameDownloader;
    public laodingManager _loadingManager;

    public static menuManager instance;
    public static startMenuState state;
    public static event Action<startMenuState> onMenuStateChanged;
  

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1;
    }

    public void updatestartMenuState(startMenuState newState)
    {
        state = newState;

        switch (state)
        {
            case startMenuState.DoctorLogin:
                handleDoctorLoginState();
                break;
            case startMenuState.Loading:
                handleLoadingState();
                break;
            case startMenuState.OpeningMenu:
                handleOpnignMenuState();
                break;
            case startMenuState.newPatientRegistration:
                handleNewPatientRegistration();
                break;
            case startMenuState.searchPatient:
                handleSearchPatienState();
                break;
            case startMenuState.PatientDesk:
                handlePatientDeskState();
                break;
            case startMenuState.models:
                handleModelsState();
                break;
            case startMenuState.tabPanel:
                handleTabPanelState();
                break;
            case startMenuState.roster:
                handleRosterState();
                break;
            case startMenuState.notes:
                handleNotesState();
                break;
            case startMenuState.profile:
                handleProfileState();
                break;
        }
        onMenuStateChanged?.Invoke(state);
    }

    private void handleDoctorLoginState()
    {
        doctorLoginPanel.SetActive(true);
        loadingPanel.SetActive(false);
        openingMenuPanel.SetActive(false);
        newPatientRegisterPanel.SetActive(false);
        searchPatientPanel.SetActive(false);
        patientDeskPanel.SetActive(false );
        modelsPanel.SetActive(false);
        tabPanel.SetActive(false);
        rosterPanel.SetActive(false);
        notesPanel.SetActive(false);
        profilePanel.SetActive(false);

    }

    private void handleLoadingState()
    {
        doctorLoginPanel.SetActive(false);
        loadingPanel.SetActive(true);
        openingMenuPanel.SetActive(false);
        newPatientRegisterPanel.SetActive(false);
        searchPatientPanel.SetActive(false);
        patientDeskPanel.SetActive(false);
        modelsPanel.SetActive(false);
        rosterPanel.SetActive(false);
        notesPanel.SetActive(false);
        profilePanel.SetActive(false);

        _loadingManager.LoadingInit();

    }

    private void handleOpnignMenuState()
    {
        doctorLoginPanel.SetActive(false);
        loadingPanel.SetActive(false);
        openingMenuPanel.SetActive(true);
        newPatientRegisterPanel.SetActive(false);
        searchPatientPanel.SetActive(false);
        patientDeskPanel.SetActive(false);
        modelsPanel.SetActive(false);
        tabPanel.SetActive(false);
        rosterPanel.SetActive(false);
        notesPanel.SetActive(false);
        profilePanel.SetActive(false);

        //_patientDataManager.UpdatePaitentData();

        menuManager.instance._welChat.showChat("Hello, Welcome " + _menuApiManager.personRole + _menuApiManager.loginFullName + ", choose what you want to do..");


    }

    private void handleNewPatientRegistration()
    {
        doctorLoginPanel.SetActive(false);
        loadingPanel.SetActive(false);
        openingMenuPanel.SetActive(true);
        newPatientRegisterPanel.SetActive(true);
        searchPatientPanel.SetActive(searchPatientPanel.activeSelf);
        patientDeskPanel.SetActive(false);
        modelsPanel.SetActive(false);
        tabPanel.SetActive(false);
        rosterPanel.SetActive(false);
        notesPanel.SetActive(false);
        profilePanel.SetActive(false);

        _patientDataManager.fetchPatientData();
    }

    private void handleSearchPatienState()
    {
        doctorLoginPanel.SetActive(false);
        loadingPanel.SetActive(false);
        openingMenuPanel.SetActive(true);
        newPatientRegisterPanel.SetActive(false);
        searchPatientPanel.SetActive(true);
        patientDeskPanel.SetActive(false);
        modelsPanel.SetActive(false);
        tabPanel.SetActive(false);
        rosterPanel.SetActive(false);
        notesPanel.SetActive(false);
        profilePanel.SetActive(false);

        _patientDataManager.fetchPatientData();
        _patientDataManager.clearSearchView();
        _patientDataManager.SearchField.text = "";
    }

    private void handlePatientDeskState()
    {
        doctorLoginPanel.SetActive(false);
        loadingPanel.SetActive(false);
        openingMenuPanel.SetActive(false);
        newPatientRegisterPanel.SetActive(false);
        searchPatientPanel.SetActive(false);
        patientDeskPanel.SetActive(true);
        modelsPanel.SetActive(false);
        tabPanel.SetActive(false);
        rosterPanel.SetActive(false);
        notesPanel.SetActive(false);
        profilePanel.SetActive(false);


    }

    private void handleModelsState()
    {
        doctorLoginPanel.SetActive(false);
        loadingPanel.SetActive(false);
        openingMenuPanel.SetActive(false);
        newPatientRegisterPanel.SetActive(false);
        searchPatientPanel.SetActive(false);
        patientDeskPanel.SetActive(false);
        modelsPanel.SetActive(true);
        tabPanel.SetActive(false);
        rosterPanel.SetActive(false);
        notesPanel.SetActive(false);
        profilePanel.SetActive(false);
    }

    private void handleTabPanelState()
    {
        doctorLoginPanel.SetActive(false);
        loadingPanel.SetActive(false);
        openingMenuPanel.SetActive(false);
        newPatientRegisterPanel.SetActive(false);
        searchPatientPanel.SetActive(false);
        patientDeskPanel.SetActive(false);
        modelsPanel.SetActive(false);
        tabPanel.SetActive(true);
        rosterPanel.SetActive(false);
        notesPanel.SetActive(false);
        profilePanel.SetActive(false);
    }

    private void handleRosterState()
    {
        doctorLoginPanel.SetActive(false);
        loadingPanel.SetActive(false);
        openingMenuPanel.SetActive(false);
        newPatientRegisterPanel.SetActive(false);
        searchPatientPanel.SetActive(false);
        patientDeskPanel.SetActive(false);
        modelsPanel.SetActive(false);
        tabPanel.SetActive(false);
        rosterPanel.SetActive(true);
        notesPanel.SetActive(false);
        profilePanel.SetActive(false);

        rosterPanel.transform.parent.gameObject.SetActive(true);
        Debug.Log("called");
    }

    private void handleNotesState()
    {
        doctorLoginPanel.SetActive(false);
        loadingPanel.SetActive(false);
        openingMenuPanel.SetActive(false);
        newPatientRegisterPanel.SetActive(false);
        searchPatientPanel.SetActive(false);
        patientDeskPanel.SetActive(false);
        modelsPanel.SetActive(false);
        tabPanel.SetActive(false);
        rosterPanel.SetActive(false);
        notesPanel.SetActive(true);
        profilePanel.SetActive(false);
    }

    private void handleProfileState()
    {
        doctorLoginPanel.SetActive(false);
        loadingPanel.SetActive(false);
        openingMenuPanel.SetActive(false);
        newPatientRegisterPanel.SetActive(false);
        searchPatientPanel.SetActive(false);
        patientDeskPanel.SetActive(false);
        modelsPanel.SetActive(false);
        tabPanel.SetActive(false);
        rosterPanel.SetActive(false);
        notesPanel.SetActive(false);
        profilePanel.SetActive(true);
    }
  


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(state == startMenuState.newPatientRegistration)
            {
                updatestartMenuState(startMenuState.OpeningMenu);
            }
        }
    }

    #region helperFuntionToActivateStates

    public void LoginStateOnClick()
    {
        menuManager.instance.updatestartMenuState(startMenuState.DoctorLogin);
    }

    public void OpeningMenuOnClick()
    {
        menuManager.instance.updatestartMenuState(startMenuState.OpeningMenu);
        _errorBox.gameObject.SetActive(false);
    }

    public void NewPatientRegistrationOnClick()
    {
        menuManager.instance.updatestartMenuState(startMenuState.newPatientRegistration);
    }

    public void SearchPatientOnClick()
    {
        menuManager.instance.updatestartMenuState(startMenuState.searchPatient);
    }

    public void PatientDeskOnClick()
    {
        menuManager.instance.updatestartMenuState(startMenuState.PatientDesk);
    }

    public void ModelsOnClick()
    {
        menuManager.instance.updatestartMenuState(startMenuState.models);
    }

    public void RosterOnClick()
    {
        menuManager.instance.updatestartMenuState(startMenuState.roster);
    }

    public void NotesOnClick()
    {
        menuManager.instance.updatestartMenuState(startMenuState.notes);
    }

    public void ProfileOnClick()
    {
        menuManager.instance.updatestartMenuState(startMenuState.profile);
    }
    
    public void ExitOnClick()
    {
        Application.Quit();
    }
    #endregion
}
