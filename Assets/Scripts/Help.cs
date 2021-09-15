using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Help : MonoBehaviour {


    public GameObject father;

    private void OnDisable()
    {
        father.SetActive(false);
    }
}
