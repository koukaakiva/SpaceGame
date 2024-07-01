using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using RaycastHit = Unity.Physics.RaycastHit;

public class ActerSelectionManager : MonoBehaviour {
    [SerializeField]
    private Camera camera;

    public static ActerSelectionManager instance = null;
    private Entity entity;
    private World world;
    private UnityEvent<Entity> ActerSelected;

    private void OnAwake() {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this);
        DontDestroyOnLoad(this);
    }

    private void OnEnable() {
        if (camera == null) camera = Camera.main;
        world = World.DefaultGameObjectInjectionWorld;
        ActerSelected = new UnityEvent<Entity>();
    }

    public ActerSelectionManager getInstance() {
        return this;
    }

    public void SubscribeToActerSelected(UnityAction<Entity> callback) {
        ActerSelected.AddListener(callback);
    }

    private void OnDisable() {
        if (world.IsCreated && !world.EntityManager.Exists(entity))
            world.EntityManager.DestroyEntity(entity);
    }

    private void OnSelect(InputValue value) {
        Vector2 screenPosition = value.Get<Vector2>();
        UnityEngine.Ray ray = camera.ScreenPointToRay(screenPosition);
        RaycastInput input = new RaycastInput() {
            Start = ray.origin,
            Filter = CollisionFilter.Default,
            End = ray.GetPoint(camera.farClipPlane)
        };
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        PhysicsWorldSingleton physicsWorldSingleton = entityManager.CreateEntityQuery(typeof(PhysicsWorldSingleton)).GetSingleton<PhysicsWorldSingleton>();
        CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
        //NativeList<DistanceHit> distanceHits = new NativeList<DistanceHit>(Allocator.Temp);
        if(collisionWorld.CastRay(input, out RaycastHit hit)){
            Debug.Log($"{hit.Entity.Index}");
            //EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            //ecb.SetComponent(hit.Entity, new Movement {
            //    velocity = float3.zero,
            //    destination = new float3(0, 5, 0)
            //});
            //ecb.Playback(entityManager);
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            ecb.AddComponent(hit.Entity, new SelectedTag {});
            ecb.Playback(entityManager);
            ActerSelected.Invoke(hit.Entity);
        }
    }
}