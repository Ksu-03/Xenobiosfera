using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f; // если есть бег
    public float gravity = -9.81f;
    public float jumpHeight = 1f;

    [Header("References")]
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;

    [Header("Movement Settings")]
    public bool canMove = true; // ЭТА ПЕРЕМЕННАЯ НУЖНА - используем её

    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!canMove) return; // если энергии нет - стоим на месте

        // Проверка земли
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Движение (простое, без бега для простоты)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * walkSpeed * Time.deltaTime);

        // Прыжок
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Гравитация
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}