using UnityEngine;
using System.Collections;

public class WaveGenerator : MonoBehaviour {

	public float amplitude = 2;
	public float frequency = 2;
	public float startMoving = 0;
	public CharacterController character;

	public char xyz = 'x';
	float angle = 0;
	float original = 0;
	public Vector3 m_velocity;
	void Start(){
		if(xyz=='x')
			original = transform.position.x;
		else if (xyz=='y')
			original = transform.position.y;
		else
			original = transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
		Invoke("move", startMoving);
	}

	void move()
    {
		angle += Time.deltaTime * frequency;
		Vector3 pos = transform.position;

		if (xyz == 'x')
			pos.x = original + Mathf.Sin(angle) * amplitude;
		else if (xyz == 'y')
			pos.y = original + Mathf.Sin(angle) * amplitude;
		else
			pos.z = original + Mathf.Sin(angle) * amplitude;

		m_velocity = (transform.position - pos) / Time.fixedDeltaTime;
		transform.position = pos;
		character.attachedRigidbody.position = pos;
	}
}
