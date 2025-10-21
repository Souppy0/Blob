using System;
using UnityEngine;

 public class Gem : MonoBehaviour,Iitem
{
    public static event Action<int> OnGemCollect;
    [SerializeField] public int worth  = 5;
    public void Collect()
    {
        Destroy(gameObject);
        OnGemCollect.Invoke(worth);
    }

}
