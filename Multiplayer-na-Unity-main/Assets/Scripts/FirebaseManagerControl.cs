using Firebase;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FirebaseManagerControl : MonoBehaviour
{
    DatabaseReference mDatabaseRef;
    public List<Questions> questions;

    string theme;

    public delegate void GetQuestionsEvent(string theme);
    public static GetQuestionsEvent GetQuestionsHandle;

    private void OnEnable()
    {
        GetQuestionsHandle += GetQuestions;
    }

    private void OnDisable()
    {
        GetQuestionsHandle -= GetQuestions;
    }

    void GetQuestions(string theme)
    {
        this.theme = theme;
        mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        StartCoroutine(ReadAwnser((List<Questions> awnser) =>
        {
            GameManager.SetQtQuestionsHandle?.Invoke(questions.Count);
            QuestionManager.SetQuestionsHandle?.Invoke(questions);
        }));
    }

    

    public IEnumerator ReadAwnser(Action<List<Questions>> onCallback)
    {
        var awnser = mDatabaseRef.Child(theme).GetValueAsync();
        yield return new WaitUntil(predicate: () => awnser.IsCompleted);

        if(awnser != null)
        {
            DataSnapshot snapshot = awnser.Result;
            questions = new List<Questions>();
            foreach (DataSnapshot itemSnapshot in snapshot.Children)
            {
                string[] Awnsers = { (string)itemSnapshot.Child("Awnsers").Child("0").Value, (string)itemSnapshot.Child("Awnsers").Child("1").Value , (string)itemSnapshot.Child("Awnsers").Child("2").Value , (string)itemSnapshot.Child("Awnsers").Child("3").Value };
                Questions question = new Questions((string)itemSnapshot.Child("Question").Value,Awnsers, (string)itemSnapshot.Child("CorrectAwnser").Value);
                this.questions.Add(question);
            }
            onCallback.Invoke(snapshot.Value as List<Questions>);
        }
    }
}