using System;
using IniParser.Model;
using IniParser.Parser;

namespace IniParser
{
	/// <summary>
	///     Represents an INI data parser for strings.
	///
	/// </summary>
	/// <remarks>
	///     This class is deprecated and kept for backwards compatibility.
	///     It's just a wrapper around <see cref="T:IniParser.Parser.IniDataParser" /> class.
	///     Please, replace your code.
	/// </remarks>
	// Token: 0x02000004 RID: 4
	[Obsolete("Use class IniDataParser instead. See remarks comments in this class.")]
	public class StringIniParser
	{
		/// <summary>
		///     This instance will handle ini data parsing and writing
		/// </summary>
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002265 File Offset: 0x00000465
		// (set) Token: 0x06000011 RID: 17 RVA: 0x0000226D File Offset: 0x0000046D
		public IniDataParser Parser { get; protected set; }

		/// <summary>
		///     Ctor
		/// </summary>
		// Token: 0x06000012 RID: 18 RVA: 0x00002276 File Offset: 0x00000476
		public StringIniParser() : this(new IniDataParser())
		{
		}

		/// <summary>
		///     Ctor
		/// </summary>
		/// <param name="parser"></param>
		// Token: 0x06000013 RID: 19 RVA: 0x00002283 File Offset: 0x00000483
		public StringIniParser(IniDataParser parser)
		{
			this.Parser = parser;
		}

		/// <summary>
		/// Parses a string containing data formatted as an INI file.
		/// </summary>
		/// <param name="dataStr">The string containing the data.</param>
		/// <returns>
		/// A new <see cref="T:IniParser.Model.IniData" /> instance with the data parsed from the string.
		/// </returns>
		// Token: 0x06000014 RID: 20 RVA: 0x00002292 File Offset: 0x00000492
		public IniData ParseString(string dataStr)
		{
			return this.Parser.Parse(dataStr);
		}

		/// <summary>
		/// Creates a string from the INI data.
		/// </summary>
		/// <param name="iniData">An <see cref="T:IniParser.Model.IniData" /> instance.</param>
		/// <returns>
		/// A formatted string with the contents of the
		/// <see cref="T:IniParser.Model.IniData" /> instance object.
		/// </returns>
		// Token: 0x06000015 RID: 21 RVA: 0x000022A0 File Offset: 0x000004A0
		public string WriteString(IniData iniData)
		{
			return iniData.ToString();
		}
	}
}
