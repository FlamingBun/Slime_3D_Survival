using System.Collections;
using UnityEngine;

public interface Buff
{
    public void OnTriggerEnter(Collider other);

    public void OnTriggerExit(Collider other); 

    public IEnumerator GetBuff();
}
