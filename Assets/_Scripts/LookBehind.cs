using UnityEngine;

// Hold Tab to swing the camera round to the front of the player, so you can
// see what is chasing you, then let go to swing back.
//
// Only the camera moves. PlayerControllerTopDown steers by the CameraRig
// object, which stays put, so holding Tab does not invert your controls or
// make the character spin round.
public class LookBehind : MonoBehaviour
{
    [Header("Controls")]
    public KeyCode lookKey = KeyCode.Tab;

    [Header("Feel")]
    // How quickly the camera swings round. Higher is snappier.
    public float turnSpeed = 6f;
    // How far to push the camera away from the player while looking back,
    // so it does not end up inside the character model.
    public float pullBack = 1.5f;

    private Vector3 restPosition;
    private Quaternion restRotation;

    // 0 = normal view, 1 = fully turned to the front. Moves smoothly between
    // the two so the swing is not a jarring snap.
    private float blend;

    private void Start()
    {
        restPosition = transform.localPosition;
        restRotation = transform.localRotation;
    }

    private void LateUpdate()
    {
        float target = Input.GetKey(lookKey) ? 1f : 0f;
        blend = Mathf.MoveTowards(blend, target, Time.deltaTime * turnSpeed);

        // Swing part way round the player. At blend 1 this is a half turn,
        // which puts the camera in front looking back.
        Quaternion swing = Quaternion.Euler(0f, 180f * blend, 0f);

        Vector3 position = swing * restPosition;

        // Push outwards as we swing, to clear the character model.
        Vector3 outward = new Vector3(position.x, 0f, position.z);
        if (outward.sqrMagnitude > 0.0001f)
        {
            position += outward.normalized * pullBack * blend;
        }

        transform.localPosition = position;
        transform.localRotation = swing * restRotation;
    }
}
