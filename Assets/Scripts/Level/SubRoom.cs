using UnityEngine;

class SubRoom : MonoBehaviour
{
    public Door rightD, leftD, topD, botD;
    public Room room;
    public GameObject rightW, leftW, topW, botW;

    public void DoorTouch(Door d)
    {
        if (d == rightD)
            Level.instance.ChangeRoom(new Vector2(1, 0), this);
        if(d == leftD)
            Level.instance.ChangeRoom(new Vector2(-1, 0), this);
        if (d == topD)
            Level.instance.ChangeRoom(new Vector2(0, 1), this);
        if (d == botD)
            Level.instance.ChangeRoom(new Vector2(0, -1), this);
    }

    public void Disable()
    {
        if(rightD != null)
            rightD.Close();
        if(leftD != null)
            leftD.Close();
        if(topD != null)
            topD.Close();
        if(botD != null)
            botD.Close();
        gameObject.SetActive(false);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void WrapInRoom()
    {
        room = new Room(this);
    }
}