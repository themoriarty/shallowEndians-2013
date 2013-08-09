namespace Icfpc2013
{
    public class Problem
    {
        #region Public Properties

        public string id { get; set; }

        public string[] operators { get; set; }

        #endregion

        #region Properties

        private double size { get; set; }

        private bool? solved { get; set; }

        private double? timeLeft { get; set; }

        #endregion

        #region Public Methods and Operators

        public override string ToString()
        {
            return Utils.ToString(this);
        }

        #endregion
    }
}