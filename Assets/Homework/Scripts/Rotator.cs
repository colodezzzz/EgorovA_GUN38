using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 _rotate;

    private IEnumerator Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        while (true)
        {
            rigidbody.rotation = Quaternion.Lerp(rigidbody.rotation, Quaternion.LookRotation(_rotate), Time.deltaTime);
            yield return null;
        }
    }
}
