namespace Icfpc2013.API
{
    public class RequestWindow
    {
        public double resetsIn { get; set; }
        public double amount { get; set; }
        public double limit { get; set; }

        public override string ToString()
        {
            return Utils.ToString(this);
        }
    }
}
