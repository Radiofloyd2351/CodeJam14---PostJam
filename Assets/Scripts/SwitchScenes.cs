using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{

    public GameObject player;
    public void SwitchScenes1() {
        PlayerStats.playerPos = player.transform.position;
        SceneManager.LoadScene(sceneBuildIndex:1);
        PlayerStats.transitionning = true;
    }

    public void SwitchScenes2() {
        SceneManager.LoadScene(sceneBuildIndex: 0);
        PlayerStats.transitionning = false;
    }
}
