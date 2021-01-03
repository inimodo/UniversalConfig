using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalConfig
{
    partial class UniversalConfigCreator : UniversalConfigMeta
    {
        public UniversalConfigCreator(string s_Path)
        {
            if (!File.Exists(s_Path))
            {
                this.o_Stream = File.Create(s_Path);
            }
            else
            {
                Open(s_Path);
            }
        }
        public void AppendUnit(string s_Unitname)
        {
            if (this.s_pFileContent == "")
            {
                this.s_pFileContent = Meta.s_Header + "{#unit#}";
            }

            this.s_pFileContent = this.s_pFileContent.Replace("#unit#", CreateUnit(ref s_Unitname) + "{#"+s_Unitname+"#}#unit#");
        }
        public bool AppendRegister(string s_Unitname,string s_Registername, UniversalConfigTypes i_Type )
        {
            this.s_pFileContent = this.s_pFileContent.Replace("#" + s_Unitname + "#", CreateRegister(ref s_Registername,i_Type) + "#" + s_Unitname + "#");

            return false;
        }
        public string Build()
        {
            char[] c_Content = this.s_pFileContent.ToCharArray();
            bool b_ClearMode = false;
            this.s_pFileContentBuild = "";
            for (int i_Index = 0; i_Index < c_Content.Length; i_Index++)
            {
                if (b_ClearMode && c_Content[i_Index] == '#') b_ClearMode = false;
                else if (!b_ClearMode && c_Content[i_Index] == '#') b_ClearMode = true;
                if (!(b_ClearMode || c_Content[i_Index] == '#')){
                    this.s_pFileContentBuild += c_Content[i_Index];
                }
            }

            using (StreamWriter o_Writer = new StreamWriter(this.o_Stream))
            {
                o_Writer.WriteLine(this.s_pFileContentBuild);
            }

            return this.s_pFileContent;
        }
    }

}
