

using Friflo.Engine.ECS;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Diagnostics;

namespace VoxelGame.Voxel;

public class OctreeNode
{
    public Vector3 Position { get; set; }  // Position of the node
    public float Size { get; set; }
    public Vector3 Color { get; set; }
    public bool HasVoxel;
    public OctreeNode[] Children { get; set; }

    public bool IsLeaf => Children == null;

    public OctreeNode(Vector3 position, float size)
    {
        Position = position;
        Size = size;
        Children = null;
        HasVoxel = false;
    }
    
      public bool ContainsPosition(Vector3 position)
    {
        return position.X >= Position.X && position.X < Position.X + Size &&
               position.Y >= Position.Y && position.Y < Position.Y + Size &&
               position.Z >= Position.Z && position.Z < Position.Z + Size;
    }

    public void SetVoxel(Vector3 position, Vector3 color, float size, bool hasVoxel)
    {
        if (size <= Size / 2)
        {
            int index = GetChildIndex(position);
            if (Children == null)
            {
                Children = new OctreeNode[8];
            }

            if (Children[index] == null)
            {
                Vector3 childPosition = GetChildOrigin(Position, Size, index);
                Children[index] = new OctreeNode(childPosition, Size / 2);
            }

            Children[index].SetVoxel(position, color, size, hasVoxel);
        }
        else
        {
            // If this node is at the desired size, set the voxel here
            HasVoxel = hasVoxel;
            Color = color;
        }
    }

    public int GetChildIndex(Vector3 position)
    {
        int index = 0;
        if (position.X >= Position.X + Size / 2) index |= 1;
        if (position.Y >= Position.Y + Size / 2) index |= 2;
        if (position.Z >= Position.Z + Size / 2) index |= 4;
        return index;
    }

    public Vector3 GetChildOrigin(Vector3 origin, float size, int index)
    {
        float halfSize = size / 2;
        return origin + new Vector3(
            (index & 1) == 0 ? 0 : halfSize,
            (index & 2) == 0 ? 0 : halfSize,
            (index & 4) == 0 ? 0 : halfSize
        );
    }

    public bool IsEmpty()
    {
        // If this node has a voxel, it's not empty
        if (HasVoxel)
            return false;

        // If this node has children, check if any child is not empty
        if (Children != null)
        {
            for (int i = 0; i < 8; i++)
            {
                if (Children[i] != null && !Children[i].IsEmpty())
                {
                    return false;
                }
            }
        }

        // If no voxel in this node and no children are non-empty, this node is empty
        return true;
    }
}
public class VoxelOctree
{
    public OctreeNode root;
    public float Size;

    public VoxelOctree(float Size, Vector3 Center)
    {
        this.Size = Size;
        root = new OctreeNode(Center, Size);

    }


    public bool IsPositionWithinBounds(Vector3 position)
    {
        float halfSize = Size / 2;
        return position.X >= root.Position.X - halfSize && position.X <= root.Position.X + halfSize &&
               position.Y >= root.Position.Y - halfSize && position.Y <= root.Position.Y + halfSize &&
               position.Z >= root.Position.Z - halfSize && position.Z <= root.Position.Z + halfSize;
    }

    public void SetVoxel(Vector3 position, Vector3 color, float size, bool isActive)
    {
        SetVoxelRecursive(root, position, color, size, isActive);
    }

    // private void SetVoxelRecursive(OctreeNode node, Vector3 position, Vector3 color, Vector3 nodeOrigin, float nodeSize, bool isActive)
    // {
    //     if (nodeSize <= minNodeSize)
    //     {
    //         node.IsLeaf = true;
    //         node.HasVoxel = isActive;
    //         node.Position = nodeOrigin;
    //         node.Color = color;
    //         return;
    //     }

    //     Vector3 midPoint = nodeOrigin + new Vector3(nodeSize / 2);
    //     int childIndex = GetChildIndex(position, midPoint);

    //     if (node.Children[childIndex] == null)
    //     {
    //         node.Children[childIndex] = new OctreeNode();
    //     }

    //     Vector3 childOrigin = GetChildOrigin(nodeOrigin, nodeSize, childIndex);

    //     SetVoxelRecursive(node.Children[childIndex], position, color, childOrigin, nodeSize / 2, isActive);

    //     if (node.CanCollapse())
    //     {
    //         CollapseNode(node);
    //     }
    // }

    public bool IsNeighborVoxel(OctreeNode node, Vector3 offset)
    {
        Vector3 neighborPos = node.Position + offset;
        return ContainsVoxel(neighborPos);
    }
    private bool ContainsVoxel(Vector3 position)
    {
        OctreeNode node = GetNodeAtPosition(position);
        return node != null && node.HasVoxel;
    }

    private int GetChildIndex(Vector3 voxelPosition, Vector3 nodeMidpoint)
    {
        int index = 0;

        if (voxelPosition.X >= nodeMidpoint.X)
            index |= 1;
        if (voxelPosition.Y >= nodeMidpoint.Y)
            index |= 2;
        if (voxelPosition.Z >= nodeMidpoint.Z)
            index |= 4;

        return index;
    }

    public Vector3 GetChildOrigin(Vector3 nodeOrigin, float nodeSize, int childIndex)
    {
        float halfSize = nodeSize / 2;
        return new Vector3(
            nodeOrigin.X + (childIndex & 1) * halfSize,
            nodeOrigin.Y + ((childIndex & 2) >> 1) * halfSize,
            nodeOrigin.Z + ((childIndex & 4) >> 2) * halfSize
        );
    }

    // private void CollapseNode(OctreeNode node)
    // {
    //     if (node.CanCollapse())
    //     {
    //         node.IsLeaf = true;
    //         node.HasVoxel = false;
    //         foreach (var child in node.Children)
    //         {
    //             if (child != null && child.HasVoxel)
    //             {
    //                 node.HasVoxel = true;
    //                 break;
    //             }
    //         }
    //         node.Children = null;
    //     }
    // }

    private void SetVoxelRecursive(OctreeNode node, Vector3 position, Vector3 color, float size, bool hasVoxel)
    {
        if (size <= node.Size / 2)
        {
            int index = node.GetChildIndex(position);
            if (node.Children == null)
            {
                node.Children = new OctreeNode[8];
            }

            if (node.Children[index] == null)
            {
                Vector3 childPosition = node.GetChildOrigin(node.Position, node.Size, index);
                node.Children[index] = new OctreeNode(childPosition, node.Size / 2);
            }
            node.HasVoxel = hasVoxel;

            SetVoxelRecursive(node.Children[index], position, color, size, hasVoxel);
        }
        else
        {
            // If this node is at the desired size, set the voxel here
            node.HasVoxel = hasVoxel;
            node.Color = color;
        }
    }

    public OctreeNode GetNodeAtPosition(Vector3 position)
    {
        return GetNodeAtPositionRecursive(root, position);
    }

     private OctreeNode GetNodeAtPositionRecursive(OctreeNode node, Vector3 position)
    {
        if (node == null) return null;

        if (!node.ContainsPosition(position))
            return null;

        if (node.IsLeaf)
            return node;

        int index = node.GetChildIndex(position);

        if (node.Children == null || node.Children[index] == null)
            return null;

        return GetNodeAtPositionRecursive(node.Children[index], position);
    }

    public OctreeNode GetNeighbor(Vector3 position, Vector3 direction)
    {
        Vector3 neighborPosition = position + direction * Size;
        return GetNodeAtPosition(neighborPosition);
    }
}