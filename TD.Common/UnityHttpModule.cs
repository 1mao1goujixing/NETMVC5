using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TD.Common
{
    public class UnityHttpModule : IHttpModule
    {
        internal const string UnityObjects = "UnityObjects";

        #region IHttpModule Members

        public void Dispose()
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items.Clear();
            }
        }

        public void Init(HttpApplication context)
        {
            context.EndRequest += ContextEndRequest;
        }

        #endregion

        private static void ContextEndRequest(object sender, EventArgs eventArgs)
        {
            Hashtable container = HttpContext.Current.Items[UnityObjects] as Hashtable;

            if (container != null)
            {
                foreach (string key in container.Keys)
                {
                    var disposable = container[key] as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
                HttpContext.Current.Items.Remove(UnityObjects);
            }
        }

    }
}
