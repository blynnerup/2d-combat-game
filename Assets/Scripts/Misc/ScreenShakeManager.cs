using Cinemachine;
using Scene_Management;

namespace Misc
{
    public class ScreenShakeManager : Singleton<ScreenShakeManager>
    {
        private CinemachineImpulseSource _source;

        protected override void Awake()
        {
            base.Awake();
            _source = GetComponent<CinemachineImpulseSource>();
        }

        public void ShakeScreen()
        {
            _source.GenerateImpulse();
        }
    }
}
