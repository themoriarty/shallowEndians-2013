namespace Icfpc2013.API
{
    using Newtonsoft.Json;

    using System;
    using System.IO;
    using System.Net;

    public static class API
    {
        public static bool PrintDebug = true;

        private static string ServerBaseURL = "http://icfpc2013.cloudapp.net/";
        private static string AuthKey = "0228lbnS6eUkEgnFKAvtWPCsjeEJsEBaBcMKA87K";
        private static string AuthPosfix = "vpsH1H";
        private static string FullAuthStr = string.Format("{0}{1}", AuthKey, AuthPosfix);

        public static void Test()
        {
            // var status = API.GetTeamStatus();
            // Console.WriteLine(status);

            var problems = API.GetProblems();

            if (problems != null && problems.Length > 0)
            {
                foreach (var problem in problems)
                {
                    Console.WriteLine(problem);
                }
            }

            Console.ReadKey(); 
        }

        #region HttpGetPost

        private static string HttpGet(string url, out HttpStatusCode? code)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
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

        #endregion HttpGetPost

        #region GetTeamStatus

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

        #endregion GetTeamStatus

        #region GetProblems

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

        #endregion GetProblems
    }
}

