using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerItemFader : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        ItemFader[] fades = other.GetComponentsInChildren<ItemFader>();
        if(fades.Length>0)
        {
            foreach (var item in fades)
            {
                item.FadeOut();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        ItemFader[] fades = other.GetComponentsInChildren<ItemFader>();
        if(fades.Length>0)
        {
            foreach (var item in fades)
            {
                item.FadeIn();
            }
        }
    }
}
