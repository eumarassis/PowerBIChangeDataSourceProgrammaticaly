
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;

namespace ChangeDataSourceProgrammaticaly
{
    class Program
    {
        static void Main(string[] args)
        {
            //Updates  all PowerBI Workspaces defined in the confid
            foreach (PowerBIWorkspaceConfig config in Constants.WORKSPACE_LIST)
            { 
                PowerBIClient client = null;

                //Create Power BI client for based on App.config
                client = CreatePowerBIClient(config);

                //Update data source credential on App.config
                UpdateDatasourceCredentials(config, client);
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Creates instance of Power BI client Power BI workspace's used define on  @data
        /// </summary>
        /// <param name="data"></param>
        static PowerBIClient CreatePowerBIClient(PowerBIWorkspaceConfig data)
        { // Create a user password cradentials.
            var credential = new UserPasswordCredential(data.PowerBIUserName, data.PowerBIPassword);

            // Authenticate using created credentials
            var authenticationContext = new AuthenticationContext(Constants.AUTHORITY_URL);
            var authenticationResult = authenticationContext.AcquireTokenAsync(Constants.RESOURCE_URL, Constants.CLIENT_ID, credential).Result;

            if (authenticationResult == null)
            {
                Console.WriteLine("Error during authentication");
            }

            var tokenCredentials = new Microsoft.Rest.TokenCredentials(authenticationResult.AccessToken, "Bearer");

            return new PowerBIClient(new Uri(Constants.API_URL), tokenCredentials);
            
        }

        /// <summary>
        /// Update data source credentials on Workspace defined <paramref name="config"/> using the Power BI client instance defined on <paramref name="client"/>.   
        /// </summary>
        /// <param name="config">Power BI Workspace and datasource configuration</param>
        /// <param name="client">Power BI client instance</param>
        static void UpdateDatasourceCredentials(PowerBIWorkspaceConfig config, PowerBIClient client )
        {
            var userName = config.DatasourceUserID;
            var password = config.DatasourceUserPassword;

            var credentials = string.Format("{{\"credentialData\":[{{\"name\":\"username\",\"value\":{0}}},{{\"name\":\"password\",\"value\":{1}}}]}}", JsonConvert.SerializeObject(userName), JsonConvert.SerializeObject(password));

            var requestBody = new UpdateDatasourceRequest
            {
                CredentialDetails = new CredentialDetails
                {
                    Credentials = credentials,
                    CredentialType = "Basic",
                    EncryptedConnection = "Encrypted",
                    EncryptionAlgorithm = "None",
                    PrivacyLevel = "None"
                }
            };

            var reports = client.Reports.GetReportsInGroupAsync(config.PowerBIGroupID).Result;

            foreach (Report report in reports.Value)
            {
                Console.WriteLine("Report Name: {0}, Group ID: {1}, Updating to Datasource UserName: {2}", report.Name, config.PowerBIGroupID, config.DatasourceUserID);

                try
                {
                    var dataSources = client.Datasets.GetDatasources( config.PowerBIGroupID, report.DatasetId).Value;
                    var gatewayId = dataSources.First().GatewayId;
                    var datasourceId = dataSources.First().DatasourceId;

                    client.Gateways.UpdateDatasource(gatewayId, datasourceId, requestBody);

                    Console.WriteLine("Success");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error {0}", ex.Message);
                }
            }
        }

    }
}
