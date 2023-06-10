using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
public class CreatingQuestionManager : MonoBehaviour
{
    DatabaseReference mDatabaseRef;

    [SerializeField]
    private TMP_InputField theme;
    [SerializeField]
    private TMP_InputField question;
    [SerializeField]
    private TMP_InputField awnser1;
    [SerializeField]
    private TMP_InputField awnser2;
    [SerializeField]
    private TMP_InputField awnser3;
    [SerializeField]
    private TMP_InputField awnser4;
    [SerializeField]
    private Toggle[] toogles;
    [SerializeField]
    private TMP_Dropdown dropDown;


    public List<Questions> questions;

    public void Start()
    {
        mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        StartCoroutine(ReadAwnser((List<Questions> awnser) =>
        {
            AddtoDropDown();
        }));
        
    }

    public void WriteData()
    {
        for (int i = 0; i < questions.Count; i++)
        {
            string json = JsonUtility.ToJson(questions[i]);
            mDatabaseRef.Child(theme.text).Child(i.ToString()).SetRawJsonValueAsync(json);
        }
    }

    public void AddQuestion()
    {
        foreach (var item in questions)
        {
            if (question.text == item.Question)
            {
                item.Awnsers[0] = awnser1.text;
                item.Awnsers[1] = awnser2.text;
                item.Awnsers[2] = awnser3.text;
                item.Awnsers[3] = awnser4.text;
                if (toogles[0].isOn)
                {
                    item.CorrectAwnser = awnser1.text;
                    AddtoDropDown();
                    return;
                }
                else
                {
                    if (toogles[1].isOn)
                    {
                        item.CorrectAwnser = awnser2.text;
                        AddtoDropDown();
                        return;
                    }
                    else
                    {
                        if (toogles[2].isOn)
                        {
                            item.CorrectAwnser = awnser3.text;
                            AddtoDropDown();
                            return;
                        }
                        else
                        {
                            if (toogles[3].isOn)
                            {
                                item.CorrectAwnser = awnser4.text;
                                AddtoDropDown();
                                return;
                            }
                        }
                    }
                }
                return;
            }
            
        }
        string[] awnsers = new string[4];
        awnsers[0] = awnser1.text;
        awnsers[1] = awnser2.text;
        awnsers[2] = awnser3.text;
        awnsers[3] = awnser4.text;

        if (toogles[0].isOn)
        {
            questions.Add(new Questions(question.text, awnsers, awnser1.text));
            AddtoDropDown();
            return;
        }
        else
        {
            if (toogles[1].isOn)
            {
                questions.Add(new Questions(question.text, awnsers, awnser2.text));
                AddtoDropDown();
                return;
            }
            else
            {
                if (toogles[2].isOn)
                {
                    questions.Add(new Questions(question.text, awnsers, awnser3.text));
                    AddtoDropDown();
                    return;
                }
                else
                {
                    if (toogles[3].isOn)
                    {
                        questions.Add(new Questions(question.text, awnsers, awnser4.text));
                        AddtoDropDown();
                        return;
                    }
                }
            }
        }
    }

    void AddtoDropDown()
    {
        dropDown.options.Clear();
        foreach (Questions option in questions)
        {
            dropDown.options.Add(new TMP_Dropdown.OptionData(option.Question));
        }
        dropDown.value = 0;
        dropDown.RefreshShownValue();
    }

    public IEnumerator ReadAwnser(Action<List<Questions>> onCallback)
    {
        var awnser = mDatabaseRef.Child(theme.text).GetValueAsync();
        yield return new WaitUntil(predicate: () => awnser.IsCompleted);

        if (awnser != null)
        {
            DataSnapshot snapshot = awnser.Result;
            questions = new List<Questions>();
            foreach (DataSnapshot itemSnapshot in snapshot.Children)
            {
                string[] Awnsers = { (string)itemSnapshot.Child("Awnsers").Child("0").Value, (string)itemSnapshot.Child("Awnsers").Child("1").Value, (string)itemSnapshot.Child("Awnsers").Child("2").Value, (string)itemSnapshot.Child("Awnsers").Child("3").Value };
                Questions question = new Questions((string)itemSnapshot.Child("Question").Value, Awnsers, (string)itemSnapshot.Child("CorrectAwnser").Value);
                this.questions.Add(question);
            }
            onCallback.Invoke(snapshot.Value as List<Questions>);
        }
    }

    public void SelectQuestion()
    {
        question.text = questions[dropDown.value].Question;
        awnser1.text = questions[dropDown.value].Awnsers[0];
        awnser2.text = questions[dropDown.value].Awnsers[1];
        awnser3.text = questions[dropDown.value].Awnsers[2];
        awnser4.text = questions[dropDown.value].Awnsers[3];
        toogles[0].isOn = false;
        toogles[1].isOn = false;
        toogles[2].isOn = false;
        toogles[3].isOn = false;
        if (awnser1.text == questions[dropDown.value].CorrectAwnser)
        {
            toogles[0].isOn = true;
            return;
        }
        else
        {
            if (awnser2.text == questions[dropDown.value].CorrectAwnser)
            {
                toogles[1].isOn = true;
                return;
            }
            else
            {
                if (awnser3.text == questions[dropDown.value].CorrectAwnser)
                {
                    toogles[2].isOn = true;
                    return;
                }
                else
                {
                    if (awnser4.text == questions[dropDown.value].CorrectAwnser)
                    {
                        toogles[3].isOn = true;
                        return;
                    }
                }
            }
        }
    }
}
