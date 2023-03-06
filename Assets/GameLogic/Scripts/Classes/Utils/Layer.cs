using UnityEngine;

namespace Assets
{
    public class Layer
    {
        public static string Ground => "Ground";
        public static int GroundMask => LayerMask.GetMask(Ground);

        public static string Entity => "Entity";
        public static int EntityMask => LayerMask.GetMask(Entity);
    }
}
