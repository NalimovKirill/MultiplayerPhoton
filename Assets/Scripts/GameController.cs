using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private int countOfCoins = 15;  // Count of coins in Game
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private TMP_Text _coinText;

    [SerializeField] private Image _healthBar;
    [SerializeField] private TMP_Text _healthIntText;
    [SerializeField] private float _healthsFill = 100f;

    [SerializeField] private GameObject _winPanel;
    [SerializeField] private TMP_Text _winPanelText;
    [SerializeField] private GameObject _waitingPanelText;

    private int _damage = 10;

    private int _coins = 0;

    private PhotonView _view;

    [HideInInspector]
    public UnityEvent OnStartGame = new UnityEvent();

    private void Start()
    {
        _view = GetComponent<PhotonView>();

        _coinText.text = _coins.ToString();
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < countOfCoins; i++)
            {
                Vector2 randomPosition = new Vector2(Random.Range(-7, 7), Random.Range(-4, 4));
                PhotonNetwork.Instantiate("CoinPrefab", randomPosition, Quaternion.identity);
            }
        }

        PlayerController playerController = FindObjectOfType<PlayerController>();

        _coinText.text = "Amount coins: ";

        _healthBar.fillAmount = 100f;
        _healthIntText.text = playerController.Health.ToString();

        playerController.OnCollectCoin.AddListener(SetCoinsText);
        playerController.OnDamageTaken.AddListener(SetHealthText);
        playerController.OnWinPopUp.AddListener(OpenWinPopUp);
    }
    private void Update()
    {
        if (PhotonNetwork.PlayerList.Length >= 2)
        {
            _waitingPanelText.SetActive(false);
            OnStartGame?.Invoke();
        }
    }

    private void SetCoinsText()
    {
        _coins++;
        _coinText.text = "Amount coins: " + _coins.ToString();
    }

    private void SetHealthText()
    {
        _healthsFill -= _damage;
        _healthBar.fillAmount = _healthsFill / 100f;
        _healthIntText.text = _healthsFill.ToString();
    }

    private void OpenWinPopUp()
    {
        _view.RPC("WinPanel", RpcTarget.All);
    }

    [PunRPC]
    private void WinPanel()
    {
        PlayerController winPlayer = FindObjectOfType<PlayerController>();

        string nick = winPlayer.GetComponent<PhotonView>().Owner.NickName;
        string coins = winPlayer.CoinAmount.ToString();

        winPlayer.isReadyToStart = false;

        Time.timeScale = 0;

        _winPanel.SetActive(true);

        _winPanelText.text = (nick + " Won"+ "\n" + "Amount coins: " + coins);
    }
}
