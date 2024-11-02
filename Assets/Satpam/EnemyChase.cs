using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    public static bool isChasing = false;  // Static variabel untuk status pengejaran

    public float attackRange = 2.0f;         // Jarak serang musuh
    public float attackCooldown = 1.5f;      // Jeda antar serangan
    private float nextAttackTime = 0;        // Waktu serangan berikutnya

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (isChasing)
        {
            agent.SetDestination(player.position);

            // Cek jarak antara musuh dan player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
            {
                AttackPlayer();
                nextAttackTime = Time.time + attackCooldown;  // Atur jeda serangan berikutnya
            }
        }
    }

    public void StartChasingPlayer()
    {
        isChasing = true;  // Set pengejaran mulai
    }

    private void AttackPlayer()
    {
        HealthManager playerHealth = player.GetComponent<HealthManager>();
        if (playerHealth != null)
        {
            playerHealth.GameOver();  // Langsung memanggil metode GameOver
        }
    }
}