using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    [SerializeField] string TargetComponent;
    private List<GameObject> ObjectsInRange = new List<GameObject>();
    [SerializeField] Transform OriginPoint;
    public bool HasTarget => TargetedObject != null;
    public GameObject TargetedObject { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!(ObjectsInRange.Count == 0))
        {
            var closestObject = ObjectsInRange[0];
            var closestDistance = Vector3.Distance(ObjectsInRange[0].transform.position, OriginPoint.position);

            for (int i = 1; i < ObjectsInRange.Count; i++)
            {
                var distance = Vector3.Distance(ObjectsInRange[i].transform.position, OriginPoint.position);
                if (distance < closestDistance)
                    closestObject = ObjectsInRange[i];
            }

            TargetedObject = closestObject;
        }
        else
        {
            TargetedObject = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent(TargetComponent) != null)
            ObjectsInRange.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent(TargetComponent) != null)
            ObjectsInRange.Remove(collision.gameObject);
    }

}
