using _Project.Scripts.Character;
using UnityEngine;

public class SkillData
{
    public SkillsController Owner;
    public SkillSlot SkillSlot;

    public StatisticsHolder StatisticsHolder
    {
        get
        {
            if (_statisticsHolder == null)
                _statisticsHolder = Owner.GetComponent<StatisticsHolder>();
            return _statisticsHolder;
        }
    }
    private StatisticsHolder _statisticsHolder;
}
