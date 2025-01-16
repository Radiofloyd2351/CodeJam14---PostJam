using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinNature : MonoBehaviour
{
    public static bool natureWin;
    void Update()
    {
        // Check if all children are targeted
        if (AreAllChildrenTargeted()) {
            Progression.Win(Level.Nature);
            natureWin = true;
        }
        else
        {
            natureWin = false;
        }
    }

    private bool AreAllChildrenTargeted()
    {
        if(transform.childCount == 0)
            return true;

        return false;
    }

}
