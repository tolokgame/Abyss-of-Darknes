using UnityEngine;

public class EnemyInstantiate : MonoBehaviour
{
    public GameObject enemy; // Перетягни вимкненого ворога зі сцени

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.SetActive(true);
            gameObject.SetActive(false); // Вимикаємо тригер, щоб спрацював лише раз
        }
    }
}
