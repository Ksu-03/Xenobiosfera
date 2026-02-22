using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Allian : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        if (dist > 5)
        {
            agent.SetDestination(player.position);
            GetComponent<Animator>().SetBool("Attack", false);
        }
        else
        {
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            GetComponent<Animator>().SetBool("Attack", true);
        }
    }
}
