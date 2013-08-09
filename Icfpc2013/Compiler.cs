namespace Icfpc2013
{
    using System;
    using System.CodeDom.Compiler;

    using Microsoft.CSharp;

    public class Compiler
    {
        #region Public Methods and Operators

        public static T Eval<T>(string exp)
        {
            var provider = new CSharpCodeProvider();
            var results = provider.CompileAssemblyFromSource(new CompilerParameters(), @"using System;
                class foo
                {
                    public static " + typeof(T).Name + @" bar()
                    {
                        return " + exp + @";
                    }
                }");
            if (results.Errors.Count > 0)
            {
                throw new FormatException(results.Errors[0].ErrorText);
            }

            return (T)results.CompiledAssembly.GetType("foo").GetMethod("bar").Invoke(null, null);
        }

        public long Run()
        {
            return Eval<long>("1+1");
        }

        #endregion
    }
}