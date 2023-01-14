using System;
using WindowsInstaller;
using System.Collections.Generic;

namespace MsiReader
{
    /// <summary>
    /// The purpose of this class is to read the properties of an MSI file.
    /// </summary>
    internal class MsiReader
    {
        // Fields

        private readonly Installer _msiInstaller;
        private readonly Database _database;
        private readonly SortedDictionary<string, Table> _tables = new SortedDictionary<string, Table>();


        // Constructor

        /// <summary>
        /// Create an instance of the class from the MSI file.
        /// <param name="filename">Full path to the MSI file.</param>
        /// </summary>
        public MsiReader(string filename)
        {
            // Get the type of the Windows Installer object 
            Type installerType = Type.GetTypeFromProgID("WindowsInstaller.Installer");

            // Create the Windows Installer object 
            _msiInstaller = (Installer)Activator.CreateInstance(installerType);
            this._database = _msiInstaller.OpenDatabase(filename, MsiOpenDatabaseMode.msiOpenDatabaseModeReadOnly);
        }

        #region Methods

        private void FillColumnsOrder(SortedDictionary<string, Table> tables)
        {
            try
            {
                // Open a view on the Property table for the version property 
                string request = "SELECT * FROM _Columns";
                WindowsInstaller.View view = this._database.OpenView(request);

                // Execute the view query 
                view.Execute(null);

                // Get the record from the view 
                Record record = view.Fetch();

                while (record != null)
                {
                    string tableName = record.StringData[1];
                    int number = record.IntegerData[2];
                    string columnName = record.StringData[3];

                    tables[tableName].GetColumn(columnName).Order = number;
                    tables[tableName].IsOrdered = true;
                    record = view.Fetch();
                }
                view.Close();
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Return all properties with their value.
        /// </summary>
        /// <returns>Return a Dictionnary with property name as key and property value as value.</returns>
        internal Table GetAllMSIProperties()
        {
            if (!_tables.ContainsKey("Property"))
            {
                GetAllMSITables();
            }

            return GetAllMSIValuesFromTable(_tables["Property"]);
        }

        /// <summary>
        /// Return the list of all Tables in this MSI file.
        /// </summary>
        /// <returns>List of all Table name.</returns>
        internal SortedDictionary<string, Table> GetAllMSITables()
        {
            WindowsInstaller.View view = null;

            try
            {
                // Open a view on the Property table for the version property 
                view = this._database.OpenView("SELECT * FROM _Validation");

                // Execute the view query 
                view.Execute(null);

                // Get the record from the view 
                Record record = view.Fetch();

                while (record != null)
                {
                    string tableName = record.StringData[1];
                    string columnName = record.StringData[2];
                    string nullable = record.StringData[3];
                    int? minValue = null;
                    if (!string.IsNullOrEmpty(record.StringData[4]))
                    {
                        minValue = record.IntegerData[4];
                    }
                    int? maxValue = null;
                    if (!string.IsNullOrEmpty(record.StringData[5]))
                    {
                        maxValue = record.IntegerData[5];
                    }
                    string keyTable = record.StringData[6];
                    short? keyColumn = null;
                    if (!string.IsNullOrEmpty(record.StringData[7]))
                    {
                        keyColumn = (short)record.IntegerData[7];
                    }
                    string category = record.StringData[8];
                    string set = record.StringData[9];
                    string description = record.StringData[10];

                    Column column = new Column(columnName, nullable, minValue, maxValue, keyTable, keyColumn, category, set, description);

                    if (_tables.ContainsKey(tableName))
                    {
                        _tables[tableName].Columns.Add(column);
                    }
                    else
                    {
                        Table table = new Table
                        {
                            Name = tableName
                        };
                        table.Columns.Add(column);
                        _tables.Add(tableName, table);
                    }
                    record = view.Fetch();
                }
            }
            catch (Exception) { }
            finally
            {
                view?.Close();
            }

            FillColumnsOrder(_tables);
            return _tables;
        }

        /// <summary>
        /// Query the requested Table from the MSI file and return all properties with their values. 
        /// </summary>
        /// <param name="table">Name of the MSI Table to query.</param>
        /// <returns>Return a Dictionnary with property name as key and property value as value.</returns>
        internal Table GetAllMSIValuesFromTable(Table table)
        {
            WindowsInstaller.View view = null;

            try
            {
                string columnsName = String.Empty;
                foreach (Column column in table.Columns)
                {
                    columnsName += $"{column.Name}, ";
                    column.Values.Clear();
                }
                columnsName = columnsName.Substring(0, columnsName.LastIndexOf(","));

                // Open a view on the Property table for the version property 
                view = this._database.OpenView($"SELECT {columnsName} FROM {table.Name}");

                // Execute the view query 
                view.Execute(null);

                // Get the record from the view 
                Record record = view.Fetch();

                while (record != null)
                {
                    int i = 1;
                    foreach (Column column in table.Columns)
                    {
                        column.Values.Add(record.StringData[i]);
                        i++;
                    }
                    record = view.Fetch();
                }
            }
            catch (Exception) { }
            finally
            {
                view?.Close();
            }

            return table;
        }

        /// <summary>
        /// Retrieves the value of a property.
        /// </summary>
        /// <param name="PropertyName">Name of the Property to query.</param>
        /// <returns>Return a string which contain the result of the query. If an error occurs, return an empty string.</returns>
        internal string RetrievePropertyValue(string PropertyName)
        {
            string result = string.Empty;
            WindowsInstaller.View view = null;

            try
            {
                // Open a view on the Property table for the version property 
                view = this._database.OpenView($"SELECT * FROM Property WHERE Property='{PropertyName}'");

                // Execute the view query 
                view.Execute(null);

                // Get the record from the view 
                Record record = view.Fetch();
                result = record.get_StringData(2);
            }
            catch (Exception) { }
            finally
            {
                view?.Close();
            }

            return result;
        }

        #endregion
    }
}
