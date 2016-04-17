using UnityEngine;

class Door : MonoBehaviour
{
    public bool open = false;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(open && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            transform.parent.GetComponent<SubRoom>().DoorTouch(this);
    }
}