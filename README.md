# UniversalConfig
<img src="https://www.ucpsystems.com/version/universalconfig/universalconfig_logo.png" width="100" height="100">
UniversalConfig provides a straightforward way for external data storage in a human-readable format. Sometimes it is not necessary to use complex file types especially if those need a big library to be included and contain a bunch of stuff you don’t even need. Just keep it simple with the UCF filetype which is designed for fast implementation and easy usage. If you are not satisfied with the functionality provided, just download the source code and modify it for your needs.

## Structure 
UC Library contains four files housing the two classes called _UniversalConfigCreator_ and _UniversalConfigReader_. 

The _UniversalConfigCreator_ (short _Creator_) class contains all functions needed to create (and in the future edit) config files. When a new config file is created, the programmer must lay the blueprint for this configs structure: This is accomplished through the _Creator_ by creating (or rather appending) a so-called _Unit_ to the config. This _Unit_ is comparable to a struct in C# or C/C++ because it acts as a data group. This group of data is now empty and has no place where data can be stored. For the _Unit_ to become useable, a _Register_ needs to be added.  _Registers_ are comparable to variables in programming and therefore need a name and a datatype. If all _Units_ and _Register_ are created the file can be _Build_ and written in a text file or somewhere else.

For data to be stored or accessed _UniversalConfigReader_ needs to be used. Reading or writing is done by addressing the data by its _Unit_ and_Register_ name and what data type is expected. This allows having two _Registers_ inside a _Unit_ with the same name, with the condition of different data types. All default C# datatypes are usable and others that contain the _TryParse_ function, too. As the data is stored in plain text, it can also be directly accessed without being parsed. Arrays are supported and can be accessed raw or formatted as a datatype. Now only 1D Arrays are supported but this is subject to change. For string arrays, only use this raw method, because the string datatype does not contain a _TryParse_ function. Any register can be used as either an array or single value type without special declaration. By clearing a _Register_ its value field can now be used as both a single value or a array type. 
## Functions
### UniversalConfigCreator
```CS
UniversalConfigCreator(string s_Path)
```

The constructor takes the absolute or relative path of the config file. All file endings are allowed but _.ucf_ is recommended. 

```CS
void AppendUnit(string s_Unitname)
```

Appends to the config a _Unit_.

```CS
void AppendRegister(string s_Unitname,string s_Registername, Type i_Type )
```

Appends a _Register_ to the specified _Unit_. 

```CS
string Build()
```

Finishes the config creation and writes the config. The return value is the written config file.

### UniversalConfigReader
```CS
UniversalConfigReader(string i_Path)
```

The constructor takes the absolute or relative path of the config file. All file endings are allowed but _.ucf_ is recommended. 

```CS
bool LoadConfig()
```

Loads content of the file into memory. The function is automatically called when Constructor is called. Can be used as a reset because data is only written to the config with _ SaveConfig _ and read by _ LoadConfig _. Returns _true_ if successful.

```CS
bool SaveConfig()
```

Saves the edits made to the file. Returns _true_ if successful.

```CS
string GetRawValue(string s_Unitname,string s_Register, Type i_Type )
```

Gets the raw ASCII values at a specific _Unit_, _Register_ and Datatype. 

```CS
void SetRawValue(string s_Unitname, string s_Register, Type i_Type,string s_Value="NULL")
```

Sets the raw ASCII values at a specific _Unit_, _Register_ and Datatype. 

```CS
void SetValue<T>(string s_Unitname, string s_Register, T i_Value)
```

Sets data at a specific _Unit_ and _Register_. 

```CS
T GetValue<T>(string s_Unitname, string s_Register)
```

Gets data at a specific _Unit_ and _Register_. 

```CS
T[] GetArray<T>(string s_Unitname, string s_Register)
```

Gets data array at a specific _Unit_ and _Register_. 

```CS
void SetArray<T>(string s_Unitname, string s_Register, T[] i_Value)
```

Sets data array at a specific _Unit_ and _Register_. 

```CS
string[] GetAsStringArray(string s_Unitname, string s_Register,Type o_Type)
```

Gets a raw data array at a specific _Unit_ and _Register_. 

## Usage
Both classes are designed to be used with the ```using``` keyword as shown in those examples.

_Example for file Creation_
```CS
using (UniversalConfigCreator o_Creator = new UniversalConfigCreator("testconfig.ucf"))
{
    o_Creator.AppendUnit("floatingpoint");
    o_Creator.AppendRegister("floatingpoint", "floattype", typeof(float));
    o_Creator.AppendRegister("floatingpoint", "doubletype", typeof(double));

    o_Creator.AppendUnit("integer");
    o_Creator.AppendRegister("integer", "inttype", typeof(int));
    o_Creator.AppendRegister("integer", "shorttype", typeof(short));

    o_Creator.AppendUnit("text");
    o_Creator.AppendRegister("text", "stringtype", typeof(string));
    o_Creator.AppendRegister("text", "chartype", typeof(char));

    o_Creator.Build();
}
```

_Example file reading and writing_	

```CS
using (UniversalConfigReader o_Reader = new UniversalConfigReader("testconfig.ucf"))
{
    o_Reader.SetValue<float>("floatingpoint", "floattype", 10.0f);

    int integer = o_Reader.GetValue<int>("integer", "inttype");

    char[] chararray = o_Reader.GetArray<char>("text", "chartype");

    o_Reader.SaveConfig();             
}
```
