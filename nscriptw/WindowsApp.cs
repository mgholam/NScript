using System;
using System.Windows.Forms;

namespace NScript
{
	/// <summary>
	/// Summary description for WindowsApp.
	/// </summary>
	public class WindowsApp : BaseApp
	{
		private static NotifyIcon icon;
		private Timer timer;
		private System.Drawing.Icon [] icons;
		private int currentIconIndex = 0;

		public WindowsApp()
		{
		}

		protected override void ShowErrorMessage(string message)
		{
			MessageBox.Show(message, EntryAssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		protected override void ExecutionLoop(System.IAsyncResult result)
		{
			icon = new NotifyIcon();
			timer = new Timer();
			icons = new System.Drawing.Icon[4];
			
			for(int i = 0; i < 4; i++)
			{
				icons[i] = (System.Drawing.Icon)GetResourceObject("AnimationIcon" + (i + 1).ToString());
			}

			icon.Icon = icons[currentIconIndex];
            icon.Text = "Nscript: Double click on this icon to stop the execution";// GetResourceString("IconTip");
			icon.Visible = true;
			icon.DoubleClick += new EventHandler(this.OnIconDoubleClick);

			timer.Tick += new EventHandler(this.OnTimerTick);
			timer.Interval = 100;
			timer.Start();

			Application.Run();
			
			icon.Dispose();
			timer.Dispose();
		}

		private void OnIconDoubleClick(object sender, EventArgs e)
		{
			if (MessageBox.Show("Execution will be canceled. Are you sure?", EntryAssemblyName, MessageBoxButtons.YesNo) == DialogResult.Yes)
                Application.Exit();
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			//Change the icon
			currentIconIndex++;
			
			if (currentIconIndex == 4)
				currentIconIndex = 0;

			icon.Icon = icons[currentIconIndex];
			icon.Visible = true;
		}

		[STAThread]
		public static void Main(string[] args)
		{
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
			new WindowsApp().Run(args);
		}

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            icon.Visible = false;
            MessageBox.Show(e.ToString());            
            Application.Exit();
        }
	}
}
