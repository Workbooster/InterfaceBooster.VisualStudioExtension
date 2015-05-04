using InterfaceBooster.ProviderPluginApi.Communication;
using InterfaceBooster.ProviderPluginApi.Data;
using InterfaceBooster.ProviderPluginApi.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace $safeprojectname$.V1.Endpoints
{
    public class ExampleEndpoint : IReadEndpoint
    {
        #region MEMBERS

        private Schema _Schema;
        private IEnumerable<object[]> _DummyData;

        #endregion

        #region PROPERTIES

        /*
         * The full path of this Endpoint is: 
         *      \\[ConnectionIdentifier]\Path\To\My\Example
         */

        public string Name { get { return "Example"; } }
        public string[] Path { get { return new string[] { "Path", "To", "My" }; } }

        public LocalizedText Description
        {
            get
            {
                return new LocalizedText("This is an example")
                    .Add("DE", "Das ist ein Beispiel")
                    .Add("FR", "Ceci est un exemple");
            }
        }

        #endregion

        #region PUBLIC METHODS

        public ExampleEndpoint()
        {
            // create a dummy Schema with three fields
            this._Schema = new Schema();
            this._Schema.Fields.Add(Field.New<int>("Id", isNullable: false));
            this._Schema.Fields.Add(Field.New<string>("Name", isNullable: true));
            this._Schema.Fields.Add(Field.New<bool>("IsArchived", isNullable: false));

            // create dummy data with four records
            this._DummyData = new List<object[]>() {
                new object[] { 15, "Mike", false },
                new object[] { 17, "Nancy", false },
                new object[] { 24, "Alisa", true },
                new object[] { 29, "Paul", false },
            };
        }
        
        public ReadResource GetReadResource()
        {
            // create a resource with a Schema and one question
            var res = new ReadResource();
            res.Schema = this._Schema;
            res.Questions.Add(Question.New<bool>("LoadArchived", isRequired: false, description: "Also load archived records."));

            return res;
        }

        public ReadResponse RunReadRequest(IReadRequest request)
        {
            bool loadArchived = request.Answers.GetAnswerValue<bool>("LoadArchived", false);

            var response = new ReadResponse(request);

            if (loadArchived)
            {
                // the user also would like to get the archived records
                // append all records to the RecordSet
                response.RecordSet = new RecordSet(this._Schema, this._DummyData);
            }
            else
            {
                // the user only would like to get the active records
                // only take the records that not are archived
                var listOfActiveRecords = this._DummyData.Where(r => (bool)r[2] == false);

                response.RecordSet = new RecordSet(this._Schema, listOfActiveRecords);
            }

            return response;
        }

        #endregion
    }
}
