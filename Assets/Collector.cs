using UnityEngine;

public class Collector : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Iitem item = collision.GetComponent<Iitem>();
        if(item !=null)
        {
            item.Collect();
        }
    }
}
