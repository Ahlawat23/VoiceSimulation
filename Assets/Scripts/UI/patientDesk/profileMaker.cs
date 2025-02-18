using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class profileMaker : MonoBehaviour
{
   

    [Header("UI elments")]
    public Text nameText;
    public Text MobileText;
    public Text EmailText;
    public Text nativeLanguageText;
    public Text idText;
    public Text ageText;
    public Text DisablityText;
    public Text AddressText;


    [Header("other scripts")]
    public patientDataManager _patienDataManager;

    private void OnEnable()
    {
        fetchpatientInfo();
    }
    void fetchpatientInfo()
    {
        _patienDataManager.fetchPatientData();
        patientDataManager.patient newPatient = _patienDataManager.searchPatientData(PermanentData.getCurrentPatientId(Application.dataPath + "/" + PermanentData.CURRENT_PATIENT_AND_COUNTER_FILE_NAME));

        
        nameText.text = newPatient.patientFullName;
        MobileText.text = newPatient.phone;
        EmailText.text = newPatient.email;
        nativeLanguageText.text = newPatient.nativeLanaguage;
        idText.text = newPatient.id;
        ageText.text = newPatient.age;
        DisablityText.text = newPatient.disabalityType;
        AddressText.text = newPatient.address;
}
}
