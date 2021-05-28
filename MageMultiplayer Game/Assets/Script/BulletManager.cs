using UnityEngine;
using Photon.Pun;


public class BulletManager : MonoBehaviour
{
    public float bulletSpeed = 1000f;
    public GameObject explosao;
    Rigidbody bulletRB;

    public float bulletTimeLife = 3f;
    float bulletTimeCount = 0f;
    
    void Start()
    {
        bulletRB = GetComponent<Rigidbody>();
        bulletRB.AddForce(transform.forward * bulletSpeed);
    }

    void Update()
    {
        if(bulletTimeCount >= bulletTimeLife)
        {
            Destroy(this.gameObject);
        }

        bulletTimeCount += Time.deltaTime; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && other.GetComponent<PlayerController>())
        {
            Debug.LogError("DANO");
            other.GetComponent<PlayerController>().TakeDamage(-10f);
            PhotonNetwork.Instantiate(explosao.name, other.gameObject.transform.position, other.gameObject.transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
