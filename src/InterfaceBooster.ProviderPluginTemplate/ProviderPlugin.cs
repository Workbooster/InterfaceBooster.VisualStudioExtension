using InterfaceBooster.ProviderPluginApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace $safeprojectname$
{
    public class ProviderPlugin : IProviderPlugin
    {
        public IProviderPluginInstance CreateProviderPluginInstance(Guid id, IHost host)
        {
            switch (id.ToString().ToUpper())
            {
                case "$guid1$":
                    return new V1.ProviderPluginInstance(host);
            }

            throw new Exception(String.Format("Requested ProviderPluginInstance with id {0} not found.", id.ToString()));
        }
    }
}
