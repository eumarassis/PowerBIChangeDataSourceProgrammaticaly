using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeDataSourceProgrammaticaly
{
    public class PowerBIWorkspaceConfig
    {
        public PowerBIWorkspaceConfig()
        {
        }
        public string PowerBIGroupID { get; set; }

        public string PowerBIUserName { get; set; }

        public string PowerBIPassword { get; set; }

        public string DatasourceUserID { get; set; }
        public string DatasourceUserPassword { get; set; }
    }

    public static class Constants
    {

        public static readonly string AUTHORITY_URL = ConfigurationManager.AppSettings["authorityUrl"];
        public static readonly string RESOURCE_URL = ConfigurationManager.AppSettings["resourceUrl"];
        public static readonly string CLIENT_ID = ConfigurationManager.AppSettings["clientId"];
        public static readonly string API_URL = ConfigurationManager.AppSettings["apiUrl"];

 
        public static readonly PowerBIWorkspaceConfig CONTOSO_DATA = new PowerBIWorkspaceConfig()
        {
            PowerBIUserName = ConfigurationManager.AppSettings["WorkspaceContoso_pbiUsername"],
            PowerBIPassword = ConfigurationManager.AppSettings["WorkspaceContoso_pbiPassword"],
            PowerBIGroupID = ConfigurationManager.AppSettings["WorkspaceContoso_pbiGroupId"],
            DatasourceUserID = ConfigurationManager.AppSettings["WorkspaceContoso_DatasourceUserID"],
            DatasourceUserPassword = ConfigurationManager.AppSettings["WorkspaceContoso_DatasourceUserPassword"]
        };

        public static readonly PowerBIWorkspaceConfig FABRIKAM_DATA = new PowerBIWorkspaceConfig()
        {
            PowerBIUserName = ConfigurationManager.AppSettings["WorkspaceFabrikam_pbiUsername"],
            PowerBIPassword = ConfigurationManager.AppSettings["WorkspaceFabrikam_pbiPassword"],
            PowerBIGroupID = ConfigurationManager.AppSettings["WorkspaceFabrikam_pbiGroupId"],
            DatasourceUserID = ConfigurationManager.AppSettings["WorkspaceFabrikam_DatasourceUserID"],
            DatasourceUserPassword = ConfigurationManager.AppSettings["WorkspaceFabrikam_DatasourceUserPassword"]
        };

        public static readonly List<PowerBIWorkspaceConfig> WORKSPACE_LIST = new List<PowerBIWorkspaceConfig>()
        {
            CONTOSO_DATA,
            FABRIKAM_DATA
        };


    }
}
