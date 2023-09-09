using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using UnityEngine;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    [SerializeField] private string _player;
    [SerializeField] private float minX, maxX, minY, maxY;

    private void Start()
    {
        Vector2 randomPosition = new Vector2(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY));
        PhotonNetwork.Instantiate(_player, randomPosition, Quaternion.identity);

        Debug.Log(PhotonNetwork.LocalPlayer.NickName);

        PhotonPeer.RegisterType(typeof(Vector2Int), 242, SerializeVector2Int, DeserializeVector2Int);
    }

    public static object DeserializeVector2Int(byte[] data)
    {
        Vector2Int result = new Vector2Int();
        result.x = BitConverter.ToInt32(data,0);
        result.y = BitConverter.ToInt32(data,4);

        return result;
    }

    public static byte[] SerializeVector2Int(object obj)
    {
        Vector2Int vector = (Vector2Int)obj;
        byte[] results = new byte[8];

        BitConverter.GetBytes(vector.x).CopyTo(results, 0);
        BitConverter.GetBytes(vector.y).CopyTo(results, 4);

        return results;
    }
}
