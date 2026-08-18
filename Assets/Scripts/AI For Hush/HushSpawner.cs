using System.Collections;
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

        IsEnemyAlive = true;
    }

     IEnumerator SpawnLoop()
    {
        float randomTime = Random.Range(MinInterval, MaxInterval);

        yield return new WaitForSeconds(randomTime);

        SpawnEnemy();
    }
}
