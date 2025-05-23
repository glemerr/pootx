using UnityEditor;
using UnityEditor.Analytics;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
       
        }
    }
}
