using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cell;
    public int initialPopulationRange = 25;
    public int initialPopulation = 10;
    private List<GameObject> cellPop = new List<GameObject>();

    void Start()
    {
        Populate(initialPopulation);
    }

    public void OnButtonPress()
    {
       UpdatePopulation();
    }

    private void UpdatePopulation()

    {
        foreach (GameObject aCell in cellPop)
        {
            CellBehavior cellScript = (CellBehavior) aCell.GetComponent("CellBehavior");
            cellScript.GenerationUpdate();
        }
    }

        
        
    // Update is called once per frame
    void Update()
    {
       //if (GameObject (cell.rotation) = 0);
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
                int z = (int) Random.Range(-initialPopulationRange, initialPopulationRange);
                tryPosition = new Vector3(x, 0, z);
                num = CellBehavior.CountNeighbors(tryPosition, 0);
                Debug.Log(num);
            } while (num > 1);

            GameObject gObj = Instantiate(cell, tryPosition, Quaternion.identity);
            CellBehavior cellScript = (CellBehavior) gObj.GetComponent("CellBehavior");
            cellScript.SetGameManager(this);
            cellPop.Add(gObj);
        }

    }
    public void RemoveCell(GameObject cell)
    {
        cellPop.Remove(cell);
        Debug.Log($"Number of cells: {cellPop.Count}");
    }

}
