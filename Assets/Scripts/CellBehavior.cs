using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CellBehavior : MonoBehaviour
{
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private new Transform transform;
    [SerializeField] private int NumNeighbors;
    [SerializeField] private int senseRadius = 2;
    [SerializeField] private GameManager gameManager;


    private void Awake()
    {
        // caching for performance
    }

    public void SetGameManager(GameManager _gm)
    {
        gameManager = _gm;
    }

    public static int GetNeighbors(Vector3 _position, int _senseRadius, out Collider[] _colliders)
    {
        Collider[] _neighbors = new Collider[(int) Math.Pow((_senseRadius + 3), 2)];
        int neighborCountIncludingItself =
            Physics.OverlapSphereNonAlloc(_position, _senseRadius, _neighbors);
        _colliders = _neighbors;
        return neighborCountIncludingItself;
    }

    public static int CountNeighbors(Vector3 _position, int _senseRadius)
    {
        Collider[] _colliders;
        int neighborsIncludingItself = GetNeighbors(_position, _senseRadius, out _colliders);
        return neighborsIncludingItself - 1;
    }

    public static bool CanSpawn(Vector3 _position)
    {
        return CountNeighbors(_position, 0) == -1;
    }

    public static Collider GetCell(Vector3 _position)
    {
        Collider[] _colliders;
        int neighborsIncludingItself = GetNeighbors(_position, 0, out _colliders);
        return _colliders[0];
    }

    bool CanSpawn()
    {
        int countNeighbors = CountNeighbors(transform.position, senseRadius);
        if (countNeighbors == 3)
        {
            return true;
        }

        return false;
    }

    bool CanLive()
    {
        int countNeighbors = CountNeighbors(transform.position, senseRadius);
        if (countNeighbors == 2 || countNeighbors == 3)
        {
            return true;
        }

        return false;
    }

    public void Reproduce()
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x != 0 && y != 0)
                {
                    if (CanSpawn(transform.position + new Vector3(x, y, 0)))
                    {
                        gameManager.SpawnCell(transform.position + new Vector3(x, y, 0));
                    }
                }
            }
        }
    }

    public void Die()
    {
        gameManager.RemoveCell(rigidbody.gameObject);
        Destroy(rigidbody.gameObject);
    }


    void Start()
    {
    }


    public void GenerationUpdate()
    {
        Debug.Log("GenerationUpdate");
        if (!CanLive())
        {
            Die();
            return;
        }

        Reproduce();
    }
}