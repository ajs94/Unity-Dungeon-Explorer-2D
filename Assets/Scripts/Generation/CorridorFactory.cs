using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public enum CorridorType
{
    Default, Tall, Mined, Painting
}

public class CorridorFactory : MonoBehaviour
{
    public Tilemap board;

    // test/empty corridors
    public CorridorManager defaultCorrdior;

    public void InitializeScripts(Tilemap gameBoard)
    {
        board = gameBoard;

        // empty default corridor
        defaultCorrdior = GetComponent<CorridorManager>();
    }

    // factory for the different corridor scripts
    public void ChooseCorridorType(int index, int bestIndex, Corridor corridor, Room[] rooms, bool inOut)
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
            defaultCorrdior.SetupCorridor(rooms[index], rooms[bestIndex], corridor, false, board);
        }
    }
}