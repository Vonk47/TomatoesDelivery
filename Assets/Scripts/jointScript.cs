using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jointScript : MonoBehaviour
{
    ConfigurableJoint joint;
    void Start()
    {
        joint = transform.gameObject.AddComponent<ConfigurableJoint>();
        joint.anchor = new Vector3(0, 0, 0);
        joint.axis = new Vector3(0, 1, 0);
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;
        joint.angularXMotion = ConfigurableJointMotion.Limited;
        joint.angularYMotion = ConfigurableJointMotion.Limited;
        joint.angularZMotion = ConfigurableJointMotion.Limited;
    }
}
