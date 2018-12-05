using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {
    
    public static GameObject findClosestGameObject(GameObject self, string tag) {

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float distance = Mathf.Infinity;

        foreach (GameObject gameObject in gameObjects) {
            float currDistance = (gameObject.transform.position - self.transform.position).sqrMagnitude;
            if (currDistance < distance) {
                closest = gameObject;
                distance = currDistance;
            }
        }
        return closest;
    }

}
