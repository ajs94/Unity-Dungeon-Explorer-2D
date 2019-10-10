using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public Tilemap board;

    public Room[] rooms;
    public List<Corridor> corridors = new List<Corridor>();
    public List<Corridor> moreCorridors = new List<Corridor>();

    public RoomFactory roomFactory;
    public CorridorFactory corridorFactory;

    // the maximum size of rooms (exclusive); don't go lower than 11 for this
    private int maxRoomSize = 16;

    // the size of the map being made; larger maps have more rooms
    private int mapSizeX = 75;
    private int mapSizeY = 75;

    // check if two rooms overlap the same area
    public bool RoomsOverlapping(Room room1, Room room2)
    {
        if (    room1.vectorOffset.x < (room2.vectorOffset.x + room2.col + 3) &&
                room2.vectorOffset.x < (room1.vectorOffset.x + room1.col + 3) &&
                room1.vectorOffset.y < (room2.vectorOffset.y + room2.rows+ 5) &&
                room2.vectorOffset.y < (room1.vectorOffset.y + room1.rows+ 5))
        {
            return true;
        }
        return false;
    }

    // create a Room object (not instantiated in game yet just the object)
    public Room CreateRoom()
    {
        Room room = new Room();

        Vector3Int randomPos = new Vector3Int(Random.Range(1, mapSizeX), Random.Range(1, mapSizeY), 0);
        room.SetupRoom(maxRoomSize, maxRoomSize, randomPos);

        return room;
    }

    public void SetupBoard(int level)
    {
        corridorFactory = GetComponent<CorridorFactory>();
        corridorFactory.InitializeScripts(board);
        roomFactory = GetComponent<RoomFactory>();
        roomFactory.InitializeScripts(board);

        Vector3Int startPos = new Vector3Int(1, 1, 0);

        rooms = new Room[((mapSizeX * mapSizeY) / (maxRoomSize * maxRoomSize))];
        rooms[0] = new Room();
        rooms[0].SetupRoom(maxRoomSize, maxRoomSize, startPos);

        for (int i = 1; i < rooms.Length; i++)
        {
            rooms[i] = CreateRoom();
            bool iter = true;
            while (iter)
            {
                for (int n = 0; n < i; n++)
                {
                    if (RoomsOverlapping(rooms[i], rooms[n]))
                    {
                        rooms[i] = CreateRoom();
                        break;
                    }
                    else if (n == i - 1)
                    {
                        iter = false;
                    }
                }
            }
        }

        // Sort by distance closest to rooms[0]
        rooms = rooms.OrderBy(room => Vector3.Distance(rooms[0].vectorOffset, room.vectorOffset)).ToArray<Room>();

        // connect each room with the nearest unconnected room
        for (int i = 0; i < rooms.Length - 1; i++)
        {
            int bestIndex = i + 1;
            for (int n = i + 1; n < rooms.Length; n++)
            {
                if (Vector3.Distance(rooms[i].vectorOffset, rooms[n].vectorOffset) <
                    Vector3.Distance(rooms[i].vectorOffset, rooms[bestIndex].vectorOffset))
                {
                    bestIndex = n;
                }
            }
            corridors.Add(new Corridor());
            corridorFactory.ChooseCorridorType(i, bestIndex, corridors[i], rooms, true);
        }

        // connect each room with the one nearest it
        for (int i = 0; i < rooms.Length - 1; i++)
        {
            int bestIndex = i + 1;
            for (int n = 0; n < rooms.Length - 1; n++)
            {
                if (n != i && Vector3.Distance(rooms[i].vectorOffset, rooms[n].vectorOffset) <
                                Vector3.Distance(rooms[i].vectorOffset, rooms[bestIndex].vectorOffset))
                {
                    bestIndex = n;
                }
            }
            moreCorridors.Add(new Corridor());
            corridorFactory.ChooseCorridorType(i, bestIndex, moreCorridors[i], rooms, false);
        }

        for (int i = 0; i < rooms.Length; i++)
        {
            roomFactory.ChooseRoomType(i, rooms);
        }
    }
}