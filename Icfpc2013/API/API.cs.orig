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

<<<<<<< HEAD
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
=======
        private static string AuthPosfix = "vpsH1H";

        private static string ServerBaseURL = "http://icfpc2013.cloudapp.net/";
>>>>>>> b4d89841ce1f89de29913308070fbcd43e51d395

        #endregion

        #region Public Methods and Operators

<<<<<<< HEAD
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
=======
        public static Problem[] GetProblems(out HttpStatusCode? code)
>>>>>>> b4d89841ce1f89de29913308070fbcd43e51d395
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
<<<<<<< HEAD
            return GetTeamStatusRaw(out code);
        }

        private static string GetTeamStatusRaw(out HttpStatusCode? code)
        {
            string url = string.Format("{0}status?auth={1}", ServerBaseURL, FullAuthStr);
            return HttpGet(url, out code);
=======
            return GetProblems(out code);
>>>>>>> b4d89841ce1f89de29913308070fbcd43e51d395
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

<<<<<<< HEAD
        #region GetRealProblems
=======
        #region Methods
>>>>>>> b4d89841ce1f89de29913308070fbcd43e51d395

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

<<<<<<< HEAD
        public static Problem[] GetRealProblems(out HttpStatusCode? code)
        {
            try
            {
                var statusRaw = GetRealProblemsRaw(out code);
                return JsonConvert.DeserializeObject<Problem[]>(statusRaw);
=======
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
>>>>>>> b4d89841ce1f89de29913308070fbcd43e51d395
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

<<<<<<< HEAD
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
=======
        #endregion
>>>>>>> b4d89841ce1f89de29913308070fbcd43e51d395
    }
}
