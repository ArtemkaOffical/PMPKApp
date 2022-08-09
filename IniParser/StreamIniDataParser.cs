using System;
using System.IO;
using IniParser.Model;
using IniParser.Model.Formatting;
using IniParser.Parser;

namespace IniParser
{
	/// <summary>
	///     Represents an INI data parser for streams.
	/// </summary>
	// Token: 0x02000003 RID: 3
	public class StreamIniDataParser
	{
		/// <summary>
		///     This instance will handle ini data parsing and writing
		/// </summary>
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000021B4 File Offset: 0x000003B4
		// (set) Token: 0x0600000A RID: 10 RVA: 0x000021BC File Offset: 0x000003BC
		public IniDataParser Parser { get; protected set; }

		/// <summary>
		///     Ctor
		/// </summary>
		// Token: 0x0600000B RID: 11 RVA: 0x000021C5 File Offset: 0x000003C5
		public StreamIniDataParser() : this(new IniDataParser())
		{
		}

		/// <summary>
		///     Ctor
		/// </summary>
		/// <param name="parser"></param>
		// Token: 0x0600000C RID: 12 RVA: 0x000021D2 File Offset: 0x000003D2
		public StreamIniDataParser(IniDataParser parser)
		{
			this.Parser = parser;
		}

		/// <summary>
		///     Reads data in INI format from a stream.
		/// </summary>
		/// <param name="reader">Reader stream.</param>
		/// <returns>
		///     And <see cref="T:IniParser.Model.IniData" /> instance with the readed ini data parsed.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">
		///     Thrown if <paramref name="reader" /> is <c>null</c>.
		/// </exception>
		// Token: 0x0600000D RID: 13 RVA: 0x000021E1 File Offset: 0x000003E1
		public IniData ReadData(StreamReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			return this.Parser.Parse(reader.ReadToEnd());
		}

		/// <summary>
		///     Writes the ini data to a stream.
		/// </summary>
		/// <param name="writer">A write stream where the ini data will be stored</param>
		/// <param name="iniData">An <see cref="T:IniParser.Model.IniData" /> instance.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///     Thrown if <paramref name="writer" /> is <c>null</c>.
		/// </exception>
		// Token: 0x0600000E RID: 14 RVA: 0x00002202 File Offset: 0x00000402
		public void WriteData(StreamWriter writer, IniData iniData)
		{
			if (iniData == null)
			{
				throw new ArgumentNullException("iniData");
			}
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			writer.Write(iniData.ToString());
		}

		/// <summary>
		///     Writes the ini data to a stream.
		/// </summary>
		/// <param name="writer">A write stream where the ini data will be stored</param>
		/// <param name="iniData">An <see cref="T:IniParser.Model.IniData" /> instance.</param>
		/// <param name="formatter">Formaterr instance that controls how the ini data is transformed to a string</param>
		/// <exception cref="T:System.ArgumentNullException">
		///     Thrown if <paramref name="writer" /> is <c>null</c>.
		/// </exception>
		// Token: 0x0600000F RID: 15 RVA: 0x0000222C File Offset: 0x0000042C
		public void WriteData(StreamWriter writer, IniData iniData, IIniDataFormatter formatter)
		{
			if (formatter == null)
			{
				throw new ArgumentNullException("formatter");
			}
			if (iniData == null)
			{
				throw new ArgumentNullException("iniData");
			}
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			writer.Write(iniData.ToString(formatter));
		}
	}
}
