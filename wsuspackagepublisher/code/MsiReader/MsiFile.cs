using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsiReader
{
    public class MsiFile
    {
        // Fields

        /// <summary>
        /// List of all the names of common properties.
        /// </summary>
        public enum PropertyName
        {
            ProductCode,
            ProductName,
            Manufacturer,
            ProductVersion,
            UpgradeCode
        }
        private readonly MsiReader msiReader;


        //  Constructor

        /// <summary>
        /// Create an instance of the class and open the MSI file.
        /// </summary>
        /// <param name="filename">Full path to the MSI file to open.</param>
        public MsiFile(string filename)
        {
            this.Path = filename;
            this.msiReader = new MsiReader(filename);
            ReadCommonProperties();
        }


        #region Properties

        /// <summary>
        /// Gets or sets the name of the manufacturer.
        /// </summary>
        public string Manufacturer { get; private set; }

        /// <summary>
        /// Gets or sets the path to the MSI File to query.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Gets or sets the product code of the MSI file. For example {23170F69-40C1-2701-1602-000001000000}
        /// </summary>
        public string ProductCode { get; private set; }

        /// <summary>
        /// Gets or set the name of the product.
        /// </summary>
        public string ProductName { get; private set; }

        /// <summary>
        /// Gets or sets the version of the product.
        /// </summary>
        public string ProductVersion { get; private set; }

        /// <summary>
        /// Gets or sets the upgrade code of the MSI file.
        /// </summary>
        public string UpgradeCode { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the value of the property.
        /// </summary>
        /// <param name="propertyName">Name of the property to query.</param>
        /// <returns></returns>
        public string GetPropertyValue(string propertyName)
        {
            return this.msiReader.RetrievePropertyValue(propertyName);
        }

        /// <summary>
        /// Return all properties with their value.
        /// </summary>
        /// <returns></returns>
        public Table GetAllProperties()
        {
            return this.msiReader.GetAllMSIProperties();
        }

        /// <summary>
        /// Return the list of all Tables.
        /// </summary>
        /// <returns></returns>
        public SortedDictionary<string, Table> GetAllTables()
        {
            return this.msiReader.GetAllMSITables();
        }

        /// <summary>
        /// Query the requested Table from the MSI file and return all properties with their values. 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public Table GetAllValuesFromTable(Table table)
        {
            return this.msiReader.GetAllMSIValuesFromTable(table);
        }

        private void ReadCommonProperties()
        {
            this.ProductCode = this.msiReader.RetrievePropertyValue("ProductCode");
            this.ProductName = this.msiReader.RetrievePropertyValue("ProductName");
            this.Manufacturer = this.msiReader.RetrievePropertyValue("Manufacturer");
            this.ProductVersion = this.msiReader.RetrievePropertyValue("ProductVersion");
            this.UpgradeCode = this.msiReader.RetrievePropertyValue("UpgradeCode");
        }

        #endregion
    }
}
