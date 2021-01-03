using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace UniversalConfig
{
    public partial class UniversalConfigReader : UniversalConfigMeta
    {
        public UniversalConfigReader(string i_Path)
        {
            Open(i_Path);
        }
        public bool LoadConfig()
        {
            try
            {
                StreamReader o_Reader = new StreamReader(this.o_Stream);
                this.o_Stream.Seek(0, 0);
                this.s_pContent = o_Reader.ReadToEnd();
            }
            catch (Exception o_Exception)
            {
                this.s_Error = o_Exception.Message;
                return false;
            }
            return true;
            
        }
        public bool SaveConfig()
        {
            try
            {
                StreamWriter o_Writer = new StreamWriter(this.o_Stream);
                this.o_Stream.Seek(0, 0);
                o_Writer.Write(this.s_pContent);
                o_Writer.Flush();
            }
            catch (Exception o_Exception)
            {
                this.s_Error = o_Exception.Message;
                return false;
            }
            return true;
        }
        public string GetRawValue(string s_Unitname,string s_Register, UniversalConfigTypes i_Type )
        {
            string s_Output="";
            if (this.s_pContent != null)
            {
                char[] c_Value = this.s_pContent.ToCharArray();
                string s_RawUnit = CreateUnit(ref s_Unitname);
                string s_RawRegister = CreateRegister(ref s_Register, i_Type).Split('=')[0];

                if (!this.s_pContent.Contains(s_RawUnit) || !this.s_pContent.Contains(s_RawRegister)) return null;

                int i_UnitIndex= this.s_pContent.IndexOf(s_RawUnit);
                int i_RegIndex= this.s_pContent.IndexOf(s_RawRegister, i_UnitIndex)+ s_RawRegister.Length+1;
                for (int i_StopIndex = i_RegIndex; i_StopIndex < c_Value.Length && c_Value[i_StopIndex] != ']'; i_StopIndex++)
                {
                    s_Output += c_Value[i_StopIndex];
                }
            }
            else return null;
            return s_Output;
        }
        public void SetRawValue(string s_Unitname, string s_Register, UniversalConfigTypes i_Type,string s_Value="NULL")
        {
            string s_Output = "";
            if (this.s_pContent != null)
            {
                char[] c_Value = this.s_pContent.ToCharArray();
                string s_RawUnit = CreateUnit(ref s_Unitname);
                string s_RawRegister = CreateRegister(ref s_Register, i_Type).Split('=')[0];

                if (!this.s_pContent.Contains(s_RawUnit) || !this.s_pContent.Contains(s_RawRegister)) return ;

                int i_UnitIndex = this.s_pContent.IndexOf(s_RawUnit);
                int i_RegIndex = this.s_pContent.IndexOf(s_RawRegister, i_UnitIndex) + s_RawRegister.Length + 1;
                for (int i_StopIndex = i_RegIndex; i_StopIndex < c_Value.Length && c_Value[i_StopIndex] != ']'; i_StopIndex++)
                {
                    s_Output += c_Value[i_StopIndex];
                }

                string s_OldRegister = CreateRegister(ref s_Register, i_Type, s_Output);
                string s_NewRegister = CreateRegister(ref s_Register, i_Type, s_Value);
                this.s_pContent = this.s_Content.Replace(s_OldRegister, s_NewRegister);
            }
        }
        public void SetValue(string s_Unitname, string s_Register, int i_Value)
        {
            SetRawValue(s_Unitname, s_Register, UniversalConfigTypes.Int,i_Value.ToString());
        }
        public void SetValue(string s_Unitname, string s_Register, float f_Value)
        {
            SetRawValue(s_Unitname, s_Register, UniversalConfigTypes.Float, f_Value.ToString());

        }
        public void SetValue(string s_Unitname, string s_Register, string s_Value)
        {
            SetRawValue(s_Unitname, s_Register, UniversalConfigTypes.String, s_Value);
        }
        public void SetValue(string s_Unitname, string s_Register, int[] i_Value)
        {

            string s_Values = i_Value[0].ToString();
            for (int i_Index = 1; i_Index < i_Value.Length; i_Index++)
            {
                s_Values += "|" + i_Value[i_Index].ToString();
            }
            SetRawValue(s_Unitname, s_Register, UniversalConfigTypes.IntArray, s_Values);

        }
        public void SetValue(string s_Unitname, string s_Register, float[] f_Value)
        {
            string s_Values = f_Value[0].ToString();
            for (int i_Index = 1; i_Index < f_Value.Length; i_Index++)
            {
                s_Values += "|" + f_Value[i_Index].ToString(); 
            }
            SetRawValue(s_Unitname, s_Register, UniversalConfigTypes.FloatArray, s_Values);
        }
        public void SetValue(string s_Unitname, string s_Register, string[] s_Value)
        {
            string s_Values = s_Value[0];
            for (int i_Index = 1; i_Index < s_Value.Length; i_Index++)
            {
                s_Values += "|" + s_Value[i_Index];
            }
            SetRawValue(s_Unitname, s_Register, UniversalConfigTypes.StringArray, s_Values);
        }


        public bool GetValue(string s_Unitname, string s_Register, ref int i_Value)
        {
            return int.TryParse(GetRawValue(s_Unitname, s_Register, UniversalConfigTypes.Int),out i_Value);
        }
        public bool GetValue(string s_Unitname, string s_Register, ref float f_Value)
        {
            return float.TryParse(GetRawValue(s_Unitname, s_Register, UniversalConfigTypes.Float), out f_Value);
        }
        public bool GetValue(string s_Unitname, string s_Register, ref string s_Value)
        {
            s_Value = GetRawValue(s_Unitname, s_Register, UniversalConfigTypes.String);
            if ( s_Value== null) return false;
            return true;
        }
        public bool GetValue(string s_Unitname, string s_Register, ref int[] i_Value)
        {
            string s_Values = GetRawValue(s_Unitname, s_Register, UniversalConfigTypes.IntArray);
            if (s_Values == null) return false;
            string[] s_pValues = s_Values.Split('|');
            i_Value = new int[s_pValues.Length];
            for (int i_Index = 0; i_Index < s_pValues.Length; i_Index++)
            {
                if (!int.TryParse(s_pValues[i_Index], out i_Value[i_Index]))
                {
                    return false;
                }
            }
            return true;
        }
        public bool GetValue(string s_Unitname, string s_Register, ref float[] f_Value)
        {
            string s_Values = GetRawValue(s_Unitname, s_Register, UniversalConfigTypes.FloatArray);
            if (s_Values == null) return false;
            string[] s_pValues = s_Values.Split('|');
            f_Value = new float[s_pValues.Length];
            for (int i_Index = 0; i_Index < s_pValues.Length; i_Index++)
            {
                if (!float.TryParse(s_pValues[i_Index], out f_Value[i_Index]))
                {
                    return false;
                }
            }
            return true;

        }
        public bool GetValue(string s_Unitname, string s_Register, ref string[] s_Value)
        {
            string s_Values = GetRawValue(s_Unitname, s_Register, UniversalConfigTypes.StringArray);
            if (s_Values == null) return false;
            s_Value = s_Values.Split('|');

            return true;
        }



    }
}