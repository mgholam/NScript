using System;
using System.Runtime.InteropServices;

namespace NScript
{
	/// <summary>
	/// Summary description for ConsoleApp.
	/// </summary>
	public class ConsoleApp : BaseApp
	{
		public ConsoleApp()
		{
		}
		
		delegate bool CtrlHandlerRoutine(int eventType);

		[DllImport("kernel32.dll", SetLastError=true)]
        extern static bool SetConsoleCtrlHandler(CtrlHandlerRoutine handler);		
		
		private bool CtrlHandler(int eventType)
		{
			Console.Write("Execution will be canceled. Are you sure (Y/N)?");
		
			if (String.Compare(Console.ReadLine(), "Y", true) == 0)
			{
                Environment.Exit(1);
			}
		
			return true;
		}

		protected override void ShowErrorMessage(string message)
		{
			Console.Error.WriteLine(message);
		}

		protected override void ExecutionLoop(System.IAsyncResult result)
		{
			result.AsyncWaitHandle.WaitOne();
		}

        [STAThread]
		public static void Main(string[] args)
		{
			ConsoleApp app = new ConsoleApp();
			CtrlHandlerRoutine handler = new CtrlHandlerRoutine(app.CtrlHandler);
			
			//Install a console ctrl handler so that we may unload the exceution domain
			//If the user wants to cancel by pressing Ctrl+C
			SetConsoleCtrlHandler(handler);
			app.Run(args);
			
			GC.KeepAlive(handler);
		}
	}
}
