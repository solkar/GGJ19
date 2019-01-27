using System.Collections.Generic;
using UnityEngine;

public class Services : Singleton<Services>
{
    public List<Enemies.Enemy> enemies;
    
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Instantiate(Resources.Load<Services>("Services"));
    }
}
