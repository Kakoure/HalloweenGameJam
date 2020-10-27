using UnityEngine;

public class Ping : MonoBehaviour
{
    [SerializeField]
    private float radius = 1;
    private Collider2D collider;

    public void SetRadius(float r)
    {
        radius = r;
        transform.localScale = Vector3.one * r;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        SetRadius(radius);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
