using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace UniversalConfig
{

    public enum UniversalConfigTypes
    {
        Int = 0,
        Float = 1,
        String = 3,
        Bool=4,
        IntArray =5,
        FloatArray = 6,
        StringArray = 7,
        BoolArray = 8
    }
    partial class UniversalConfigMeta : IDisposable
    {
        protected static class Meta
        {
            public static readonly string s_Header = "[UCFGv2]";
            public static readonly string s_Unit = "[#1]";
            public static readonly string s_Register = "[#1:#2=#3]";
            public static readonly string s_RegisterFront = "[#1:#2=";
            public static readonly string s_RegisterBack = "]";        
            public static readonly string s_Null = "NULL";        
            public static readonly string[] s_Types = { "INT", "FLT", "STR" ,"TFV", "ITA","FTA", "SRA", "TFA" };        
            public static readonly char[] c_Forbiden = { ']','[','{', '}', ':', '=' ,'#'};        
        }
        public bool b_Disposed;
        private string s_pPath;
        public string s_Path { get { return this.s_pPath; } }
        protected Stream o_Stream;

        public string s_Error = "";
    }

    public partial class UniversalConfigCreator : UniversalConfigMeta
    {
        private string s_pFileContent = "";
        private string s_pFileContentBuild = "";
        public string S_FileContent { get { return s_pFileContentBuild; } }
    }

    public partial class UniversalConfigReader : UniversalConfigMeta
    {

    }
}
