using System;
using StarterAssets;
using UnityEngine;

public class CheckPointsManager : MonoBehaviour
{
    [SerializeField] private GameObject CurrentCheckPoint;
    private GameObject checpointInRange;
    private StatisticsHolder playerStats;
    private bool IsPlayerinRange = false;

    void Awake()
    {
        playerStats = GetComponent<StatisticsHolder>();
    }

    public void RespawnPlayer()
    {
        if (CurrentCheckPoint != null)
        {
            GameObject player = this.gameObject;
            if (player != null)
            {
                player.transform.position = CurrentCheckPoint.transform.position + new Vector3(3, 0, 3);
                ThirdPersonController controller = player.GetComponent<ThirdPersonController>();
                controller.enabled = true;
                controller.Respawn();
            }
        }
    }
    

    void Update()
    {
        if (IsPlayerinRange && Input.GetKeyDown(KeyCode.Q))
        {   
            SetCurrentCheckPoint();
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ChecPoint"))
        {
            checpointInRange = other.gameObject;
            IsPlayerinRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ChecPoint"))
        {
            checpointInRange = null;
            IsPlayerinRange = false;
        }
    }

    internal void SetCurrentCheckPoint()
    {
        if(checpointInRange != null) CurrentCheckPoint = checpointInRange;
    }
}
