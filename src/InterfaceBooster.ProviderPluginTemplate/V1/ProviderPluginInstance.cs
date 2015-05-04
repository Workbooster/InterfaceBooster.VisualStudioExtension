using InterfaceBooster.ProviderPluginApi;
using InterfaceBooster.ProviderPluginApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace $safeprojectname$.V1
{
    public class ProviderPluginInstance : IProviderPluginInstance
    {
        #region PROPERTIES

        public IHost Host { get; private set; }
        public IList<Question> ConnectionSettingQuestions { get; private set; }

        #endregion

        #region PUBLIC METHODS

        public ProviderPluginInstance(IHost host)
        {
            this.Host = host;

            // Two example questions
            this.ConnectionSettingQuestions = new List<Question>() {
                Question.New<string>("Username", new string[] { "Credentials", }, true),
                Question.New<string>("Password", new string[] { "Credentials", }, true),
            };
        }

        public IProviderConnection CreateProviderConnection(ConnectionSettings settings)
        {
            if (!ValidateSettings(settings))
                throw new Exception("Invalid settings");
            
            // If all settings are valid create a connection
            return new ProviderConnection(settings);
        }

        #endregion

        #region INTERNAL METHODS

        private bool ValidateSettings(ConnectionSettings settings)
        {
            // Check whether all setting values are valid

            return true;
        }

        #endregion
    }
}
