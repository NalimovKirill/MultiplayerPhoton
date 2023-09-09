using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class LauncherPUN : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _createRoomInput;
    [SerializeField] private TMP_InputField _connectToRoomInput;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("Lobby");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
    }
}
