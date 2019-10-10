using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Room
{
    public bool startRoomFlag = false;
    public Vector3Int vectorOffset = new Vector3Int();
    public Transform roomHolder;
    public bool lightMade = false;

    public int rows;
    public int col;

    public List<Vector3> roomExits = new List<Vector3>();

    private int roomMin = 5;

    public void SetupRoom(int maxWidth, int maxHeight, Vector3Int startPos)
    {
        rows = Random.Range(roomMin, maxHeight);
        col = Random.Range(roomMin, maxWidth);
        vectorOffset = startPos;
    }
}