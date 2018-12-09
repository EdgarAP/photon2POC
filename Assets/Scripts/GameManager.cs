using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks {

	public override void OnLeftRoom() {
		SceneManager.LoadScene(0);
	}

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
	}

	void LoadArena()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			Debug.LogError("Trying to Load a level but we are not the master Client");
		}
		Debug.LogFormat("Loading level: {0}", PhotonNetwork.CurrentRoom.PlayerCount);
		PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Debug.LogFormat("OnPlayerEnteredRoom() {0}", newPlayer.NickName);

		if (PhotonNetwork.IsMasterClient)
		{
			Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

			LoadArena();
		}
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		Debug.LogFormat("OnPLayerLeftRoom() {0}", otherPlayer.NickName);

		if (PhotonNetwork.IsMasterClient)
		{
			Debug.LogFormat("OnPlayerLeftRoom() IsMasterClient {0}", PhotonNetwork.IsMasterClient);

			LoadArena();
		}
	}
}
