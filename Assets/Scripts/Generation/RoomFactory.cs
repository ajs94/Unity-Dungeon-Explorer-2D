using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class RoomFactory : MonoBehaviour
{
    public Tilemap board;

    // test/empty rooms
    public RoomManager defaultRoom;

    // actual gameplay rooms


    public void InitializeScripts(Tilemap gameBoard)
    {
        board = gameBoard;

        // empty tall and short rooms
        defaultRoom = GetComponent<RoomManager>();
    }

    // factory for the different room scripts
    public void ChooseRoomType(int index, Room[] rooms)
    {
        int choice = Random.Range(0, 6);

        if (false)
        {
        }
        /*
        else if (choice >= 0)
        {
            defaultRoom.SetupScene(rooms[index], board);
        }
        */
        // the test room, not meant to be actually made
        else
        {
            defaultRoom.SetupScene(rooms[index], board);
        }
    }
}