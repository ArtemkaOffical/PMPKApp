using System;
using IniParser.Model;
using IniParser.Model.Configuration;

namespace IniParser.Parser
{
	// Token: 0x02000007 RID: 7
	public class ConcatenateDuplicatedKeysIniDataParser : IniDataParser
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000028FD File Offset: 0x00000AFD
		// (set) Token: 0x06000033 RID: 51 RVA: 0x0000290A File Offset: 0x00000B0A
		public new ConcatenateDuplicatedKeysIniParserConfiguration Configuration
		{
			get
			{
				return (ConcatenateDuplicatedKeysIniParserConfiguration)base.Configuration;
			}
			set
			{
				base.Configuration = value;
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002913 File Offset: 0x00000B13
		public ConcatenateDuplicatedKeysIniDataParser() : this(new ConcatenateDuplicatedKeysIniParserConfiguration())
		{
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002920 File Offset: 0x00000B20
		public ConcatenateDuplicatedKeysIniDataParser(ConcatenateDuplicatedKeysIniParserConfiguration parserConfiguration) : base(parserConfiguration)
		{
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000292C File Offset: 0x00000B2C
		protected override void HandleDuplicatedKeyInCollection(string key, string value, KeyDataCollection keyDataCollection, string sectionName)
		{
			keyDataCollection[key] = keyDataCollection[key] + this.Configuration.ConcatenateSeparator + value;
		}
	}
}
