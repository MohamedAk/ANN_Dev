using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anzu.AnnPortal.Common.Ioc
{
    public static class IocContainer
    {
        /// <summary>
        /// The container.
        /// </summary>
        private static readonly IUnityContainer _container = new UnityContainer();

        /// <summary>
        /// Initializes static member of the <see cref="IocContainer"/> class.
        /// </summary>
        static IocContainer()
        {
            try
            {
                InitializeContainer();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Resolves this instance.
        /// </summary>
        /// <returns>The type of t.</returns>
        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        /// <summary>
        /// Resolves the specified.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns>Object.</returns>
        public static object Resolve(Type t)
        {
            return _container.Resolve(t);
        }

        /// <summary>
        /// Initializes the container.
        /// </summary>
        private static void InitializeContainer()
        {
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            
            section.Configure(_container, "CommonContainer");
        }
    }
}
