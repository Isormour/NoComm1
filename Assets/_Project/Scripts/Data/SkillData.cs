using UnityEngine;

public class SkillData
{
    public Transform Owner;

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
