using UnityEngine;

public class Services : Singleton<Services>
{
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Instantiate(Resources.Load<Services>("Services"));
    }
}
