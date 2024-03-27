using System.Collections;
using System.Collections.Generic;
using EG_Spawn;
using Unity.Burst;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;

public partial struct EG_Spawn_PrefabLoadSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PrefabConfig>();
    }

    // [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;

        var config = SystemAPI.GetSingleton<PrefabConfig>();
        var configEntity = SystemAPI.GetSingletonEntity<PrefabConfig>();
        var prefabsReference = SystemAPI.GetBuffer<EntityPrefabReferenceElement>(configEntity);
        if (prefabsReference.Length <= 0)
            return;

        // Adding the RequestEntityPrefabLoaded component will request the prefab to be loaded.
        // It will load the entity scene file corresponding to the prefab and add a PrefabLoadResult
        // component to the entity. The PrefabLoadResult component contains the entity you can use to
        // instantiate the prefab (see the PrefabReferenceSpawnerSystem system).
        state.EntityManager.AddComponentData(configEntity, new RequestEntityPrefabLoaded
        {
            Prefab = prefabsReference[0].Value
        });
    }
}
