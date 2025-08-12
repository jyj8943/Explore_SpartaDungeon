using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("이동 관련")] 
    public float moveSpeed;
    private Vector2 curMovementInput;
    public float jumpPower;
    public LayerMask groundLayerMask;
    public float runSpeed;
    private bool isRunning = false;
    public float runStamina;

    [Header("카메라 관련")] 
    public Transform cameraContainer;
    public Transform ThirdPersonCameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private bool isFirstPerson = true;

    private Vector2 mouseDelta;

    private Rigidbody _rigidbody;

    [HideInInspector] public bool canLook = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    { 
        Move();
    }

    private void LateUpdate()
    {
        if (canLook && isFirstPerson)
        {
            cameraContainer.gameObject.SetActive(true);
            ThirdPersonCameraContainer.gameObject.SetActive(false);
            CameraLook();
        }
        else if (canLook && !isFirstPerson)
        {
            cameraContainer.gameObject.SetActive(false);
            ThirdPersonCameraContainer.gameObject.SetActive(true);
            ThirdPersonCameraLook();
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;

        // isRunning이 true, 스태미나가 비어 있지 않다면 달리도록 구현
        if (isRunning && CharacterManager.Instance.Player.condition.CanUseStamina(5))
        {
            dir *= runSpeed;
            if (dir.magnitude > 0)
            {
                CharacterManager.Instance.Player.condition.DecreaseStamina(runStamina);
            }
        }
        else
        {
            isRunning = false;
            dir *= moveSpeed;
        }
        
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }
    
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    // shift를 통해 달리기 기능 추가
    public void OnRunInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isRunning = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isRunning = false;
        }
    }

    // 카메라 시점을 바꾸는 메서드
    public void OnChangePerspective(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (isFirstPerson)
            {
                isFirstPerson = false;
            }
            else
            {
                isFirstPerson = true;
            }
        }
    }
    
    private bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    private void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    private void ThirdPersonCameraLook()
    {
        Vector2 mouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = ThirdPersonCameraContainer.rotation.eulerAngles;

        float x = camAngle.x - mouse.y;
        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 45f);
        }
        else
        {
            x = Mathf.Clamp(x, 320f, 361f);
        }
        
        transform.rotation = Quaternion.Euler(x, camAngle.y + mouse.x, camAngle.z);
    }

    public void PlayerBoost(float value)
    {
        moveSpeed *= value;
        runSpeed *= value;
    }

    public void PlayerBoostEnd(float value)
    {
        moveSpeed /= value;
        runSpeed /= value;
    }
}
