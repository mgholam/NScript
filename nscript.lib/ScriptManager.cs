using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace NScript
{
    /// <summary>
    /// Summary description for ScriptManager.
    /// </summary>
    public class ScriptManager
    {
        static List<string> _refs = new List<string>();

        private static void AddReferencesFromFile(CompilerParameters compilerParams, Stream embed)
        {
            StreamReader reader = new StreamReader(embed);
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                if (File.Exists(line))
                    compilerParams.ReferencedAssemblies.Add(line);
                else
                {
                    Assembly a = Assembly.LoadWithPartialName(Path.GetFileNameWithoutExtension(line));
                    if (a != null)
                        compilerParams.ReferencedAssemblies.Add(a.Location);
                    else
                        throw new Exception("Reference not found : " + line);
                }
            }
        }

        private static void AddReferencesFromFile(CompilerParameters compilerParams, string nrfFile)
        {
            AddReferencesFromFile(compilerParams, File.OpenRead(nrfFile));
        }

        public static void CompileAndExecuteFile(string file, string[] args)
        {
            CompileAndExecuteFile(file, args, false);
        }

        public static void CompileAndExecuteFile(string file, string[] args, bool createexe)
        {
            //Currently only csharp scripting is supported
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            CodeDomProvider compiler = CodeDomProvider.CreateProvider("CSharp");

            CompilerParameters compilerparams = new CompilerParameters();
            compilerparams.GenerateInMemory = true;
            compilerparams.GenerateExecutable = true;
            compilerparams.CompilerOptions = "/unsafe";
            if (createexe)
                compilerparams.OutputAssembly = file.Substring(0, file.LastIndexOf('.')) + ".exe";

            //Add assembly references from nscript.nrf or <file>.nrf
            string nrfFile = Path.ChangeExtension(file, "nrf");

            Regex regex = new Regex(
                @"\/\/\s*ref\s*\:\s*(?<refs>.*)",
                 RegexOptions.IgnoreCase);

            compilerparams.ReferencedAssemblies.Add(typeof(ScriptManager).Assembly.Location);

            foreach (Match m in regex.Matches(File.ReadAllText(file)))
            {
                string str = m.Groups["refs"].Value.Trim();
                if (str == "" || str.StartsWith("//"))
                    continue;
                _refs.Add(str);
                Assembly a = Assembly.LoadFrom(str);
                if (a != null)
                    compilerparams.ReferencedAssemblies.Add(a.Location);
            }
            //else
            {
                // load SCRIPT refs
                if (File.Exists(nrfFile))
                    AddReferencesFromFile(compilerparams, nrfFile);

                // load default nscript.nrf
                nrfFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "nscript.nrf");

                if (File.Exists(nrfFile))
                    AddReferencesFromFile(compilerparams, nrfFile);
                else
                {
                    // use embedded file here
                    AddReferencesFromFile(compilerparams,
                        Assembly.GetExecutingAssembly().GetManifestResourceStream("nscript.NScript.nrf"));
                }
            }
            CompilerResults results = compiler.CompileAssemblyFromFile(compilerparams, file);

            if (results.Errors.HasErrors == true)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var e in results.Errors)
                    sb.AppendLine(e.ToString());
                throw new Exception(sb.ToString());
            }
            else if(createexe==false)
                results.CompiledAssembly.EntryPoint.Invoke(null, BindingFlags.Static, null, new object[] { args }, null);
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (File.Exists(args.Name))
                return Assembly.LoadFrom(args.Name);
            string[] ss = args.Name.Split(',');
            string fname = ss[0] + ".dll";
            if (File.Exists(fname))
                return Assembly.LoadFrom(fname);
            else
            {
                foreach (var s in _refs)
                {
                    if (s.ToLower().Contains(fname.ToLower()))
                        return Assembly.LoadFrom(s);
                }
            }

            return null;// Assembly.GetExecutingAssembly();
        }
    }
}
