using System;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour {
    public InputAction select;
    public Camera camera;
    public Transform camerFocus;

    private Entity entity;
    private World world;
    private Vector2 movementValue;
    private float rotationValue;

    private void OnEnable() {
        select.started += MouseClicked;
        select.Enable();
        if (camera == null) camera = Camera.main;
        world = World.DefaultGameObjectInjectionWorld;
    }

    private void Update() {
        Vector3 movememtDirection = camerFocus.forward * movementValue.y + camerFocus.right * movementValue.x;
        float movementSpeed = 50F;
        if(rotationValue == 0)
            camerFocus.position += movememtDirection * movementSpeed * Time.deltaTime;
        camerFocus.eulerAngles += new Vector3(0, rotationValue * movementSpeed * Time.deltaTime, 0);
    }

    private void OnDisable() {
        select.started -= MouseClicked;
        select.Disable();
        if (world.IsCreated && !world.EntityManager.Exists(entity))
            world.EntityManager.DestroyEntity(entity);
    }

    private void MouseClicked(InputAction.CallbackContext context) {
        Vector2 screenPosition = context.ReadValue<Vector2>();
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

    private void OnMovement(InputValue value) {
        movementValue = value.Get<Vector2>();
    }

    private void OnRotation(InputValue value) {
        Debug.Log(value.Get<float>());
        rotationValue = value.Get<float>();
    }
}

public struct ClickedPosition : IBufferElementData {
    public RaycastInput value;
}