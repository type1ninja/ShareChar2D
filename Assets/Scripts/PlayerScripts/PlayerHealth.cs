using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerHealth : MonoBehaviour {

    CharacterController charControl;

    public void Start()
    {
        charControl = GetComponent<CharacterController>();
    }

    void Update () {
        if (transform.position.y <= -50)
        {
            transform.position = new Vector3(0, 10, 0);
            charControl.Move(Vector3.zero);
        }
	}
}
