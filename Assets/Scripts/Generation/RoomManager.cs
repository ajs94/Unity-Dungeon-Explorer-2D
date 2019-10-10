using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public Tilemap board;

    public RuleTile[] floorTiles;
    public GameObject[] wallTiles;

    public List<Vector3> gridPosition = new List<Vector3>();

    public Room room;

    public void InitialiseList()
    {
        gridPosition.Clear();

        //Loop through x axis (columns).
        for (int x = 0; x <= room.col; x++)
        {
            //Within each column, loop through y axis (rows).
            for (int y = 0; y < room.rows; y++)
            {
                gridPosition.Add(new Vector3(x, 0f, y) + room.vectorOffset);
            }
        }
    }

    // Blank else if's are special/wall tiles unimplemented
    public void RoomSetup()
    {
        for (int x = 0; x < room.col; x++)
        {
            for (int y = 0; y < room.rows; y++)
            {

                Collider[] intersecting = Physics.OverlapSphere(new Vector3(x, 0f, y) + room.vectorOffset, .1f);
                if (intersecting.Length != 0)
                {
                    // if object exists here
                }
                else if (room.roomExits.Contains(new Vector3(x, 0f, y) + room.vectorOffset))
                {
                    // if its a room exit tile
                }
                else
                {
                    RuleTile toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                    board.SetTile(new Vector3Int(x, y, 0) + room.vectorOffset, toInstantiate);
                }
            }
        }
    }

    public virtual void SetupScene(Room newRoom, Tilemap gameBoard)
    {
        room = newRoom;
        board = gameBoard;
        room.roomHolder = new GameObject("Room").transform;

        RoomSetup();
        InitialiseList();
    }
}
