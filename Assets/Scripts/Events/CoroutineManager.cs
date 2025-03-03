using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{

    private Dictionary<int, Coroutine> coroutines = new();

    public static CoroutineManager instance;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RunCoroutine(IEnumerator routine) {
        StartCoroutine(routine);
    }
    public void RunCoroutine(IEnumerator routine, int id) {
        if (coroutines.ContainsKey(id)) {
            StopCoroutine(coroutines[id]);
            coroutines[id] = StartCoroutine(routine);
        } else {
            coroutines.Add(id, StartCoroutine(routine));
        }
    }

    public void CancelCoroutine(int id) {
        if (coroutines.ContainsKey(id)) {
            StopCoroutine(coroutines[id]);
            coroutines.Remove(id);
        }
    }
}
