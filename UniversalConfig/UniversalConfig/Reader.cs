using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
namespace UniversalConfig
{
    public partial class UniversalConfigReader : UniversalConfigMeta
    {
        public UniversalConfigReader(string i_Path)
        {
            Open(i_Path);
            LoadConfig();
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
        public string GetRawValue(string s_Unitname,string s_Register, Type i_Type )
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
        public void SetRawValue(string s_Unitname, string s_Register, Type i_Type,string s_Value="NULL")
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

        public void SetValue<T>(string s_Unitname, string s_Register, T i_Value)
        {
            SetRawValue(s_Unitname, s_Register, typeof(T), i_Value.ToString());
        }


        public T GetValue<T>(string s_Unitname, string s_Register)
        {

            Type o_Type = typeof(T);
            string s_Value = GetRawValue(s_Unitname, s_Register, o_Type);
            MethodInfo o_Parse =  o_Type.GetMethod("TryParse",new Type[] { typeof(string),typeof(T).MakeByRefType()});
            if (o_Parse != null)
            { 
                Object[] o_Params = new object[] { s_Value, null };
                Object o_result = o_Parse.Invoke(null, o_Params);

                if(o_result != null)return (T)o_Params[1];
            }
            return default(T);
        }

        public void SetArray<T>(string s_Unitname, string s_Register, T[] i_Value)
        {
            string s_Values = i_Value[0].ToString();
            for (int i_Index = 1; i_Index < i_Value.Length; i_Index++)
            {
                s_Values += "|" + i_Value[i_Index].ToString();
            }
            SetRawValue(s_Unitname, s_Register, typeof(T), s_Values);
        }


        public T[] GetArray<T>(string s_Unitname, string s_Register)
        {
            Type o_Type = typeof(T);
            string s_Values = GetRawValue(s_Unitname, s_Register, o_Type);
            MethodInfo o_Parse = o_Type.GetMethod("TryParse", new Type[] { typeof(string), typeof(T).MakeByRefType() });

            if (s_Values == null) return null;
            string[] s_pValues = s_Values.Split('|');
            T[] i_Value = new T[s_pValues.Length];
            for (int i_Index = 0; i_Index < s_pValues.Length; i_Index++)
            {
                if (o_Parse != null)
                {
                    Object[] o_Params = new object[] { s_pValues[i_Index], null };
                    Object o_result = o_Parse.Invoke(null, o_Params);

                    if (o_result != null) i_Value[i_Index] = (T)o_Params[1];
                    else return null;
                }
                else return null;
            }
            return i_Value;

        }

    }
}