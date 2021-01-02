using System;
using System.IO;

namespace Source
{
    public static class u_Confighandler
    {
        //UCP CONFIG
        private static class header {
            public static readonly string s_header = "[UCFG/C]";
            public static readonly string s_unit = "[UNIT=(#1)]";
            public static readonly string s_register = "[REG=(#1);TYPE=(#2);VALUE=(#3)]";
            public static readonly string s_register_q = "[REG=(#1);TYPE=(#2);VALUE=(";
            public static readonly string s_register_s = ")]";
        }

        private static StreamWriter o_writer;
        private static StreamReader o_reader;

        private static string s_configfile;
        public static void configset(string s_configpath) {
            s_configfile = s_configpath;
        }

        public static string read()
        {
            if (!File.Exists(s_configfile))
            {
                return null;
            }
            kill();
            o_reader = new StreamReader(s_configfile);
            if (o_reader != null) {
                string s_file = o_reader.ReadToEnd();
                o_reader.Close();

                s_file = s_file.Replace(Environment.NewLine,"");
                s_file = s_file.Replace('\t',' ');
                s_file = s_file.Replace(" ","");

                return s_file;
            }
            return null;
        }
        public static string write(string s_value)
        {
            kill();
            try
            {
                o_writer = new StreamWriter(s_configfile);
                o_writer.WriteLine(s_value);
            }
            catch (Exception)
            {}

            kill();
            return null;
        }
        public static void kill() {
            if(o_writer!= null)o_writer.Close();
           if(o_reader != null) o_reader.Close();
        }

        private static string getsupunit(string s_file,string s_unit) {
            
            if (!s_file.Contains(header.s_header)) return null;
            if (!s_file.Contains(header.s_unit.Replace("#1", s_unit))) return null;

            char[] c_unitreg = header.s_unit.Replace("#1", s_unit).ToCharArray();
            char[] c_register = s_file.ToCharArray();

            int i_start = 0,i_finish = 0;
            for (int i_registerindex = 0; i_registerindex < c_register.Length; i_registerindex++)
            {
                bool b_match = true;
                for (int i_supindex = 0; i_supindex < c_unitreg.Length; i_supindex++)
                {
                    if (c_register[i_registerindex + i_supindex] != c_unitreg[i_supindex]) {
                        b_match = false;
                    }
                }
                if (b_match) {
                    i_start = i_registerindex + c_unitreg.Length;
                    break;
                }
            }

            for (int i_registerindex = i_start; i_registerindex < c_register.Length; i_registerindex++)
            {
                if (c_register[i_registerindex] == '}') {
                    i_finish = i_registerindex;
                    break;
                }
            }

            string s_finish = "";
            for (int i_buildindex = i_start+1; i_buildindex < i_finish; i_buildindex++)
            {
                s_finish += c_register[i_buildindex];
            }

            return s_finish;
        }

        private static string getregister(string s_unit, string s_register,string s_type)
        {
            string s_regheader = header.s_register_q.Replace("#1", s_register).Replace("#2", s_type);

            string[] s_registers = s_unit.Split(',');
            int i_sel_register = -1;
            for (int i_regindex = 0; i_regindex < s_registers.Length; i_regindex++)
            {
                if (s_registers[i_regindex].Contains(s_register) && s_registers[i_regindex].Contains(s_type)) {
                    i_sel_register = i_regindex;
                    break;
                }
            }
            if (i_sel_register == -1)
            {
                return null;
            }

            return s_registers[i_sel_register].Replace(s_regheader, "").Replace(header.s_register_s,"");
        }

        public static int readint(string s_unit,string s_register) {
            string s_file = read();
            if (s_file == null) return 0;

            string s_sup = getsupunit(s_file, s_unit);
            if (s_sup == null) return 0;
            string s_reg = getregister(s_sup, s_register,"int");
            if (s_reg == null) return 0;

            return int.Parse(s_reg);
        }

        public static string readstring(string s_unit, string s_register)
        {
            string s_file = read();
            if (s_file == null) return null;

            string s_sup = getsupunit(s_file, s_unit);
            if (s_sup == null) return null;
            string s_reg = getregister(s_sup, s_register, "str");
            if (s_reg == null) return null;

            return s_reg;
        }
        public static float readfloat(string s_unit, string s_register)
        {
            string s_file = read();
            if (s_file == null) return 0;

            string s_sup = getsupunit(s_file, s_unit);
            if (s_sup == null) return 0;
            string s_reg = getregister(s_sup, s_register, "flt");
            if (s_reg == null) return 0;

            return float.Parse(s_reg);
        }


        public static void writestring(string s_unit, string s_register,string s_value)
        {
            string s_file = read();
            if (s_file == null) return;

            string s_sup = getsupunit(s_file, s_unit);
            if (s_sup == null) return;
   
            string s_reg = getregister(s_sup, s_register, "str");
            if (s_reg == null) return;

            string s_placeholder = header.s_register.Replace("#1", s_register).Replace("#2", "str").Replace("#3", s_reg);
            string s_placevalue = header.s_register.Replace("#1", s_register).Replace("#2", "str").Replace("#3", s_value);

            s_file= s_file.Replace(s_placeholder, s_placevalue);

            write(s_file);
        }


        public static void writeint(string s_unit, string s_register, int i_value)
        {
            string s_file = read();
            if (s_file == null) return;

            string s_sup = getsupunit(s_file, s_unit);
            if (s_sup == null) return;
            string s_reg = getregister(s_sup, s_register, "int");
            if (s_reg == null) return;

            string s_placeholder = header.s_register.Replace("#1", s_register).Replace("#2", "int").Replace("#3", s_reg);
            string s_placevalue = header.s_register.Replace("#1", s_register).Replace("#2", "int").Replace("#3", i_value.ToString());

            s_file = s_file.Replace(s_placeholder, s_placevalue);

            write(s_file);
        }

    }
}
