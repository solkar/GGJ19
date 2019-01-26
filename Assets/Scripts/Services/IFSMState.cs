using UnityEngine;
using System.Collections;

public interface IFSMState
{
    IEnumerator Enter();
    void Exit();
}
