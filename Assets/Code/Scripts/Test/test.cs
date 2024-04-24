using UnityEngine;
using QuantumRevenant.PixelsinTheSpace;
    
public class test : MonoBehaviour
{
    public PostMortemBulletAction tag;
    GameObject[] testobj;
    [ContextMenu("Test")]
    public void Test()
    {
        PostMortemBulletAction postMortem = tag;

        if (postMortem.HasFlag(PostMortemBulletAction.Explode))
            Debug.Log("explode()");
        if (postMortem.HasFlag(PostMortemBulletAction.Summon))
            Debug.Log("summon()");

        Debug.Log("gameObject.SetActive(false);");
    }
}