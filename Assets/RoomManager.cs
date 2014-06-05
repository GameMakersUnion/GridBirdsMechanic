using UnityEngine;
using System.Collections;
using Assets;

public class RoomManager {

    public Mappable[,] grid;
    private static RoomManager instance; 

    private RoomManager(int width, int height)
    {
        grid = new Mappable[width, height];

    }
    public static RoomManager init(int width, int height){ 
        instance = new RoomManager(width, height);
        return instance;
    }
    public static RoomManager get(){
        if (instance == null) throw new System.Exception("grid was not initialized");
        return instance;
    }

    public bool checkEmpty(int x, int y)
    {
        return grid[x,y]== null;
    }

    public Mappable getAt(int x, int y)
    {
        return grid[x,y];
    }

    public Mappable attack(int x, int y, int damage)
    {
        return null;
    }

    public void register(Mappable m, int x, int y)
    {
        grid[x, y] = m;
    }
    public void unregister(Mappable m, int x, int y)
    {
        if (m == grid[x,y]) grid[x, y] = null;
        else throw new System.Exception("Trying to unregister wrong mappable");
    }
}
