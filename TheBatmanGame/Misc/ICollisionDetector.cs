using TheBatmanGame.GameObjects;

namespace TheBatmanGame.Misc
{
    public interface ICollisionDetector
    {
        bool AreCollided(GameObject go1, GameObject go2);
    }
}