using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading;

public class DynamicNavigation : BehaviorSingleton<DynamicNavigation>
{

    [SerializeField]    NavMeshCollectGeometry m_UseGeometry = NavMeshCollectGeometry.PhysicsColliders;
    [SerializeField]    LayerMask m_LayerMask = ~0;
    [SerializeField]    int m_AgentTypeID;
    //NavMesh.GetSettingsByID

    public NavMeshCollectGeometry UseGeometry { get { return m_UseGeometry; } set { m_UseGeometry = value; } }
    public LayerMask LayerMask { get { return m_LayerMask; } set { m_LayerMask = value; } }
    public int AgentTypeID { get { return m_AgentTypeID; } set { m_AgentTypeID = value; } }

    public NavMeshDataInstance NavMeshInstance { get; protected set; }

    public NavMeshData BuildNavigation(Transform root)
    {
        return NavMeshBuilder.BuildNavMeshData(NavMesh.GetSettingsByID(m_AgentTypeID), GetSource(root), 
            new Bounds(Vector3.zero, Vector3.one * 100000f), Vector3.zero, Quaternion.identity);
    }

    public void SetNavMeshData(NavMeshData data)
    {
        if (NavMeshInstance.valid)
            NavMeshInstance.Remove();

        NavMeshInstance = NavMesh.AddNavMeshData(data);
    }

    List<NavMeshBuildSource> GetSource(Transform root)
    {
        var sources = new List<NavMeshBuildSource>();
        NavMeshBuilder.CollectSources(root, LayerMask, UseGeometry, 0, new List<NavMeshBuildMarkup>(), sources);

        sources.RemoveAll((x) => (x.component != null && x.component.gameObject.GetComponent<NavMeshAgent>() != null));

        return sources;
    }
}
