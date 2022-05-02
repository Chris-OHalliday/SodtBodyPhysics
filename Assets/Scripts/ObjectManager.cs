using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public MeshClass[] sceneObjs;
    public GameObject[] rigidObjects;

    private void Awake()
    {
        sceneObjs = FindObjectsOfType<MeshClass>();
    }

}
