/*using UnityEngine.UI;
public class HealthController : MonoBehaviour
{
    private HealthDealModel healthDealModel;

    public HealthBarView healthBarView;

    private void Awake()
    {
        healthDealModel = new HealthDealModel() { Health = 100 };

        healthDealModel.OnHealthChange += healthBarView.SetHealth;
        
 

    }

    private void Start()
    {
        healthBarView.SetHealth(healthDealModel.Health);
    }

    public void TakeDamage()
    {
        healthDealModel.TakeDamage(damage);
    }
}
*/