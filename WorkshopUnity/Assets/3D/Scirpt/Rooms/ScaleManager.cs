using UnityEngine;
using TMPro;
public class ScaleManager : MonoBehaviour
{
    public TextMeshProUGUI scaletext;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            var rigidbody = other.GetComponent<Rigidbody>();
            var mass = rigidbody.mass * rigidbody.velocity.y / 5;
            mass = -mass;
            scaletext.text = (int)mass + "KG";
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            scaletext.text = "0 KG";
        }
    }
}
