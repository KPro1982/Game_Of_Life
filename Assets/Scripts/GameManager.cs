using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngineInternal.MathfInternal;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public bool mustSnapOnGrid = true;
    // Start is called before the first frame update
    public GameObject cell;
    public int initialPopulationRange = 25;
    public int initialPopulation = 10;
    private List<GameObject> cellPop = new List<GameObject>();
    private bool creativeMode;

    void Start()
    {
        Populate(initialPopulation);
        creativeMode = true;
    }

    public void OnButtonPress()
    {
       UpdatePopulation();
    }
    public void OnCreativeToggle(bool _value)
    {

        creativeMode = _value;
        Debug.Log($"Creative: {creativeMode}");
    }
    private void UpdatePopulation()

    {
        for (int i = 0; i < cellPop.Count; i++)
        {
            CellBehavior cellScript = (CellBehavior) cellPop[i].GetComponent("CellBehavior");
            cellScript.GenerationUpdate();
        }
    }

        
        
    // Update is called once per frame
    void Update()
    {

        if (creativeMode)
        {
            int leftButton = 0, rightButton = 1;
            
            if (Input.GetMouseButtonUp(leftButton))
            {
                Vector3 mousePos = Input.mousePosition;
                Vector3 _location = Camera.main.ScreenToWorldPoint(mousePos);
                Vector3 snapped = FixZ(SnapCellToGrid(_location));
                
                if (CellBehavior.CanSpawn(snapped))
                {
                    SpawnCell(snapped);
                }
                
            } 
            else if (Input.GetMouseButtonUp(rightButton))
            {
                Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider col = CellBehavior.GetCell(FixZ(target));
                CellBehavior cell = (CellBehavior) col.gameObject.GetComponent("CellBehavior");
                cell.Die();
            }
        }
    }

    Vector3 FixZ(Vector3 _position)
    {
        return (new Vector3(_position.x, _position.y, 1));
    }
    Vector3 SnapCellToGrid(Vector3 _location)
    {
        if (mustSnapOnGrid)
        {
            return new Vector3(Mathf.Round(_location.x), Mathf.Round(_location.y), Mathf.Round(_location.z));
        }

        return _location;

    }
    

    void Populate(int size)
    {

        for (int i = 0; i < size; i++)
        {
            Vector3 tryPosition;
            int num;
            do
            {
                int x = (int) Random.Range(-initialPopulationRange, initialPopulationRange);
                int y = (int) Random.Range(-initialPopulationRange, initialPopulationRange);
                int z = (int) Random.Range(-initialPopulationRange, initialPopulationRange);
                tryPosition = FixZ(new Vector3(x, y, z));
                num = CellBehavior.CountNeighbors(tryPosition, 0);
                Debug.Log(num);
            } while (num > 1);
            SpawnCell(tryPosition);
        }

    }

    

    public void SpawnCell(Vector3 _position)
    {
 

        GameObject gObj = Instantiate(cell, _position, Quaternion.identity);
        CellBehavior cellScript = (CellBehavior) gObj.GetComponent("CellBehavior");
        cellScript.SetGameManager(this);
        Debug.Log($"New cell: {_position.x}, {_position.y}, {_position.z}");
        cellPop.Add(gObj);
    }

    public void RemoveCell(GameObject cell)
    {
        cellPop.Remove(cell);
        Debug.Log($"Number of cells: {cellPop.Count}");
    }
    
    

}
