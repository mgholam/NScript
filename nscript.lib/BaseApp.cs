using System;
using System.IO;
using System.Resources;
using System.Reflection;

namespace NScript
{
    /// <summary>
    /// Summary description for BaseApp.
    /// </summary>
    public abstract class BaseApp
    {
        private static ResourceManager resMgr = new ResourceManager("NScript.NScript", typeof(BaseApp).Assembly);
        private string fileName;

        public BaseApp()
        {
        }

        protected abstract void ExecutionLoop(IAsyncResult result);
        protected abstract void ShowErrorMessage(string message);

        protected string EntryAssemblyName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
            }
        }

        protected static object GetResourceObject(string name)
        {
            return resMgr.GetObject(name);
        }

        delegate void CompileAndExecuteRoutine(string file, string[] args);

        private void CompileAndExecute(string file, string[] args)
        {
            try
            {
                ScriptManager.CompileAndExecuteFile(file, args);
            }
            catch (Exception e)
            {
                ShowErrorMessage(e.InnerException.ToString());
                Environment.Exit(1);
            }
            Environment.Exit(0);
        }

        protected void Run(string[] args)
        {
            if (args.Length < 1)
            {
                ShowErrorMessage("Usage : [/c] scriptfile  arg1 arg2 ...");
                ShowErrorMessage("    /c -> compile script to exe");
                ShowErrorMessage("    references in script -> // ref : mylib.dll");
                return;
            }

            if(args[0] == "/c")
            {
                ScriptManager.CompileAndExecuteFile(args[1], args, true);
                return;
            }

            fileName = args[0];

            if (!File.Exists(fileName))
            {
                ShowErrorMessage("File does not exist : " + args[0]);
                return;
            }

            //Create new argument array removing the file name
            string[] newargs = new String[args.Length - 1];
            Array.Copy(args, 1, newargs, 0, args.Length - 1);

            CompileAndExecuteRoutine asyncDelegate = new CompileAndExecuteRoutine(this.CompileAndExecute);
            IAsyncResult result = asyncDelegate.BeginInvoke(fileName, newargs, null, null);

            //For a windows app a message loop and for a console app a simple wait
            ExecutionLoop(result);

            asyncDelegate.EndInvoke(result);
        }
    }
}
