using System;
using System.Text;

namespace NScript
{
    public class Helper
	{
		public static string ShamsiDate(DateTime date)
		{
			int gy, gm, gd, sy, sd, sm, d4, d100, d400, i, j, k, ly, gyearoff;
			int[] leapcycle = new int[] { 33, 30, 26, 22, 17, 13, 9, 5, 1 };
			gyearoff = 226894; gy = date.Year; gm = date.Month; gd = date.Day;
			d4 = (gy - 1) / 4; d100 = (gy - 1) / 100; d400 = (gy - 1) / 400; ly = 0;
			if (((gy % 4 == 0) && (gy % 100 > 0)) || (gy % 400 == 0)) ly = 1;
			j = date.DayOfYear;
			i = (gy - 1) * 365 + d4 - d100 + d400 + j; i -= gyearoff; sy = (i * 33) / 12053;
			k = (sy / 33); j = k; k = sy - (j * 33); j = 0;
			foreach (int ll in leapcycle)
			{
				if (k >= ll) j++;
			}
			j += (sy / 33) * 8; sd = i - (sy * 365) - j; ly = 0; i = sy % 33;
			if (Array.IndexOf(leapcycle, i) > -1) ly = 1;
			sy++;
			if (sd <= 0)
			{
				sy--; ly = 0; i = sy % 33;
				if (Array.IndexOf(leapcycle, i) > -1) ly = 1;
				if (ly == 1) sd = 366;
				else sd = 365;
			}
			else
			{
				if (sd == 366 && ly == 0) { sy++; sd = 1; }
			}
			sm = 1;
			while ((sd > 30 && sm > 6) || (sd > 31 && sm < 7))
			{
				if (sm < 7) sd -= 31;
				if (sm > 6) sd -= 30;
				sm++;
			}

			return sy.ToString() + "/" + sm.ToString("00") + "/" + sd.ToString("00");
		}

		public static void GacInstall(string filename)
		{
			System.EnterpriseServices.Internal.Publish gac = new System.EnterpriseServices.Internal.Publish();
			gac.GacInstall(filename);
		}

        public static void GacUnInstall(string assemblyName)
        {
            System.EnterpriseServices.Internal.Publish gac = new System.EnterpriseServices.Internal.Publish();
            gac.GacRemove(assemblyName);
        }
	}

	public delegate void Function(StringBuilder ar);
}
