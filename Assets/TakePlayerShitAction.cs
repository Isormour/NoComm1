using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TakePlayerShit", story: "Get [Player]", category: "Action", id: "f42123a0123e83d2eddfa63828528956")]
public partial class TakePlayerShitAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Player;

    protected override Status OnStart()
    {
        Player.Value = PlayerAnchors.Instance.gameObject;
        return Status.Running;
    }


}

