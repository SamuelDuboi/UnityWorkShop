using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class MyEventSystem : MonoBehaviour
{
    public UnityAction OnJump;
    public UnityAction<float > OnResize;
    public static MyEventSystem instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            OnJump.Invoke();
        }
        if (Input.GetButtonDown("LeftBumper"))
        {
            OnResize.Invoke(1);
        }
        if (Input.GetButtonDown("RightBumper"))
        {
            OnResize.Invoke(-1);
        }
        if (Input.GetButtonDown("Restart"))
        {
            SceneManager.LoadScene(0);
        }
    }
}
