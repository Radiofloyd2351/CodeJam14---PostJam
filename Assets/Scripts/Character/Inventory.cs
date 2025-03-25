using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<InstrumentInfo> instruments = new List<InstrumentInfo>();
    public List<InstrumentInfo> Instruments { get { return instruments; } }

    public void Push(InstrumentInfo instrument) {  instruments.Add(instrument); }

}
