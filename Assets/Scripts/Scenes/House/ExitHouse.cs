using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitHouse : MonoBehaviour
{
    public void Exit()
    {
        EventBus.OnExitHouse.Invoke();
    }
}
