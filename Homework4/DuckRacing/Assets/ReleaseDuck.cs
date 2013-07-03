using UnityEngine;
using System.Collections;

public class ReleaseDuck : MonoBehaviour
 {
    public GameObject Duck;

    void OnTriggerEnter(Collider collision)
    {
        print(collision.gameObject.name);

        if (collision.gameObject.name == "chassis collider")
        {
            GameObject newDuck = Instantiate(Duck, transform.position, Quaternion.identity) as GameObject;

            newDuck.transform.position = new Vector3(newDuck.transform.position.x, 10, newDuck.transform.position.z);
        }
    }
}
