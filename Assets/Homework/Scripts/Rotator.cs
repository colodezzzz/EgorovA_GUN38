using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 _rotate;

    private IEnumerator Start()
    {
        while (true)
        {
            transform.Rotate(_rotate * Time.deltaTime);
            yield return null;
        }
    }
}
