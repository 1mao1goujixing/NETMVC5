using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace TD.Common
{
    public class HttpContainerBuilder
    {
        public static readonly IUnityContainer UnityContainer = new UnityContainer();
        public static readonly List<Type> EntityContexts = new List<Type>();

        #region RegisterDataContext
        /// <summary>
        /// 注册DataContext
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        public static void RegisterDataContext<TContext>() where TContext : DbContext
        {
            Type contextType = typeof(TContext);
            UnityContainer.RegisterType(contextType);
            EntityContexts.Add(contextType);

        }

        /// <summary>
        ///  注册DataContext
        /// </summary>
        /// <typeparam name="TContext">连接</typeparam>
        /// <typeparam name="TLifetimeManager">生命周期</typeparam>
        public static void RegisterDataContext<TContext, TLifetimeManager>() where TContext : DbContext where TLifetimeManager : LifetimeManager
        {
            Type contextType = typeof(TContext);
            TLifetimeManager lm = (TLifetimeManager)Activator.CreateInstance(typeof(TLifetimeManager), new object[] { contextType });
            UnityContainer.RegisterType(contextType, lm);
            EntityContexts.Add(contextType);

        }

        #endregion

        #region 注册业务Service
        /// <summary>
        /// 注册业务Service
        /// </summary>
        /// <typeparam name="TService">业务的service类型</typeparam>
        /// <typeparam name="TLifetimeManager">生命周期关系类型</typeparam>
        /// <param name="useInterceptor">是否用拦截器</param>
        public static void RegisterBusinessService<TService, TLifetimeManager>(bool useInterceptor) where TLifetimeManager : LifetimeManager where TService : IServices
        {
            Type serviceType = typeof(TService);
            TLifetimeManager lm = (TLifetimeManager)Activator.CreateInstance(typeof(TLifetimeManager), new object[] { serviceType });
            UnityContainer.RegisterType(serviceType, lm);
            if (useInterceptor)
            {
                UnityContainer.AddNewExtension<Interception>().Configure<Interception>().SetInterceptorFor(serviceType, new InterfaceInterceptor());

            }

        }

        /// <summary>
        /// 注册业务Service
        /// </summary>
        /// <typeparam name="TLifetimeManager"></typeparam>
        /// <param name="assembly">程序集</param>
        /// <param name="useInterceptor">是否启动拦截器</param>
        /// <param name="IServiceName">this继承的接口名字</param>
        public static void RegisterBusinessService<TLifetimeManager>(Assembly assembly, bool useInterceptor, string IServiceName) where TLifetimeManager : LifetimeManager
        {
            Type[] serviceTypes = assembly.GetTypes();

            foreach (Type serviceType in serviceTypes)
            {

                Type interfceParent = serviceType.GetInterfaces().FirstOrDefault(o => o.GetInterface(IServiceName) != null);
                if (interfceParent != null)
                {
                    TLifetimeManager lm = (TLifetimeManager)Activator.CreateInstance(typeof(TLifetimeManager), new object[] { interfceParent });
                    UnityContainer.RegisterType(interfceParent, serviceType, lm);
                    if (useInterceptor)
                    {
                        UnityContainer.AddNewExtension<Interception>().Configure<Interception>().SetInterceptorFor(interfceParent, new InterfaceInterceptor());

                    }
                }
            }



        }

        #endregion

        #region RegisterDataRepository
        /// <summary>
        /// 注册Repository
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="useInterceptor">启用拦截器</param>
        /// <param name="IRepositoryName">继承的接口名称</param>
        //public static void RegisterDataRepository(Assembly assembly, bool useInterceptor, string IRepositoryName)
        //{
        //    Type[] repositoryTypes = assembly.GetTypes();
        //    foreach (Type repositoryType in repositoryTypes)
        //    {
        //        Type interfaceParent = repositoryType.GetInterfaces().FirstOrDefault(o => o.GetInterface(IRepositoryName) != null);
        //        if (interfaceParent != null)
        //        {
        //            UnityContainer.RegisterType(repositoryType,null,"", new HttpContextLifetimeManager(repositoryType));

        //            if (useInterceptor)
        //            {
        //                UnityContainer.AddNewExtension<Interception>()
        //                    .Configure<Interception>()
        //                    .SetInterceptorFor(interfaceParent, new InterfaceInterceptor());
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 注册Repository
        /// </summary>
        /// <typeparam name="TLifetimeManager">生命周期管理类型</typeparam>
        /// <param name="assembly">程序集</param>
        /// <param name="useInterceptor">启用拦截器</param>
        /// <param name="IRepositoryName">继承的接口名称</param>
        public static void RegisterDataRepository<TLifetimeManager>(Assembly assembly, bool useInterceptor, string IRepositoryName) where TLifetimeManager : LifetimeManager
        {
            Type[] repositoryTypes = assembly.GetTypes();
            foreach (Type repositoryType in repositoryTypes)
            {
                Type interfaceParent = repositoryType.GetInterfaces().FirstOrDefault(o => o.GetInterface(IRepositoryName) != null);
                if (interfaceParent != null)
                {
                    TLifetimeManager lm = (TLifetimeManager)Activator.CreateInstance(typeof(TLifetimeManager), new object[] { repositoryType });
                    UnityContainer.RegisterType(repositoryType, lm);


                    if (useInterceptor)
                    {
                        UnityContainer.AddNewExtension<Interception>()
                            .Configure<Interception>()
                            .SetInterceptorFor(interfaceParent, new InterfaceInterceptor());
                    }
                }
            }
        }


        #endregion

        #region Resolve

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resolverOverrides"></param>
        /// <returns></returns>
        public static T Resolve<T>(params ResolverOverride[] resolverOverrides)
        {
            return UnityContainer.Resolve<T>(resolverOverrides);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="resolverOverrides"></param>
        /// <returns></returns>
        public static object Resolve(Type type, params ResolverOverride[] resolverOverrides)
        {
            return UnityContainer.Resolve(type, resolverOverrides);
        }

        #endregion
    }
}
