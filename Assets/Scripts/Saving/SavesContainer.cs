using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SavesContainer
{
    public InventorySave inventory = new();
    public ProgressionSave progression = new();
    public CheckPointSave checkPoint = new();
}
