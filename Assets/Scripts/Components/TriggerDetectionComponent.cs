using Managers;
using UnityEngine;
using Views;

namespace Components
{
    [RequireComponent(typeof(Collider))]
    public class TriggerDetectionComponent : BaseComponent
    {
        public override void Initialize()
        {
        }
        public override void UnInitialize()
        {
        }

        public override void Subscribe()
        {
        }
        public override void UnSubscribe()
        {
        }

        private void OnTriggerEnter(Collider otherCollider)
        {
            if (CollisionDetectionManager.Instance == null)
            {
                return;
            }

            var view1 = this.gameObject.GetComponent<IView>();
            var view2 = otherCollider.gameObject.GetComponent<IView>();
            CollisionDetectionManager.Instance.HandleOnTriggerEnter(view1, view2);
        }
        private void OnTriggerExit(Collider otherCollider)
        {
            if (CollisionDetectionManager.Instance == null)
            {
                return;
            }

            var view1 = this.gameObject.GetComponent<IView>();
            var view2 = otherCollider.gameObject.GetComponent<IView>();
            CollisionDetectionManager.Instance.HandleOnTriggerExit(view1, view2);
        }
    }
}
