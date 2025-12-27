
namespace Swarm.Runtime
{
    public class ModeECS : ModeBase
    {
        public override Mode ModeType => Mode.ECS;

        public override void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public override void Clear() { }
    }
}
