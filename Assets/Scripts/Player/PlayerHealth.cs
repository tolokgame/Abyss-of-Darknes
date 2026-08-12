using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int Health = 1;

    public void TakeDamage()
    {
        if (Health <= 0)
        {
            Death();
        }
    }
    private void  Death()
    {
        Destroy(gameObject);
    }
}
