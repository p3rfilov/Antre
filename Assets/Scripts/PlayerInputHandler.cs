﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
public class PlayerInputHandler : MonoBehaviour
{
    //private Camera cam;

    //private void Awake ()
    //{
    //    cam = transform.GetComponent<Camera>();
    //}

    //private void Update ()
    //{
    //    if (Input.GetButtonDown("Fire1"))
    //    {
    //        var _components = GetObjectUnderMouse()?.GetComponents<IClickable>();
    //        foreach (var item in _components)
    //        {
    //            if (item != null)
    //            {
    //                item.Click();
    //            }
    //        }
    //    }
    //}

    //private Transform GetObjectUnderMouse ()
    //{
    //    Ray ray = cam.ScreenPointToRay(Input.mousePosition);

    //    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
    //    {
    //        return hit.transform;
    //    }
    //    return null;
    //}
}
