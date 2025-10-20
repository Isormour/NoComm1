using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;

public class ThrowedShield : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    public Vector3 initialDirection;
    

    public float timeFlying;
    public bool HitSomething;
    public bool HitEnemy;
    public float force;
    public bool commingBack = false;
    public AnimationCurve CustomShit;
    public Rigidbody rb;
    public InsideSphere beka;
    public Collider colider;
    public float toEnemyForce;

    private void Awake()
    {
        initialDirection = PlayerAnchors.Instance.transform.forward + Vector3.up/5f;
        rb.transform.position = PlayerAnchors.Instance.rightShield.transform.position;
        colider.enabled = false;
        rb.AddForce(initialDirection * force, ForceMode.VelocityChange);
        StartCoroutine(delayXD());
    }

    IEnumerator delayXD()
    {
        yield return new WaitForSeconds(0.2f);

        colider.enabled = true;



    }

    void FixedUpdate()
    {
        timeFlying += Time.deltaTime;


        if (timeFlying > 3f && !commingBack)
        {
            StartCoroutine(ComeBackShield());
        }

        else
        {
            if (HitSomething)
            {
                return;
            }
            // Szukamy najbli�szego przeciwnika
            GameObject closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (GameObject xd in beka.objectsInside)
            {
                if (xd == null) continue; // zabezpieczenie przed nullami

                if (xd.CompareTag("Enemy"))
                {

                    if (!xd.GetComponent<StatisticsHolder>().IsDead)
                    {
                        float dist = Vector3.Distance(transform.position, xd.transform.position);
                        if (dist < closestDistance)
                        {
                            closestDistance = dist;
                            closestEnemy = xd;
                        }
                    }

                }
            }

            if (closestEnemy != null)
            {
                Debug.Log("napierdalam");
                // Kierunek w stron� przeciwnika
                Vector3 dir = (closestEnemy.transform.position + Vector3.up - transform.position).normalized;

                // Je�li masz Rigidbody:
                if (rb != null)
                {
                    rb.linearVelocity *= 0.92f;
                    rb.AddForce(dir * toEnemyForce, ForceMode.Force); // 'force' � public float
                }
                else
                {
                    // bez fizyki � po prostu poruszamy transformem
                    //transform.position += dir * speed * Time.deltaTime;
                }
            }
            else
            {
                // Brak przeciwnika � lecimy prosto
                //transform.position += initialDirection * speed * Time.deltaTime;
            }

            Vector3 desiredUp = Vector3.up;
            Vector3 currentUp = transform.up;

            // Kierunek momentu potrzebny, by dopasować orientację
            Vector3 torqueToUpright = Vector3.Cross(currentUp, desiredUp);

            // Siła momentu (reguluj w zależności od masy / stabilności)
            float uprightStrength = 8f;

            rb.AddTorque(torqueToUpright * uprightStrength, ForceMode.Acceleration);

            // --- 2. Obrót wokół lokalnej osi Z ---
            float rotationSpeed = 1800f; // stopni/s
            float angularVelocity = rotationSpeed * Mathf.Deg2Rad;

            rb.transform.rotation *= Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f);


            // --- 1. Prostowanie osi "up" do góry ---

            // Obrót w lokalnej osi Z (transform.forward = oś Z lokalna)
            //rb.AddTorque(transform.forward * angularVelocity, ForceMode.Acceleration);


        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        


        if (collision.gameObject.CompareTag("Enemy"))
        {

            StatisticsHolder enemy = collision.gameObject.GetComponent<StatisticsHolder>();
            DamageData damageData = new DamageData()
            {
                Damage = 25f,
                DamageSourcePosition = transform.position,
                Target = enemy.transform,
                Owner = PlayerAnchors.Instance.transform,
            };
            enemy.TakeDamage(damageData);
            HitEnemy = true;
            StartCoroutine(ComeBackShield());
        }

        else
        {
            HitSomething = true;
        }

            Debug.Log(collision.gameObject.name, collision.gameObject);
        rb.useGravity = true;
    }

    public IEnumerator ComeBackShield()
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;


        float duration = 2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            transform.position = Vector3.Slerp(startPos, PlayerAnchors.Instance.rightShield.transform.position + (Vector3.up*(25f*(1f-t))), CustomShit.Evaluate(t));
            if (t>0.3f)
            {
                transform.rotation = Quaternion.Slerp(startRot, PlayerAnchors.Instance.rightShield.transform.rotation, CustomShit.Evaluate(t));
            }
            

            yield return null;
        }

        // Upewnij si�, �e ko�czy dok�adnie w celu
        transform.position = PlayerAnchors.Instance.rightShield.transform.position;
        transform.rotation = PlayerAnchors.Instance.rightShield.transform.rotation;
    }
}
