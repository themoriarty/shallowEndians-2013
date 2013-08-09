namespace Icfpc2013.API
{
    public class Problem
    {
        public string id { get; set; }
        double size { get; set; }
        public string[] operators { get; set; }
        bool? solved { get; set; }
        double? timeLeft { get; set; }

        public override string ToString()
        {
            return Utils.ToString(this);
        }
    }
}
