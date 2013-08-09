namespace Icfpc2013.API
{
    public class RequestWindow
    {
        #region Public Properties

        public double amount { get; set; }

        public double limit { get; set; }

        public double resetsIn { get; set; }

        #endregion

        #region Public Methods and Operators

        public override string ToString()
        {
            return Utils.ToString(this);
        }

        #endregion
    }
}