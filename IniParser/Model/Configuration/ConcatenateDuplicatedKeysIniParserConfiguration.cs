using System;

namespace IniParser.Model.Configuration
{
	// Token: 0x02000010 RID: 16
	public class ConcatenateDuplicatedKeysIniParserConfiguration : IniParserConfiguration
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00003A21 File Offset: 0x00001C21
		public new bool AllowDuplicateKeys
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00003A24 File Offset: 0x00001C24
		public ConcatenateDuplicatedKeysIniParserConfiguration()
		{
			this.ConcatenateSeparator = ";";
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00003A37 File Offset: 0x00001C37
		public ConcatenateDuplicatedKeysIniParserConfiguration(ConcatenateDuplicatedKeysIniParserConfiguration ori) : base(ori)
		{
			this.ConcatenateSeparator = ori.ConcatenateSeparator;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00003A4C File Offset: 0x00001C4C
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00003A54 File Offset: 0x00001C54
		public string ConcatenateSeparator { get; set; }
	}
}
