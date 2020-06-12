using UnityEngine;
using System.Collections;

public class WaveGenerator : MonoBehaviour {

	public float amplitude = 2;
	public float frequency = 2;
	public float startMoving = 0;
	private Rigidbody rb;
	


	public char xyz = 'x';
	float angle = 0;
	float original = 0;
	
	void Start(){
		rb = GetComponent<Rigidbody>();
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
		{
			Debug.Log("HHHH");
			rb.velocity = new Vector3(Mathf.Sin(angle) * amplitude, 0, 0);
		}
		//pos.x = original + Mathf.Sin(angle) * amplitude;
		else if (xyz == 'y')
			pos.y = original + Mathf.Sin(angle) * amplitude;
		else
			pos.z = original + Mathf.Sin(angle) * amplitude;
		transform.position = pos;
	}
}
