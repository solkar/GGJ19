using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterHouse : MonoBehaviour
{
    public void Enter()
    {
        EventBus.OnEnterHouse.Invoke();
    }
}
