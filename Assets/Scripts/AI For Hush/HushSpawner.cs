using System.Collections;
using UnityEngine;

public class HushSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] EnemyPrefabs;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private GameObject Player;

    private bool IsEnemyAlive;

    public float MinInterval = 10f;
    public float MaxInterval = 30f;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private void SpawnEnemy()
    {
        if (IsEnemyAlive)
            return;

        GameObject EnemyPrefab =
            EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)];

        Vector3 spawnPosition =
            PlayerCamera.transform.position +
            PlayerCamera.transform.forward * 5f;

        GameObject enemy = Instantiate(
            EnemyPrefab,
            spawnPosition,
            PlayerCamera.transform.rotation
        );

        HushAI hush = enemy.GetComponent<HushAI>();
        hush.Initialize(Player, PlayerCamera, this);

        IsEnemyAlive = true;
    }

    public void EnemyDied()
    {
        IsEnemyAlive = false;
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            float randomTime = Random.Range(MinInterval, MaxInterval);
            yield return new WaitForSeconds(randomTime);

            if (!IsEnemyAlive)
            {
                SpawnEnemy();
            }
        }
    }
}





/*using System.Collections;
using UnityEngine;

public class HushSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] EnemyPrefabs;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private GameObject Player;

    private bool IsEnemyAlive;

    public float MinInterval = 10f;
    public float MaxInterval = 30f;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private void SpawnEnemy()
    {
        if (IsEnemyAlive)
            return;

        GameObject EnemyPrefab =
            EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)];

        Vector3 spawnPosition =
            PlayerCamera.transform.position +
            PlayerCamera.transform.forward * 5f;

        GameObject enemy = Instantiate(
            EnemyPrefab,
            spawnPosition,
            PlayerCamera.transform.rotation
        );

        HushAI hush = enemy.GetComponent<HushAI>();
        hush.Initialize(Player, PlayerCamera);

        IsEnemyAlive = true;
    }

    private IEnumerator SpawnLoop()
    {
        float randomTime = Random.Range(MinInterval, MaxInterval);

        yield return new WaitForSeconds(randomTime);

        SpawnEnemy();
    }
}






/*using System.Collections;
using UnityEngine;

public class HushSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] EnemyPrefabs;
    [SerializeField] private Camera PlayerCamera;

    private bool IsEnemyAlive;


    public float MinInterval = 10f;
    public float MaxInterval = 30f;
    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private void SpawnEnemy()
    {
        if (IsEnemyAlive)
            return;
        GameObject EnemyPrefab = EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)];

        Vector3 spawnPosition = PlayerCamera.transform.position + PlayerCamera.transform.forward * 5f;


        Instantiate(EnemyPrefab, spawnPosition, PlayerCamera.transform.rotation);

        GameObject enemy = Instantiate(EnemyPrefab, spawnPosition, PlayerCamera.transform.rotation);

        HushAI hush = enemy.GetComponent<HushAI>();
        hush.Initialize(Player, PlayerCamera);

        IsEnemyAlive = true;
    }

     IEnumerator SpawnLoop()
    {
        float randomTime = Random.Range(MinInterval, MaxInterval);

        yield return new WaitForSeconds(randomTime);

        SpawnEnemy();
    }
}
*/