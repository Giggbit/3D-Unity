using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CharacterControllerScript : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction runAction;
    private InputAction jumpAction;

    private Rigidbody rb;
    private Animator animator;
    private Transform cameraTransform;
    private Transform groundCheckedTransform;

    public LayerMask groundLayer;

    private float walkSpeed = 2f;
    private float runSpeed = 6f;
    private float rotationSpeed = 10f;
    private float jumpForce = 4f;
    private float fallSpeedThreshold = -5f;
    private float currentSpeed;
    private float targetSpeed;

    private float accelerationTime = 0.3f; // Время перехода от ходьбы к бегу
    private float animationDampTime = 0.1f; // Время сглаживания анимации

    private bool isJumping = false;
    private bool isRunning;
    private bool isGrounded;

    void Start() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        animator.applyRootMotion = false;
        moveAction = InputSystem.actions.FindAction("Move");
        runAction = InputSystem.actions.FindAction("Sprint");
        jumpAction = InputSystem.actions.FindAction("Jump");
        cameraTransform = Camera.main.transform;
        groundCheckedTransform = GameObject.Find("GroundCheck").GetComponent<Transform>();

        jumpAction.performed += ctx => TryJump();
    }

    void FixedUpdate() {
        MoveCharacter();
        HandleGroundCheck();
        HandleFallAnimation();
    }

    private void MoveCharacter() {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 inputDirection = new Vector3(moveInput.x, 0, moveInput.y);

        if (inputDirection.magnitude < 0.1f) {
            animator.SetFloat("speed", 0, animationDampTime, Time.deltaTime);
            return;
        }

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        bool isRunning = runAction.IsPressed();
        targetSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 moveDirection = (cameraForward * inputDirection.z + cameraRight * inputDirection.x).normalized;

        // Условие, предотвращающее резкие повороты, когда персонаж не двигается
        //if (moveDirection.sqrMagnitude > 0.01f) {
        //    Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        //}
        if (moveDirection.sqrMagnitude > 0.01f) {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            // Проверяем разницу углов, если она небольшая, не обновляем поворот
            if (Quaternion.Angle(transform.rotation, targetRotation) > 1f) {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        animator.SetFloat("speed", isRunning ? 1f : 0.5f, animationDampTime, Time.deltaTime);

        // Двигаем персонажа
        Vector3 newVelocity = moveDirection * targetSpeed;
        newVelocity.y = rb.linearVelocity.y; 
        rb.linearVelocity = newVelocity;
    }

    private void HandleGroundCheck() {
        // Проверяем, на земле ли персонаж
        bool wasGrounded = isGrounded; // Запоминаем прошлое состояние
        isGrounded = Physics.Raycast(groundCheckedTransform.position, Vector3.down, 0.5f, groundLayer);
        animator.SetBool("isInAir", !isGrounded);

        if (!wasGrounded && isGrounded) {
            isJumping = false; // Сбрасываем флаг прыжка ТОЛЬКО при приземлении
        }
    }

    private void HandleFallAnimation() {
        if (!isGrounded && rb.linearVelocity.y < fallSpeedThreshold) {
            animator.SetFloat("fallSpeed", Mathf.Abs(rb.linearVelocity.y));
            animator.SetBool("isFalling", true);
        }
        else {
            animator.SetBool("isFalling", false);
        }
    }

    private void TryJump() {
        if (isGrounded && !isJumping) {
            StartCoroutine(Jump());
        }
    }

    private IEnumerator Jump() {
        isJumping = true;
        animator.SetTrigger("jump");
        yield return new WaitForSeconds(0.15f);
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    }

}
