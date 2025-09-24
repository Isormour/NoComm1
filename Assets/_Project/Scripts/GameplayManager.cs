using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;
    public PlayerController Player;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
