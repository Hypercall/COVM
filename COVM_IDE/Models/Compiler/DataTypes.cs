using System;
using System.Collections.Generic;
using System.Text;

namespace COVM_IDE.Models.Compiler
{
    class DataTypes
    {
        public enum Type : byte
        {
            BYTE = 1,
            WORD = 2,
            INTEGER = 4,
            LONG = 8
        }

		public static Type TranslateSize(Int64 val)
		{
			// 64 bit
			if (val > 2147483647 && val < 9223372036854775807)
				return Type.LONG;
			// 32 bit
			if (val > 32767 && val < 2147483647)
				return Type.INTEGER;
			// 16 bit
			if (val > 255 && val < 32767)
				return Type.WORD;
			// 8 bit
			if (val > 0 && val < 255)
				return Type.BYTE;
			return Type.BYTE;
		}
	}
}
