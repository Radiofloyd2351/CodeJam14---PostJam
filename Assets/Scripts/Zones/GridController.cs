using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GridController : MonoBehaviour {
    public GameObject normal;
    public GameObject tech;
    public GameObject nature;
    public GameObject hell;

    public void Update() {
        if (Progression.tech) {
            normal.SetActive(false);
            tech.SetActive(true);
        }
        if (Progression.nature) {
            tech.SetActive(false);
            nature.SetActive(true);
        }
        if (Progression.hell) {
            nature.SetActive(false);
            hell.SetActive(true);
        }

    }
}
