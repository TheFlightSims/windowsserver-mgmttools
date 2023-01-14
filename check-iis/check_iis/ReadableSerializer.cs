using System;
using System.Collections;
using System.Reflection;

internal class ReadableSerializer
{

    public static string Serialize(object o, bool bSkipEmpty = false, int indent = 0)
    {
        if (o == null)
        {
            return "null";
        }

        if (o.GetType() == typeof(string))
        {
            return "\"" + ((string)o).Replace("\"", "\\\"") + "\"";
        }

        if (o.GetType() == typeof(int))
        {
            return ((int)o).ToString();
        }

        if (o.GetType() == typeof(int))
        {
            return ((int)o).ToString();
        }

        if (o.GetType() == typeof(long))
        {
            return ((long)o).ToString();
        }

        if (o.GetType() == typeof(bool))
        {
            return ((bool)o).ToString().ToLower();
        }

        if (o.GetType() == typeof(float))
        {
            return ((float)o).ToString();
        }

        if (o.GetType() == typeof(TimeSpan))
        {
            return ("\"" + (TimeSpan)o).ToString() + "\"";
        }

        string spacer = "  ";
        string indention = "  ";
        indent++;
        for (int i = 0; i < indent; i++)
        {
            spacer += indention;
        }


        if (o is IList)
        {
            string listOutput = "";
            bool listFirst = true;
            IList l = (IList)o;
            foreach (object o2 in l)
            {
                if (!listFirst)
                {
                    listOutput += "";
                }
                else
                {
                    listFirst = false;
                }
                listOutput += Serialize(o2, bSkipEmpty, indent);
            }
            return listOutput;
        }


        // It's an actual object, so we need to walk the object model for fields to output.
        Type oClass = o.GetType();
        string output = "\n";
        bool first = true;

        foreach (MemberInfo mi in oClass.GetMembers())
        {
            // Skip empty.
            if (mi.MemberType == MemberTypes.Property && bSkipEmpty == true)
            {
                string amIempty = Serialize(oClass.InvokeMember(mi.Name, BindingFlags.GetProperty, null, o, null));
                if (amIempty == "\"\"" || amIempty == "null" || amIempty == "[]")
                {
                    continue;
                }

            }
            if (mi.MemberType == MemberTypes.Field || mi.MemberType == MemberTypes.Property)
            {

                if (!first)
                {
                    output += "\n";
                }
                else
                {
                    first = false;
                }

                output += spacer + mi.Name + " = ";

                if (mi.MemberType == MemberTypes.Field)
                {
                    output += Serialize(oClass.InvokeMember(mi.Name, BindingFlags.GetField, null, o, null), bSkipEmpty, indent);
                }
                else if (mi.MemberType == MemberTypes.Property)
                {
                    output += Serialize(oClass.InvokeMember(mi.Name, BindingFlags.GetProperty, null, o, null), bSkipEmpty, indent);
                }
            }

        }

        //output += "\n";
        return output;

    }

}