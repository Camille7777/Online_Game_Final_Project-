using UnityEngine;
using System.Collections;

public class WaveGenerator : MonoBehaviour {

	public float amplitude = 2;
	public float frequency = 2;
	public float startMoving = 0;
	public char xyz = 'x';
	float angle = 0;
	float original = 0;

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
		transform.position = pos;
	}
}
