using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool mustSnapOnGrid = true;

    // Start is called before the first frame update
    public GameObject cellPrefab;
    public int initialPopulationRange = 25;
    public int initialPopulation = 10;
    private readonly List<GameObject> cellPop = new List<GameObject>();
    private bool creativeMode;

    private void Start()
    {
        Populate(initialPopulation);
        creativeMode = true;
    }


    // Update is called once per frame
    private void Update()
    {
        if (creativeMode)
        {
            int leftButton = 0, rightButton = 1;

            if (Input.GetMouseButtonUp(leftButton))
            {
                var mousePos = Input.mousePosition;
                var _location = Camera.main.ScreenToWorldPoint(mousePos);
                var snapped = FixZ(SnapCellToGrid(_location));

                if (Helper.IsEmpty(snapped))
                {
                    SpawnCell(snapped);
                }
            }
            else if (Input.GetMouseButtonUp(rightButton))
            {
                var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Helper.GetCell(SnapCellToGrid(FixZ(target))).Die();
            }
        }
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
        for (var i = 0; i < cellPop.Count; i++)
        {
            var cellScript = (Cell) cellPop[i].GetComponent("Cell");
            cellScript.GenerationUpdate();
        }
    }

    private Vector3 FixZ(Vector3 _position) => new Vector3(_position.x, _position.y, 1);

    public static Vector3 SnapCellToGrid(Vector3 _location) =>
        new Vector3(Mathf.Round(_location.x), Mathf.Round(_location.y), Mathf.Round(_location.z));


    private void Populate(int size)
    {
        for (var i = 0; i < size; i++)
        {
            Vector3 tryPosition;
            int num;
            do
            {
                var x = Random.Range(-initialPopulationRange, initialPopulationRange);
                var y = Random.Range(-initialPopulationRange, initialPopulationRange);
                var z = Random.Range(-initialPopulationRange, initialPopulationRange);
                tryPosition = FixZ(new Vector3(x, y, z));
                num = Helper.CountNeighbors(tryPosition, 0);
                Debug.Log(num);
            } while (num > 1);

            SpawnCell(tryPosition);
        }
    }


    public void SpawnCell(Vector3 _position)
    {
        var gObj = Instantiate(cellPrefab, _position, Quaternion.identity);
        var cellScript = (Cell) gObj.GetComponent("Cell");
        cellPop.Add(gObj);
    }

    public void RemoveCell(GameObject cell)
    {
        cellPop.Remove(cell);
    }
}