using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerName : NetworkBehaviour
{
	
	public string playerName;
	LobbyManager lobbyManager;

	private IEnumerator Start()
	{
		lobbyManager = FindObjectOfType<LobbyManager>();

		if (IsServer)
		{
			while (NetworkManager.Singleton.ConnectedClients.Count != lobbyManager.joinnedLobby.Players.Count)
			{
				yield return new WaitForSeconds(1);
			}
			yield return new WaitForSeconds(1);
			for (int i = 0; i < NetworkManager.Singleton.ConnectedClients.Count; i++)
			{
				NetworkManager.Singleton.ConnectedClients[(ulong)i].PlayerObject.GetComponentInChildren<PlayerName>()
					.SetPlayerNameClientRpc(lobbyManager.joinnedLobby.Players[i].Data["name"].Value);
			}
		}
	}


	[ServerRpc]
	public void SetPlayerNameServerRpc()
	{		
		SetPlayerNameClientRpc(lobbyManager.playerNameInput.text);
	}

	[ClientRpc]
	public void SetPlayerNameClientRpc(string playerNameReceived)
	{
		playerName = playerNameReceived;
	}
}
