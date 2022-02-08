using System;
using Extensions;
using UnityEngine;
using Views;

namespace Managers
{
    public class CollisionDetectionManager : BaseManager<CollisionDetectionManager>
    {
        public event Action<IView ,IView> TriggerEnter = delegate { };
        public event Action<IView ,IView> TriggerExit = delegate { };

        public event Action<IView ,IView> CollisionEnter = delegate { };
        public event Action<IView ,IView> CollisionExit = delegate { };

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

        public void HandleOnTriggerEnter(IView view1, IView view2)
        {
            if (view1 == null ||
                view2 == null)
            {
                return;
            }

            Debug.Log($"{this.GetType().Name}.{ReflectionExtensions.GetCallerMemberName()}" +
                      $"\n{nameof(view1)}={view1.GetType().Name}" +
                      $"\n{nameof(view2)}={view2.GetType().Name}");

            TriggerEnter.Invoke(view1, view2);
        }
        public void HandleOnTriggerExit(IView view1, IView view2)
        {
            if (view1 == null ||
                view2 == null)
            {
                return;
            }

            Debug.Log($"{this.GetType().Name}.{ReflectionExtensions.GetCallerMemberName()}" +
                      $"\n{nameof(view1)}={view1.GetType().Name}" +
                      $"\n{nameof(view2)}={view2.GetType().Name}");

            TriggerExit.Invoke(view1, view2);
        }

        public void HandleOnCollisionEnter(IView view1, IView view2)
        {
            Debug.Log($"{this.GetType().Name}.{ReflectionExtensions.GetCallerMemberName()}" +
                      $"\n{nameof(view1)}={view1.GetType().Name}" +
                      $"\n{nameof(view2)}={view2.GetType().Name}");

            CollisionEnter.Invoke(view1, view2);
        }
        public void HandleOnCollisionExit(IView view1, IView view2)
        {
            Debug.Log($"{this.GetType().Name}.{ReflectionExtensions.GetCallerMemberName()}" +
                      $"\n{nameof(view1)}={view1.GetType().Name}" +
                      $"\n{nameof(view2)}={view2.GetType().Name}");

            CollisionExit.Invoke(view1, view2);
        }
    }
}
