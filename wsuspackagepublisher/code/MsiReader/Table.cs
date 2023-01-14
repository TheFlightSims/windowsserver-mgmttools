using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsiReader
{
    public sealed class Table
    {

        public Table()
        {
            IsOrdered = false;
        }

        #region {Properties}

        /// <summary>
        /// Get the list of all Columns in this table.
        /// </summary>
        public List<Column> Columns { get; set; } = new List<Column>();

        public bool IsOrdered { get; set; }

        /// <summary>
        /// Get or Set the name of this Table.
        /// </summary>
        public string Name { get; set; } = String.Empty;

        #endregion {Properties }

        #region {Methods}

        public Column GetColumn(string columnName)
        {
            Column result = null;

            foreach (Column column in Columns)
            {
                if (String.Compare(column.Name, columnName, true) == 0)
                {
                    result = column;
                    break;
                }
            }

            return result;
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion {Methods}
    }
}
