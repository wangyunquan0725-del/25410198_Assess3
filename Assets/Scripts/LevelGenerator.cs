using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] tiles;
    public int[,] oldMap = {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,8},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0}
    };
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(GameObject.Find("Manual Level Layout"));
        int[,] newMap = BuildFullMap(oldMap);
        LevelGenerate(newMap);
        Camera cam = Camera.main;
        cam.orthographicSize = newMap.GetLength(0)/2+1.5f;
        cam.transform.position = new Vector3(newMap.GetLength(1)/2,  -newMap.GetLength(0)/2,-5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    int[,] BuildFullMap(int[,] oldMap)
    {
        int rows = oldMap.GetLength(0);
        int cols = oldMap.GetLength(1);
        int fullRows = 2 * rows - 1; 
        int fullCols = 2 * cols;

        int[,] fullMap = new int[fullRows, fullCols];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int tile = oldMap[r, c];
                fullMap[r, c] = tile;
                fullMap[r, (fullCols - 1) - c] = tile;
                fullMap[(fullRows - 1) - r, c] = tile;
                fullMap[(fullRows - 1) - r, (fullCols - 1) - c] = tile;
            }
        }

        return fullMap;
    }
    
    void LevelGenerate(int[,] map)
    {
        for (int r = 0; r < map.GetLength(0); r++)
        {
            for (int c = 0; c < map.GetLength(1); c++)
            {
                int tileId = map[r, c];
                if (tileId == 0) continue;
                Instantiate(tiles[tileId], new Vector3(c , -r , 0), Rotate(map,r,c,tileId), transform);
            }
        }
    }

    Quaternion Rotate(int[,] map, int r, int c, int id)
    {
        if (id == 5 || id == 6) return Quaternion.identity;
        bool up= r > 0 && IsWall(map[r - 1, c]);
        bool down= r < map.GetLength(0)-1 && IsWall(map[r + 1, c]);
        bool left= c > 0 && IsWall(map[r, c - 1]);
        bool right= c < map.GetLength(1)-1 && IsWall(map[r, c + 1]);

        switch (id)
        {
            //corner
            case 1:
            case 3:
                if (up && down && left && right)
                {
                    if (!IsWall(map[r + 1, c + 1])) return Quaternion.Euler(0, 0, 0);
                    if (!IsWall(map[r + 1, c - 1])) return Quaternion.Euler(0, 0, 270);
                    if (!IsWall(map[r - 1, c - 1])) return Quaternion.Euler(0, 0, 180);
                    if (!IsWall(map[r - 1, c + 1])) return Quaternion.Euler(0, 0, 90);
                }
                if (up && right)  return Quaternion.Euler(0, 0, 90);
                if (down && right) return Quaternion.Euler(0, 0, 0);
                if (down && left) return Quaternion.Euler(0, 0, 270);
                if (up && left)   return Quaternion.Euler(0, 0, 180);
                break;

            //wall
            case 2:
            case 4:
            case 8:
                if (!up || !down ) return Quaternion.Euler(0, 0, 90);
                break;
            //T junction
            case 7:
                if (!down)
                {
                    if (!IsWall(map[r - 1, c + 1])) return Quaternion.Euler(0, 0, 180);
                    return Quaternion.Euler(180, 0, 0);
                }
                
                if (!up)  return Quaternion.Euler(0, !IsWall(map[r + 1, c - 1])?0:180, 0);
                if (!right)
                {
                    if (!IsWall(map[r - 1, c - 1])) return Quaternion.Euler(0, 0, -90);
                    return Quaternion.Euler(180, 0, -90);
                }
                if (!left)
                {
                    if (!IsWall(map[r - 1, c + 1]))  return Quaternion.Euler(180, 0, 90);
                    return Quaternion.Euler(0, 0, 90);
                }
                break;
        }
        return Quaternion.identity;
    }
    
    bool IsWall(int tileId)
    {
        return (tileId == 1 || tileId == 2 || tileId == 3 || tileId == 4 || tileId == 7 || tileId == 8);
    }
}
