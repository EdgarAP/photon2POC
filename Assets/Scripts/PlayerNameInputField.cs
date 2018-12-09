using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class PlayerNameInputField : MonoBehaviour
{

    const string PLAYER_NAME_PREF_KEY = "PlayerName";

    void Start()
    {
        string defaultName = string.Empty;

        InputField _inputField = this.GetComponent<InputField>();
        if (_inputField != null)
        {
            if (PlayerPrefs.HasKey(PLAYER_NAME_PREF_KEY))
            {
                defaultName = PlayerPrefs.GetString(PLAYER_NAME_PREF_KEY);
                _inputField.text = defaultName;
            }
        }

        PhotonNetwork.NickName = defaultName;
    }

    public void SetPlayerName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("Player name is null or empty");
            return;
        }
        PhotonNetwork.NickName = name;

        PlayerPrefs.SetString(PLAYER_NAME_PREF_KEY, name);
    }

}
