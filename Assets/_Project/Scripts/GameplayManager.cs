using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;
    public PlayerController Player;
    public PlayerSkill[] Skills;
    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        Player.SetSkillInSlot(0, Skills[0]);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
