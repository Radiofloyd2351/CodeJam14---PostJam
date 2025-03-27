using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<Instrument, InventoryInstrument> instruments = new();
    //private List<InstrumentInfo> instruments = new List<InstrumentInfo>();
    //public List<InstrumentInfo> Instruments { get { return instruments; } }

    [SerializeField] private GameObject draggableImageTemplate;
    [SerializeField] private Transform instrumentCollection;
    [SerializeField] private float positionModifier;

    public InventoryInstrument selectedInstrument;

    //public void Push(InstrumentInfo instrument) {  instruments.Add(instrument); }

    public static Inventory instance;
    private void Start() {
        instance = this;
    }
    public void Build() {
        int i = 0;
        foreach (KeyValuePair<Instrument,InventoryInstrument> instrument in instruments) {
            Destroy(instrument.Value);
            instruments.Remove(instrument.Key);
        }
        foreach (string instrumentText in Saver.instance.saveDict["save"].inventory.instruments) {
            if (System.Enum.TryParse(instrumentText, out Instrument instrument)) {
                // Push(DefaultValues.instance.instrumentInfoTypeTemplates[instrument]);
                GameObject newImage = Instantiate(draggableImageTemplate, instrumentCollection);
                newImage.GetComponent<UnityEngine.UI.Image>().sprite = DefaultValues.instance.GetInfoType(instrument).image;
                newImage.GetComponent<InventoryInstrument>().info = DefaultValues.instance.GetInfoType(instrument);
                //newImage.transform.position += new Vector3(i * positionModifier,0f,0f);
                instruments.Add(instrument, newImage.GetComponent<InventoryInstrument>());
            }
            i++;
        }
    }
}
