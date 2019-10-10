using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class CorridorManager : MonoBehaviour
{
    public Tilemap board;
    public Corridor currentCorridor;
    public int corridorWidthMax = 3;    // exclusive

    public GameObject door;

    public RuleTile[] floorTiles;

    public Vector3Int currentPos;
    public Vector3Int startPos;
    public Vector3Int endPos;

    public virtual void AssignWidth(Room room1, Room room2)
    {
        startPos = room1.vectorOffset;

        // currentCorridor.corridorWidth = Random.Range(1, corridorWidthMax);
        currentCorridor.corridorWidth = 1;

        // determine the width of the hallway
        // on the side
        if (((room1.vectorOffset.y + room1.rows) - room2.vectorOffset.y >= currentCorridor.corridorWidth) &&
                (room1.vectorOffset.y + room1.rows) > room2.vectorOffset.y)
        {
            currentPos = new Vector3Int((room1.col / 2),
                Mathf.RoundToInt(room2.vectorOffset.y - room1.vectorOffset.y), 0) + room1.vectorOffset;
            endPos = new Vector3Int((room2.col / 2),
                currentPos.y - room2.vectorOffset.y, 0) + room2.vectorOffset;
        }
        // above right-ish
        else if ((room1.vectorOffset.x + room1.col - room2.vectorOffset.x >= currentCorridor.corridorWidth) &&
                (room1.vectorOffset.x + room1.col) > room2.vectorOffset.x &&
                (room1.vectorOffset.x < room2.vectorOffset.x))
        {
            currentPos = new Vector3Int(Mathf.RoundToInt(room2.vectorOffset.x - room1.vectorOffset.x), (room1.rows / 2), 0) + room1.vectorOffset;
            endPos = new Vector3Int(currentPos.x - room2.vectorOffset.x, (room2.col / 2), 0) + room2.vectorOffset;
        }
        // above left-ish
        else if ((room2.vectorOffset.x + room2.col - room1.vectorOffset.x >= currentCorridor.corridorWidth) &&
                (room2.vectorOffset.x + room2.col) > room1.vectorOffset.x &&
                (room2.vectorOffset.x < room1.vectorOffset.x))
        {
            currentPos = new Vector3Int(0, (room2.rows / 2), 0) + room1.vectorOffset;
            endPos = new Vector3Int(currentPos.x - room2.vectorOffset.x, (room2.col / 2) + 1, 0) + room2.vectorOffset;
        }
        // outside the length/ width
        else
        {
            // to the above right with not enough space b/t for a corridor
            if (room1.vectorOffset.x <= room2.vectorOffset.x &&
                room1.vectorOffset.x + room1.col > room2.vectorOffset.x)
            {
                currentPos = new Vector3Int(0, room1.rows / 2, 0) + room1.vectorOffset;
                endPos = new Vector3Int((room2.col / 2), room2.rows / 2, 0) + room2.vectorOffset;
            }
            // to the above left
            else if (room1.vectorOffset.x >= room2.vectorOffset.x &&
                     room2.vectorOffset.x + room2.col > room1.vectorOffset.x)
            {
                currentPos = new Vector3Int((room2.vectorOffset.x + room2.col - room1.vectorOffset.x + 1), room1.rows / 2, 0) + room1.vectorOffset;
                endPos = new Vector3Int((room2.col / 2), room2.rows / 2, 0) + room2.vectorOffset;
            }
            // directly left/ right
            else if (room1.vectorOffset.y + room1.rows + 1 > room2.vectorOffset.y &&
                    (room2.vectorOffset.x + room2.col < room1.vectorOffset.x ||
                     room1.vectorOffset.x + room1.col < room2.vectorOffset.x))
            {
                currentPos = new Vector3Int(room1.col / 2, room1.rows / 2, 0) + room1.vectorOffset;
                endPos = new Vector3Int(room2.col / 2, room1.vectorOffset.y + room1.rows - room2.vectorOffset.y + 1, 0) + room2.vectorOffset;
            }
            else
            {
                currentPos = new Vector3Int((room1.col / 2), room1.rows / 2, 0) + room1.vectorOffset;
                endPos = new Vector3Int((room2.col / 2), room2.rows / 2, 0) + room2.vectorOffset;
            }
        }
    }

    /* The start of the generation functions
     * 
     */
    public void SetupConnection(Room room1, Room room2, bool VerticalFirst)
    {
        // the one with the high z coordinate should be room2
        if (room1.vectorOffset.y > room2.vectorOffset.y)
        {
            Room temp = room1;
            room1 = room2;
            room2 = temp;
        }

        AssignWidth(room1, room2);

        if (VerticalFirst)
        {
            BuildVertically(room1, room2);
            BuildHorizontally(room1, room2);
        }
        else
        {
            BuildHorizontally(room1, room2);
            BuildVertically(room1, room2);
        }
    }

    public void BuildVertically(Room room1, Room room2)
    {
        while (currentPos.y < endPos.y)
        {
            for (int x = 0; x < currentCorridor.corridorWidth; x++)
            {
                bool outsideRoom1 = (currentPos.x + x < room1.vectorOffset.x || currentPos.y < room1.vectorOffset.y) ||
                                    (currentPos.x > room1.vectorOffset.x + room1.col - 1 || currentPos.y > room1.vectorOffset.y + room1.rows - 1);
                bool outsideRoom2 = (currentPos.x + x < room2.vectorOffset.x || currentPos.y < room2.vectorOffset.y) ||
                                    (currentPos.x > room2.vectorOffset.x + room2.col - 1 || currentPos.y > room2.vectorOffset.y + room2.rows - 1);

                if (outsideRoom1 && outsideRoom2)
                {
                    currentCorridor.corridorPosition.Add(currentPos + new Vector3Int(x, 0, 0));
                    RuleTile toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                    board.SetTile(new Vector3Int(currentPos.x + x, currentPos.y, 0), toInstantiate);
                }
            }
            currentPos.y++;
        }
    }

    public void BuildHorizontally(Room room1, Room room2)
    {
        // if to the right
        if (currentPos.x < endPos.x)
        {
            while (currentPos.x <= endPos.x)
            {
                for (int z = 0; z < currentCorridor.corridorWidth; z++)
                {
                    bool outsideRoom1 = (currentPos.x < room1.vectorOffset.x || currentPos.y + z < room1.vectorOffset.y) ||
                                        (currentPos.x > room1.vectorOffset.x + room1.col - 1 || currentPos.y > room1.vectorOffset.y + room1.rows - 1);
                    bool outsideRoom2 = (currentPos.x < room2.vectorOffset.x || currentPos.y + z < room2.vectorOffset.y) ||
                                        (currentPos.x > room2.vectorOffset.x + room2.col - 1 || currentPos.y > room2.vectorOffset.y + room2.rows - 1);

                    if (outsideRoom1 && outsideRoom2)
                    {
                        currentCorridor.corridorPosition.Add(currentPos + new Vector3Int(0, z, 0));
                        RuleTile toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                        board.SetTile(new Vector3Int(currentPos.x, currentPos.y + z, 0), toInstantiate);
                    }
                }
                currentPos.x++;
            }
        }
        // if to the left
        else if (currentPos.x > endPos.x)
        {
            while (currentPos.x >= endPos.x)
            {
                for (int z = 0; z < currentCorridor.corridorWidth; z++)
                {
                    bool outsideRoom1 = (currentPos.x < room1.vectorOffset.x || currentPos.y + z < room1.vectorOffset.y) ||
                                        (currentPos.x > room1.vectorOffset.x + room1.col - 1 || currentPos.y > room1.vectorOffset.y + room1.rows - 1);
                    bool outsideRoom2 = (currentPos.x < room2.vectorOffset.x || currentPos.y + z < room2.vectorOffset.y) ||
                                        (currentPos.x > room2.vectorOffset.x + room2.col - 1 || currentPos.y > room2.vectorOffset.y + room2.rows - 1);

                    if (outsideRoom1 && outsideRoom2)
                    {
                        currentCorridor.corridorPosition.Add(currentPos + new Vector3Int(0, z, 0));
                        RuleTile toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                        board.SetTile(new Vector3Int(currentPos.x, currentPos.y + z, 0), toInstantiate);
                    }
                }
                currentPos.x--;
            }
        }
    }

    public virtual void SetupCorridor(Room room1, Room room2, Corridor corridor, bool VerticalFirst, Tilemap gameBoard)
    {
        currentCorridor = corridor;
        board = gameBoard;
        SetupConnection(room1, room2, VerticalFirst);
        currentCorridor.corridorHolder = new GameObject("Corridor").transform;
    }
}
