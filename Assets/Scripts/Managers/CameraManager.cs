using UnityEngine;
using UnityEngine.InputSystem;

//Helpful tutorial: https://www.youtube.com/watch?v=pJQndtJ2rk0&ab_channel=CodeMonkey ;
//TODO: Add click drag rotation;
//TODO: Add click drag movement;
//TODO: Add edge scroll maybe;
//TODO: Add zooming;

public class CameraManager: MonoBehaviour {
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private Transform camerFocus;

    public static CameraManager Instance { get; private set; }
    private Vector2 movementValue;
    private float rotationValue;

    private void OnAwake() {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);
        DontDestroyOnLoad(this);
    }

    private void OnEnable() {
        if (camera == null) camera = Camera.main;
    }

    private void Update() {
        Vector3 movememtDirection = camerFocus.forward * movementValue.y + camerFocus.right * movementValue.x;
        float movementSpeed = 50F;
        camerFocus.position += movememtDirection * movementSpeed * Time.deltaTime;
        camerFocus.eulerAngles += new Vector3(0, rotationValue * movementSpeed * Time.deltaTime, 0);
    }

    private void OnMove(InputValue value) {
        movementValue = value.Get<Vector2>();
    }

    private void OnRotate(InputValue value) {
        rotationValue = value.Get<float>();
    }
}