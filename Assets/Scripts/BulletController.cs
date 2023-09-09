using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Player Owner { get; private set; }

    private Vector3 _shootDir;
    private int _damage = 10;
    private float _speed = 30f;

    private void Start()
    {
        Destroy(gameObject, 3.0f);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerController enemy = collider.GetComponent<PlayerController>();

        if (enemy != null)
        {
            PhotonView enemyView = collider.GetComponent<PhotonView>();

            if (Owner != enemyView.Owner)
            {
                enemy.ApplyDamage(_damage);

                Destroy(gameObject);
            }
        }
    }

    public void InitializeBullet(Player owner, Vector3 originalDirection, float lag)
    {
        _shootDir = originalDirection;

        Owner = owner;

        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = originalDirection;
    }

    private void Update()
    {
        if (_shootDir == null)
        {
            transform.position += Vector3.right * Time.deltaTime * _speed;
        }
        else
        {
            transform.position += _shootDir * Time.deltaTime * _speed;
        }
    }
}

