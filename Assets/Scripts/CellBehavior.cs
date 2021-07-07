using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CellBehavior : MonoBehaviour
{
    [SerializeField] private bool mustSnapOnGrid = true;
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
public static int GetNeighbors(Vector3 _position, int _senseRadius)
      {
          Collider[] _neighbors = new Collider[(int)Math.Pow((_senseRadius+3),2)];
          int neighborCountIncludingItself = Physics.OverlapSphereNonAlloc(_position, _senseRadius,  _neighbors);
          return neighborCountIncludingItself - 1;
            
      }

public static int CountNeighbors(Vector3 _position, int _senseRadius)
      {
          return GetNeighbors(_position, _senseRadius);

      }


bool CanReproduce()
      {
          return false;
      }

bool  CanLive()
{
    int countNeighbors = CountNeighbors(transform.position, senseRadius);
    if(countNeighbors == 2 || countNeighbors == 3)
    {
        return true;
    }
    return false;
}

public void Reproduce()
      {
          return;
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

        if (CanReproduce())
        {
            Reproduce();
        }
    }
    
}