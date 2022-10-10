using HomenetBase;
using RestSharp;
using RestSharp.Authenticators;
using System;

namespace HomenetClient
{
    public class HomenetConnector
    {
        #region ------------- Exceptions ----------------------------------------------------------
        public class ConnectionException : Exception
        {
            public ConnectionException(string message) : base(message) 
            { 
            }

            public override string ToString()
            {
                return base.Message;
            }
        }

        private int _timeoutInSeconds;
        #endregion



        #region ------------- Fields --------------------------------------------------------------
        private RestClient _RestClient;
        #endregion



        #region ------------- Init ----------------------------------------------------------------
        public HomenetConnector(string url, string userName, string password, int timeoutInSeconds = 30)
        {
            _timeoutInSeconds = timeoutInSeconds;
            _RestClient = new RestClient(url);

            var passwordHashBase64 = Authentication.Calculate_SHA256_base64(password);
            _RestClient.Authenticator = new HttpBasicAuthenticator(userName, passwordHashBase64);
        }
        #endregion



        #region ------------- Methods -------------------------------------------------------------
        public T Get<T>(string path) where T : new()
        {
            var request = new RestRequest(path, Method.Get);
            request.Timeout = _timeoutInSeconds * 1000;
            RestResponse<T> response = _RestClient.Execute<T>(request);
            ErrorHandling<T>(response);
            return response.Data;
        }

        public RestResponse Post<T>(string path, object item) where T : new()
        {
            var request = new RestRequest(path, Method.Post);
            request.Timeout = _timeoutInSeconds * 1000;
			request.AddJsonBody(item);
            var response = _RestClient.Execute<T>(request);
            ErrorHandling<T>(response);
			return response;
        }

        public RestResponse Post(string path, object item)
        {
            var request = new RestRequest(path, Method.Post);
            request.Timeout = _timeoutInSeconds * 1000;
			request.AddJsonBody(item);
            var response = _RestClient.Execute(request);
            ErrorHandling(response);
			return response;
        }

        public RestResponse Put(string path, object item)
        {
            var request = new RestRequest(path, Method.Put);
            request.Timeout = _timeoutInSeconds * 1000;
            request.AddJsonBody(item);
            var response = _RestClient.Execute(request);
            ErrorHandling(response);
            return response;
        }

        internal RestResponse Delete(string path, object item)
        {
            var request = new RestRequest(path, Method.Delete);
            request.Timeout = _timeoutInSeconds * 1000;
            request.AddJsonBody(item);
            var response = _RestClient.Execute(request);
            ErrorHandling(response);
            return response;
        }

        #endregion



        #region ------------- Implementation ------------------------------------------------------
        private void ErrorHandling<T>(RestResponse<T> response) where T : new()
        {
            if (response.ResponseStatus == ResponseStatus.Aborted)
                throw new ConnectionException("HTTP request was aborted");
            if (response.ResponseStatus == ResponseStatus.TimedOut)
                throw new ConnectionException("HTTP request was timed out");
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                if (response.StatusCode == 0)
                {
                    throw new ConnectionException(
                        $"No connection could be established with the server.\n\n" +
                        "Please check server name, user and password\n" +
                        "Try to login with these in a browser and check if username and password are ok\n\n");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new ConnectionException($"Internal server error! (500)");
                }
                else
                {
                    throw new ConnectionException(
                        $"No connection could be established with the server.\n" +
                        $"The server was reachable, but responded with status {response.StatusCode.ToString()} (status code {response.StatusCode})\n" +
                        $"The response status was {response.ResponseStatus.ToString()}\n\n" +
                        "Please check server name, user and password\n" +
                        "Try to login with these in a browser and check if username and password are ok\n\n");
                }
            }
        }

        private void ErrorHandling(RestResponse response)
        {
            if (response.ResponseStatus == ResponseStatus.Aborted)
                throw new ConnectionException("HTTP request was aborted");
            if (response.ResponseStatus == ResponseStatus.TimedOut)
                throw new ConnectionException("HTTP request was timed out");
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                if (response.StatusCode == 0)
                {
                    throw new ConnectionException(
                        $"No connection could be established with the server.\n\n" +
                        "Please check server name, user and password\n" +
                        "Try to login with these in a browser and check if username and password are ok\n\n");
                }
                else
                {
                    throw new ConnectionException(
                        $"No connection could be established with the server.\n" +
                        $"The server was reachable, but responded with status {response.StatusCode.ToString()} (status code {response.StatusCode})\n" +
                        $"The response status was {response.ResponseStatus.ToString()}\n\n" +
                        "Please check server name, user and password\n" +
                        "Try to login with these in a browser and check if username and password are ok\n\n");
                }
            }
        }
        #endregion
    }
}
