using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class UnitDisplayController : MonoBehaviour
{
    public int counter;
    
    private List<Transform> _childrenList = new List<Transform>();
    private void Awake()
    {
        transform.GetComponentsInChildren<Transform>(true,_childrenList);
        
        _childrenList.Remove(transform);
        
        Assert.IsTrue(_childrenList.Count > 0);
    }

    void Update()
    {
        counter = Mathf.Min(counter, _childrenList.Count);
        
        for (int i = 0; i < _childrenList.Count; i++)
        {
            if (i < counter)
            {
                _childrenList[i].gameObject.SetActive(true);
            }
            else
            {
                _childrenList[i].gameObject.SetActive(false);
            }
        }

    }
}
