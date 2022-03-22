using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.IO;


namespace WikiDataSearch
{
    public partial class WikiSearch : Form
    {
        public WikiSearch()
        {
            InitializeComponent();
        }
        static int row = 10;
        static int column = 3;
        string[,] wikiArray = new string[row, column];
        string defaultName = "Default.bin";

        int currentRow = 0;

        private void displayArray()
        {
            listViewOutput.Items.Clear();
            listViewOutput.ForeColor = Color.Black;
            for (int x = 0; x < row; x++)
            {
                ListViewItem lvi = new ListViewItem(wikiArray[x, 0]);
                lvi.SubItems.Add(wikiArray[x, 1]);
                lvi.SubItems.Add(wikiArray[x, 2]);
                listViewOutput.Items.Add(lvi);
            }
        }

        public void clearTextBox()
        {
            textBoxInput.Clear();
            textBoxName.Clear();
            textBoxCategory.Clear();
            textBoxStructure.Clear();
            textBoxDefinition.Clear();
        }



        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxName.Text) &&
                !string.IsNullOrWhiteSpace(textBoxStructure.Text) &&
                !string.IsNullOrWhiteSpace(textBoxCategory.Text) &&
                !string.IsNullOrWhiteSpace(textBoxDefinition.Text))
            {
                for (int x = 0; x < row; x++)
                {
                    if (wikiArray[x, 0] == " ")
                    {
                        //wikiArray[x, 0] = textBoxInput.Text;
                        wikiArray[x, 0] = textBoxName.Text;
                        wikiArray[x, 1] = textBoxCategory.Text;
                        wikiArray[x, 2] = textBoxStructure.Text;
                        wikiArray[x, 3] = textBoxDefinition.Text;

                        var result = MessageBox.Show("Would you like to add this information?", "Are you sure?",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (result == DialogResult.OK)
                            break;
                        else
                        {

                            wikiArray[x, 0] = "";
                            wikiArray[x, 1] = "Category";
                            wikiArray[x, 2] = "Structure";
                            wikiArray[x, 3] = "Definition";
                            break;
                        }
                    }
                }
            }
            displayArray();
            clearTextBox();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            // Reminder in case you want to change to listview instead of listbox
            int data = listViewOutput.SelectedIndices[0];
            if (data >= 0)
            {
                DialogResult result = MessageBox.Show("Would you like to delete this definition?",
                    "Are you sure you want to delete this?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // "" Emptying the textboxes
                    wikiArray[data, 0] = "";
                    wikiArray[data, 1] = "";
                    wikiArray[data, 2] = "";
                    wikiArray[data, 3] = "";
                    displayArray();
                    clearTextBox();
                }
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            // Change to list view
            int data = listViewOutput.SelectedIndices[0];
            if (data >= 0)
            {
                var result = MessageBox.Show("Do you wish to edit this definition?"
                    , "Finalise edit?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    wikiArray[data, 0] = textBoxName.Text;
                    wikiArray[data, 1] = textBoxStructure.Text;
                    wikiArray[data, 2] = textBoxCategory.Text;
                    wikiArray[data, 3] = textBoxDefinition.Text;
                    clearTextBox();

                }
            }
        }


        private void WikiSearch_Load(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialogVG = new OpenFileDialog();
            openFileDialogVG.InitialDirectory = Application.StartupPath;
            openFileDialogVG.Filter = "BIN Files|*.bin";
            openFileDialogVG.Title = "Select a BIN File";
            if (openFileDialogVG.ShowDialog() == DialogResult.OK)
            {
                openRecord(openFileDialogVG.FileName);
            }
            else
            {
                for(int x = 0; x < row; x++)
                {
                    wikiArray[x, 0] = " ";
                    wikiArray[x, 1] = "~";
                    wikiArray[x, 2] = " ";
                } 
            }
            displayArray();
        }


        private void buttonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialogVG = new OpenFileDialog();
            openFileDialogVG.InitialDirectory = Application.StartupPath;
            openFileDialogVG.Filter = "BIN Files|*.bin";
            openFileDialogVG.Title = "Select a BIN File";
            if (openFileDialogVG.ShowDialog() == DialogResult.OK)
            {
                openRecord(openFileDialogVG.FileName);
            }
        }
        private void openRecord(string openFileName)
        {
            try
            {
                using (Stream stream = File.Open(openFileName, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    for (int y = 0; y < column; y++)
                    {
                        for (int x = 0; x < row; x++)
                        {
                            wikiArray[x, y] = (string)bin.Deserialize(stream);
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            displayArray();
        }


        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialogVG = new SaveFileDialog();
            saveFileDialogVG.Filter = "bin file|*.bin";
            saveFileDialogVG.Title = "Save a bin File";
            saveFileDialogVG.InitialDirectory = Application.StartupPath;
            saveFileDialogVG.DefaultExt = "bin";
            saveFileDialogVG.ShowDialog();

            string fileName = saveFileDialogVG.FileName;
            if (saveFileDialogVG.FileName != "")
            {
                saveRecord(fileName);
            }
            else
            {
                saveRecord(defaultName);
            }
        }

        private void saveRecord(string saveFileName)
        {
            try
            {
                using (Stream stream = File.Open(saveFileName, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    for (int y = 0; y < column; y++)
                    {
                        for (int x = 0; x < row; x++)
                        {
                            bin.Serialize(stream, wikiArray[x, y]);
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void listViewOutput_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listViewOutput_Click(object sender, EventArgs e)
        {
            int currentRecord = listViewOutput.SelectedIndices[0];
            textBoxName.Text = wikiArray[currentRecord, 0];
            textBoxCategory.Text = wikiArray[currentRecord, 1];
            textBoxStructure.Text = wikiArray[currentRecord, 2];
            //textBoxDefinition.Text = wikiArray[currentRecord, 3];
        }

        private void textBoxInput_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
