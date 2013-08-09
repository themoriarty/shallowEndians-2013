namespace Icfpc2013.API
{
    public class Status
    {
        public double easyChairId { get; set; }
        public double contestScore { get; set; }
        public double lightningScore { get; set; }
        public double trainingScore { get; set; }
        public double mismatches { get; set; }
        public double numRequests { get; set; }
        public RequestWindow RequestWindow { get; set; }
        public CpuWindow cpuWindow { get; set; }

        public override string ToString()
        {
            return Utils.ToString(this);
        }
    }
}
