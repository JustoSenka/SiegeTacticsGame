using UnityEngine;

namespace Assets
{
    public class Game : MonoBehaviour
    {
        // Manager type classes
        public static MouseInput MouseInput { get; private set; } = new MouseInput();
        public static Selection Selection { get; private set; } = new Selection();
        public static HexDatabase HexDatabase { get; private set; } = new HexDatabase();

        // References to mono behaviours


        private void Awake()
        {
            MouseInput = new MouseInput();
            Selection = new Selection();
            HexDatabase = new HexDatabase();
        }

        void Start()
        {
            HexDatabase.Start();
        }

        void Update()
        {
            MouseInput.Update();
        }
    }
}
