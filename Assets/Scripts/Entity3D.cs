using UnityEngine;

public class Entity3D : MonoBehaviour
{
    private void Awake()
    {
        transform.rotation = Quaternion.Euler(-30, 0, 0);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        UpdateZPosition();
    }

    private void UpdateZPosition()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y * 2f);
    }
}