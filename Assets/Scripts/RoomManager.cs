using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _createRoomInput;
    [SerializeField] private TMP_InputField _connectToRoomInput;
    [SerializeField] private TMP_InputField _playerNickInput;
    [SerializeField] private TMP_Text _errorWindow;

    private string _playerNickName;
    private string _errorText;
    
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(_playerNickName))
        {
            FillNickName();
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        if (string.IsNullOrEmpty(_createRoomInput.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(_createRoomInput.text, roomOptions);
    }
    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(_playerNickName))
        {
            FillNickName();
            return;
        }
        PhotonNetwork.JoinRoom(_connectToRoomInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }

    public void ChangeNickName()
    {
        _playerNickName = _playerNickInput.text;
        PhotonNetwork.NickName = _playerNickName;

        Debug.Log(PhotonNetwork.NickName);
    }

    private void FillNickName()
    {
        _errorText = "Change Your NickName before connect";
        _errorWindow.text = _errorText;
    }
}
