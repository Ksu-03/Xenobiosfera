using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 1f;

    [Header("References")]
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;

    [Header("Energy")]
    public bool canRun = true;

    private Vector3 velocity;
    private bool isGrounded;
    private float currentSpeed;
    private PlayerStats stats;

    void Start()
    {
        // Если контроллер не назначен в инспекторе, пытаемся получить его компонент
        if (controller == null)
            controller = GetComponent<CharacterController>();

        // Получаем компонент PlayerStats на этом же объекте
        stats = GetComponent<PlayerStats>();

        // Проверяем, все ли необходимые компоненты есть
        if (controller == null)
            Debug.LogError("PlayerController: CharacterController не найден!");

        if (stats == null)
            Debug.LogError("PlayerController: PlayerStats не найден!");
    }

    void Update()
    {
        // Проверка, стоит ли игрок на земле
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundMask);

        // Если на земле и падаем вниз - останавливаем падение
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Получаем ввод от игрока (WASD или стрелки)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Проверка на бег (Shift + движение вперед)
        bool runPressed = Input.GetKey(KeyCode.LeftShift) && canRun;

        if (runPressed && z > 0) // бежим только вперед
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        // Передаем в PlayerStats информацию, бежит ли игрок
        if (stats != null)
        {
            stats.isRunning = (runPressed && z > 0);
        }

        // Движение в стороны и вперед/назад
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Прыжок
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Применяем гравитацию
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}

