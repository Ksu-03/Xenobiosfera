using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Allian : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;
    Animator animator;
    PlayerStats playerStats; // ссылка на здоровье игрока

    [Header("Настройки атаки")]
    public float attackDistance = 2f;      // дистанция атаки (было 5 - слишком много)
    public float damageAmount = 10f;       // урон за удар
    public float attackCooldown = 1.5f;    // кулдаун между ударами

    [Header("Поиск игрока")]
    public float searchInterval = 0.5f;     // как часто искать игрока

    private float lastAttackTime;
    private float lastSearchTime;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        FindPlayer();
    }

    private void Update() // Используем Update для плавности
    {
        // Периодически ищем игрока
        if (Time.time > lastSearchTime + searchInterval)
        {
            FindPlayer();
            lastSearchTime = Time.time;
        }

        // Если игрока нет - ничего не делаем
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        // Отладка - видим дистанцию в консоли
        Debug.Log($"Дистанция до игрока: {dist}, атака на: {attackDistance}");

        if (dist > attackDistance)
        {
            // Бежим к игроку
            agent.isStopped = false;
            agent.SetDestination(player.position);
            if (animator != null) animator.SetBool("Attack", false);
        }
        else
        {
            // Останавливаемся и атакуем
            agent.isStopped = true; // заставляем остановиться
            agent.velocity = Vector3.zero; // сбрасываем скорость

            // Поворачиваемся к игроку
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            if (animator != null) animator.SetBool("Attack", true);

            // Наносим урон
            TryAttack();
        }
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerStats = playerObj.GetComponent<PlayerStats>(); // получаем компонент здоровья
        }
        else
        {
            player = null;
            playerStats = null;
        }
    }

    void TryAttack()
    {
        // Проверяем, можем ли атаковать (кулдаун)
        if (Time.time < lastAttackTime + attackCooldown) return;

        // Проверяем, есть ли у игрока здоровье
        if (playerStats != null && !playerStats.isDead)
        {
            // НАНОСИМ УРОН!
            playerStats.TakeDamage(damageAmount);
            lastAttackTime = Time.time;

            Debug.Log($"👾 Монстр АТАКУЕТ! Урон: {damageAmount}, Здоровье игрока: {playerStats.health}");

            // Можно добавить эффект удара
            // Instantiate(hitEffect, player.position, Quaternion.identity);
        }
    }

    // Визуализация дистанции атаки
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}