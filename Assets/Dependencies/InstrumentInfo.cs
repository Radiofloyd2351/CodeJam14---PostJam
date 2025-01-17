using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentInfo : MonoBehaviour
{
    [SerializeField] public ModeIndicator indicator;
    [SerializeField] public GameObject grid;
    [SerializeField] public RuntimeAnimatorController controller;
    [SerializeField] public string instrumentName;
    [SerializeField] public Instrument type;
}
