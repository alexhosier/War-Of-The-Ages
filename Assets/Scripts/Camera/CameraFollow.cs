using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    #region Variables

    [SerializeField] private Transform toFollow;
    [SerializeField] private Vector3 offset;

    #endregion

    #region Update method

    void Update()
    {
        // Move the camera to the object
        transform.position = toFollow.position + offset;        
    }

    #endregion

}
