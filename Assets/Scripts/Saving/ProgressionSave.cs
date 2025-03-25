using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionSave
{
    public List<string> killedBosses = new();
    public List<string> storyConditions = new();
    public SerializedDictionary<string, int> storyProgression = new();
}
