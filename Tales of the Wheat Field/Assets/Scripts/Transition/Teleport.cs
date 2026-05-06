using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Vector3 pos_V3;
    [SceneName]
    public string posName_String;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            EventHandler.CallTransitionEvent(posName_String, pos_V3);
        }
    }
}
