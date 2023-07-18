using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public InputAction input;
    public Camera camera;

    private Entity entity;
    private World world;

    private void OnEnable()
    {
        input.started += MouseClicked;
        input.Enable();

        if(camera == null) camera = Camera.main;

        world = World.DefaultGameObjectInjectionWorld;
    }

    private void OnDisable()
    {
        input.started -= MouseClicked;
        input.Disable();
        if (world.IsCreated && !world.EntityManager.Exists(entity))
        {
            world.EntityManager.DestroyEntity(entity);
        }
    }

    private void MouseClicked(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = context.ReadValue<Vector2>();
        UnityEngine.Ray ray = camera.ScreenPointToRay(screenPosition);
        if(world.IsCreated && !world.EntityManager.Exists(entity)) {
            entity = world.EntityManager.CreateEntity();
            world.EntityManager.AddBuffer<ClickedPosition>(entity);
        }
        RaycastInput input = new RaycastInput() {
            Start = ray.origin,
            Filter = CollisionFilter.Default,
            End = ray.GetPoint(camera.farClipPlane)
        };
        world.EntityManager.GetBuffer<ClickedPosition>(entity).Add(new ClickedPosition() { value = input });
    }
}

public struct ClickedPosition : IBufferElementData {
    public RaycastInput value;
}