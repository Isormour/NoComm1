using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckDistance", story: "Check [distance] between [gameobject1] and [gameobject2]", category: "Action", id: "0f9040be64b80f178092640c3191856e")]
public partial class CheckDistanceAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Distance;
    [SerializeReference] public BlackboardVariable<GameObject> Gameobject1;
    [SerializeReference] public BlackboardVariable<GameObject> Gameobject2;

    protected override Status OnStart()
    {
        if (Gameobject1.Value == null || Gameobject2.Value == null)
        {
            return Status.Failure;
        }

        else
        {
            float xd = Vector3.Distance(Gameobject1.Value.transform.position, Gameobject2.Value.transform.position);
            Distance.Value = xd;
            return Status.Success;
        }
    }
}

