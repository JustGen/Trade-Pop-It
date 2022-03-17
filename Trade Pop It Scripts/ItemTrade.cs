using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class ItemTrade : MonoBehaviour
{
    [SerializeField] private int _insisePrice;

    public int InsidePrice
    {
        get
        {
            return _insisePrice;
        }
    }
}
