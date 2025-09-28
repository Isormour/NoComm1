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


        for (int i = 0; i < Skills.Length; i++)
        {
            Player.SetSkillInSlot(i, Skills[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
