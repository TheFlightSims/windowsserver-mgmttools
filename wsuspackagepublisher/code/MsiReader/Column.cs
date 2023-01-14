using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsiReader
{
    public sealed class Column
    {
        private string _nullable;

        public Column(string name, string nullable, Nullable<int> minValue, Nullable<int> maxValue, string keyTable, Nullable<Int16> keyColumn, string category, string set, string description)
        {
            Name = name;
            Nullable = nullable;
            MinValue = minValue;
            MaxValue = maxValue;
            KeyTable = keyTable;
            KeyColumn = KeyColumn;
            Category = category;
            Set = set;
            Description = description;
        }

        #region {Properties - Propriétés}

        /// <summary>
        /// Get or Set the Name of this Column.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Identifies whether the column may contain a Null value.
        /// This column may have one of the following values : 
        /// Y : Yes, the column may have a Null value.
        /// N : No, the column may not have a Null value.
        /// </summary>
        public string Nullable
        {
            get { return _nullable; }
            set
            {
                if (value.ToUpper() == "Y" || value.ToUpper() == "N")
                    _nullable = value.ToUpper();
            }
        }

        /// <summary>
        /// This field applies to columns having numeric value. The field contains the minimum permissible value.
        /// This can be the minimum value for an integer or the minimum value for a date or version string.
        /// </summary>
        public int? MinValue { get; set; }

        /// <summary>
        /// This field applies to columns having numeric value. The field is the maximum permissible value.
        /// This may be the maximum value for an integer or the maximum value for a date or version string.
        /// </summary>
        public int? MaxValue { get; set; }

        /// <summary>
        /// This field applies to columns that are external keys.
        /// The field identified in Column must link to the column number specified by KeyColumn in the table named in KeyTable.
        /// This can be a list of tables separated by semicolons.
        /// </summary>
        public string KeyTable { get; set; }

        /// <summary>
        /// This field applies to table columns that are external keys.
        /// The field identified in Column must link to the column number specified by KeyColumn in the table named in KeyTable.
        /// The permissible range of the KeyColumn field is 1-32.
        /// </summary>
        public short? KeyColumn { get; set; }

        /// <summary>
        /// This is the type of data contained by the database field specified by the Table and Column columns of the _Validation table.
        /// If this is a type having a numeric value, such as Integer, DoubleInteger or Time/Date, then enter null into this field and specify the value's range using the MinValue and MaxValue columns.
        /// Use the Category column to specify the non-numeric data types described in Column Data Types.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// This is a list of permissible values for this field separated by semicolons.
        /// This field is usually used for enumerations.
        /// </summary>
        public string Set { get; set; }

        /// <summary>
        /// A description of the data that is stored in the column.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The order of the column within the table.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// List of values contains in this Columns. This Property is fill by the 'GetAllMSIValueFromTable' method of the MsiReader Class.
        /// </summary>
        public List<string> Values { get; } = new List<string>();

        #endregion {Properties - Propriétés}


        #region {Methods - Méthodes}

        public override string ToString()
        {
            return Name;
        }

        #endregion {Methods - Méthodes}
    }
}
