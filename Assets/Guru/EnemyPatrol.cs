using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPatrol : MonoBehaviour
{
    public List<Transform> patrolPoints;     // Destinasi untuk patroli
    public float speed = 2f;                 // Kecepatan patroli
    public float detectionRange = 5f;        // Jarak penglihatan enemy
    public int damage = 1;                   // Damage yang diberikan kepada player
    public Collider safeZoneCollider;        // Collider untuk safe zone (gunakan Collider dengan isTrigger aktif)
    private int currentPointIndex = 0;       // Indeks titik patroli yang sedang dituju
    private Transform player;                // Referensi ke player
    private HealthManager healthManager;     // Referensi ke HealthManager
    private bool isIdle = false;             // Status apakah enemy sedang idle
    private bool isShouting = false;         // Status apakah enemy sedang berteriak
    private bool hasDealtDamage = false;     // Status apakah enemy sudah mengurangi nyawa player dalam deteksi ini
    private bool isPlayerInSafeZone = false; // Status apakah player berada di safe zone

    void Start()
    {
        if (patrolPoints.Count > 0)
        {
            transform.position = patrolPoints[currentPointIndex].position;
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthManager = player.GetComponent<HealthManager>();
    }

    void Update()
    {
        if (!isIdle && !isShouting)
        {
            Patrol();
        }
        
        DetectPlayer();
    }

    private void Patrol()
    {
        if (patrolPoints.Count == 0)
            return;

        Transform targetPoint = patrolPoints[currentPointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // Jika enemy sudah mencapai titik destinasi, mulai idle sebelum ke titik berikutnya
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            StartCoroutine(IdleBeforeNextPoint());
        }
    }

    private IEnumerator IdleBeforeNextPoint()
    {
        isIdle = true;  // Set enemy ke status idle
        yield return new WaitForSeconds(3f);  // Tunggu selama 3 detik

        // Pindah ke titik berikutnya setelah idle selesai
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
        isIdle = false;  // Kembali ke status patroli
    }

    private void DetectPlayer()
    {
        // Jika player berada di safe zone atau enemy sedang berteriak, tidak melakukan deteksi
        if (isPlayerInSafeZone || player == null || healthManager == null || isShouting)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Jika player dalam jarak penglihatan dan tidak di safe zone
        if (distanceToPlayer <= detectionRange)
        {
            if (!hasDealtDamage)
            {
                healthManager.TakeDamage(damage);  // Kurangi nyawa player
                hasDealtDamage = true;             // Tandai bahwa damage sudah diberikan
                StartCoroutine(ShoutCoroutine());  // Mulai teriakan selama 5 detik
            }
        }
    }

    private IEnumerator ShoutCoroutine()
    {
        isShouting = true;  // Set enemy ke status teriak
        Debug.Log("Enemy is shouting!");

        yield return new WaitForSeconds(5f);  // Waktu teriakan selama 5 detik

        Debug.Log("Enemy finished shouting.");
        isShouting = false;        // Kembali ke status normal untuk melanjutkan patroli
        hasDealtDamage = false;    // Reset status damage agar bisa mengurangi nyawa lagi saat mendeteksi player berikutnya
    }

    private void OnTriggerEnter(Collider other)
    {
        // Cek jika player masuk ke dalam safe zone
        if (other.CompareTag("Player") && other == player.GetComponent<Collider>())
        {
            isPlayerInSafeZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Cek jika player keluar dari safe zone
        if (other.CompareTag("Player") && other == player.GetComponent<Collider>())
        {
            isPlayerInSafeZone = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Menampilkan radius penglihatan di editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
