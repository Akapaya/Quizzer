using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;


public class QuestionManager : MonoBehaviour
{
    //
    public int indexQuestions;

    [Header("Huds e portas")]
    public TMP_Text Quest;
    public TMP_Text [] Respostas;
    public List<Questions> questions;
    [SerializeField] private GameObject questioPanel;
    [SerializeField] private GameObject verificadoPerguntas;


    public delegate void StartGameEvent(int i);
    public static StartGameEvent StartGameHandle;

    public delegate void SetQuestionsEvent(List<Questions> questions);
    public static SetQuestionsEvent SetQuestionsHandle;

    private void Start()
    {
        //MudarQuestao();
    }

    private void OnEnable()
    {
        StartGameHandle += MudarQuestao;
        SetQuestionsHandle += SetQuestions;
    }

    private void OnDisable()
    {
        StartGameHandle -= MudarQuestao;
        SetQuestionsHandle += SetQuestions;
    }

    public void SetQuestions(List<Questions> questions)
    {
        this.questions = questions;
    }

    public void MudarQuestao(int index)
    {
        indexQuestions = index;
        questioPanel.SetActive(true);
        Quest.text = questions[index].Question;
        Respostas[0].text = questions[index].Awnsers[0];
        Respostas[1].text = questions[index].Awnsers[1];
        Respostas[2].text = questions[index].Awnsers[2];
        Respostas[3].text = questions[index].Awnsers[3];
        
    }
    public void VerificarRespostas()
    {

        Invoke("MudarQuestao", 7f);
    }

    private void Update()
    {
        if(questions.Count>0)
        {
            verificadoPerguntas.SetActive(true);
        }
    }

    public void SelectAwnser(TMP_Text i)
    {
        UIManager.QuestionsPanelHandle?.Invoke(false);
        if (i.text.Equals(questions[indexQuestions].CorrectAwnser))
            PlayerProfile.SetPlayerAwnserHandle?.Invoke();
        questions.RemoveAt(indexQuestions);
    }
}