using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using UnityEngine.UI;
using System;

public class notesMaker : MonoBehaviour
{
    public class oneNote
    {
       public  string patientId;
        public string date;
       public string note;

    }

    [Header("ui")]
    public InputField notesField;
    public notePrefab notesPrefab;
    public Transform notesSpawner;

    [Header("inner")]
    public string ReadText;
    public string allNotesDataPath;
    public List<oneNote>  allNotes = new List<oneNote>();
    public List<oneNote> notesOfPatientWithId = new List<oneNote>();
    public string currentPatienId;

    JSONNode _AllNotesNode;

    private void Awake()
    {
        allNotesDataPath = Application.dataPath + "/allNotes.text";
        currentPatienId = PermanentData.getCurrentPatientId(Application.dataPath + "/" + PermanentData.CURRENT_PATIENT_AND_COUNTER_FILE_NAME);
    }

    private void OnEnable()
    {
        currentPatienId = PermanentData.getCurrentPatientId(Application.dataPath + "/" + PermanentData.CURRENT_PATIENT_AND_COUNTER_FILE_NAME);
        fetchPatientNotesData();
        fetchAllNotesWithId(currentPatienId);
    }
    public void fetchPatientNotesData()
    {
        Debug.Log("fetching patients notes");
        if (!File.Exists(allNotesDataPath))
            return;

        destroyAllNotes();
        allNotes.Clear();
        notesOfPatientWithId.Clear();

        ReadText = File.ReadAllText(allNotesDataPath);

        _AllNotesNode = JSON.Parse(ReadText);

        populateNotesList();
  
    }

    void destroyAllNotes()
    {
        
        for (int i = 0; i < notesSpawner.childCount; i++)
        {
            Destroy(notesSpawner.GetChild(i).gameObject);
        }
    }

    void populateNotesList()
    {
        int i = 0;
        
        while (_AllNotesNode[i] != null)
        {
            addToNotesList(_AllNotesNode[i]["patientId"], _AllNotesNode[i]["note"], _AllNotesNode[i]["date"]);
            i++;
        }
        
    }

    void addToNotesList(string paitentId, string note, string date)
    {
        oneNote newNote = new oneNote();
        newNote.patientId = paitentId;
        newNote.date = date;
        newNote.note = note; 
        
        allNotes.Add(newNote);
       
    }

    void fetchAllNotesWithId(string id)
    {
       
        for (int i = 0; i < allNotes.Count; i++)
        {
            Debug.Log(allNotes[i].patientId + "  " + id);
            if (allNotes[i].patientId == id) 
            {
                
                notesOfPatientWithId.Add(allNotes[i]);
            }
        }
        

        
        showAllTheNotesOfCurrentPatient();
    }

    void showAllTheNotesOfCurrentPatient()
    {
        for (int i = 0; i < notesOfPatientWithId.Count; i++)
        {
            genrateNoteInScrollView(notesOfPatientWithId[i].note, notesOfPatientWithId[i].date);
        }
    }

    void genrateNoteInScrollView(string note, string date)
    {
        notePrefab newNoteObject = Instantiate(notesPrefab, notesSpawner);
        newNoteObject.setText(note, date);
    }


    //STROING THE NOTE
    public void storenewNote()
    {
        oneNote newNote = new oneNote();

        newNote.patientId = currentPatienId;
        newNote.note = notesField.text;
        newNote.date = DateTime.Now.ToString();

        genrateNoteInScrollView(newNote.note, newNote.date);
        allNotes.Add(newNote);

        
        wrtieAllNotesInTextFile(makeJasonStringOfAllNotes());
    }

    public string makeJasonStringOfAllNotes()
    {
        string finalJson = "[";
        for (int i = 0; i < allNotes.Count; i++)
        {
            string newJson = JsonUtility.ToJson(allNotes[i]);
            finalJson = finalJson + ", " + newJson;
        }
        finalJson = finalJson + "]";

        return finalJson;
    }

    public void wrtieAllNotesInTextFile(string allText)
    {
        
        
        File.WriteAllText(allNotesDataPath, allText);
    }
}
