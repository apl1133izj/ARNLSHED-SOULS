using UnityEngine;

public class DeshTest : MonoBehaviour
{
    Rigidbody rb;
    public float hp = 20000000;
    public Shild shild;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Desh"))
        {
            rb.AddForce(gameObject.transform.forward * 30,ForceMode.Impulse);
        }
        if (other.gameObject.CompareTag("ShildAttack"))
        {
            hp -= shild.damageHap;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Desh"))
        {
            rb.AddForce(gameObject.transform.forward * 30, ForceMode.Impulse);
        }
    }
}