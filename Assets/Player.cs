using UnityEngine;
using System.Collections;
using Assets;
using System.Collections.Generic;

public enum PlayerState
{
    dead,
    attack,
    moving,
    idle
}
public enum PlayerDirection
{
    up,
    down,
    left,
    right
}

public enum PlayerNum { P1, P2, P3, P4, P5 };

public class Player : MonoBehaviour, Mappable {

    public Sprite idle, attack, move;

    public PlayerNum PlayerNumber;
    private PlayerState _state = PlayerState.idle;
    public PlayerState state { get { return _state; } set { _state = value; UpdateSprite(); } }

    private PlayerDirection _direction = PlayerDirection.down;
    public PlayerDirection direction { get { return _direction; } set { _direction = value; UpdateSprite(); } }

    private int cellX, cellY;
    //private int destX, destY;
    private Vector3 targetPos, prevPos;
	private bool damn;
    public float speed = 0.1f;
    private float walkProgress;

	// Use this for initialization
	
    void Start () {
        grid = GameObject.Find("Grid");
        gridSpawner = grid.GetComponent<GridSpawner>();

        float Xoffset = (this.transform.position.x + 0.4f) % 0.8f;
        if (Xoffset >= 0.4f) { Xoffset -= 0.8f; };
        float Yoffset = (this.transform.position.y + 0.4f) % 0.8f;
        if (Yoffset >= 0.4f) { Yoffset -= 0.8f; };
        float newx = this.transform.position.x - Xoffset;
        float newy = this.transform.position.y - Yoffset;
        this.transform.position = new Vector3(newx, newy, this.transform.position.z);

        int currentX = FindCellPositionX(transform.position.x);
        int currentY = FindCellPositionY(transform.position.y);

        RoomManager.get().register(this, currentX, currentY);
	}
	private GameObject grid;
    private GridSpawner gridSpawner;
	// Update is called once per frame
	void Update () {
        float x = Input.GetAxis("Horizontal" + PlayerNumber.ToString());
        float y = Input.GetAxis("Vertical" + PlayerNumber.ToString());
        //Debug.Log("X:" + x + " Y:" + y);
	    if (state != PlayerState.moving)
        {
			if (damn)
			{
				damn = false;
			}
			else
			{
            	float Xoffset = (this.transform.position.x + 0.4f) % 0.8f;
            	if (Xoffset >= 0.4f) { Xoffset -= 0.8f; }; 
            	float Yoffset = (this.transform.position.y + 0.4f) % 0.8f;
            	if (Yoffset >= 0.4f) { Yoffset -= 0.8f; };
				float newx = this.transform.position.x - Xoffset;
				float newy = this.transform.position.y - Yoffset;
                //newx = Mathf.Round(this.transform.position.x - 0.4f) + 0.4f;
                //newy = Mathf.Round(this.transform.position.y - 0.4f) + 0.4f;
				this.transform.position = new Vector3(newx, newy, this.transform.position.z);
			}
        }
        if (state == PlayerState.idle)
        {

            float zone = 0.5f;
            int tx = 0, ty = 0;
            PlayerDirection? temp = null;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                if (x > zone) { tx = 1; temp = PlayerDirection.right; }
                else if (x < -zone) { tx = -1; temp = PlayerDirection.left; }
            }
            else
            {
                if (y > zone) { ty = -1; temp = PlayerDirection.up; }
                else if (y < -zone) { ty = 1; temp = PlayerDirection.down; }
            }
            //Debug.Log("x:" + x + "  y: "+ y + "playernum:"+PlayerNumber);
            if (temp != null)
            {
                int currentX = FindCellPositionX(transform.position.x);
                int currentY = FindCellPositionY(transform.position.y);
                //Debug.Log("CurrentX:" + currentX + " CurrentY:" + currentY);

                int nextX = currentX + tx;
                int nextY = currentY + ty;

                if (!( nextX >= gridSpawner.numberOfCellsX
                    || nextX < 0
                    || nextY >= gridSpawner.numberOfCellsY
                    || nextY < 0))
                {
                    //Debug.Log("Movement detected");
                    if (RoomManager.get().checkEmpty(nextX, nextY))
                    {
                        state = PlayerState.moving;
                        direction = (PlayerDirection)temp;
                        //destX = currentX + tx;
                        //destY = currentY + ty;
                        prevPos = transform.position;
                        targetPos = new Vector3(prevPos.x + (tx * gridSpawner.cellWidth),
                                                prevPos.y + (ty * gridSpawner.cellHeight), 0);
                        damn = true;
                        walkProgress = 0f;
                        RoomManager.get().register(this, nextX, nextY);
                        RoomManager.get().unregister(this, currentX, currentY);

                    }
                }
            }
        }
        else if (state == PlayerState.moving)
        {
            walkProgress += speed / 100f;
            if (walkProgress >= 1f)
            {
                walkProgress = 0f;
                transform.position = targetPos;
                state = PlayerState.idle;
            }
            else
            {
                transform.position = Vector3.Lerp(prevPos, targetPos, walkProgress);
            }
        }
        
	}

    int FindCellPositionX(float xpos)
    {
        return (int)((xpos) / gridSpawner.cellWidth);
    }
    int FindCellPositionY(float ypos)
    {
        return (int)((ypos) / gridSpawner.cellHeight);
    }
    private void UpdateSprite()
    {

        if (this.state == PlayerState.attack)
        {
            this.GetComponent<SpriteRenderer>().sprite = attack;
        }
        if (this.state == PlayerState.idle)
        {
            this.GetComponent<SpriteRenderer>().sprite = idle;
        }
        if (this.state == PlayerState.moving)
        {
            this.GetComponent<SpriteRenderer>().sprite = move;
        }


        this.transform.rotation = Quaternion.identity;
        if (this.direction == PlayerDirection.up)
        {
            this.transform.Rotate(Vector3.forward * 180);
        }
        if (this.direction == PlayerDirection.down)
        {
            this.transform.Rotate(Vector3.forward * 0);
        }
        if (this.direction == PlayerDirection.left)
        {
            this.transform.Rotate(Vector3.forward * 90);
        }
        if (this.direction == PlayerDirection.right)
        {
            this.transform.Rotate(Vector3.forward * -90);
        }
 	    //throw new System.NotImplementedException();
    } 
}
