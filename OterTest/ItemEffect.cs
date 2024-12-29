using UnityEngine;

public class ItemEffect : MonoBehaviour
{
    public GameObject paritcle;
    void Start()
    {
        Invoke("Paticle", 2f);
    }
    void Paticle()
    {
        paritcle.SetActive(true);
    }
}
