using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;



public class Astar : MonoBehaviour
{
   
    [SerializeField]
    private Tilemap tilemap;

    private Vector3Int startPos, goalPos;

    private Node current;

    private HashSet<Node> openList;

    private HashSet<Node> closedList;

    private Stack<Vector3> path;

    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();

    private static HashSet<Vector3Int> noDiagonalTiles = new HashSet<Vector3Int>();

    public Tilemap MyTilemap { get => tilemap; }

    public static HashSet<Vector3Int> NoDiagonalTiles { get => noDiagonalTiles; }

    public Stack<Vector3> Algorithm(Vector3 start, Vector3 goal)
    {
        startPos = MyTilemap.WorldToCell(start);
        goalPos = MyTilemap.WorldToCell(goal);

        current = GetNode(startPos);

        openList = new HashSet<Node>();

        closedList = new HashSet<Node>();

        foreach(KeyValuePair<Vector3Int, Node> node in allNodes)
        {
            node.Value.Parent = null;
        }

        allNodes.Clear();

        openList.Add(current);

        path = null;

        while (openList.Count > 0 && path == null)
        {
            List<Node> neighbors = findNeighbors(current.Position);

            ExamineNeighbors(neighbors, current);

            UpdateCurrentTile(ref current);

            path = GeneratePath(current);

           
        }

       
        if(path != null)
        {
            return path;
        }

        return null;
       
    }

    private List<Node> findNeighbors(Vector3Int parentposition)
    {
        List<Node> neighbors  = new List<Node>();

        for(int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
               

                if( y!= 0 || x!= 0)
                {
                    Vector3Int neighborPosition = new Vector3Int(parentposition.x - x, parentposition.y - y, parentposition.z);

                    if (neighborPosition != startPos && !GameManager.MyInstance.Blocked.Contains(neighborPosition))
                    {
                        Node neighbor = GetNode(neighborPosition);
                        neighbors.Add(neighbor);
                    }
                    
                }
            }
        }

        return neighbors;
    }

    private bool ConnectedDiagonally(Node currentNode,Node neighbor)
    {
        Vector3Int direct = currentNode.Position - neighbor.Position;

        Vector3Int first = new Vector3Int(current.Position.x + (direct.x  *-1), current.Position.y, current.Position.z);
        Vector3Int second = new Vector3Int(current.Position.x, current.Position.y + (direct.y * -1), current.Position.z);

        if(GameManager.MyInstance.Blocked.Contains(first) || GameManager.MyInstance.Blocked.Contains(second))
        {
            return false;
        }

        return true;
    }

    private void ExamineNeighbors(List<Node> neighbors, Node current)
    {
        for(int i = 0; i < neighbors.Count; i++)
        {
            Node neighbor = neighbors[i];

            if (!ConnectedDiagonally(current, neighbor))
            {
                continue;
            }

            int gScore = DetermineGScore(neighbors[i].Position, current.Position);

            if (gScore == 14 && NoDiagonalTiles.Contains(neighbor.Position) && NoDiagonalTiles.Contains(current.Position))
            {
                continue;
            }

            if (openList.Contains(neighbor))
            {
                if(current.G + gScore < neighbor.G)
                {
                    CalcVAlues(current, neighbor, gScore);
                }
            }
            else if (!closedList.Contains(neighbor))
            {
                CalcVAlues(current, neighbor, gScore);

                openList.Add(neighbor);
            }

            

            

                
        }
    }

    private void CalcVAlues(Node parent, Node neighbor, int cost)
    {
        neighbor.Parent = parent;

        neighbor.G = parent.G + cost;

        neighbor.H = ((Math.Abs((neighbor.Position.x - goalPos.x)) + Math.Abs((neighbor.Position.y - goalPos.y))) * 10);

        neighbor.F = neighbor.G + neighbor.H; 
    }

    private int DetermineGScore(Vector3Int neighbor, Vector3Int current)
    {
        int gScore = 0;

        int x = current.x - neighbor.x;
        int y = current.y - neighbor.y;

        if(Math.Abs(x-y) % 2 == 1)
        {
            gScore = 10;
        }
        else
        {
            gScore = 14;
        }

        return gScore;
    }

    private void UpdateCurrentTile(ref Node current)
    {
        openList.Remove(current);

        closedList.Add(current);

        if(openList.Count > 0)
        {
            current = openList.OrderBy(x => x.F).First();
        }
    }

    private Node GetNode(Vector3Int position)
    {
        if (allNodes.ContainsKey(position))
        {
            return allNodes[position];
        }
        else
        {
            Node node = new Node(position);
            allNodes.Add(position, node);
            return node;
        }
    }

    //public void ChangeTileType(TileButton button)
    //{
    //    tileType = button.MyTileType;
    //}

    //private void ChangeTile(Vector3Int clickPos)
    //{
       
    //    if(tileType == GameManager.MyInstance.Blocked.Contains(clickPos))
    //    {
    //        tilemap.SetTile(clickPos, water);
    //        waterTiles.Add(clickPos);
    //    }
    //    else
    //    {
    //        if (tileType == TileType.Start)
    //        {
    //            if (start)
    //            {
    //                tilemap.SetTile(startPos, tiles[3]);
    //            }
    //            start = true;
    //            startPos = clickPos;
    //        }
    //        else if (tileType == TileType.Goal)
    //        {
    //            if (goal)
    //            {
    //                tilemap.SetTile(goalPos, tiles[3]);
    //            }
    //            goal = true;
    //            goalPos = clickPos;


                
    //        }

    //        tilemap.SetTile(clickPos, tiles[(int)tileType]);
    //    }

    //    changeTiles.Add(clickPos);

     
    //}

    private Stack<Vector3> GeneratePath(Node current)
    {
        if(current.Position == goalPos)
        {
            Stack<Vector3> finalpath = new Stack<Vector3>();

            while(current != null)
            {
                finalpath.Push(MyTilemap.CellToWorld(current.Position));   

                current = current.Parent;
            }
             
            return finalpath;
        }
         
        return null;
    }

    //public void Reset()
    //{
      

        
    //    foreach(Vector3Int position in changeTiles)
    //    {
    //        tilemap.SetTile(position, tiles[3]);
    //    }
    //    foreach(Vector3Int position in path)
    //    {
    //        tilemap.SetTile(position, tiles[3]);
    //    }

    //    tilemap.SetTile(startPos, tiles[3]);
    //    tilemap.SetTile(goalPos, tiles[3]);
    //    waterTiles.Clear();
    //    allNodes.Clear();
    //    start = false;
    //    goal = false;
    //    path = null;
    //    current = null;
    //}
}
