using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;
using System;
using System.Globalization;


public class SearchPatientGameGraphLogger : MonoBehaviour
{
    public class gameGraphdata
    {
        public string id;
        public string overralRating;
        public string gamebase;
        public string date;
    }

    public string gameDataPath;
    public string ReadText;

    public List<gameGraphdata> gameGraphDataList = new List<gameGraphdata>();
    public List<gameGraphdata> oneIdAllGameGraphDatas = new List<gameGraphdata>();
  

    public void startGraph()
    {
        gameDataPath = Application.dataPath + PermanentData.RECENT_GAME_LOG_FILE_NAME;
        readDataFromAllGameDataTextFile();
    }

    public void readDataFromAllGameDataTextFile()
    {
        oneIdAllGameGraphDatas.Clear();
        gameGraphDataList.Clear();
        Debug.Log("reading data from text file..");
        ReadText = File.ReadAllText(gameDataPath);

        JSONNode node = JSON.Parse(ReadText);

        int i = 0;
        while (node[i] != null)
        {
            //Debug.Log(node[i]);
            addToList(
                node[i]["patientId"],
                node[i]["gamebase"],
                node[i]["overralrating"],
                node[i]["date"]
                );

           
            i++;
        }
    }

    void addToList(string id, string gameBase, string overralRating, string date)
    {
        gameGraphdata newData = new gameGraphdata();
        newData.id = id;
        newData.gamebase = gameBase;
        //Debug.Log("overral rating : " + overralRating);
        newData.overralRating = overralRating;

        
        DateTime myDate = new DateTime();
        try
        {
             myDate = DateTime.ParseExact(date, "dd-MM-yyyy HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        
        string s = myDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
        newData.date = s;
        


        gameGraphDataList.Add(newData);
    }

    public void findDataWithID(string id)
    {
        Debug.Log(id);
        oneIdAllGameGraphDatas.Clear();
        for (int i = 0; i < gameGraphDataList.Count; i++)
            if (gameGraphDataList[i].id == id)
                oneIdAllGameGraphDatas.Add(gameGraphDataList[i]);


        Debug.Log(oneIdAllGameGraphDatas.Count);
    }


    public List<int> fetchFrequencyGradeWithId()
    {
        List<int> frequencyGrades = new List<int>();
        List<string> dates = new List<string>();
        for (int i = 0; i < oneIdAllGameGraphDatas.Count; i++)
        {
            if (oneIdAllGameGraphDatas[i].gamebase == "frequency" || oneIdAllGameGraphDatas[i].gamebase == "Frequency")
            {
                

                if (oneIdAllGameGraphDatas[i].overralRating != "" )
                {
                    dates.Add(oneIdAllGameGraphDatas[i].date);

                    int rating = (int)(float.Parse(oneIdAllGameGraphDatas[i].overralRating));

                    if(rating > 0 && rating < 10)
                    {
                        
                        frequencyGrades.Add(rating);
                    }
                    else
                    {
                        if (rating >= 10)
                        {
                            frequencyGrades.Add(9);
                        }
                        frequencyGrades.Add(0);
                    }

                }

                else
                {
                    dates.Add(oneIdAllGameGraphDatas[i].date);
                    frequencyGrades.Add(0);
                }
            }

        }
        //Debug.Log(frequencyGrades.Count);

        return averageOutByDate(dates, frequencyGrades);
    }

    public List<int> fetchLoudnessGradeWithId()
    {
        List<int> LoudnessGrades = new List<int>();
        List<string> dates = new List<string>();
        for (int i = 0; i < oneIdAllGameGraphDatas.Count; i++)
        {
            if (oneIdAllGameGraphDatas[i].gamebase == "loudness" || oneIdAllGameGraphDatas[i].gamebase == "Loudness")
            {
                if (oneIdAllGameGraphDatas[i].overralRating != "")
                {
                    dates.Add(oneIdAllGameGraphDatas[i].date);
                    int rating = (int)(float.Parse(oneIdAllGameGraphDatas[i].overralRating));

                    if (rating > 0 && rating < 10)
                    {
                        LoudnessGrades.Add(rating);
                    }
                    else
                    {
                        if (rating >= 10)
                        {
                            LoudnessGrades.Add(9);
                        }
                        LoudnessGrades.Add(0);
                    }
                }

                else
                {
                    dates.Add(oneIdAllGameGraphDatas[i].date);
                    LoudnessGrades.Add(0);
                }
            }
        }
       //Debug.Log(LoudnessGrades.Count);
          
        return averageOutByDate(dates, LoudnessGrades);
    }

    public List<int> fetchRecogtionGradeWithId()
    {
        List<int> recognitionGrades = new List<int>();
        List<string> dates = new List<string>();
        for (int i = 0; i < oneIdAllGameGraphDatas.Count; i++)
        {
            if (oneIdAllGameGraphDatas[i].gamebase == "recognition" || oneIdAllGameGraphDatas[i].gamebase == "Recognition")
            {
                if (oneIdAllGameGraphDatas[i].overralRating != "")
                {
                    dates.Add(oneIdAllGameGraphDatas[i].date);
                    int rating = (int)(float.Parse(oneIdAllGameGraphDatas[i].overralRating));
                    
                    if (rating > 0 && rating < 10)
                    {
                        recognitionGrades.Add(rating);
                    }
                    else
                    {
                        if (rating >= 10)
                        {
                            recognitionGrades.Add(9);
                        }
                        recognitionGrades.Add(0);
                    }
                }

                else
                {
                    dates.Add(oneIdAllGameGraphDatas[i].date);
                    recognitionGrades.Add(0);
                }
            }

        }
        //Debug.Log(recognitionGrades.Count);
        return averageOutByDate(dates, recognitionGrades);
    }


    List<int> averageOutByDate(List<string> dates, List<int> grades)
    {
        List<int> finalOverralRating = new List<int>();
       
        int avgGradeForDay = 0;
        int numOfi = 0;

        if(grades.Count < 2)
        {
            return grades;
        }


        for (int i = 1; i < dates.Count; i++)
        {
            
           
            if (dates[i-1] == dates[i])
            {

                avgGradeForDay += grades[i];
                numOfi++;
               
            }
            else
            {
                if(numOfi!= 0)
                avgGradeForDay = avgGradeForDay / numOfi;
                if (avgGradeForDay >= 10)
                {
                    finalOverralRating.Add(9);
                }
                else if (avgGradeForDay <= 0)
                {
                    finalOverralRating.Add(0);
                }
                else
                {
                    finalOverralRating.Add(avgGradeForDay);

                }

                
               

                avgGradeForDay = grades[i];
                numOfi = 0;
            }
        }
        
       
        
        
        if (numOfi != 0)
        {

            avgGradeForDay = avgGradeForDay / numOfi;
            finalOverralRating.Add(avgGradeForDay);
        }

        

      
        return finalOverralRating;

    }

    List<int> averageOutByMonth(List<string> dates, List<int> grades)
    {
        List<int> finalOutByMonth = new List<int>();
        List<string> months = new List<string>();

        //fetch the months
        
        for (int i = 0; i < dates.Count; i++)
        {
            months.Add(dates[i].Substring(3, 2));
        }

        int avgGradeForMonth = 0;
        int numOfi = 0;
        for (int i = 1; i < dates.Count; i++)
        {
           
            if (months[i - 1] == months[i])
            {

                avgGradeForMonth += grades[i];
                numOfi++;
            }
            else
            {
                if (numOfi != 0)
                    avgGradeForMonth = avgGradeForMonth / numOfi;
                finalOutByMonth.Add(avgGradeForMonth);
                avgGradeForMonth = grades[i];
                numOfi = 0;
            }
        }


        return finalOutByMonth;
    }

}
