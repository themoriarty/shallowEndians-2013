namespace Icfpc2013
{
    internal class NodeId : Node
    {
        public long Eval()
        {
            throw new System.NotImplementedException();
        }


        public string ToString(int indentLevel)
        {
            return Name;
        }

        public string Name { get; set; }

        public long Cost()
        {
            return 1;
        }
    }
}