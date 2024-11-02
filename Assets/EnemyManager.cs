using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    public bool isAnyEnemyChasing { get; set; } = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Membuat instance ini tetap ada antar scene
        }
        else
        {
            Destroy(gameObject);  // Menghindari duplikasi instance
        }
    }
}
