using System.Collections.Generic;
using UnityEngine;


namespace VoxelSystem
{
    public class MultiValueVoxelModel
    {
        public List<int>[] Voxels
        {
            get { return voxels; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public int Depth
        {
            get { return depth; }
        }

        public Vector3 PivotPoint
        {
            get { return pivotPoint; }
        }

        public float VoxelSize
        {
            get { return voxelSize; }
        }

        public string[] ObjectNames
        {
            get { return objectNames; }
        }

        List<int>[] voxels;
        int width, height, depth;
        Vector3 pivotPoint;
        float voxelSize;
        private string[] objectNames;

        public MultiValueVoxelModel(List<int>[] v, int w, int h, int d, Vector3 pivot, float vSize, string[] names)
        {
            voxels = v;
            width = w;
            height = h;
            depth = d;
            pivotPoint = pivot;
            voxelSize = vSize;
            objectNames = names;
        }
    }
}