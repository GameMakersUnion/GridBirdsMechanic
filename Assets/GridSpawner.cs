using UnityEngine;
using System.Collections;

public class GridSpawner : MonoBehaviour {


    
    public int numberOfCellsX = 16, numberOfCellsY = 10;
    public float cellWidth = 0.8f, cellHeight = 0.8f;
    public GameObject floor, line;
	// Use this for initialization
	void Start () {
        float mapWidth = numberOfCellsX * cellWidth;
        float mapHeight = numberOfCellsY * cellHeight;
        floor.transform.localScale = new Vector3(mapWidth *100, mapHeight*100, 0);

        Bounds bounds = floor.renderer.bounds;
        Vector3 min = bounds.min, max = bounds.max;

        //var sr = floor.GetComponent("SpriteRenderer");




        for (int x = 0; x < numberOfCellsX; x++)
        {
            MakeLine(new Vector2(x * cellWidth, mapHeight/2), false);
        }
        for (int y = 0; y < numberOfCellsY; y++)
        {
            MakeLine(new Vector2( mapWidth/2, y * cellHeight), true);
        }

        RoomManager.init(numberOfCellsX, numberOfCellsY);
	}
    void MakeLine(Vector2 middle, bool horizontal)
    {
        GameObject newline = (GameObject)Instantiate(line, new Vector3(middle.x, middle.y, 0), Quaternion.identity);
        int stretch = 10;
        if (horizontal)
        {
            newline.transform.localScale =new Vector3(numberOfCellsX * cellWidth * 100, stretch, 0);
        }
        else
        {
            newline.transform.localScale = new Vector3(stretch, numberOfCellsY * cellHeight * 100, 0);
            
        }
        newline.transform.parent = this.transform;

    }

	
	// Update is called once per frame
	void Update () {
	
	}
}
