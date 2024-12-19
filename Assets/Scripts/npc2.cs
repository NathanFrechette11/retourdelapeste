using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc2 : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        // quand le npc2 entre en contact avec le fromage qui est posé...
        if (other.gameObject.CompareTag("fromagePosé"))
        {
            // on detruit ce npc2
            Destroy(this.gameObject);
        }
    }
}
