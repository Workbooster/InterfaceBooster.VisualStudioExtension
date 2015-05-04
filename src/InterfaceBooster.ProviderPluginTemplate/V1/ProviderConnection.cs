using InterfaceBooster.ProviderPluginApi;
using InterfaceBooster.ProviderPluginApi.Data;
using InterfaceBooster.ProviderPluginApi.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace $safeprojectname$.V1
{
    public class ProviderConnection : IProviderConnection
    {
        #region PROPERTIES

        public IEnumerable<IEndpoint> Endpoints { get; private set; }
        public ConnectionSettings Settings { get; private set; }

        #endregion

        #region PUBLIC METHODS

        public ProviderConnection(ConnectionSettings setting)
        {
            this.Settings = setting;

            Endpoints = new List<IEndpoint>() 
            {
                new Endpoints.ExampleEndpoint(),
            };
        }

        #endregion
    }
}
