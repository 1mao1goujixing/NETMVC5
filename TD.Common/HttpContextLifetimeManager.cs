using Microsoft.Practices.Unity;
using System;
using System.Collections;
using System.Web;

namespace TD.Common
{
    public class HttpContextLifetimeManager : LifetimeManager
    {
        private string TypeName { get; set; }

        public Hashtable Container
        {
            get
            {
                var container = HttpContext.Current.Items[UnityHttpModule.UnityObjects] as Hashtable;
                if (container == null)
                {
                    container = Hashtable.Synchronized(new Hashtable());
                    HttpContext.Current.Items[UnityHttpModule.UnityObjects] = container;
                }
                return container;
            }
        }

        public HttpContextLifetimeManager(Type type)
        {
            TypeName = type.AssemblyQualifiedName;
        }

        public override object GetValue()
        {
            return Container[TypeName];
        }

        public override void SetValue(object newValue)
        {
            Container[TypeName] = newValue;
        }

        public override void RemoveValue()
        {
            Container.Remove(TypeName);
        }

    }

    internal class HttpContextLifetimeManager<T> : HttpContextLifetimeManager
    {
        public HttpContextLifetimeManager()
            : base(typeof(T))
        {
        }
    }
}
