using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject gun;
    public GameObject bullet;
    public GameObject bulletSpawnPoint;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float bulletSpeed;



    // Start is called before the first frame update
    void Start()
    {
        speed = 3f;
        bulletSpeed = 20f;
        InvokeRepeating("bulletSpawner", 1f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        gun.transform.Rotate(Vector3.left * speed);
    }

    void bulletSpawner()
    {
        Vector3 pos = bulletSpawnPoint.transform.position;
        GameObject aBullet = Instantiate(bullet, pos, bulletSpawnPoint.transform.rotation);
        aBullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
        Destroy(aBullet, 5.0f);
    }
}
