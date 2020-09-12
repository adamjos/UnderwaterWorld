using UnityEngine;
using UnityEditor;

public class FastIKFabrik : MonoBehaviour
{

    // Chain length of bones
    public int chainLength = 2;

    // Target the chain should be sent to
    public Transform target;
    public Transform pole;

    // Solver iterations per update
    [Header("Solver Parameters")]
    public int iterations = 10;

    // Distance when the solver stops
    public float delta = 0.001f;

    // Strength of going back to the start postion
    [Range(0, 1)]
    public float snapBackStrength = 1f;

    protected float[] bonesLenght; // Target to origin
    protected float completeLength;
    protected Transform[] bones;
    protected Vector3[] positions;
    protected Vector3[] startDirectionSucc;
    protected Quaternion[] startRotationBone;
    protected Quaternion startRotationTarget;
    protected Transform root;


    private void Awake()
    {
        Init();
    }


    void Init()
    {
        // Initial array
        bones = new Transform[chainLength + 1];
        positions = new Vector3[chainLength + 1];
        bonesLenght = new float[chainLength];
        startDirectionSucc = new Vector3[chainLength + 1];
        startRotationBone = new Quaternion[chainLength + 1];

        root = transform;
        for (var i = 0; i <= chainLength; i++)
        {
            if (root == null)
                throw new UnityException("The chain value is longer than the ancestor chain");
            root = root.parent;
        }


        if (target == null)
        {
            target = new GameObject(gameObject.name + "Target").transform;
            SetPositionRootSpace(target, GetPositionRootSpace(transform));
        }
        startRotationTarget = GetRotationRootSpace(target);

        completeLength = 0f;

        var current = transform;
        for (var i = bones.Length - 1; i >= 0; i--)
        {
            bones[i] = current;
            startRotationBone[i] = GetRotationRootSpace(current);

            if (i == bones.Length - 1)
            {
                // Leaf
                startDirectionSucc[i] = GetPositionRootSpace(target) - GetPositionRootSpace(current);
            }
            else
            {
                // Mid-bone
                startDirectionSucc[i] = GetPositionRootSpace(bones[i + 1]) - GetPositionRootSpace(current);
                bonesLenght[i] = startDirectionSucc[i].magnitude;
                completeLength += bonesLenght[i];
            }

            current = current.parent;
        }
    }

    private void LateUpdate()
    {
        ResolveIK();
    }


    private void ResolveIK ()
    {
        if (target == null)
        {
            return;
        }

        if (bonesLenght.Length != chainLength)
        {
            Init();
        }

        //  root
        //  (bone0) (bonelen 0) (bone1) (bonelen 1) (bone2)...
        //   x--------------------x--------------------x---...


        // Get positions
        for (int i = 0; i < bones.Length; i++)
        {
            positions[i] = GetPositionRootSpace(bones[i]);
        }

        var targetPosition = GetPositionRootSpace(target);
        var targetRotation = GetRotationRootSpace(target);

        // 1st check, is it possible to reach?
        if ((targetPosition - GetPositionRootSpace(bones[0])).sqrMagnitude >= completeLength * completeLength)
        {
            var direction = (targetPosition - positions[0]).normalized;

            for (int i = 1; i < positions.Length; i++)
            {
                positions[i] = positions[i - 1] + direction * bonesLenght[i - 1];
            }
        }
        else
        {
            for (int i = 0; i < positions.Length - 1; i++)
            {
                positions[i + 1] = Vector3.Lerp(positions[i + 1], positions[i] + startDirectionSucc[i], snapBackStrength);
            }

            for (int iteration = 0; iteration < iterations; iteration++)
            {
                // Backwards
                for (int i = positions.Length - 1; i > 0; i--)
                {
                    if (i == positions.Length - 1)
                    {
                        positions[i] = targetPosition; // set it to target
                    }
                    else
                    {
                        positions[i] = positions[i + 1] + (positions[i] - positions[i + 1]).normalized * bonesLenght[i];
                    }
                }

                // Forward
                for (int i = 1; i < positions.Length; i++)
                {
                    positions[i] = positions[i - 1] + (positions[i] - positions[i - 1]).normalized * bonesLenght[i - 1];
                }

                // Close enough?
                if ((positions[positions.Length - 1] - targetPosition).sqrMagnitude < delta * delta)
                {
                    break;
                }
            }
        }

        if (pole != null)
        {
            for (int i = 1; i < positions.Length - 1; i++)
            {
                var plane = new Plane(positions[i + 1] - positions[i - 1], positions[i - 1]);
                var projectedPole = plane.ClosestPointOnPlane(pole.position);
                var projectedBone = plane.ClosestPointOnPlane(positions[i]);
                var angle = Vector3.SignedAngle(projectedBone - positions[i - 1], projectedPole - positions[i - 1], plane.normal);
                positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (positions[i] - positions[i - 1]) + positions[i - 1];
            }
        }

        // Set postions
        for (int i = 0; i < positions.Length; i++)
        {
            if (i == positions.Length - 1)
            {
                SetRotationRootSpace(bones[i], Quaternion.Inverse(targetRotation) * startRotationTarget * Quaternion.Inverse(startRotationBone[i]));

                //bones[i].rotation = target.rotation * Quaternion.Inverse(startRotationTarget) * startRotationBone[i];
            }
            else
            {
                SetRotationRootSpace(bones[i], Quaternion.FromToRotation(startDirectionSucc[i], positions[i + 1] - positions[i]) * Quaternion.Inverse(startRotationBone[i]));

                //bones[i].rotation = Quaternion.FromToRotation(startDirectionSucc[i], positions[i + 1] - positions[i]) * startRotationBone[i];
            }

            SetPositionRootSpace(bones[i], positions[i]);

            //bones[i].position = positions[i];
        }

    }


    private Vector3 GetPositionRootSpace(Transform current)
    {
        if (root == null)
            return current.position;
        else
            return Quaternion.Inverse(root.rotation) * (current.position - root.position);
    }

    private void SetPositionRootSpace(Transform current, Vector3 position)
    {
        if (root == null)
            current.position = position;
        else
            current.position = root.rotation * position + root.position;
    }

    private Quaternion GetRotationRootSpace(Transform current)
    {
        //inverse(after) * before => rot: before -> after
        if (root == null)
            return current.rotation;
        else
            return Quaternion.Inverse(current.rotation) * root.rotation;
    }

    private void SetRotationRootSpace(Transform current, Quaternion rotation)
    {
        if (root == null)
            current.rotation = rotation;
        else
            current.rotation = root.rotation * rotation;
    }

        private void OnDrawGizmos()
    {
        var current = this.transform;
        for (int i = 0; i < chainLength && current != null && current.parent != null; i++)
        {
            var scale = Vector3.Distance(current.position, current.parent.position) * 0.1f;
            Handles.matrix = Matrix4x4.TRS(current.position, Quaternion.FromToRotation(Vector3.up, current.parent.position - current.position), new Vector3(scale, Vector3.Distance(current.parent.position, current.position), scale));
            Handles.color = Color.green;
            Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);
            current = current.parent;
        }
    }
}
