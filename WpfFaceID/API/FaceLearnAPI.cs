using System;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPython.Compiler;
using IronPython.Hosting;
using IronPython.Runtime;
using IronPython;


namespace WpfFaceAPI
{
    public class FaceLearnAPI
    {
        private ScriptEngine engine = Python.CreateEngine();
        private ScriptScope scope = null;

        delegate void VoidFunc(string val);

 

    }
}
