using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPatrol : MonoBehaviour
{
    public List<Transform> patrolPoints;     // Destinasi untuk patroli
    public float speed = 2f;                 // Kecepatan patroli
    public float detectionRange = 5f;        // Jarak penglihatan enemy
    public int damage = 1;                   // Damage yang diberikan kepada player
    public BoxCollider safeZoneCollider;     // Box Collider untuk safe zone (gunakan Collider dengan isTrigger aktif)
    private int currentPointIndex = 0;       // Indeks titik patroli yang sedang dituju
    private Transform player;                // Referensi ke player
    private HealthManager healthManager;     // Referensi ke HealthManager
    private bool isIdle = false;             // Status apakah enemy sedang idle
    private bool isShouting = false;         // Status apakah enemy sedang berteriak
    private bool hasDealtDamage = false;     // Status apakah enemy sudah mengurangi nyawa player dalam deteksi ini
    private bool isPlayerInSafeZone = false; // Status apakah player berada di safe zone
    public SafeZoneTrigger safeZoneTrigger;

    void Start()
    {
        if (patrolPoints.Count > 0)
        {
            transform.position = patrolPoints[currentPointIndex].position;
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthManager = player.GetComponent<HealthManager>();

        // Pastikan Box Collider safe zone sudah diatur dengan isTrigger = true
        if (safeZoneCollider != null)
        {
            safeZoneCollider.isTrigger = true;
        }
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
        if (player == null || healthManager == null || isShouting)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Cek jika player dalam jarak penglihatan dan status safe zone-nya
        if (distanceToPlayer <= detectionRange && !safeZoneTrigger.IsPlayerHidden() && !player.GetComponent<TPMovementRB>().IsInvincible())
        {
            if (!hasDealtDamage)
            {
                healthManager.TakeDamage(damage);
                hasDealtDamage = true;
                StartCoroutine(ShoutCoroutine());
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
            hasDealtDamage = false;   // Reset status damage agar tidak mengurangi nyawa selama player di safe zone
            isShouting = false;       // Pastikan enemy berhenti berteriak saat player di safe zone
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