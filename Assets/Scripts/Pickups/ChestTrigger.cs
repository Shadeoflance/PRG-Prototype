using UnityEngine;

class ChestTrigger : MonoBehaviour
{
    bool triggered = false;
    void OnTriggerEnter2D(Collider2D col)
    {
        if(!triggered && col.gameObject.IsPlayer())
        {
            transform.parent.GetComponent<Chest>().Open();
            triggered = true;
        }
    }
}