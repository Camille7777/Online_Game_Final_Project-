using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

public class WaveGenerator : MonoBehaviour {

	public float amplitude = 2;
	public float frequency = 2;
	public float starttime = 0.0f;
	public char xyz='x';

	float angle = 0;
	float originalX = 0;

	void Start(){
		if(xyz=='x')
		originalX = transform.position.x;
		else if (xyz == 'y')
			originalX = transform.position.y;
		else
			originalX = transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {

		Invoke("move", starttime);
	}

	void move()
	{
		angle += Time.deltaTime * frequency;
		Vector3 pos = transform.position;
		if (xyz == 'x')
			pos.x = originalX + Mathf.Sin(angle) * amplitude;
		else if (xyz == 'y')
			pos.y = originalX + Mathf.Sin(angle) * amplitude;
		else
			pos.z = originalX + Mathf.Sin(angle) * amplitude;
		transform.position = pos;
	}
}
