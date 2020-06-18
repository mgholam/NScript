using System;
using System.CodeDom.Compiler;

namespace NScript
{
	/// <summary>
	/// Summary description for IScriptManagerCallback.
	/// </summary>
	public interface IScriptManagerCallback
	{
		void OnCompilerError(CompilerErrorCollection errors);
	}
}
