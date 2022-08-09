using System;
using System.Reflection;

namespace IniParser.Exceptions
{
	/// <summary>
	/// Represents an error ococcurred while parsing data 
	/// </summary>
	// Token: 0x02000005 RID: 5
	public class ParsingException : Exception
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000016 RID: 22 RVA: 0x000022A8 File Offset: 0x000004A8
		// (set) Token: 0x06000017 RID: 23 RVA: 0x000022B0 File Offset: 0x000004B0
		public Version LibVersion { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000022B9 File Offset: 0x000004B9
		// (set) Token: 0x06000019 RID: 25 RVA: 0x000022C1 File Offset: 0x000004C1
		public int LineNumber { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000022CA File Offset: 0x000004CA
		// (set) Token: 0x0600001B RID: 27 RVA: 0x000022D2 File Offset: 0x000004D2
		public string LineValue { get; private set; }

		// Token: 0x0600001C RID: 28 RVA: 0x000022DB File Offset: 0x000004DB
		public ParsingException(string msg) : this(msg, 0, string.Empty, null)
		{
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000022EB File Offset: 0x000004EB
		public ParsingException(string msg, Exception innerException) : this(msg, 0, string.Empty, innerException)
		{
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000022FB File Offset: 0x000004FB
		public ParsingException(string msg, int lineNumber, string lineValue) : this(msg, lineNumber, lineValue, null)
		{
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002308 File Offset: 0x00000508
		public ParsingException(string msg, int lineNumber, string lineValue, Exception innerException) : base(string.Format("{0} while parsing line number {1} with value '{2}' - IniParser version: {3}", new object[]
		{
			msg,
			lineNumber,
			lineValue,
			Assembly.GetExecutingAssembly().GetName().Version
		}), innerException)
		{
			this.LibVersion = Assembly.GetExecutingAssembly().GetName().Version;
			this.LineNumber = lineNumber;
			this.LineValue = lineValue;
		}
	}
}
