using UnityEngine;
using System.Collections;
using Holojam;

public class UserInstantiator : MonoBehaviour {

    public PlayerController player;
    public ObjectController userPrefab;
    public float count = 4;

	// Use this for initialization
	void Start () {
        for (int i = 1; i <= count; i++)
        {
            string label = "VR" + i;

            if (!label.Equals(player.label))
            {
                ObjectController user = GameObject.Instantiate<ObjectController>(userPrefab);
                user.label = label;
            }
        }
	}
}
