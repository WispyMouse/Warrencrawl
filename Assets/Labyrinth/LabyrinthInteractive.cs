using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthInteractive : MonoBehaviour
{
    public InteractiveData Data;
    public GameObject WorldObject;

    public void EnableInteractive()
    {
        if (WorldObject)
        {
            WorldObject.SetActive(true);
        }
    }

    public void DisableInteractive()
    {
        if (WorldObject)
        {
            WorldObject.SetActive(false);
        }
    }
}
