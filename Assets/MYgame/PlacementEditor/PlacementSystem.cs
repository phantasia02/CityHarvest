using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour {
    public GameObject ParentObject;
    public GameObject[] prefab;
    public LayerMask layerMask = 1;
    public LayerMask layerMask2 = 1;
    public LayerMask TargetlayerMask = 1;
    public string prefabTag;
    public bool randomizePrefab;
    public int minRage = 0, maxRage = 1;
    public int toInstantiate = 0;
   
    public float radius = 1;
    public float spread = 1;
    public int amount = 1;
    public float Minsize = 0.1f;
    public float Maxsize = 1.0f;
    public float positionOffset = 2f;

    public bool canPlaceOver;
    public bool canAling;
    public bool isRandomS;
    public bool isRandomR;
    public bool hideInHierarchy;
}
