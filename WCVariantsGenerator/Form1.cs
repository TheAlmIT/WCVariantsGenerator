using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace WCVariantsGenerator
{
    public partial class Form1 : Form
    {
        string parentFilePath = string.Empty; string variationsFilePath = string.Empty; string simpleProductsFilePath = string.Empty;
        List<string> listA = new List<string>();
        List<string> generatedImageSKUList = new List<string>();
        DataTable originalTable = null;
        public Form1()
        {
            InitializeComponent();
            Logger.SetLogWriter(new LogWriterFactory().Create());
        }

        private void brnOpenCSV_Click(object sender, EventArgs e)
        {
            dlgOpenCSV.Filter = "CSVFiles|*.csv";
            DialogResult dr = dlgOpenCSV.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                txtFileName.Text = dlgOpenCSV.FileName;
                originalTable = new DataTable();
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateParent();
            MessageBox.Show("Completed");
        }
        #region Generate Parent
        private void GenerateParent()
        {
            parentFilePath = ConfigurationManager.AppSettings["ParentFileNameIncludingPath"] + string.Format("{0}.csv", DateTime.Now.ToString("MMddyyyy_hhmmss"));
            if (originalTable == null || originalTable.Rows == null || originalTable.Rows.Count == 0)
            {
                if (!string.IsNullOrEmpty(txtFileName.Text))
                {
                    originalTable = GetDataTableFromCSVFile(txtFileName.Text);
                }
                else
                {
                    MessageBox.Show("Select the input file");
                    return;
                }
            }
            if (originalTable.Rows.Count > 0)
            {
                originalTable.DefaultView.Sort = "post_name ASC, stock desc";
                originalTable = originalTable.DefaultView.ToTable();
                DataTable parentDT = originalTable.Copy();
                DataTable finalParentDT = originalTable.Clone();

                string cPostName = string.Empty;
                foreach (DataRow dr in originalTable.Rows)
                {
                    string postName = Convert.ToString(dr["post_name"]);
                    if (cPostName != postName)
                    {
                        cPostName = postName;
                        DataRow[] drs = parentDT.Select("sku ='" + dr["sku"] + "'");
                        if (drs.Length == 1)
                        {

                            DataRow[] drVariations = parentDT.Select("post_name ='" + postName + "'");
                            //Compute Parent Attributes
                            if (drVariations.Length > 1)
                            //if (drVariations.Length == 1)
                            {
                                string paSize = string.Empty;
                                for (int i = 0; i < drVariations.Length; i++)
                                {
                                    paSize += Convert.ToString(drVariations[i]["attribute:Size"]) + (i == drVariations.Length - 1 ? string.Empty : "|");
                                }
                                drs[0]["parent_sku"] = @"""""";
                                drs[0]["attribute:pa_size"] = @"""" + paSize + @"""";
                                string defaultSizeName = Convert.ToString(drs[0]["attribute:size"]).ToLower().Replace("  "," ").Replace(" ","-");
                                drs[0]["attribute_default:pa_size"] = @"""" + defaultSizeName + @"""";
                                drs[0]["attribute:size"] = @"""""";
                                drs[0]["attribute_data:pa_size"] = @"""" + "0|0|1" + @"""";
                                drs[0]["manage_stock"] = @"""" + "yes" + @"""";
                                //drs[0]["stock"] = @"""" + "0" + @"""";
                                drs[0]["sku"] = Convert.ToString(drs[0]["sku"]).Trim() + "P";
                                drs[0]["sale_price"] = string.Empty;
                                drs[0]["tax:product_type"] = @"""" + "variable" + @"""";
                                //drs[0]["row_type_id"] = 1;

                                finalParentDT.ImportRow(drs[0]);
                            }
                        }
                    }
                }
                WriteDataTableToCSV(finalParentDT, parentFilePath);
            }
        }
        #endregion

        #region Generate Variations
        private void GenerateVariations()
        {
            variationsFilePath = ConfigurationManager.AppSettings["VariationsFileNameIncludingPath"] + string.Format("{0}.csv", DateTime.Now.ToString("MMddyyyy_hhmmss"));
            string variationColumns = ConfigurationManager.AppSettings["VariationColumns"];
            if (originalTable == null || originalTable.Rows == null || originalTable.Rows.Count == 0)
            {
                if (!string.IsNullOrEmpty(txtFileName.Text))
                {
                    originalTable = GetDataTableFromCSVFile(txtFileName.Text);
                }
                else
                {
                    MessageBox.Show("Select the input file");
                    return;
                }
            }
            if (originalTable.Rows.Count > 0)
            {
                //originalTable.Columns.Add("Parent", typeof(String), "post_title");
                originalTable.Columns["backorders"].ColumnName = "meta:_backorders";
                originalTable.Columns["manage_stock"].ColumnName = "meta:_manage_stock";
                originalTable.Columns["stock_status"].ColumnName = "meta:_stock_status";
                originalTable.Columns["tax_status"].ColumnName = "meta:_tax_status";
                originalTable.Columns["visibility"].ColumnName = "meta:_visibility";
                originalTable.Columns["attribute:pa_size"].ColumnName = "meta:attribute_pa_size";

                originalTable.DefaultView.Sort = "post_name ASC";
                originalTable = originalTable.DefaultView.ToTable();
                DataTable varientsDT = originalTable.Copy();
                DataTable finalVarienceDT = originalTable.Clone();

                string cPostName = string.Empty;
                foreach (DataRow dr in originalTable.Rows)
                {
                    string postName = Convert.ToString(dr["post_name"]);
                    if (cPostName != postName)
                    {
                        cPostName = postName;
                        DataRow[] drs = varientsDT.Select("sku ='" + dr["sku"] + "'");
                        if (drs.Length == 1)
                        {

                            DataRow[] drVariations = varientsDT.Select(@"post_name ='" + postName + "'");
                            //Compute Child Attributes
                            if (drVariations.Length > 1)
                            //if (drVariations.Length == 1)
                            {
                                foreach (DataRow drv in drVariations)
                                {
                                    drv["parent_sku"] = Convert.ToString(drs[0]["sku"]).Trim() + "P";
                                    drv["meta:_manage_stock"] = "yes";
                                    drv["sale_price"] = string.Empty;

                                    finalVarienceDT.ImportRow(drv);
                                }
                            }
                        }
                    }
                }
                WriteDataTableToCSV(finalVarienceDT, variationsFilePath, variationColumns);
            }
        }
        #endregion

        #region Generate Simple Products
        private void GenerateSimpleProducts()
        {
            simpleProductsFilePath = ConfigurationManager.AppSettings["SimpleProdFileNameIncludingPath"] + string.Format("{0}.csv", DateTime.Now.ToString("MMddyyyy_hhmmss"));
            if (originalTable == null || originalTable.Rows == null || originalTable.Rows.Count == 0)
            {
                if (!string.IsNullOrEmpty(txtFileName.Text))
                {
                    originalTable = GetDataTableFromCSVFile(txtFileName.Text);
                }
                else
                {
                    MessageBox.Show("Select the input file");
                    return;
                }
            }
            if(originalTable.Rows.Count > 0)
            {
                originalTable.DefaultView.Sort = "post_name ASC";
                originalTable = originalTable.DefaultView.ToTable();
                DataTable simpleProducttDT = originalTable.Copy();
                DataTable finalSimpleProdDT = originalTable.Clone();

                string cPostName = string.Empty;
                foreach (DataRow dr in originalTable.Rows)
                {
                    string postName = Convert.ToString(dr["post_name"]);
                    if (cPostName != postName)
                    {
                        cPostName = postName;
                        DataRow[] drs = simpleProducttDT.Select("sku ='" + dr["sku"] + "'");
                        if (drs.Length == 1)
                        {

                            DataRow[] drVariations = simpleProducttDT.Select("post_name ='" + postName + "'");
                            //Compute Simple Product Attributes
                            if (drVariations.Length == 1)
                            {
                                drs[0]["post_title"] += "-" + drs[0]["attribute:SIZE"];
                                drs[0]["attribute:SIZE"] = string.Empty;
                                drs[0]["attribute:pa_size"] = string.Empty;
                                drs[0]["parent_sku"] = string.Empty;
                                drs[0]["manage_stock"] = "yes";
                                drs[0]["attribute_data:pa_size"] = string.Empty;
                                drs[0]["sale_price"] = string.Empty;
                                finalSimpleProdDT.ImportRow(drs[0]);
                            }
                        }
                    }
                }
                WriteDataTableToCSV(finalSimpleProdDT, simpleProductsFilePath);
            }
        }

        #endregion
        private static DataTable GetDataTableFromCSVFile(string fileName)
        {
            DataTable csvData = new DataTable();

            try
            {

                using (TextFieldParser csvReader = new TextFieldParser(fileName))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        if (column == "stock")
                        {
                            datecolumn.DataType = Type.GetType("System.Int32");
                        }
                        csvData.Columns.Add(datecolumn);
                    }

                    while (!csvReader.EndOfData)
                    {
                        try
                        {
                            string[] fieldData = csvReader.ReadFields();
                            //Making empty value as null
                            for (int i = 0; i < fieldData.Length; i++)
                            {
                                if (fieldData[i] == "")
                                {
                                    fieldData[i] = null;
                                }
                            }
                            csvData.Rows.Add(fieldData);
                        }
                        catch(Exception e)
                        {
                            Logger.Write(fileName + "- Line Number :" + csvReader.LineNumber.ToString() + e.Message);
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (!csvData.Columns.Contains("rowtypeid"))
                {
                    csvData.Columns.Add("rowtypeid", typeof(string));
                }
            }
            return csvData;
        }
        /// <summary>
        /// This function useful for writing a DataTable content to CSV File
        /// </summary>
        /// <param name="dt">DataTable which meant to be written as CSV</param>
        /// <param name="filePath">CSV File Name with full path</param>
        private void WriteDataTableToCSV(DataTable dt, string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            StringBuilder sb = new StringBuilder();
            //Write Header
            DataColumnCollection columns = dt.Columns;
            for (int i = 0; i < columns.Count; i++)
            {
                sb.Append(@"""" + columns[i].ColumnName + @"""" + (i == columns.Count - 1 ? string.Empty : ","));
            }
            WriteLineToFile(filePath, sb.ToString());
            foreach (DataRow row in dt.Rows)
            {
                UpdateRowStatusInOriginalTable(row["sku"].ToString(), "1");
                int columnCount = row.ItemArray.Length;
                sb = new StringBuilder();
                for (int i = 0; i < columnCount; i++)
                {
                    //sb.Append(@"""" + row[i].ToString() + @"""" + (i == columnCount - 1 ? string.Empty : ","));
                    sb.Append(row[i].ToString() + (i == columnCount - 1 ? string.Empty : ","));
                }
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(sb.ToString());
                }
            }
        }
        private void WriteDataTableToCSV(DataTable dt, string filePath, string requiredColulmns)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            StringBuilder sb = new StringBuilder();
            //Write Header
            WriteLineToFile(filePath, requiredColulmns);
            string[] requiredColumnNames = requiredColulmns.Split(",".ToCharArray());
            dt = dt.DefaultView.ToTable(false, requiredColumnNames);
            foreach (DataRow row in dt.Rows)
            {
                UpdateRowStatusInOriginalTable(row["sku"].ToString(), "1");
                int columnCount = row.ItemArray.Length;
                sb = new StringBuilder();
                for (int i = 0; i < columnCount; i++)
                {
                    sb.Append(row[i].ToString() + (i == columnCount - 1 ? string.Empty : ","));
                }
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(sb.ToString());
                }
            }
        }

        private void WriteLineToFile(string filePath, string strToWrite)
        {
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine(strToWrite);
            }
        }

        private void btnVariationProds_Click(object sender, EventArgs e)
        {
            GenerateVariations();
            MessageBox.Show("Generated Varaiations");
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            originalTable = new DataTable();
            txtFileName.Text = string.Empty;
        }

        private void btnSimpleProds_Click(object sender, EventArgs e)
        {
            GenerateSimpleProducts();
            MessageBox.Show("Generated Simple Products");

        }
        private void UpdateRowStatusInOriginalTable(string sku, string rowTypeId)
        {
            DataRow[] rows = originalTable.Select("sku='" + sku + "'");
            if (rows != null && rows.Length > 0)
            {
                rows[0]["rowtypeid"] = rowTypeId;
            }
        }
        private void GenerateNonProcessedRecords()
        {
            string missedSKUsFile = ConfigurationManager.AppSettings["MissedSKUsFileNameIncludingPath"] + string.Format("{0}.csv", DateTime.Now.ToString("MMddyyyy_hhmmss"));
            StringBuilder sb = new StringBuilder();
            DataRow[] rows = originalTable.Select("rowtypeid is null");
            if (rows != null && rows.Length > 0)
            {
                foreach (DataRow row in rows)
                {
                    sb.AppendLine(row["sku"].ToString());
                }
                using (StreamWriter sw = File.AppendText(missedSKUsFile))
                {
                    sw.WriteLine(sb.ToString());
                }
                MessageBox.Show("Generated Missing Records");
            }
            else
            {
                MessageBox.Show("No Missing Records");
            }
        }

        private void btnMissingSKUs_Click(object sender, EventArgs e)
        {
            GenerateNonProcessedRecords();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dlgOpenCSV.Filter = "CSVFiles|*.csv";
            DialogResult dr = dlgOpenCSV.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                txtImageSKUFileName.Text = dlgOpenCSV.FileName;
                originalTable = new DataTable();
            }
        }

        private void btnGenerateImgCSV_Click(object sender, EventArgs e)
        {
            string imageSKUOutputFile = String.Format(ConfigurationManager.AppSettings["ImagesOutputSkuFileName"], DateTime.Now.ToString("MMddyyyy_hhmmss"));
            try
            {
                if (string.IsNullOrEmpty(txtImageSKUFileName.Text) && !File.Exists(txtImageSKUFileName.Text)
                    && string.IsNullOrEmpty(txtSimpleProdFile.Text) && !File.Exists(txtSimpleProdFile.Text)
                    && string.IsNullOrEmpty(txtVariantsFile.Text) && !File.Exists(txtVariantsFile.Text))
                {
                    MessageBox.Show("Select All SKU File(s)");
                    return;
                }
                listA = new List<string>();
                generatedImageSKUList = new List<string>();
                string NewUrl = System.Configuration.ConfigurationManager.AppSettings["NewUrlHost"];
                string Query = System.Configuration.ConfigurationManager.AppSettings["Query"];
                string ImageSourceFolder = System.Configuration.ConfigurationManager.AppSettings["ImageSourceFolder"];
                int urlFeildNr = Convert.ToInt32(ConfigurationManager.AppSettings["URLFieldNumber"]);
                int skuFieldNr = Convert.ToInt32(ConfigurationManager.AppSettings["SKUFieldNumber"]);
                int productNameFieldNr = Convert.ToInt32(ConfigurationManager.AppSettings["ProductNameFieldNumber"]);
                string showFilePath = ConfigurationManager.AppSettings["ShowFilePath"];


                DataTable simpleProdTable = new DataTable();
                if (!string.IsNullOrEmpty(txtSimpleProdFile.Text) && File.Exists(txtSimpleProdFile.Text))
                {
                    simpleProdTable = GetDataTableFromCSVFile(txtSimpleProdFile.Text);
                }
                else
                {
                    MessageBox.Show("Select the valid Simple Prod CSV file");
                    return;
                }

                DataTable variantProdTable = new DataTable();
                if (!string.IsNullOrEmpty(txtVariantsFile.Text) && File.Exists(txtVariantsFile.Text))
                {
                    variantProdTable = GetDataTableFromCSVFile(txtVariantsFile.Text);
                }
                else
                {
                    MessageBox.Show("Select the valid Variants Prod CSV file");
                    return;
                }
                using (TextFieldParser parser = new TextFieldParser(txtImageSKUFileName.Text))
                {
                    //List<string> listB = new List<string>();
                    //List<string> split = new List<string>();

                    parser.TextFieldType = FieldType.Delimited;

                    //   int count = 1;

                    parser.SetDelimiters(",");
                    parser.ReadLine();
                    while (!parser.EndOfData)
                    {

                        //Processing row
                        string[] fields = parser.ReadFields();

                        string[] image = fields[urlFeildNr].Split('"');

                        var SKUFields = fields[skuFieldNr];
                        if (image.Count() > 2)
                        {
                            var s = image[1];

                            s = s.Replace("\"", "");

                            var CountOfString = s.LastIndexOf('/');



                            if (!string.IsNullOrEmpty(SKUFields) && (SKUFields.Contains(",") || SKUFields.Contains("-")))
                            {
                                Logger.Write(SKUFields + "," + s);
                                string[] separators = { ",", "-" };
                                var skus = SKUFields.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                foreach(string sku in skus)
                                {
                                    CheckAndAddToImageCSVList(simpleProdTable, variantProdTable, sku, s);
                                }
                            }
                            else
                            {
                                CheckAndAddToImageCSVList(simpleProdTable, variantProdTable, SKUFields, s);
                            }
                        }
                    }
                    using (TextWriter tw = new StreamWriter(imageSKUOutputFile))
                    {
                        foreach (String line in listA)
                        {
                            tw.WriteLine(line);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            MessageBox.Show("Completed and generated file-" + imageSKUOutputFile);
        }

        private void CheckAndAddToImageCSVList(DataTable simpleProdTable, DataTable variantProdTable, string sku, string url)
        {
            sku = sku.Trim();
            DataRow[] skuRows = simpleProdTable.Select("sku='" + sku + "'");
            if (skuRows != null && skuRows.Length > 0)
            {
                AddToImageCSVList(sku, url);
            }
            else
            {
                skuRows = variantProdTable.Select("sku='" + sku + "'");
                if (skuRows != null && skuRows.Length > 0)
                {
                    string parentSKU = skuRows[0]["parent_sku"].ToString();
                    AddToImageCSVList(parentSKU, url);
                }
            }
        }
        private void AddToImageCSVList(string sku, string url)
        {
            if(!generatedImageSKUList.Contains(sku))
            {
                listA.Add(sku + "," + url);
                generatedImageSKUList.Add(sku);
            }

        }
        private void btnSimpleFileOpen_Click(object sender, EventArgs e)
        {
            dlgOpenCSV.Filter = "CSVFiles|*.csv";
            DialogResult dr = dlgOpenCSV.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                txtSimpleProdFile.Text = dlgOpenCSV.FileName;
            }
        }

        private void btnVariantsFileOpen_Click(object sender, EventArgs e)
        {
            dlgOpenCSV.Filter = "CSVFiles|*.csv";
            DialogResult dr = dlgOpenCSV.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                txtVariantsFile.Text = dlgOpenCSV.FileName;
            }
        }

        private void btnUploadVendorFile_Click(object sender, EventArgs e)
        {
            dlgVendorFile.Filter = "CSVFiles|*.csv";
            DialogResult dr = dlgVendorFile.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                txtVendorFile.Text = dlgVendorFile.FileName;
            }

        }

        private void btnGenerateRaw_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtVendorFile.Text))
            {
                DataTable rawTable = GetDataTableFromCSVFile(txtFileName.Text);
                string brandMasterFileName = System.Configuration.ConfigurationManager.AppSettings["BrandMasterFile"];
                DataTable brandsTable = new DataTable();
                if (String.IsNullOrEmpty(brandMasterFileName))
                {
                    brandsTable = GetDataTableFromCSVFile(brandMasterFileName);
                }
                string rangeLookupMasterFileName = System.Configuration.ConfigurationManager.AppSettings["RangeLookupMasterFileName"];
                DataTable RangeLookupMasterTable = new DataTable();
                if (String.IsNullOrEmpty(rangeLookupMasterFileName))
                {
                    RangeLookupMasterTable = GetDataTableFromCSVFile(rangeLookupMasterFileName);
                }
                string productNamePhraseFileName = System.Configuration.ConfigurationManager.AppSettings["ProductNamePhraseFileName"];
                DataTable productNamePhraseTable = new DataTable();
                if (String.IsNullOrEmpty(productNamePhraseFileName))
                {
                    productNamePhraseTable = GetDataTableFromCSVFile(productNamePhraseFileName);
                }
                string packingSizeLookUpFileName = System.Configuration.ConfigurationManager.AppSettings["PackingSizeLookUpFileName"];
                DataTable packingSizeLookUpTable = new DataTable();
                if (String.IsNullOrEmpty(packingSizeLookUpFileName))
                {
                    packingSizeLookUpTable = GetDataTableFromCSVFile(packingSizeLookUpFileName);
                }

                //Read Vendor RAw File
                DataTable rawInventory = GetDataTableFromCSVFile(txtVendorFile.Text);

                //// Requirement #2
                //DataColumn stockCol = rawInventory.Columns.Add("Stock", typeof(Int32));
                //stockCol.Expression = "Convert(IIF(Qty='','0',Qty), 'System.Int32') - Convert(IIF(PreSold='','0',PreSold), 'System.Int32')";

                // Requirement #1 a & #1 b - Selected Categories
                DataRow[] selectedRawInventory = rawInventory.Select("Category IN ('cricket', 'sporting items') AND SubCategory NOT IN ('carrom - accessories','clearance items','volleyball equipment','speciality order items')");

                foreach(DataRow row in selectedRawInventory)
                {
                    // Requirement #2 a Stock
                    int stock; int qty; int preSold;
                    Int32.TryParse(row["Qty"] == null ? "0" : row["Qty"].ToString(), out qty);
                    Int32.TryParse(row["PreSold"] == null ? "0" : row["PreSold"].ToString(), out preSold);
                    stock = qty - preSold;

                    // Requirement #2 c Units Per Case and Stock
                    int unitsPerCase = 0; decimal finalSuggestedPrice = 0; decimal suggestedPrice = 0;
                    Int32.TryParse(row["UnitsPerCase"] == null ? "0" : row["UnitsPerCase"].ToString(), out unitsPerCase);
                    if(unitsPerCase > 1)
                    {
                        stock = stock / unitsPerCase;
                        string strSuggestedPrice = row["SuggestedPrice"] == null ? "0" : row["SuggestedPrice"].ToString();
                        strSuggestedPrice = strSuggestedPrice.Replace("$","");
                        Decimal.TryParse(strSuggestedPrice, out suggestedPrice);
                        finalSuggestedPrice = stock * suggestedPrice;
                    }

                    // Requirement #3 - Brand Id
                    string brandId = string.Empty; string brandName = string.Empty;
                    brandName = row["Brand"] == null ? string.Empty : row["Brand"].ToString();
                    DataRow[] brandRows = brandsTable.Select("ReplaceWith = '" + brandName + "'");
                    if(brandRows != null && brandRows.Length > 0)
                    {
                        brandId = brandRows[0]["FindWord"].ToString();
                    }

                    // Requirement #2 b - Status
                    string status = "publish";
                    if (stock <= 0 || string.IsNullOrEmpty(brandId))
                    {
                        status = "Draft";
                    }

                    // Requirement #4 a - Body Protector
                    string category = row["SubCategory"] == null ? string.Empty : row["SubCategory"].ToString();
                    bool bodyProtectorCategory = false;
                    if(category.StartsWith("BODY PROTECTOR"))
                    {
                        bodyProtectorCategory = true;
                    }

                    // Requirement #4 b - Category
                    string subCategory = string.Empty;
                    DataRow[] categoryRows = productNamePhraseTable.Select("FindWord = '" + category + "'");
                    if (categoryRows != null && categoryRows.Length > 0)
                    {
                        category = categoryRows[0]["ReplaceWith"] == null? string.Empty : categoryRows[0]["ReplaceWith"].ToString();
                        // Requirement #4 c - SubCategory
                        subCategory = categoryRows[0]["DisplayName"] == null ? string.Empty : categoryRows[0]["DisplayName"].ToString();
                    }
                    
                    // Requirement #5 - Range 
                    string description = row["Description"] == null ? string.Empty : row["Description"].ToString();
                    string[] rangeWords = description.Split(" ".ToCharArray());
                    string rangeName = string.Empty; string brandNm = string.Empty;
                    if (rangeWords != null && rangeWords.Length > 0)
                    {
                        for (int i = 0; i < rangeWords.Length - 1; i++)
                        {
                            DataRow[] rangeLookupRows = RangeLookupMasterTable.Select("Brand = '" + rangeWords[i] + "' and RangeLookupWord = '" + rangeWords[i+1] + "'");
                            if (rangeLookupRows != null && rangeLookupRows.Length > 0)
                            {
                                brandNm = rangeWords[i];
                                rangeName = rangeLookupRows[0]["RangeAssignValue"] == null ? string.Empty : rangeLookupRows[0]["RangeAssignValue"].ToString();
                                break;
                            }
                        }
                    }

                    // Requirement #6 - Residual Text
                    string residualText = string.Empty; string categoryLevel = string.Empty; string productName = string.Empty;
                    if(bodyProtectorCategory)
                    {
                        residualText = description.Substring(description.IndexOf(brandNm));
                        // Requirement #7 b
                        categoryLevel = category;
                        if (!string.IsNullOrEmpty(subCategory))
                        {
                            categoryLevel += ">" + subCategory;
                        }
                        if (!string.IsNullOrEmpty(brandName))
                        {
                            categoryLevel += ">" + brandName;
                        }
                        // Requirement #8 b
                        productName = brandName;
                        if (!string.IsNullOrEmpty(residualText))
                        {
                            productName += " " + residualText;
                        }
                        if (!string.IsNullOrEmpty(subCategory))
                        {
                            productName += " " + subCategory;
                        }
                    }
                    else
                    {
                        residualText = description.Substring(description.IndexOf(rangeName));
                        // Requirement #7 a
                        categoryLevel = subCategory;
                        if (!string.IsNullOrEmpty(brandName))
                        {
                            categoryLevel += ">" + brandName;
                        }
                        if (!string.IsNullOrEmpty(rangeName))
                        {
                            categoryLevel += ">" + rangeName;
                        }
                        // Requirement #8 a
                        productName = brandName;
                        if(!string.IsNullOrEmpty(rangeName))
                        {
                            productName += " " + rangeName;
                        }
                        if(!string.IsNullOrEmpty(residualText))
                        {
                            productName += " " + residualText;
                        }
                        if(!string.IsNullOrEmpty(category))
                        {
                            productName += " " + category;
                        }
                    }

                    //Requirement #9
                    string packingSize = string.Empty;
                    packingSize = row["PackingSize"] == null ? string.Empty : row["PackingSize"].ToString();
                    DataRow[] packingSizeRows = packingSizeLookUpTable.Select("SubCategory = '" + subCategory + "' and PackingSize = '" + packingSize + "'");
                    if (packingSizeRows != null && packingSizeRows.Length > 0)
                    {
                        packingSize = packingSizeRows[0]["NewSize"] == null ? string.Empty : packingSizeRows[0]["NewSize"].ToString();
                    }

                    // Requirement # 11 - Meta Keywords
                    string metaKeyword = productName.Replace(" ", ",");


                }
            }

        }


        
        
    }
}
