
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corridor : MonoBehaviour
{
    public List<Vector3Int> corridorPosition = new List<Vector3Int>();
    public Transform corridorHolder;

    private int corridorWidthMax = 4;
    public int corridorWidth;

    private Vector3Int currentPos;
    private Vector3Int endPos;

    /*
    public void setType(CorridorType newType)
    {
        corridorType = newType;
    }
    */
}