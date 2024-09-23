using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AStarPathfinding
{
    public int[,] grid; // 0: movable, 1 : can't move
    public int jumpForce; // maximum jump height
    private int xMin, yMin;
    private Dictionary<int, List<Vector2Int>> pathcache = new();
    private bool isUsed = false;

    public AStarPathfinding(int[,] grid, int xMin, int yMin, int jumpForce = 1)
    {
        this.grid = grid;
        this.jumpForce = jumpForce;
        this.xMin = xMin;
        this.yMin = yMin;
    }

    public async Task<List<Vector2Int>> FindPathInField(Vector2Int start, Vector2Int target, int jumpForce = 1)
    {
        while (isUsed)
        {
            await Task.Yield();
        }
        isUsed = true;
        start -= new Vector2Int(xMin, yMin);
        target -= new Vector2Int(xMin, yMin);
        this.jumpForce = jumpForce;


        isUsed = false;
        return FindPath(start, target);
    }

    public Vector2Int ApplyGravity(Vector2Int start)
    {
        Vector2Int newStart = new Vector2Int(start.x, start.y);

        // if current point is already can't move, return current Point
        if (grid[newStart.x, newStart.y] != 0)
        {
            return newStart;
        }

        // move down point reach dismovable point
        while (IsWithinBounds(newStart) && grid[newStart.x, newStart.y] == 0)
        {
            newStart.y--; // move y axis down
        }

        // if reach dismovable point, point move up
        newStart.y++;

        // if grid range out, return start point
        if (!IsWithinBounds(newStart))
        {
            return start;
        }

        return newStart;
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
    {
        // activate gravity to start point
        start = ApplyGravity(start);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        Node startNode = new Node(start, 0, Heuristic(start, target));
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                // search lowest cost node
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // if reach target point, return path
            if (currentNode.position == target)
            {
                return RetracePath(startNode, currentNode);
            }

            // Explore neighbor nodes
            ExploreNeighbors(currentNode, target, openSet, closedSet);
        }

        return new List<Vector2Int>(); // if path is not exists, return default
    }

    private void ExploreNeighbors(Node currentNode, Vector2Int target, List<Node> openSet, HashSet<Node> closedSet)
    {
        // x axis Move directions
        Vector2Int[] xDirections = { new Vector2Int(1, 0), new Vector2Int(-1, 0) }; // right, left

        foreach (var direction in xDirections)
        {
            // X Axis Search applying gravity
            Vector2Int newPos = ApplyGravity(currentNode.position + direction);
            if (IsWithinBounds(newPos))
            {
                if (grid[newPos.x, newPos.y] == 0)
                {
                    // When gravity acts on the object and it moves away from the original node, 
                    // create new nodes to cover the distance.
                    Node nodetmp = null;
                    if (currentNode.position.y > newPos.y)
                    {
                        for (int y = (currentNode.position + direction).y; y != newPos.y; y--)
                        {
                            if (nodetmp == null)
                                nodetmp = CreateNode(currentNode, new Vector2Int(newPos.x, y), target, openSet, closedSet);
                            else
                            {
                                nodetmp = CreateNode(nodetmp, new Vector2Int(newPos.x, y), target, openSet, closedSet);
                            }
                        }
                    }

                    // Add gravity activate node
                    if (nodetmp == null)
                        AddNeighbor(currentNode, newPos, target, openSet, closedSet);
                    else
                    {
                        AddNeighbor(nodetmp, newPos, target, openSet, closedSet);
                    }
                }
                // When X Axis searching reach wall, Start Y Axis Search (Stairs)
                else
                {
                    if (currentNode.jump < jumpForce)
                    {
                        Vector2Int upPos = new Vector2Int(currentNode.position.x, currentNode.position.y + 1);

                        Debug.Log($"Current Node {currentNode.position} upPos {upPos}");
                        if (IsWithinBounds(upPos) && grid[upPos.x, upPos.y] == 0)
                        {
                            AddNeighbor(currentNode, upPos, target, openSet, closedSet, currentNode.jump + 1);
                        }
                    }
                }
            }

        }
    }

    private void AddNeighbor(Node currentNode, Vector2Int newPos, Vector2Int target, List<Node> openSet, HashSet<Node> closedSet, int jump = 0)
    {
        int newCostToNeighbor = currentNode.gCost + 1; // movement cost

        // Create New Node
        Node neighbor = CreateNode(currentNode, newPos, target, openSet, closedSet, jump);
        if (neighbor != null)
        {
            // Add the node if it has a lower cost or has not been explored yet
            if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
            {
                if (!openSet.Contains(neighbor))
                {
                    if (!closedSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
    }

    private Node CreateNode(Node currentNode, Vector2Int newPos, Vector2Int target, List<Node> openSet, HashSet<Node> closedSet, int jump = 0)
    {
        // Create New Node, and Add the node if it has not been explored yet.
        Node neighbor = new Node(newPos);
        if (!openSet.Contains(neighbor) && !closedSet.Contains(neighbor))
        {
            neighbor.gCost = currentNode.gCost + 1;
            neighbor.hCost = Heuristic(neighbor.position, target);
            neighbor.parent = currentNode;
            neighbor.jump = jump;

            return neighbor;
        }


        return null;
    }



    private bool IsWithinBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < grid.GetLength(0) && pos.y >= 0 && pos.y < grid.GetLength(1);
    }

    private int Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // Manhattan distance
    }

    private List<Vector2Int> RetracePath(Node startNode, Node endNode)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Node currentNode = endNode;

        // node search to Node
        while (currentNode != startNode)
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }
        path.Add(startNode.position);
        path.Reverse();
        return path;
    }

    private class Node
    {
        public Vector2Int position;
        public int gCost; // cost to start point
        public int hCost; // cost to target point
        public int jump;
        public Node parent;

        public Node(Vector2Int position, int gCost = int.MaxValue, int hCost = int.MaxValue, int jump = 0)
        {
            this.position = position;
            this.gCost = gCost;
            this.hCost = hCost;
            this.jump = jump;
        }

        public int fCost => gCost + hCost;

        public override int GetHashCode()
        {
            return position.x ^ 2 + position.y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Node node = (Node)obj;

            return position.x == node.position.x && position.y == node.position.y;
        }
    }
}