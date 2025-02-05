using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public bool VerifyTarget(GameObject target)
    {
        if(target == null) return false;
        Debug.Log("SHIT IM HIT!!!!");
        return true;
    }

    public IEnumerator HitThis(float stunTime) {
        GetComponent<TopDownCharacterController>().DisableControls();
        yield return new WaitForSeconds(stunTime);
        GetComponent<TopDownCharacterController>().EnableControls();
    }
}
