using Unity.VisualScripting;
using UnityEngine;

public class Parameters
{
    public const string FirstName = "VR";

    public static bool withDeformation = false;
    public static bool applyShader = false;
    public static bool printFrameRate = false;
    public const float particleRedius = 0.05f;
    public static Color initialParticleColor = new Color(200f, 200f, 200f).WithAlpha(0.6f);
    public static float particlesVelocity = 1;
    public const int octreeCapacity = 20;
    public const int carOctreeCapacity = 50;
    public const float octreeWidth = 10f;
    public const float octreeHeight = 2f;
    public const float octreeDepth = 2.5f;
    public static readonly Vector3 octreeCenter = new Vector3(0f, 1f, 0f);
    public const float carWidth = 2.5f;
    public const float carHeight = 1f;
    public const float carDepth = 1f;
    public static Vector3 carCenter = new Vector3(0f, 0.5f, 0f);

}
