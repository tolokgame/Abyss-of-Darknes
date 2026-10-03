
using System.Collections;
using UnityEngine;

public class HushAI : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Camera PlayerCamera;

    private HushSpawner spawner;

    public float MinIntervalOfLife = 5f;
    public float MaxIntervalOfLife = 7f;

    [SerializeField] private float viewAngle = 45f;

    private float KillTimer = 0f;
    public float KillWaitingTime = 5f;

    private void Start()
    {
        StartCoroutine(LifePeriod());
    }

    private void Update()
    {
        if (!IsInView())
        {

            if (PlayerCamera == null)
                return;

            KillTimer += Time.deltaTime;

            if (KillTimer >= KillWaitingTime)
            {
                DealDamage();
            }
        }
    }

    public void Initialize(
        GameObject player,
        Camera camera,
        HushSpawner spawner)
    {
        this.player = player;
        PlayerCamera = camera;
        this.spawner = spawner;
    }

    private bool IsInView()
    {
        Vector3 directionToEnemy =
            (transform.position - PlayerCamera.transform.position).normalized;

        float angle = Vector3.Angle(
            PlayerCamera.transform.forward,
            directionToEnemy
        );

        if (angle < viewAngle)
        {
            float distanceToEnemy =
                Vector3.Distance(
                    PlayerCamera.transform.position,
                    transform.position
                );

            if (Physics.Raycast(
                PlayerCamera.transform.position,
                directionToEnemy,
                out RaycastHit hit,
                distanceToEnemy))
            {
                if (hit.transform == transform)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void DealDamage()
    {
        PlayerHealth playerHealth =
            player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage();
        }
    }

    private void Die()
    {
        spawner.EnemyDied();
        Destroy(gameObject);
    }

    private IEnumerator LifePeriod()
    {
        float randomTime =
            Random.Range(MinIntervalOfLife, MaxIntervalOfLife);

        yield return new WaitForSeconds(randomTime);

        Die();
    }
}









/*using System.Collections;
using UnityEngine;

public class HushAI : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Camera PlayerCamera;

    public float MinIntervalOfLife = 5f;
    public float MaxIntervalOfLife = 7f;

    [SerializeField] private float viewAngle = 45f;

    private float KillTimer = 0f;
    public float KillWaitingTime = 5f;

    private void Start()
    {
        StartCoroutine(LifePeriod());
    }

    private void Update()
    {
        if (!IsInView())
        {
            KillTimer += Time.deltaTime;
            if (KillTimer >= KillWaitingTime)
            {
                DealDamage();
            }
        }
    }

    public void Initialize(GameObject player, Camera camera)
    {
        this.player = player;
        PlayerCamera = camera;
    }
    /*private void Attack()
    {
        Vector3 killPosition = PlayerCamera.transform.position + PlayerCamera.transform.forward;

    }
    
    private bool IsInView()
    {
        Vector3 directionToEnemy = (transform.position - PlayerCamera.transform.position).normalized;


        float angle = Vector3.Angle(PlayerCamera.transform.forward, directionToEnemy);

        if (angle < viewAngle)
        {
            float distanceToEnemy = Vector3.Distance(PlayerCamera.transform.position, transform.position);


            if (Physics.Raycast(PlayerCamera.transform.position, directionToEnemy, out RaycastHit hit, distanceToEnemy))
            {

                if (hit.transform == transform)
                {
                    return true;
                }
            }
        }

        return false;

        /*Vector3 DirectionToEnemy = (transform.position - PlayerCamera.transform.position).normalized;

        float angle = Vector3.Angle(PlayerCamera.transform.forward, DirectionToEnemy);

        return angle < viewAngle;
        
    }


    public void DealDamage()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator LifePeriod()
    {
        float randomTime = Random.Range(MinIntervalOfLife, MaxIntervalOfLife);

        yield return new WaitForSeconds(randomTime);

        Die();
    }
}





/*using System.Collections;
using UnityEngine;

public class HushAI : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Camera PlayerCamera;

    public float MinIntervalOfLife = 5f;
    public float MaxIntervalOfLife = 7f;

    [SerializeField] private float viewAngle = 45f;

    private float KillTimer = 0f;
    public float KillWaitingTime = 5f;

    private void Start()
    {
        StartCoroutine(LifePeriod());
    }

    private void Update()
    {
        if(! IsInView())
        {
            KillTimer += Time.deltaTime;
            if (KillTimer >= KillWaitingTime)
            {
                DealDamage();
            }
        }
    }

    public void Initialize(GameObject player, Camera camera)
    {
        this.player = player;
        PlayerCamera = camera;
    }
    private void Attack()
    {
        Vector3 killPosition = PlayerCamera.transform.position + PlayerCamera.transform.forward;

    }
    
    private bool IsInView()
    {
        Vector3 directionToEnemy = (transform.position - PlayerCamera.transform.position).normalized;

      
        float angle = Vector3.Angle(PlayerCamera.transform.forward, directionToEnemy);

        if (angle < viewAngle)
        {
            float distanceToEnemy = Vector3.Distance(PlayerCamera.transform.position, transform.position);

          
            if (Physics.Raycast(PlayerCamera.transform.position, directionToEnemy, out RaycastHit hit, distanceToEnemy))
            {
            
                if (hit.transform == transform)
                {
                    return true; 
                }
            }
        }

        return false;

        Vector3 DirectionToEnemy = (transform.position - PlayerCamera.transform.position).normalized;

        float angle = Vector3.Angle(PlayerCamera.transform.forward, DirectionToEnemy);

        return angle < viewAngle;
        
    }


    public void DealDamage()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator LifePeriod()
    {
        float randomTime = Random.Range(MinIntervalOfLife, MaxIntervalOfLife);

        yield return new WaitForSeconds(randomTime);

        Die();
    }
}

*/