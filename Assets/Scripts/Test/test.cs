using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class test : MonoBehaviour
{
    public string tag;
    GameObject[] testobj;
    [ContextMenu("Test Tag")]
    public void Test()
    {
        try
        {
            testobj = GameObject.FindGameObjectsWithTag(tag);
            Debug.Log("exist",this);
        } catch{
            Debug.Log("No exist",this);
        }
    }
}