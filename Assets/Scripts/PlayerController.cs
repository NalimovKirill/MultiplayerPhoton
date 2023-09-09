using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IPunObservable
{
    [HideInInspector]
    public UnityEvent OnCollectCoin = new UnityEvent();
    [HideInInspector]
    public UnityEvent OnDamageTaken = new UnityEvent();
    [HideInInspector]
    public UnityEvent OnWinPopUp = new UnityEvent();

    [SerializeField] private TMP_Text _nickText;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletPositionRight;
    [SerializeField] private Transform _bulletPositionLeft;
    [SerializeField] private float _speed = 3f;

    private PhotonView _view;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Joystick _movementJoystick;
    private GameController _gameController;
    private Button _exitButton;
    private Button _shootButton;
    private Vector2Int _directionFace;

    private int _health = 100;
    public int Health { get { return _health;}}
    private int _coinAmount = 0;
    public int CoinAmount { get { return _coinAmount;}}

    public bool isReadyToStart = false;

    private void Awake()
    {
        _movementJoystick = FindObjectOfType(typeof(Joystick)).GetComponent<Joystick>();
        _gameController = FindObjectOfType(typeof(GameController)).GetComponent<GameController>();
        _exitButton = GameObject.Find("ButtonExitRoom").GetComponent<Button>();
        _shootButton = GameObject.Find("ButtonShoot").GetComponent<Button>();
    }

    private void Start()
    {
        _view = GetComponent<PhotonView>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _nickText.text = _view.Owner.NickName;

        _gameController.OnStartGame.AddListener(ReadyToStart);
        _exitButton.onClick.AddListener(ExitRoomButton);
        _shootButton.onClick.AddListener(ShootBullet);
    }

    private void FixedUpdate()
    {
        Move();
    }
    private void Update()
    {
        FlipPlayerSprite();
    }

    private void Move()
    {
        if (_view.IsMine)
        {
            if (isReadyToStart)
            {
                if (_movementJoystick.JoystikVec.y != 0)
                {
                    _rb.velocity = new Vector2(_movementJoystick.JoystikVec.x * _speed,
                        _movementJoystick.JoystikVec.y * _speed);

                    if (_movementJoystick.JoystikVec.x < 0)
                    {
                        _directionFace = Vector2Int.left;
                    }
                    else
                    {
                        _directionFace = Vector2Int.right;
                    }
                }
                else
                {
                    _rb.velocity = Vector2.zero;
                }
            }
        }
    }

    private void FlipPlayerSprite()
    {
        if (_directionFace == Vector2Int.left)
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
        }
    }

    private void ShootBullet()
    {
        if (!_view.IsMine)
            return;

        if (isReadyToStart)
        {
            Quaternion quaternion = new Quaternion();

            quaternion.Set(_movementJoystick.JoystickVecLastPosition.x, _movementJoystick.JoystickVecLastPosition.y, 0f, 1);

            _view.RPC("FireRPC", RpcTarget.AllViaServer, this.transform.position, quaternion);
        }
    }

    private void ReadyToStart()
    {
        isReadyToStart = true;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_directionFace);
        }
        else
        {
            _directionFace = (Vector2Int)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void CollectCoin(int coinAmount)
    {
        _coinAmount++;
        Debug.Log(_coinAmount.ToString());

        OnCollectCoin.Invoke();
    }

    [PunRPC]
    public void FireRPC(Vector3 position, Quaternion direction, PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        GameObject bullet;

        bullet = Instantiate(_bulletPrefab, position, Quaternion.identity) as GameObject;
        bullet.GetComponent<BulletController>().InitializeBullet(_view.Owner, new Vector3(direction.x, direction.y, direction.z), Mathf.Abs(lag));

    }
    [PunRPC]
    public void ApplyDamage(int damage) 
    {
        if (!_view.IsMine)
            return;

        _health -= damage;
        OnDamageTaken.Invoke();
        if (_health <= 0)
        {
            isReadyToStart = false;
            _view.RPC("SetNotActivePlayer", RpcTarget.All);
            OnWinPopUp?.Invoke();
        }
    }

    [PunRPC]
    private void SetNotActivePlayer()
    {
        this.gameObject.SetActive(false);
    }
    public void ExitRoomButton()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }
}
