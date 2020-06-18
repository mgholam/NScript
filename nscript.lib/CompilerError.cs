using System;

namespace NScript
{
	/// <summary>
	/// Summary description for CompilerError.
	/// </summary>
	[Serializable]
	public class CompilerError
	{
		public int Line { get; set; }
		public string File { get; set; }
		public int Column { get; set; }
		public string Text { get; set; }
		public string Number { get; set; }
		
		
		public CompilerError()
		{
		}
	
		public CompilerError(System.CodeDom.Compiler.CompilerError error)
		{
			this.Column = error.Column;
			this.File = error.FileName;
			this.Line = error.Line;
			this.Number = error.ErrorNumber;
			this.Text = error.ErrorText;
		}
	}
}
