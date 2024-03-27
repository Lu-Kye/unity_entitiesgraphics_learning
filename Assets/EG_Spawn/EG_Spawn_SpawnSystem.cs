using EG_Spawn;
using UnityEngine;
using Unity.Burst;
using Unity.Entities;
using Unity.Scenes;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

public partial struct EG_Spawn_SpawnSystem : ISystem
{
        float timer;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PrefabConfig>();
            state.RequireForUpdate<PrefabLoadResult>();
        }

        // [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            timer -= SystemAPI.Time.DeltaTime;
            if (timer > 0)
            {
                return;
            }
            timer = 1.0f;

            var config = SystemAPI.GetSingleton<PrefabConfig>();
            var cubeConfigEntity = SystemAPI.GetSingletonEntity<PrefabConfig>();
            if (!SystemAPI.HasComponent<PrefabLoadResult>(cubeConfigEntity))
            {
                return;
            }

            var prefabLoadResult = SystemAPI.GetComponent<PrefabLoadResult>(cubeConfigEntity);
            var entity = state.EntityManager.Instantiate(prefabLoadResult.PrefabRoot);
            var random = Random.CreateFromIndex((uint) state.GlobalSystemVersion);
            state.EntityManager.SetComponentData(entity,
                LocalTransform.FromPosition(random.NextFloat(-5, 5), random.NextFloat(-5, 5), 0));
        }
}
