using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject scorePanel;
    [SerializeField] private GameObject questionPanel;

    public delegate void QuestionsPanelEvent(bool status);
    public static QuestionsPanelEvent QuestionsPanelHandle;

    public delegate void ScorePanelEvent(bool status);
    public static ScorePanelEvent ScorePanelHandle;

    private void OnEnable()
    {
        QuestionsPanelHandle += QuestionPanelStatus;
        ScorePanelHandle += ScorePanelStatus;
    }

    private void OnDisable()
    {
        QuestionsPanelHandle -= QuestionPanelStatus;
        ScorePanelHandle -= ScorePanelStatus;
    }

    public void TurnScorePanelStatus()
    {
        scorePanel.SetActive(!scorePanel.activeInHierarchy);
    }

    public void QuestionPanelStatus(bool status)
    {
        questionPanel.SetActive(status);
    }

    public void ScorePanelStatus(bool status)
    {
        scorePanel.SetActive(status);
    }

    public void ReloadMainScene()
    {
        SceneManager.LoadScene(0);
    }
}
