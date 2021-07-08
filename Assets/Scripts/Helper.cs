using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Helper : MonoBehaviour
{
    public static int GetNeighbors(Vector3 _position, int _senseRadius, out Collider[] _colliders)
    {
        var _neighbors = new Collider[(int) Math.Pow(_senseRadius + 3, 2)];
        var neighborCountIncludingItself =
            Physics.OverlapSphereNonAlloc(_position, _senseRadius, _neighbors);
        _colliders = _neighbors;
        return neighborCountIncludingItself;

    }

    public static int CountNeighbors(Vector3 _position, int _senseRadius)
    {
        Collider[] _colliders;
        var neighborsIncludingItself = GetNeighbors(_position, _senseRadius, out _colliders);
        return neighborsIncludingItself;
    }

    public static bool IsEmpty(Vector3 _position) => CountNeighbors(_position, 0) == 0;

    public static Cell GetCell(Vector3 _position)
    {
        Collider[] _colliders;
        var neighborsIncludingItself = GetNeighbors(_position, 0, out _colliders);
        return (Cell) _colliders[0].gameObject.GetComponent("Cell");
    }
}
