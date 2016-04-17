using UnityEngine;

class Door : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        transform.parent.GetComponent<SubRoom>().DoorTouch(this);
    }
}