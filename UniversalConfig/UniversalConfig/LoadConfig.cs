﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace UniversalConfig
{
    public partial class UniversalConfigMeta : IDisposable
    {
        protected void CorrectString(ref string s_Name)
        {
            for (int i_Index = 0; i_Index < Meta.c_Forbiden.Length; i_Index++)
            {
                s_Name = s_Name.Replace(Meta.c_Forbiden[i_Index],'?');
            }
        }
        protected string CreateUnit(ref string s_Unitname)
        {
            CorrectString(ref s_Unitname);
            return Meta.s_Unit.Replace("#1",s_Unitname);
        }
        protected string CreateRegister(ref string s_Registername, Type i_Type,string s_Value = "NULL")
        {
            CorrectString(ref s_Registername);
            return Meta.s_Register.Replace("#1", s_Registername).Replace("#2",i_Type.ToString()).Replace("#3", s_Value);
        }
        protected bool Open(string i_Path)
        {
            this.s_pPath = i_Path;  
            try
            {
                this.o_Stream = File.Open(this.s_pPath,FileMode.Open, FileAccess.ReadWrite);
            }
            catch (Exception o_Exception)
            {
                this.s_Error = o_Exception.Message;
                this.o_Stream = null;
                return false;
            }
            
            return true;
        }
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool b_Disposing)
        {
            if (!this.b_Disposed)
            {
                if (b_Disposing)
                {
                    if(o_Stream!=null)o_Stream.Dispose();
                }

                this.b_Disposed = true;
            }
        }
    }
}
