using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetElement : MonoBehaviour {

    private Vector3 initialPosition;
	// Use this for initialization
	void Start () {
        initialPosition = transform.position;
	}

    public void Reset()
    {
        transform.position = initialPosition;
        gameObject.SetActive(true);
        int i = 0;
        while(i< transform.childCount)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            i++;
        }

    }
}
