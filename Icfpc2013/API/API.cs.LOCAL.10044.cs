using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Icfpc2013
{
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

            /*
            var problems = API.GetRealProblems();

            if (problems != null && problems.Length > 0)
            {
                foreach (var problem in problems)
                {
                    Console.WriteLine(problem);
                }
            }
            */

            var trainingProblem = GetTrainingProblem(new TrainRequest(4, new string[]{}));
            Console.WriteLine(trainingProblem);

            Console.ReadKey(); 
        }

        #region HttpGetPost

        private static string HttpPost(string url, Object obj, out HttpStatusCode? code)
        {
            try
            {
                string json = JsonConvert.SerializeObject(obj);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";

                var outStream = request.GetRequestStream();

                using (var writer = new StreamWriter(outStream))
                {
                    writer.Write(json);
                }

                outStream.Close();

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    code = response.StatusCode;

                    if (PrintDebug)
                    {
                        Console.WriteLine("HTTP response for URL {0} is {1} - {2}", url, response.StatusCode, response.StatusDescription);
                    }

                    using (var inStream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(inStream))
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
            string url = string.Format("{0}status?auth={1}", ServerBaseURL, FullAuthStr);
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

        #region GetRealProblems

        private static string GetRealProblemsRaw()
        {
            HttpStatusCode? code;
            return GetRealProblemsRaw(out code);
        }

        private static string GetRealProblemsRaw(out HttpStatusCode? code)
        {
            string url = string.Format("{0}myproblems?auth={1}", ServerBaseURL, FullAuthStr);
            return HttpGet(url, out code);
        }

        public static Problem[] GetRealProblems(out HttpStatusCode? code)
        {
            try
            {
                var statusRaw = GetRealProblemsRaw(out code);
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

        public static Problem[] GetRealProblems()
        {
            HttpStatusCode? code;
            return GetRealProblems(out code);
        }

        #endregion GetRealProblems

        #region GetTrainingProblem

        private static string GetTrainingProblemRaw(TrainRequest trainRequest)
        {
            HttpStatusCode? code;
            return GetTrainingProblemRaw(trainRequest, out code);
        }

        private static string GetTrainingProblemRaw(TrainRequest trainRequest, out HttpStatusCode? code)
        {
            string url = string.Format("{0}train?auth={1}", ServerBaseURL, FullAuthStr);
            return HttpGet(url, out code);
        }

        public static TrainingProblem GetTrainingProblem(TrainRequest trainRequest, out HttpStatusCode? code)
        {
            try
            {
                var trainingProblemRaw = GetTrainingProblemRaw(trainRequest, out code);
                return JsonConvert.DeserializeObject<TrainingProblem>(trainingProblemRaw);
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

        public static TrainingProblem GetTrainingProblem(TrainRequest trainRequest)
        {
            HttpStatusCode? code;
            return GetTrainingProblem(trainRequest, out code);
        }

        #endregion GetTrainingProblem
    }
}

