using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class HealthAddToPlayerAndmana : MonoBehaviour
{
    public PlayableDirector XD;
    public bool opened = false;
    public float howMuchOfEach = 10f;

    public void OpenAndAdd()
    {
        if (opened) return;

        XD.Play();
        opened = true;
        PlayerAnchors.Instance.STATS.ChangeAmountHealth(howMuchOfEach);
        PlayerAnchors.Instance.STATS.ChangeAmountHealth(howMuchOfEach);
        StartSpawnRoutine();
    }




    [Header("Ustawienia")]
    public GameObject prefab;
    public GameObject prefab2;

    public int count = 10;
    public Vector3 spawnArea = new Vector3(0.3f, 0.3f, 0.3f);
    public float minSpeed = 3f;
    public float maxSpeed = 7f;
    public AnimationCurve CustomPosYCurve;

    public void StartSpawnRoutine()
    {
        StartCoroutine(SpawnObjectsRoutine());
    }

    IEnumerator SpawnObjectsRoutine()
    {
        for (int i = 0; i < count; i++)
        {
            StartCoroutine(SpawnAndMoveSingle());
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f)); // losowe opóŸnienie miêdzy spawnami
        }
    }

    IEnumerator SpawnAndMoveSingle()
    {
        // losowy punkt spawnu w okolicy
        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnArea.x, spawnArea.x),
            Random.Range(-spawnArea.y, spawnArea.y),
            Random.Range(-spawnArea.z, spawnArea.z)
        );
        GameObject obj;

        int xd =  UnityEngine.Random.Range(0, 2);
        if (xd == 1)
        {
            obj =  Instantiate(prefab, transform.position + randomOffset, Random.rotation);
        }

        else
        {
            obj = Instantiate(prefab2, transform.position + randomOffset, Random.rotation);

        }

        
        float speed = Random.Range(minSpeed, maxSpeed);
        float t = 0f;

        Vector3 startPos = obj.transform.position;
        Vector3 targetPos = PlayerAnchors.Instance.transform.position;

        Vector3 additiveYpos = Vector3.zero;
       

        // lerp w czasie
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            additiveYpos.y = CustomPosYCurve.Evaluate(t);
            obj.transform.position = Vector3.Slerp(startPos, PlayerAnchors.Instance.transform.position, t) + additiveYpos;
            yield return null;
        }

        yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        Destroy(obj);
    }
}



