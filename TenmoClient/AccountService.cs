using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TenmoClient.Data;

namespace TenmoClient
{
    public class AccountService
    {
        public readonly string API_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();

        public API_Account GetAccount(int id)
        {
            RestRequest request = new RestRequest(API_URL + "accounts/" + id);
            IRestResponse<API_Account> response = client.Get<API_Account>(request);

            if (response.ResponseStatus !=ResponseStatus.Completed)
            {
                throw new HttpRequestException("Error Occurred - Unable to reach server");
            }
            else if (!response.IsSuccessful)
            {
                throw new HttpRequestException("Error occurred - received non-success response; " + (int)response.StatusCode);
            }
            else 
            { 
                return response.Data;
            }
        }
    }
}
