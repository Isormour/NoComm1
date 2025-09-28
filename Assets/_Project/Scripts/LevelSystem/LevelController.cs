using UnityEngine;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    public UnityEvent OnLevelUp;
    public int Level { get; private set; } = 1;
    public float CurrentExp { get; private set; }
    public float ExpForNextLevel { get; private set; } = 100;
    public float LevelProgress =>  CurrentExp / ExpForNextLevel;


    public void AddExp(float exp)
    {
        CurrentExp += exp;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (CurrentExp >= ExpForNextLevel)
        {
            Level++;
            CurrentExp -= ExpForNextLevel;
            CheckLevelUp();
        }
    }
}
