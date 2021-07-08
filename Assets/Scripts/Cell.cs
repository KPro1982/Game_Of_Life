using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
    // [SerializeField] private new Rigidbody rigidbody;
    // [SerializeField] private new Transform transform;
    [SerializeField] private int senseRadius = 2;
    private AdjacentCellArray _adjacentCells;
    public Vector3 Position { get; private set; }
    private GameManager _gameManager;

    public Vector3 SnappedPosition => GameManager.SnapCellToGrid(Position);


    private void Awake()
    {
        // initialize variables
        Position = transform.position;
        _adjacentCells = new AdjacentCellArray(this);
        _gameManager = FindObjectOfType<GameManager>();
        // rigidbody = new Rigidbody(); // but isn't this just replaced by unity?
        // transform = rigidbody.transform; // but isn't this just replaced by unity?
    }


    private void Start()
    {
         
    }

   private bool CanLive()
   {
       int itself = 1;
       var countNeighbors = Helper.CountNeighbors(transform.position, senseRadius) - itself; 
        if (countNeighbors == 2 || countNeighbors == 3)
        {
            return true;
        }

        return false;
    }

    public void Reproduce()
    {
    }

    public void Die()
    {
        _gameManager.RemoveCell(this.gameObject);
        Destroy(this.gameObject);
    }


    public void GenerationUpdate()
    {
        if (!CanLive())
        {
            Die();
            return;
        }

        Reproduce();
    }

    private void Register(Cell cell)
    {
        throw new NotImplementedException();
    }

    public void Refesh()
    {
        _adjacentCells.RefreshAdjacent();
    }
}