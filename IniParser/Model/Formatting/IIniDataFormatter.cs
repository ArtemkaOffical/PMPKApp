using System;
using IniParser.Model.Configuration;

namespace IniParser.Model.Formatting
{
	/// <summary>
	///     Formats a IniData structure to an string
	/// </summary>
	// Token: 0x02000012 RID: 18
	public interface IIniDataFormatter
	{
		/// <summary>
		///     Produces an string given
		/// </summary>
		/// <returns>The data to string.</returns>
		/// <param name="iniData">Ini data.</param>
		// Token: 0x060000C6 RID: 198
		string IniDataToString(IniData iniData);

		/// <summary>
		///     Configuration used by this formatter when converting IniData
		///     to an string
		/// </summary>
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000C7 RID: 199
		// (set) Token: 0x060000C8 RID: 200
		IniParserConfiguration Configuration { get; set; }
	}
}
