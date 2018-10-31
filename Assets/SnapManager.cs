﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapManager : MonoBehaviour {

    GameObject collisionParent;
    GameObject stateParent;

    public Text foundText;
    public GameObject endObject;

    int found = 0;
    const int max = 26;

    public void Start()
    {
        collisionParent = GameObject.Find("Collision");
        stateParent = GameObject.Find("States");
    }

    void UpdateText()
    {
        foundText.text = "Estados encontrados: " + found;
    }

    public void ReleasedPiece(StateSnap stateSnap)
    {
        var stateObj = stateParent.transform.Find(stateSnap.Name);
        var stateCollider = stateObj.gameObject.GetComponent<CircleCollider2D>();

        var collisionObj = collisionParent.transform.Find(stateSnap.Name);
        if (collisionObj == null)
            return;

        var collisionCollider = collisionObj.gameObject.GetComponent<CircleCollider2D>();
        if (collisionCollider == null)
            return;

        var colliding = new Collider2D[10];
        var returns = collisionCollider.OverlapCollider(new ContactFilter2D(), colliding);
        for (int i = 0; i < returns; i++)
        {
            if (colliding[i] == stateCollider)
            {
                var snap = stateObj.GetComponent<StateSnap>();
                stateObj.transform.position = new Vector3(snap.X, snap.Y, stateObj.transform.position.z);
                stateObj.GetComponent<PolygonCollider2D>().enabled = false;
                stateObj.GetComponent<CircleCollider2D>().enabled = false;

                found++;
                UpdateText();

                if (found >= max)
                {
                    EndGame();
                }

                break;
            }
        }
    }

    void EndGame()
    {
        endObject.SetActive(true);
    }
}