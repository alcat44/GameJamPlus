using UnityEngine;

public class SafeZoneTrigger : MonoBehaviour
{
    private bool isPlayerHidden = false; // Status apakah player sedang berjongkok di zona aman
    private TPMovementRB playerMovement; // Referensi ke skrip pergerakan player
    
    private void Start()
    {
        // Temukan player dan ambil referensi skrip TPMovementRB
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<TPMovementRB>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Jika yang memasuki trigger adalah player
        if (other.CompareTag("Player") && playerMovement != null)
        {
            // Periksa apakah player sedang berjongkok saat masuk ke zona aman
            if (Input.GetKey(KeyCode.LeftControl)) 
            {
                isPlayerHidden = true; // Player tersembunyi di safe zone saat crouch
            }
            else
            {
                isPlayerHidden = false; // Player terdeteksi karena tidak crouch
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Periksa terus-menerus apakah player berada di safe zone dan status crouch-nya
        if (other.CompareTag("Player") && playerMovement != null)
        {
            isPlayerHidden = Input.GetKey(KeyCode.LeftControl); // Tersembunyi jika crouch
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset status jika player keluar dari safe zone
        if (other.CompareTag("Player"))
        {
            isPlayerHidden = false;
        }
    }

    // Fungsi untuk memeriksa status player di zona aman
    public bool IsPlayerHidden()
    {
        return isPlayerHidden;
    }
}
