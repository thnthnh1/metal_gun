using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mp_BotActivator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<Mp_Bot>())
		{
			collision.GetComponent<Mp_Bot>().enableJetpack = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.GetComponent<Mp_Bot>())
		{
			collision.GetComponent<Mp_Bot>().enableJetpack = false;
		}
	}
}
