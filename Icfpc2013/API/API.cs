namespace Icfpc2013.API
{
    using System;
    using System.IO;
    using System.Net;

    using Newtonsoft.Json;

    public static class API
    {
        #region Static Fields

        public static bool PrintDebug = true;

        private static readonly string FullAuthStr = string.Format("{0}{1}", AuthKey, AuthPosfix);

        private static string AuthKey = "0228lbnS6eUkEgnFKAvtWPCsjeEJsEBaBcMKA87K";

        private static string AuthPosfix = "vpsH1H";

        private static string ServerBaseURL = "http://icfpc2013.cloudapp.net/";

        #endregion

        #region Public Methods and Operators

        public static Problem[] GetProblems(out HttpStatusCode? code)
        {
            try
            {
                var statusRaw = GetProblemsRaw(out code);
                return JsonConvert.DeserializeObject<Problem[]>(statusRaw);
            }
            catch (Exception ex)
            {
                if (PrintDebug)
                {
                    Console.WriteLine(ex);
                }
            }

            code = null;
            return null;
        }

        public static Problem[] GetProblems()
        {
            HttpStatusCode? code;
            return GetProblems(out code);
        }

        public static Status GetTeamStatus(out HttpStatusCode? code)
        {
            try
            {
                var statusRaw = GetTeamStatusRaw(out code);
                return JsonConvert.DeserializeObject<Status>(statusRaw);
            }
            catch (Exception ex)
            {
                if (PrintDebug)
                {
                    Console.WriteLine(ex);
                }
            }

            code = null;
            return null;
        }

        public static Status GetTeamStatus()
        {
            HttpStatusCode? code;
            return GetTeamStatus(out code);
        }

        public static void Test()
        {
            // var status = API.GetTeamStatus();
            // Console.WriteLine(status);
            var problems = GetProblems();

            if (problems != null && problems.Length > 0)
            {
                foreach (var problem in problems)
                {
                    Console.WriteLine(problem);
                }
            }

            Console.ReadKey();
        }

        #endregion

        #region Methods

        private static string GetProblemsRaw()
        {
            HttpStatusCode? code;
            return GetProblemsRaw(out code);
        }

        private static string GetProblemsRaw(out HttpStatusCode? code)
        {
            string url = string.Format("{0}/myproblems?auth={1}", ServerBaseURL, FullAuthStr);
            return HttpGet(url, out code);
        }

        private static string GetTeamStatusRaw()
        {
            HttpStatusCode? code;
            return GetTeamStatusRaw(out code);
        }

        private static string GetTeamStatusRaw(out HttpStatusCode? code)
        {
            string url = string.Format("{0}/status?auth={1}", ServerBaseURL, FullAuthStr);
            return HttpGet(url, out code);
        }

        private static string HttpGet(string url, out HttpStatusCode? code)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    code = response.StatusCode;

                    if (PrintDebug)
                    {
                        Console.WriteLine("HTTP response for URL {0} is {1} - {2}", url, response.StatusCode, response.StatusDescription);
                    }

                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (PrintDebug)
                {
                    Console.WriteLine(ex);
                }
            }

            code = null;
            return null;
        }

        #endregion
    }
}