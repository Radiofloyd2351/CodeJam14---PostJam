using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{

    private Dictionary<int, Coroutine> coroutines = new();

    public GameObject indicator;
    public SpriteRenderer tester;

    public static CoroutineManager instance;
    void Start()
    {
        tester = indicator.GetComponent<SpriteRenderer>();
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

    public Coroutine RunCoroutine(IEnumerator routine) {
        return StartCoroutine(routine);
    }
    public Coroutine RunCoroutine(IEnumerator routine, int id) {
        if (coroutines.ContainsKey(id)) {
            if (coroutines[id] != null) {
                StopCoroutine(coroutines[id]);
            }
            coroutines[id] = StartCoroutine(routine);
        } else {
            coroutines.Add(id, StartCoroutine(routine));
        }
        return coroutines[id];
    }

    public void CancelCoroutine(int id) {
        if (coroutines.ContainsKey(id) && coroutines[id] != null) {
            StopCoroutine(coroutines[id]);
            coroutines.Remove(id);
        }
    }
}
