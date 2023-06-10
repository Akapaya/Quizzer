using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class GameManager : NetworkBehaviour
{
	public delegate void StartGameEvent();
	public static StartGameEvent StartGameHandle;

	[SerializeField] GameObject panel;
	[SerializeField] TMP_Text text;
	[SerializeField] GameObject questionPanel;
	[SerializeField] GameObject scoreNamePanel;
	[SerializeField] GameObject UIPanel;
	public List<PlayerProfile> players;

	[SerializeField]
	private TMP_Dropdown dropDown;

	[SerializeField] private int qtQuestions;

	public delegate void SetQtQuestionsEvent(int i);
	public static SetQtQuestionsEvent SetQtQuestionsHandle;

	private void OnEnable()
    {
		StartGameHandle += ActivePanel;
		SetQtQuestionsHandle += SetQtQuestions;
	}

    private void OnDisable()
    {
		StartGameHandle -= ActivePanel;
		SetQtQuestionsHandle -= SetQtQuestions;
	}

	void SetQtQuestions(int i)
    {
		qtQuestions = i;

	}


	private void ActivePanel()
    {
		if (OwnerClientId != 0)
		{
			return;
		}
		panel.SetActive(true);
		InvokeRepeating("UpdateTheme", 5f,5f);
	}

	private void UpdateTheme()
    {
		SetThemeClientRpc(dropDown.options[dropDown.value].text);
	}
    public void Iniciate()
    {

		panel.SetActive(false);
		CancelInvoke("UpdateTheme");
		Invoke("SetQuestionServerRpc", 10f);
		
	}

	[ServerRpc]
	public void SetQuestionServerRpc()
	{
		int i = Random.Range(0, qtQuestions);
		PlayerProfile.SetQuestionHandle?.Invoke(i);
		SetScoresClientRpc(text.text);
		qtQuestions--;
		Invoke("FinishQuestion", 10f);
	}

	public void FinishQuestion()
    {
		ActivePanelScoreServerRpc();
		if (qtQuestions > 0)
		{
			Invoke("SetQuestionServerRpc", 10f);
		}
		else
        {
			ActiveExitButtonServerRpc();

		}
	}

	[ServerRpc]
	public void ActivePanelScoreServerRpc()
	{
		text.text = "";
		foreach (var item in GameObject.FindGameObjectsWithTag("Player"))
		{
			text.text += item.GetComponent<PlayerName>().playerName + ": " + item.GetComponent<PlayerProfile>().score + "\n";
		}
		SetScoresClientRpc(text.text);
		ActivePanelScoreClientRpc();
	}

	[ServerRpc]
	public void ActiveExitButtonServerRpc()
	{
		UIPanel.SetActive(true);
		ActiveExitButtonClientRpc();
	}

	[ClientRpc]
	public void SetScoresClientRpc(string scores)
	{
		text.text = scores;
	}

	[ClientRpc]
	public void ActiveExitButtonClientRpc()
	{
		UIPanel.SetActive(true);
	}

	[ClientRpc]
	public void ActivePanelScoreClientRpc()
	{
		scoreNamePanel.SetActive(true);
		questionPanel.SetActive(false);
	}

	[ClientRpc]
	public void SetThemeClientRpc(string theme)
	{
		FirebaseManagerControl.GetQuestionsHandle?.Invoke(theme);
	}
}
