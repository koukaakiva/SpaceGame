using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour {
    [SerializeField]
    private InputActionAsset inputActions;
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private Transform camerFocus;

    private Entity entity;
    private World world;
    private Vector2 movementValue;
    private float rotationValue;

    private void OnEnable() {
        if (camera == null) camera = Camera.main;
        world = World.DefaultGameObjectInjectionWorld;
    }

    private void Update() {
        Vector3 movememtDirection = camerFocus.forward * movementValue.y + camerFocus.right * movementValue.x;
        float movementSpeed = 50F;
        camerFocus.position += movememtDirection * movementSpeed * Time.deltaTime;
        camerFocus.eulerAngles += new Vector3(0, rotationValue * movementSpeed * Time.deltaTime, 0);
    }

    private void OnDisable() {
        if (world.IsCreated && !world.EntityManager.Exists(entity))
            world.EntityManager.DestroyEntity(entity);
    }

    private void OnSelect(InputValue value) {
        Vector2 screenPosition = value.Get<Vector2>();
        UnityEngine.Ray ray = camera.ScreenPointToRay(screenPosition);
        if (world.IsCreated && !world.EntityManager.Exists(entity)) {
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

    private void OnMove(InputValue value) {
        if(inputActions.FindAction("Rotate").ReadValue<float>() == 0) //Because when this is not 0 that means the shift key is down and we should be rotating not moving.
            movementValue = value.Get<Vector2>();
    }

    private void OnRotate(InputValue value) {
        rotationValue = value.Get<float>();
    }
}

public struct ClickedPosition : IBufferElementData {
    public RaycastInput value;
}