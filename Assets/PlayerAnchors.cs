using UnityEngine;

public class PlayerAnchors : MonoBehaviour
{
    public static PlayerAnchors Instance { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Animator animator;
    public GameObject rightShield;
    public GameObject leftShield;
    public StatisticsHolder STATS;

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
