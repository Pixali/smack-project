using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {

    public int roomCount;
    
    public int maxRoomSize;
    public int minRoomSize;

    public GameObject floorTile;
    public GameObject wallTile;

    private List<GameObject> rooms = new List<GameObject>();
    private List<Vector2> roomCoords = new List<Vector2>();
    private List<List<string>> blocks = new List<List<string>>();

    void Start() {
        
        // generate a grid of rooms positions
        for(int x = 0; x < Mathf.CeilToInt(Mathf.Sqrt(roomCount)); x++) {
            for(int y = 0; y < Mathf.CeilToInt(Mathf.Sqrt(roomCount)); y++) {
                roomCoords.Add(new Vector2(x, y));
            }
        }

        // choose a random spot for the rooms on that grid (allows the rooms too be relatively close together)
        for(int i = 0; i < roomCount; i++) {
            Vector2 roomPos = roomCoords[Random.Range(0, roomCoords.Count)];
            roomCoords.Remove(roomPos);
            rooms.Add(generateRoom(i, (int)roomPos.x * maxRoomSize, (int)roomPos.y * maxRoomSize));
        }
        for(int i = 0; i < roomCount; i++) {
            generateCorridor(rooms[i], i);
        }
        // now generate corridors to connect "blocks" of rooms (ensure all rooms interconnect)
        for(int i = 1; i < blocks.Count; i++) {
                generateCorridor(GameObject.Find(blocks[i][Random.Range(0, blocks[i].Count)]+"/Center"), GameObject.Find(blocks[i-1][Random.Range(0, blocks[i-1].Count)]+"/Center"), i);
        }
            
    }
    
    GameObject generateRoom(int id, int startingX, int startingY) {
        
        int width = Random.Range(minRoomSize, maxRoomSize+1);
        int height = Random.Range(minRoomSize, maxRoomSize+1);

        GameObject room = new GameObject("Room"+id);
        room.tag = "Room";

        for(int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                GameObject tile = Instantiate(floorTile, new Vector2(startingX+x, startingY+y), Quaternion.identity);
                tile.transform.parent = room.transform;
                if (x == Mathf.FloorToInt(width/2) && y == Mathf.FloorToInt(height/2)) {
                    tile.name = "Center";
                    tile.tag = "Center";
                }
            }
        }
        return room;
    }

    // directly generate a corridor between two specified rooms
    GameObject generateCorridor(GameObject startingCenterTile, GameObject closestCenterTile, int id) {
        // autism lol
        // draw horizontal line from the center of our starting room with the length of max grid size
        Vector2 line1Start = new Vector2(0, startingCenterTile.transform.position.y);
        Vector2 line1End = new Vector2(roomCount * maxRoomSize, startingCenterTile.transform.position.y);
        // draw vertical line from center of the room we wish to connect with the length of max grid size
        Vector2 line2Start = new Vector2(closestCenterTile.transform.position.x, 0);
        Vector2 line2End = new Vector2(closestCenterTile.transform.position.x, roomCount * maxRoomSize);

        Vector2 line1Dir = (line1End - line1Start).normalized;
        Vector2 line2Dir = (line2End - line2Start).normalized;
        Vector2 line1Norm = new Vector2(-line1Dir.y, line1Dir.x);
        Vector2 line2Norm = new Vector2(-line2Dir.y, line2Dir.x);

        int a = (int)line1Norm.x;
        int b = (int)line1Norm.y;
        int c = (int)line2Norm.x;
        int d = (int)line2Norm.y;

        int k1 = (int)((a * line1Start.x) + (b * line1Start.y));
        int k2 = (int)((c * line2Start.x) + (d * line2Start.y));

        // where do corridors intersect?
        int xIntersect = (d * k1 - b * k2) / (a * d - b * c);
        int yIntersect = (-c * k1 + a * k2) / (a * d - b * c);

        GameObject corridor = new GameObject("Corridor"+id);

        // generate corridors
        int mod = xIntersect > (int)startingCenterTile.transform.position.x ? 1 : -1;
        for(int x = (int)startingCenterTile.transform.position.x; mod == 1 ? (x < xIntersect+1) : (x > xIntersect-1); x+=1*mod) {
            GameObject tile = Instantiate(floorTile, new Vector2(x, startingCenterTile.transform.position.y), Quaternion.identity);
            tile.transform.parent = corridor.transform;
        }
        mod = yIntersect > (int)closestCenterTile.transform.position.y ? 1 : -1;
        for(int y = (int)closestCenterTile.transform.position.y; mod == 1 ? (y < yIntersect+1) : (y > yIntersect-1); y+=1*mod) {
            GameObject tile = Instantiate(floorTile, new Vector2(closestCenterTile.transform.position.x, y), Quaternion.identity);
            tile.transform.parent = corridor.transform;
        }

        startingCenterTile.tag = "Center";

        return corridor; 
    }

    // find closest room and generate a corridor in that direction
    GameObject generateCorridor(GameObject startingRoom, int id) {
        GameObject startingCenterTile = startingRoom.transform.Find("Center").gameObject;
        startingCenterTile.tag = "CenterIgnore";
        GameObject closestCenterTile = Utility.findClosestGameObject(startingCenterTile, "Center");
        GameObject endingRoom = closestCenterTile.transform.parent.gameObject;
        // update blocks (used later to ensure all rooms interconnect)
        bool match = false;
        foreach(List<string> block in blocks) {
           if (block.Contains(startingRoom.name)) {
                if(!block.Contains(endingRoom.name)) {
                    block.Add(endingRoom.name);
                    }
                match = true;
                break;
           } 
           if (block.Contains(endingRoom.name)) {
                if(!block.Contains(startingRoom.name)) {
                    block.Add(startingRoom.name);
                }
                match = true;
                break;
           }
        }

        if (!match) {
            blocks.Add(new List<string>{startingRoom.name, endingRoom.name});
        }

        return generateCorridor(startingCenterTile, closestCenterTile, id); 
    }

    

}
