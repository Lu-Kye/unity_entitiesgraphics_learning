using System.Collections;
using System.Collections.Generic;
using EG_Spawn;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Serialization;
using UnityEditor;
using UnityEngine;

namespace EG_Spawn
{
    public struct EntityPrefabReferenceElement : IBufferElementData
    {
        public EntityPrefabReference Value;
    }
    
    public struct PrefabConfig : IComponentData
    {
        public int Size;
    }
}

public class EG_Spawn_PrefabAuthoring : MonoBehaviour
{
    public GameObject[] Prefabs;

#if UNITY_EDITOR    
    class Baker : Baker<EG_Spawn_PrefabAuthoring>
    {
        public override void Bake(EG_Spawn_PrefabAuthoring authoring)
        {
            var prefabsReference = new NativeArray<EntityPrefabReferenceElement>(authoring.Prefabs.Length, Allocator.Temp);
            
            var i = 0;
            foreach (var prefab in authoring.Prefabs)
            {
                var prefabReference = new EntityPrefabReference(prefab);
                prefabsReference[i] = new EntityPrefabReferenceElement() { Value = prefabReference };
                i++;
            }
            
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PrefabConfig()
            {
                Size = i
            });

            var buffer = AddBuffer<EntityPrefabReferenceElement>(entity);
            buffer.AddRange(prefabsReference);

            prefabsReference.Dispose();
        }
    }
#endif    
}
