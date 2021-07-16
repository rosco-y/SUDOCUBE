using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLight : MonoBehaviour
{


    [SerializeField] GameObject _sudoCenter;
    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(_sudoCenter.transform.position);
    }
}
