using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    public static GameManager instance;
	void Start()
	{
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }
	}
	
	void Update()
	{
	}
}
