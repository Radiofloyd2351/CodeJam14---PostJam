using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFlags : MonoBehaviour
{


    public void PlayWalkSound() {

        GameplaySoundManager.instance.PlayWalkSound();
    }
}
