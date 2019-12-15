using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject king;
    void Start() {
        var transform = this.GetComponent<Transform>();
        for (int i = 0; i < 10; i++) {
            GameObject newGuy = Instantiate(king, transform.position, transform.rotation);
            newGuy.transform.parent = transform;
        }
    }
}
