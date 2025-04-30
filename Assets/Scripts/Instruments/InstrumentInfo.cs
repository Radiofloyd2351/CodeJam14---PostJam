using UnityEngine;
using System;

[CreateAssetMenu(menuName = "New Instrument")]
public class InstrumentInfo : ScriptableObject {
    [SerializeField] private string instrumentName;
    [SerializeField] private string description;
    [SerializeField] public Sprite image;
    [SerializeField] public Sprite activeIndicator;
    [SerializeField] public Sprite inactiveIndicator;
    [SerializeField] private int rarity;
    [SerializeField] public Instrument type;
}
