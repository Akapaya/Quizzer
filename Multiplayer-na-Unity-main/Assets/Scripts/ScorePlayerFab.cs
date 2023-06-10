using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePlayerFab : MonoBehaviour
{
    [SerializeField] TMP_Text name;
    [SerializeField] TMP_Text score;
    
    public void StartScorePlayerFab(string name, string score)
    {
        this.name.text = name;
        this.score.text = score;
    }
}
