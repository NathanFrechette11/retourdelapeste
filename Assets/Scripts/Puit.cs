using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puit : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
	// quand le puit rentre en collision avec le sol...
        if (collision.gameObject.CompareTag("sol"))
        {
	    // on accede au rigidbody du puit
            Rigidbody rigidbody = GetComponent<Rigidbody>();
	    // le tag du puit passe de "puit" à "puitPosé"
            this.gameObject.tag = "puitPosé";

	    // on indique aux informations du monde que l'item a été possé
            InfosMonde.instance.itemPosee = true;
        }

	// si ce puit a comme tag "puit" et quil touche a un chariot qui a été posé...
        if (collision.gameObject.CompareTag("chariotPosé") && this.gameObject.tag == "puit")
        {
	    // on detruit ce puit
            Destroy(this.gameObject);
        }
	// sinon si ce puit a comme tag "puit" et quil touche a un puit qui a été posé...
        else if (collision.gameObject.CompareTag("puitPosé") && this.gameObject.tag == "puit")
        {
	    // on detruit ce puit
            Destroy(this.gameObject);
        }
	// sinon si ce puit a comme tag "puit" et quil touche a un fromage qui a été posé...
        else if (collision.gameObject.CompareTag("fromagePosé") && this.gameObject.tag == "puit")
        {
	    // on detruit ce puit
            Destroy(this.gameObject);
        }
	// sinon si ce puit a comme tag "puit" et quil touche a une fleur qui a été posée...
        else if (collision.gameObject.CompareTag("fleurPosé") && this.gameObject.tag == "puit")
        {
	    // on detruit ce puit
            Destroy(this.gameObject);
        }
	// sinon si ce puit a comme tag "puit" et quil touche a un puit pas posé...
        else if (collision.gameObject.CompareTag("puit") && this.gameObject.tag == "puit")
        {
	    // on detruit ce puit
            Destroy(this.gameObject);
        }
	// sinon si ce puit a comme tag "puit" et quil touche a un chariot pas posé...
        else if (collision.gameObject.CompareTag("chariot") && this.gameObject.tag == "puit")
        {
	    // on detruit ce puit
            Destroy(this.gameObject);
        }
	// sinon si ce puit a comme tag "puit" et quil touche a un fromage pas posé...
        else if (collision.gameObject.CompareTag("fromage") && this.gameObject.tag == "puit")
        {
	    // on detruit ce puit
            Destroy(this.gameObject);
        }
	// sinon si ce puit a comme tag "puit" et quil touche a une fleur pas posée...
        else if (collision.gameObject.CompareTag("fleur") && this.gameObject.tag == "puit")
        {
	    // on detruit ce puit
            Destroy(this.gameObject);
        }
    }
}
