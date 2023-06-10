using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerProfile : NetworkBehaviour
{
	public delegate void SetPlayerAwnserEvent();
	public static SetPlayerAwnserEvent SetPlayerAwnserHandle;

	public delegate void SetQuestionEvent(int i);
	public static SetQuestionEvent SetQuestionHandle;

	public int score;


	private void OnEnable()
	{
		SetPlayerAwnserHandle += SendAwnser;
		SetQuestionHandle += SetQuestionClientRpc;
	}

	private void OnDisable()
	{
		SetPlayerAwnserHandle -= SendAwnser;
		SetQuestionHandle -= SetQuestionClientRpc;
	}

    public override void OnNetworkSpawn()
    {
        if(OwnerClientId != 0 && IsOwner)
        {
			//Destroy(GameObject.Find("GameManager"));
        }
    }

	private void SendAwnser()
    {
		if (IsOwner)
		{
			
			SetPlayerAwnserServerRpc();
		}
	}

    [ServerRpc]
	public void SetPlayerAwnserServerRpc()
	{
		score++;
	}

	[ClientRpc]
	public void SetPlayerAwnserClientRpc(int i)
	{
		if(IsOwner)
		SetPlayerAwnserServerRpc();
	}

	[ClientRpc]
	public void SetQuestionClientRpc(int index)
	{
		UIManager.ScorePanelHandle?.Invoke(false);
		UIManager.QuestionsPanelHandle?.Invoke(true);
		if (OwnerClientId != 0)
		{
			QuestionManager.StartGameHandle?.Invoke(index);
			return;
		}
		
		
	}

	
}
