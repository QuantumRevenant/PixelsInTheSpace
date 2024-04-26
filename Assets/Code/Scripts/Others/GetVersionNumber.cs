using System.Collections;
using System.Collections.Generic;
using QuantumRevenant.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class GetVersionNumber : MonoBehaviour
{
    [SerializeField]
    private string versionNumber;
    [SerializeField]
    private Text textElement;
    // Start is called before the first frame update
    void Start()
    {
        versionNumber = Application.version;
        Debug.Log("Actual Version Number: " + Utility.GetVersion(),this);
        textElement.text = Utility.GetVersion();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
