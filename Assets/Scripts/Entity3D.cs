using UnityEngine;

public class Entity3D : MonoBehaviour
{   
    private GameObject _entityObject;
    private Renderer _eObjectRenderer;
    private float _eObjectHeight;

    private void Awake()
    {
        _entityObject = transform.GetChild(0).gameObject;
        _eObjectRenderer = _entityObject.GetComponentInChildren<Renderer>();
        _eObjectHeight = _eObjectRenderer.bounds.min.y;

        transform.rotation = Quaternion.Euler(-30, 0, 0);
    }

    private void Update()
    {
        UpdateZPosition();
    }

    private void UpdateZPosition()
    {
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            (transform.position.y - _eObjectHeight) * 2
        );
    }

    /// <summary>
    ///   <para>Rotates the 3D object to look at the target position.</para>
    /// </summary>
    /// <param name="targetPos">The position to look at in world space.</param>
    public void LookAt(Vector3 targetPos)
    {
        float angleRad = Mathf.Atan2(targetPos.y - transform.position.y, (targetPos.x - transform.position.x) * 0.5f);
        Vector3 eulerRotation = _entityObject.transform.localRotation.eulerAngles;
        eulerRotation.y = 90 - angleRad * Mathf.Rad2Deg;
        _entityObject.transform.localRotation = Quaternion.Euler(eulerRotation);
    }
}