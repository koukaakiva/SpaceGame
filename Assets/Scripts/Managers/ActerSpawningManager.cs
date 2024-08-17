using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class ActerSpawningManager : MonoBehaviour {
    public Mesh Mesh;
    public Material Material;
    public int EntityCount;

    void Start() {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);

        //TODO: Crete from achetype with components
        Entity entityPrototype = entityManager.CreateEntity();
        entityManager.AddComponent(entityPrototype, typeof(ActerTag));
        entityManager.AddComponent(entityPrototype, typeof(Movement));
        entityManager.AddComponent(entityPrototype, typeof(RandomNumberGenerator));
        entityManager.AddComponent(entityPrototype, typeof(LocalTransform));
        entityManager.AddComponent(entityPrototype, typeof(LocalToWorld));
        entityManager.SetComponentData<LocalTransform>(entityPrototype, new LocalTransform { Position = new float3(0,0,0), Scale = 1});
        entityManager.AddBuffer<LinkedEntityGroup>(entityPrototype).Add(entityPrototype);
        entityManager.GetBuffer<LinkedEntityGroup>(entityPrototype);

        Entity visualsPrototype = entityManager.CreateEntity();
        RenderMeshUtility.AddComponents(
            visualsPrototype,
            entityManager,
            new RenderMeshDescription(
                shadowCastingMode: ShadowCastingMode.Off,
                receiveShadows: false
            ),
            new RenderMeshArray(
                new Material[] { Material },
                new Mesh[] { Mesh }
            ),
            MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0)
        );
        entityManager.AddComponentData(visualsPrototype, new LocalTransform());
        entityManager.SetComponentData<LocalTransform>(visualsPrototype, new LocalTransform { Position = new float3(0, 1, 0), Scale = 1});
        entityManager.AddComponent(visualsPrototype, typeof(Parent));

        SpawnJob spawnJob = new(){
            EntityPrototype = entityPrototype,
            VisualsPrototype = visualsPrototype,
            EntityCount = EntityCount,
            Ecb = ecb.AsParallelWriter(),
        };
        JobHandle spawnHandle = spawnJob.Schedule(EntityCount, 128);
        spawnHandle.Complete();
        ecb.Playback(entityManager);
        ecb.Dispose();
        entityManager.DestroyEntity(visualsPrototype);
        entityManager.DestroyEntity(entityPrototype);
    }
}

[GenerateTestsForBurstCompatibility]
public struct SpawnJob : IJobParallelFor {
    public Entity EntityPrototype;
    public Entity VisualsPrototype;
    public int EntityCount;
    public EntityCommandBuffer.ParallelWriter Ecb;

    public void Execute(int index) {
        //TODO: Change spawn location
        Entity entity = Ecb.Instantiate(index, EntityPrototype);
        //Ecb.AddComponent(index, entity, new LocalTransform { Position = new float3(index, 0, 0) });
        //Ecb.AddComponent(index, entity, new EntityLinkRequest { mode = LinkingMode.Parent, hash = entity.GetHashCode()});
        Entity visuals = Ecb.Instantiate(index, VisualsPrototype);
        Ecb.SetComponent(index, visuals, new Parent { Value = entity });
        //Ecb.AppendToBuffer<LinkedEntityGroup>(index, entity, entity);
        Ecb.AppendToBuffer<LinkedEntityGroup>(index, entity, new LinkedEntityGroup{ Value = visuals });
        //Ecb.AddComponent(index, visuals, new Parent { Value = entity});
        //Ecb.AddComponent(index, visuals, new LocalTransform{Position = new float3(0, 1, 0), Scale = 1 });
        //DynamicBuffer<LinkedEntityGroup> buffer = Ecb.AddBuffer<LinkedEntityGroup>(index, entity);
        //buffer.Add(visuals);
        //Ecb.AddComponent(index, visuals, new EntityLinkRequest { mode = LinkingMode.Child, hash = entity.GetHashCode() });
    }
}