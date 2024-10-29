using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform ikTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    public void Map()
    {
        ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class IKTargetFollowVRRig : MonoBehaviour
{
    [Range(0,1)]
    public float turnSmoothness = 0.1f;
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    private Transform vrHeadTarget;
    private Transform vrLeftTarget;
    private Transform vrRightTarget;

    public Vector3 headBodyPositionOffset;
    public float headBodyYawOffset;
    private void Start()
    {
        vrHeadTarget = GameObject.Find("Head VR Target").transform;
        vrLeftTarget = GameObject.Find("Left Hand  VR Target").transform;
        vrRightTarget = GameObject.Find("Right Hand  VR Target").transform;

        if (vrHeadTarget != null) head.vrTarget = vrHeadTarget;
        else Debug.LogError("vrHeadTarget not found in " + gameObject.name);
        if (vrLeftTarget != null)  leftHand.vrTarget = vrLeftTarget;
        else Debug.LogError("vrLeftTarget not found in " + gameObject.name);
        if (vrRightTarget != null) rightHand.vrTarget = vrRightTarget;
        else Debug.LogError("vrRightTarget not found in " + gameObject.name);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = head.ikTarget.position + headBodyPositionOffset;
        float yaw = head.vrTarget.eulerAngles.y;
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(transform.eulerAngles.x, yaw, transform.eulerAngles.z),turnSmoothness);

        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
}
