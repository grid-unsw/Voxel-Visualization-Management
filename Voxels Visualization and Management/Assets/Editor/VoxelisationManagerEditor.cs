using System.IO;
using UnityEditor;
using UnityEngine;
using VoxelSystem;
using Debug = UnityEngine.Debug;

[CustomEditor(typeof(VoxelisationManager))]
public class VoxelisationManagerEditor : Editor
{
    VoxelisationManager _voxelisationManager;
    private MeshFilter[] _meshFilters;
    private MultiValueVoxelModel _voxelColorModel;

    private void OnEnable()
    {
        _voxelisationManager = (VoxelisationManager) target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Build voxels"))
        {
            if (_meshFilters == null || _meshFilters.Length == 0)
            {
                _meshFilters = FindObjectsOfType(typeof(MeshFilter)) as MeshFilter[];
            }

            if (AssetDatabase.LoadAssetAtPath("Assets/Scripts/Shaders/Voxelizer.compute", typeof(ComputeShader)) == null)
            {
                throw new FileLoadException("Voxelizer compute shader is not present");
            }

            _voxelisationManager.Voxelizer = (ComputeShader)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Shaders/Voxelizer.compute", typeof(ComputeShader));


            if (_voxelisationManager.hasColor)
            {
                if (_voxelColorModel == null)
                {
                    _voxelColorModel = _voxelisationManager.GetMultiValueVoxelData(_meshFilters);

                    Debug.Log("Voxels are successfully created!");
                }

                if (_voxelisationManager.voxelMesh)
                {
                    _voxelisationManager.BuildColorMesh(_voxelColorModel);
                }

                if (_voxelisationManager.vfxVisualisation)
                {
                    _voxelisationManager.VisualiseVfxColorVoxels(_voxelColorModel);
                }

                if (_voxelisationManager.ExportAsPointCloud)
                {
                    _voxelisationManager.ExportPts(_voxelColorModel);
                }
            }
            else
            {
                foreach (var voxelData in _voxelisationManager.GetVoxelData(_meshFilters))
                {
                    if (voxelData == null) continue;
                    
                    var voxels = voxelData.GetData();

                    if (_voxelisationManager.voxelMesh)
                    {
                        _voxelisationManager.BuildMesh(voxels, voxelData.Width, voxelData.Height,
                            voxelData.Depth);
                    }

                    if (_voxelisationManager.vfxVisualisation)
                    {
                        _voxelisationManager.VisualiseVfxVoxels(voxels, voxelData.Width, voxelData.Height,
                            voxelData.Depth, voxelData.PivotPoint);
                    }

                    if (_voxelisationManager.ExportAsPointCloud)
                    {
                        _voxelisationManager.ExportPts(voxels, voxelData.Width, voxelData.Height, voxelData.PivotPoint);
                    }

                    if (_voxelisationManager.ExportAsPointCloud)
                    {
                        _voxelisationManager.ExportToPostgres(voxels, voxelData.Width, voxelData.Height, voxelData.PivotPoint);
                    }

                    voxelData.Dispose();
                }
            }
        }
    }
}
