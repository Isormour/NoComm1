using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject prefab;       // Prefab przeciwnika
        public bool infiniteSpawn = false; // Czy ma siê respiæ w nieskoñczonoœæ
        public int maxInstances = 5;       // Maksymalna liczba instancji (jeœli nie infinite)
        public float spawnInterval = 2f;   // Czas pomiêdzy spawnami
    }

    [Header("Ustawienia spawnera")]
    public float triggerRadius = 10f;           // Promieñ aktywacji
    public List<EnemySpawnData> enemiesToSpawn; // Lista przeciwników do spawnienia
    public List<Transform> spawnPoints;                // Punkt spawnienia (jeœli null, u¿ywa pozycji spawnera)

    private bool playerInRange = false;
    private List<int> currentCounts; // liczba aktywnych instancji dla ka¿dego typu przeciwnika


    public UnityEvent OnStartEvent;
    public UnityEvent OnPlayerCloseAction;

    public List<GameObject> objs;

    public void SetEnable(bool state)
    {
        foreach (var item in objs)
        {
            if (item!=null)
            {
                item.SetActive(state);
            }
        }
    }

    void Start()
    {
        currentCounts = new List<int>(new int[enemiesToSpawn.Count]);
    }

    void Update()
    {
        if (!playerInRange)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, triggerRadius);
            foreach (Collider hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    playerInRange = true;
                    StartSpawning();
                    break;
                }
            }
        }
    }

    void StartSpawning()
    {
        for (int i = 0; i < enemiesToSpawn.Count; i++)
        {
            StartCoroutine(SpawnRoutine(i));
        }
    }

    IEnumerator SpawnRoutine(int index)
    {
        EnemySpawnData data = enemiesToSpawn[index];

        while (true)
        {
            // SprawdŸ limit
            if (data.infiniteSpawn || currentCounts[index] < data.maxInstances)
            {
                // Wybierz losowy spawnpoint, który nie jest nullem
                Transform point = null;
                List<Transform> validPoints = spawnPoints.FindAll(p => p != null);

                if (validPoints.Count > 0)
                    point = validPoints[Random.Range(0, validPoints.Count)];
                else
                    point = transform; // fallback

                GameObject newEnemy = Instantiate(data.prefab, point.position, Quaternion.identity);
                currentCounts[index]++;

                // Usuwanie z licznika po zniszczeniu
                EnemyDespawnTracker tracker = newEnemy.AddComponent<EnemyDespawnTracker>();
                tracker.onDestroyed += () => currentCounts[index]--;
            }

            yield return new WaitForSeconds(data.spawnInterval);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}

public class EnemyDespawnTracker : MonoBehaviour
{
    public System.Action onDestroyed;

    void OnDestroy()
    {
        onDestroyed?.Invoke();
    }
}
