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
    public partial class WikiDataSearch : Form
    {
        public WikiDataSearch()
        {
            InitializeComponent();
        }
        // 8.1	Create a global 2D string array, use static variables for the dimensions(row, column),
        static int row = 12;
        static int column = 4; // Name, Category, Structure, Definition
        string[,] wikiDataArray = new string[row, column];
        string defaultName = "Definition.bin";

        int currentRow = 0;


        //8.6	Create a display method that will show the following information in a List box: Name and Category,
        public void displayArray()
        {
            listViewOutput.Items.Clear();
            listViewOutput.ForeColor = Color.Black;
            for (int x = 0; x < row; x++)
            {
                if (!string.IsNullOrWhiteSpace(wikiDataArray[x, 0]))
                {
                    ListViewItem lvi = new ListViewItem(wikiDataArray[x, 0]);
                    lvi.SubItems.Add(wikiDataArray[x, 1]);
                    lvi.SubItems.Add(wikiDataArray[x, 2]);
                    lvi.SubItems.Add(wikiDataArray[x, 3]);
                    listViewOutput.Items.Add(lvi);
                }

            }
        }
        //8.4	Write the code for a Bubble Sort method to sort the 2D array by Name ascending,
        //ensure you use a separate swap method that passes (by reference) the array element to be swapped (do not use any built-in array methods),
        public void bubbleSort()
        {
            for (int x = 1; x < row; x++)
            {
                for (int i = 0; i < row - 1; i++)
                {
                    if (!(string.IsNullOrEmpty(wikiDataArray[i + 1, 0])))
                    {
                        if (string.Compare(wikiDataArray[i, 0], wikiDataArray[i + 1, 0]) == 1)
                        {
                            sorting(i);
                        }
                    }
                }
            }
        }
        public void sorting(int index)
        {
            string temp;
            for (int i = 0; i < column; i++)
            {
                temp = wikiDataArray[index, i];
                wikiDataArray[index, i] = wikiDataArray[index + 1, i];
                wikiDataArray[index + 1, i] = temp;
            }
        }
        //8.3	Create a CLEAR method to clear the four text boxes so a new definition can be added,
        public void clearTextBox()
        {
            textBoxInput.Clear();
            textBoxName.Clear();
            textBoxCategory.Clear();
            textBoxStructure.Clear();
            textBoxDefinition.Clear();
        }
        private void allFunctions()
        {
            bubbleSort();
            displayArray();
            clearTextBox();
        }
        // 8.2	Create an ADD button that will store the information from the 4 text boxes into the 2D array
        private void buttonAdd_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(textBoxName.Text) &&
                !string.IsNullOrWhiteSpace(textBoxStructure.Text) &&
                !string.IsNullOrWhiteSpace(textBoxCategory.Text) &&
                !string.IsNullOrWhiteSpace(textBoxDefinition.Text))
            {
                var result = MessageBox.Show("Add new data?", "Press 'Yes' to proceed",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                for (int x = 0; x < row; x++)
                {
                    if (result == DialogResult.Yes)
                    {
                        if (wikiDataArray[x, 0] == "~" || wikiDataArray[x, 0] == "")
                        {
                            if (currentRow <= row)
                            {
                                wikiDataArray[x, 0] = textBoxName.Text;
                                wikiDataArray[x, 1] = textBoxCategory.Text;
                                wikiDataArray[x, 2] = textBoxStructure.Text;
                                wikiDataArray[x, 3] = textBoxDefinition.Text;
                                currentRow++;
                                bubbleSort();
                                displayArray();
                                clearTextBox();
                                break;
                            }
                            else
                            {
                                MessageBox.Show("The wiki is full. Delete some data to enter more");
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Text boxes are empty");
                textBoxName.Focus();
            }

        }
        // 8.2 Delete Button
        private void Delete()
        {
            try
            {
                int currentRecord = listViewOutput.SelectedIndices[0];
                if (currentRecord >= 0)
                {
                    DialogResult dialogResult = MessageBox.Show( "Would you like to delete this wiki data", 
                        "Click 'Yes' to proceed with the deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dialogResult == DialogResult.Yes)
                    {
                        ListViewItem item = new ListViewItem(wikiDataArray[currentRecord, 0]);
                        wikiDataArray[currentRecord, 0] = " ";
                        wikiDataArray[currentRecord, 1] = " ";
                        wikiDataArray[currentRecord, 2] = " ";
                        wikiDataArray[currentRecord, 3] = " ";
                        currentRow--;
                        clearTextBox();
                        bubbleSort();
                        displayArray();
                    }
                    else
                    {
                        MessageBox.Show("Item has not been deleted", "Delete Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Cannot perform this action");
            }
        }
        private void buttonDelete_Click(object sender, EventArgs e)
        {

            Delete();
            if (!string.IsNullOrWhiteSpace(textBoxInput.Text) &&
                !string.IsNullOrWhiteSpace(textBoxCategory.Text) &&
                !string.IsNullOrWhiteSpace(textBoxStructure.Text) &&
                !string.IsNullOrWhiteSpace(textBoxDefinition.Text))
            {
                clearTextBox();

            }
           
        }
        // 8.2 Edit Button
        private void buttonEdit_Click(object sender, EventArgs e)
        {

            try
            {
                int currentRow = listViewOutput.SelectedIndices[0];
                if (currentRow >= 0)
                {
                    var result = MessageBox.Show("Edit this wiki data?", "Click 'OK' to proceed with the edit.", MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question);

                    if(result == DialogResult.OK)
                    {
                        wikiDataArray[currentRow, 0] = textBoxName.Text;
                        wikiDataArray[currentRow, 1] = textBoxCategory.Text;
                        wikiDataArray[currentRow, 2] = textBoxStructure.Text;
                        wikiDataArray[currentRow, 3] = textBoxDefinition.Text;
                        allFunctions();

                    }
                }
          
            } 
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("You haven't selected a data to edit.");
            }



        }
        // 8.5	Write the code for a Binary Search for the Name in the 2D array and
        // display the information in the other textboxes when found,
        // add suitable feedback if the search in not successful and
        // clear the search textbox (do not use any built-in array methods),
        private void buttonSearch_Click(object sender, EventArgs e)
        {

            int startIndex = -1;
            int finalIndex = currentRow; // set size of data in array
            bool flag = false;
            int foundIndex = -1;
            while (!flag && !((finalIndex - startIndex) <= 1))
            {
                int newIndex = (finalIndex + startIndex) / 2;
                if (string.Compare(wikiDataArray[newIndex, 0], textBoxInput.Text) == 0)
                {
                    foundIndex = newIndex;
                    flag = true;
                    break;
                }
                else
                {
                    if (string.Compare(wikiDataArray[newIndex, 0], textBoxInput.Text) == 1)
                        finalIndex = newIndex;
                    else
                        startIndex = newIndex;
                }
            }
            if (flag)
            {
                //textBoxInput.Text = wikiDataArray[foundIndex, 0];
                textBoxName.Text = wikiDataArray[foundIndex, 0];
                textBoxCategory.Text = wikiDataArray[foundIndex, 1];
                textBoxStructure.Text = wikiDataArray[foundIndex, 2];
                textBoxDefinition.Text = wikiDataArray[foundIndex, 3];
                MessageBox.Show("Found at index: " + foundIndex);
                clearTextBox();
            }
            else
                MessageBox.Show("Not found");

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
                for (int x = 0; x < row; x++)
                {
                    wikiDataArray[x, 0] = "";
                    wikiDataArray[x, 1] = "";
                    wikiDataArray[x, 2] = "";
                    wikiDataArray[x, 3] = "";
                }
            }
            displayArray();

        }
        // 8.9	Create a LOAD button that will read the information
        // from a binary file called definitions.dat into the 2D array,
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
                            //stream.Position = 0;
                            wikiDataArray[x, y] = (string)bin.Deserialize(stream);
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
        //  8.8	Create a SAVE button so the information
        //  from the 2D array can be written into a binary file called definitions.dat which is sorted by Name,
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
                            bin.Serialize(stream, wikiDataArray[x, y]);
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        // 8.7	Create a method so the user can select a definition (Name) from the Listbox and all the information is displayed in the appropriate Textboxes,
        private void listViewOutput_Click(object sender, EventArgs e)
        {
            try
            {
            int currentRecord = listViewOutput.SelectedIndices[0];
            textBoxName.Text = wikiDataArray[currentRecord, 0];
            textBoxCategory.Text = wikiDataArray[currentRecord, 1];
            textBoxStructure.Text = wikiDataArray[currentRecord, 2];
            textBoxDefinition.Text = wikiDataArray[currentRecord, 3];
            } 
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Select a data from the list.");
            }

        }
        private void listViewOutput_DoubleClick(object sender, EventArgs e)
        {
            clearTextBox();
            displayArray();
        }

    }

}
