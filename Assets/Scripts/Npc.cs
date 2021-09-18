using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [HideInInspector]
    public bool _needLemonade;
    // Start is called before the first frame update
    void Start()
    {
        _needLemonade = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
