using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
public class MenuManager : MonoBehaviour
{
    DatabaseReference mDatabaseRef;
    public List<string> questions;

    [SerializeField]
    private TMP_Dropdown dropDown;
    public void Start()
    {
        mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        StartCoroutine(ReadAwnser((List<Questions> awnser) =>
        {
            AddtoDropDown();
        }));

    }

    void AddtoDropDown()
    {
        dropDown.options.Clear();
        foreach (string option in questions)
        {
            dropDown.options.Add(new TMP_Dropdown.OptionData(option));
        }
        dropDown.value = 0;
        dropDown.RefreshShownValue();
    }

    public IEnumerator ReadAwnser(Action<List<Questions>> onCallback)
    {
        var awnser = mDatabaseRef.GetValueAsync();
        yield return new WaitUntil(predicate: () => awnser.IsCompleted);

        if (awnser != null)
        {
            DataSnapshot snapshot = awnser.Result;
            questions = new List<string>();
            foreach (DataSnapshot itemSnapshot in snapshot.Children)
            {
                this.questions.Add(itemSnapshot.Key);
            }
            onCallback.Invoke(snapshot.Value as List<Questions>);
        }
    }

    public void CloseAplication()
    {
        Application.Quit();
    }
}
