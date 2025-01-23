using UnityEngine;
using UnityEngine.UI;

public class IndicationsText : MonoBehaviour
{
    private Text _controlIndicationText = null;
    [SerializeField] private string StandaloneTextIndication = null;
    [SerializeField] private string MobileTextIndication = null;

    private void Start()
    {
        _controlIndicationText = GetComponent<Text>();

#if UNITY_STANDALONE
        _controlIndicationText.text = StandaloneTextIndication;
#else
        _controlIndicationText.text = MobileTextIndication;
#endif
    }
}
