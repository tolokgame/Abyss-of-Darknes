/*using System;
using UnityEngine;

public class HealthDealModel : MonoBehaviour
{
    public float Health {get; set; }

    public event Action<float> OnHealthChange;

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if(Health < 0)
        {
            Health = 0;
        }

        OnHealthChange?.Invoke(Health);
    }
}
*/