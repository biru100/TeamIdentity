using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System;
using System.IO;
using System.Linq;
using System.Text;

using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class ExcelParser : EditorWindow
{
    private static readonly string table_input_path = "/ExcelTable";
    private static readonly string data_output_path = "/Resources/DataTable/";

    private static readonly List<string> excel_file_extensions = new List<string>(){ ".xls", ".xlsx" };

    private const int start_line_index = 2;

    [MenuItem("Excel/Parse Excel")]
    private static void Parse()
    {
        DirectoryInfo di = new DirectoryInfo(Application.dataPath + table_input_path);

        if (!di.Exists)
        {
            di.Create();
            return;
        }

        List<FileInfo> excelFiles = di.GetFiles().Where( (f) => excel_file_extensions.Contains(f.Extension)).ToList();

        foreach(var file in excelFiles)
        {
            IWorkbook workBook;

            using (FileStream fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            {
                workBook = file.Extension == excel_file_extensions[0] ? new HSSFWorkbook(fs) as IWorkbook : new XSSFWorkbook(fs) as IWorkbook;
            }

            for (int i = 0; i < workBook.NumberOfSheets; i++)
            {
                ISheet sheet = workBook.GetSheetAt(i);

                MakeSheetData(file, sheet);
            } 
        }
    }

    private static void MakeSheetData(FileInfo info, ISheet sheet)
    {
        StringBuilder sb = new StringBuilder();

        IRow row;
        string part;

        MakeCSFile(sheet.SheetName, sheet.GetRow(0));

        for (int i = start_line_index; i <= sheet.LastRowNum; i++)
        {
            row = sheet.GetRow(i);

            if (row == null)
                continue;
            if(row.Cells.Count == 0)
                continue;
            if (row.GetCell(0) == null)
                continue;

            foreach (ICell cell in row)
            {
                part = string.Empty;

                switch (cell.CellType)
                {
                    case CellType.String:
                        part = cell.StringCellValue;
                        break;

                    case CellType.Numeric:
                        if (HSSFDateUtil.IsCellDateFormatted(cell))
                            part = cell.DateCellValue.ToString();
                        else
                            part = cell.NumericCellValue.ToString();
                        break;

                    case CellType.Formula:
                        switch (cell.CachedFormulaResultType) // =a1+b2 ..
                        {
                            case CellType.String:
                                part = cell.StringCellValue;
                                break;
                            case CellType.Numeric:
                                part = cell.NumericCellValue.ToString();
                                break;
                            default:
                                Debug.LogError(string.Format("[Parser]Invaild Type Exception : Cell type is {0}", cell.CachedFormulaResultType));
                                break;
                        }
                        break;

                    case CellType.Blank:
                        break;

                    case CellType.Boolean:
                        part = cell.BooleanCellValue.ToString();
                        break;

                    default:
                        Debug.LogError(string.Format("[Parser]Out of case Exception : Cell type is {0}", cell.CellType));
                        break;
                }
                sb.Append(part + "\t");
            }

            sb.Append("\n");
        }

        DirectoryInfo di = new DirectoryInfo(Application.dataPath + data_output_path);

        if (!di.Exists)
            di.Create();

        File.WriteAllText(Application.dataPath + data_output_path + sheet.SheetName + ".txt", sb.ToString());
    }

    private static void MakeCSFile(string name, IRow defineRow)
    {
        StringBuilder fileData = new StringBuilder();
        fileData.Append(pre_script);
        var str = string.Format(script_class_define_format, name) + "{ \n";
        fileData.Append(str);

        foreach (ICell cell in defineRow)
        {
            if (cell.StringCellValue.Length <= 1)
                continue;

            int count = 0;
            string typeName = string.Empty;
            string variableName = GetVariableName(cell.StringCellValue, ref typeName, out count);

            //public string[] _name = new string[count]
            fileData.Append(string.Format(script_variable_define_format, typeName, variableName, GetInitializerFromType(typeName, count)) + "\n");
        }
        //public static <SomeThing> Load(string[] parts)
        fileData.Append(string.Format(script_function_start, name));
        fileData.Append(script_function_part_1);
        fileData.Append(script_function_part_2);
        fileData.Append(string.Format(script_function_class_initializer_format, name));

        foreach (ICell cell in defineRow)
        {
            if (cell.StringCellValue.Length <= 1)
                continue;

            int count = 0;
            string typeName = string.Empty;
            string variableName = GetVariableName(cell.StringCellValue, ref typeName, out count);
            string parser = GetParserFromType(typeName);

            if (typeName.Contains("["))
            {
                //doing
                //p.something[0] = parts[i++];
                //p.something[1] = parts[i++];
                //p.something[2] = parts[i++];

                for (int i = 0; i < count; ++i)
                {
                    fileData.Append(string.Format(script_parsing_board,
                        string.Format("{0}[{1}]", variableName, i),
                        parser) + "\n");
                }
            }
            else
            {
                fileData.Append(string.Format(script_parsing_board, variableName, parser) + "\n");
            }
        }

        fileData.Append(post_script);

        string path = Application.dataPath + "/Script/Tables/";

        DirectoryInfo di = new DirectoryInfo(path);

        if (!di.Exists)
            di.Create();

        File.WriteAllText(path + name + ".cs", fileData.ToString());

        AssetDatabase.Refresh();
    }

    //i_something_name
    //^here
    private static string GetTypeForString(string _prefix)
    {
        switch (_prefix)
        {
            case "i": return "int";
            case "s": return "string";
            case "f": return "float";
            case "b": return "bool";

            default: return _prefix;
        }
    }

    //something;    or
    //something = new someType[count];
    private static string GetInitializerFromType(string type, int count)
    {
        if (type.Contains("["))
        {
            return string.Format(script_array_initializer_format, type.Split('[')[0], count);
        }
        else
        {
            return ";";
        }

    }

    private static string GetVariableName(string name, ref string typeName, out int count)
    {
        typeName = name.Split('_')[0];
        var prefixSize = typeName.Length;
        if (prefixSize == 1)
        {
            //i,f,s prefixes
            name = name.Remove(0, 1);
        }
        else
        {
            //enum
            name = name.Remove(0, prefixSize);
        }

        typeName = GetTypeForString(typeName);

        if (name.Contains("["))
        {
            typeName += "[]";
            count = int.Parse(name.Split('[')[1][0].ToString());
            name = name.Split('[')[0];
        }
        else
        {
            count = 0;
        }

        return name;
    }

    private static string GetParserFromType(string type)
    {
        switch (type)
        {
            case "int":
            case "int[]": return "int.Parse(parts[i++])";

            case "string":
            case "string[]": return "parts[i++]";

            case "float":
            case "float[]": return "float.Parse(parts[i++])";

            case "bool":
            case "bool[]": return "bool.Parse(parts[i++])";

            default:
                if (type.Contains("["))
                    type = type.Split('[')[0];
                if (type.Length > 1)
                    return string.Format("({0})System.Enum.Parse(typeof({0}),parts[i++])", type);
                else
                    return string.Format("({0})int.Parse(parts[i++])", type);
        }

    }

    private const string pre_script =
@"//이 코드는 엑셀 파서에 의해 자동 생성됨. 
using System;
using System.IO;

";
    private const string script_class_define_format =
        "public class {0} ";
    private const string script_variable_define_format =
        "   public {0} {1}{2}";
    private const string script_function_start =
        "   public static {0} Load(string[] parts) ";
    private const string script_function_part_1 =
        "{\n";
    private const string script_function_part_2 =
        "       int i = 0;\n";
    private const string script_function_class_initializer_format =
        "       {0} p = new {0}();\n";

    private const string script_array_initializer_format =
        " = new {0}[{1}];";
    private const string script_parsing_board =
        "       p.{0} = {1};";

    private const string post_script =
@"
    return p;
    }
}
";
}
