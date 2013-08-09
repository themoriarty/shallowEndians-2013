namespace Icfpc2013
{
    public class Status
    {
        #region Public Properties

        public RequestWindow RequestWindow { get; set; }

        public double contestScore { get; set; }

        public CpuWindow cpuWindow { get; set; }

        public double easyChairId { get; set; }

        public double lightningScore { get; set; }

        public double mismatches { get; set; }

        public double numRequests { get; set; }

        public double trainingScore { get; set; }

        #endregion

        #region Public Methods and Operators

        public override string ToString()
        {
            return Utils.ToString(this);
        }

        #endregion
    }
}