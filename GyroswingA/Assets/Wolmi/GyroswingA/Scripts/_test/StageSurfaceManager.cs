using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StageSurfaceManager : MonoBehaviour
{
    [SerializeField] NavMeshSurface surface;
    
    void Awake()
    {
        GenerateNavMesh();
    }

    void FixedUpdate()
    {
        GenerateNavMesh();
    }

    void GenerateNavMesh()
    {
        surface.RemoveData();
        surface.BuildNavMesh();
    }
    
}
