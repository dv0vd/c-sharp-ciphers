using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace lab1
{
    public partial class Form1 : Form
    {
        private const string inputFile = "Входной файл...", outputFile = "Каталог сохранения выходных файлов...";
        private string inputFileName, outputDirectory;
        private char[] mas = { 'а','б','в','г','д','е','ё','ж','з','и','й','к','л','м','н','о','п','р','с','т','у','ф','х','ц','ч','ш','щ','ъ','ь','ы','э','ю','я',' ',',','.'};
        private List<int> simple = new List<int>();
        private int rows = 6, columns = 6;
        private bool CodeOffset;

        private static bool isSimple(int N)
        {
            bool tf = false;
            for (int i = 2; i < N; i++)
            {
                if (N % i == 0)
                {
                    tf = false;
                    break;
                }
                else
                {
                    tf = true;
                }
            }
            return tf;
        }

        private void CreateSimpleMas()
        {
            for (int i = 2; i <= mas.Length; i++)
            {
                if ((isSimple(i))&& (mas.Length%i != 0))
                {
                    simple.Add(i);
                }
            }
        }

        private void DisableFile()
        {
            label1.Enabled = false;
            label2.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void EnableFile()
        {
            label1.Enabled = true;
            label2.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void DisableText()
        {
            label8.Enabled = false;
            label9.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void EnableText()
        {
            label8.Enabled = true;
            label9.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
        }

        public Form1()
        {
            InitializeComponent();
            CreateSimpleMas();
            radioButton1.Checked = true ;
            DisableText();
            EnableFile();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog OPF = new OpenFileDialog();
            OPF.Filter = "Файлы |*.txt";
            OPF.Title = "Выбрать файл";
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                inputFileName = OPF.FileName;
                label1.Text = inputFileName;
            }
        }

        private void CopyFile(string str, string path)
        {
            using (StreamReader fin = new StreamReader(str))
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                using (StreamWriter fout = new StreamWriter(fs, Encoding.UTF8))
                {
                    while ((str = fin.ReadLine()) != null)
                    {
                        str = str.ToLower();
                        fout.WriteLine(str);
                    }
                    fout.Close();
                }
                fin.Close();
                fs.Close();
            }
        }

        private int FindSymbolIndex(char sym)
        {
            int i = 0;
            for(; i<mas.Length; i++)
            {
                if (sym == mas[i])
                    break;
            }   
            return i;
        }

        private bool CheckAlphabet(string str)
        {
            for(int i=0; i< str.Length; i++)
            {
                bool check = false;
                for(int j=0; j<mas.Length; j++)
                {
                    if(str[i] == mas[j])
                    {
                        check = true;
                        break;
                    }
                }
                if (!check)
                    return false;
            }
            return true;
        }

        private bool ShiftChecking(ref int shift, int from)
        {
            try
            {
                if(from == 1)
                    shift = Int32.Parse(textBox1.Text);
                else
                  if(from == 2)
                      shift = Int32.Parse(textBox2.Text);
            }
            catch
            {
                MessageBox.Show("Смещение должно быть целым числом!");
                return false;
            }
            return true;
        }

        private bool CheckingFile()
        {
            bool check = false;
            using (StreamReader fin = new StreamReader(inputFileName))
            {
                string str;
                while ((str = fin.ReadLine()) != null)
                {
                    str = str.ToLower();
                    if (!CheckAlphabet(str))
                    {
                        check = true;
                        break;
                    }
                }
                if (check)
                {
                    MessageBox.Show("В файле присутствуют недопустимые символы!");
                    fin.Close();
                    return true;
                }
                fin.Close();
            }
            return false;
        }

        private void FrequencyAnalizing(string path, bool file)
        {
            int count = 0;
            int[] amount = new int[mas.Length];
            int[] amountBasic = new int[mas.Length];
            for (int i = 0; i < mas.Length; i++)
            {
                amount[i] = 0;
                amountBasic[i] = 0;
            }
            if(file)
            {
                using (StreamReader fin = new StreamReader(path))
                {
                    string str;
                    while ((str = fin.ReadLine()) != null)
                    {
                        for (int i = 0; i < str.Length; i++)
                        {
                            amount[FindSymbolIndex(str[i])]++;
                            count++;
                        }
                    }
                    fin.Close();
                }
                using (StreamReader fin = new StreamReader(inputFileName))
                {
                    string str;
                    while ((str = fin.ReadLine()) != null)
                    {
                        for (int i = 0; i < str.Length; i++)
                        {
                            amountBasic[FindSymbolIndex(str[i])]++;
                        }
                    }
                    fin.Close();
                }
            }
            else
            {
                String[] s = textBox4.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                for(int k=0; k<s.Length; k++)
                {
                    string str = s[k];
                    for (int i = 0; i < str.Length; i++)
                    {
                        amountBasic[FindSymbolIndex(str[i])]++;
                        count++;
                    }
                }
               s = textBox5.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
               for (int k = 0; k < s.Length; k++)
               {
                    string str = s[k];
                    for (int i = 0; i < str.Length; i++)
                    {
                        amount[FindSymbolIndex(str[i])]++;
                        count++;
                    }
               }
            }
            listView2.Clear();
            listView1.Clear();
            listView1.View = View.Details;
            listView1.Columns.Add("Символ");
            listView1.Columns.Add( "Частота");
            listView1.Columns[1].Width = 100;
            listView2.View = View.Details;
            listView2.Columns.Add("Символ");
            listView2.Columns.Add("Частота");
            listView2.Columns[1].Width = 100;
            listView1.Items.Add(new ListViewItem(new string[] { "Всего", count.ToString() }));
            listView2.Items.Add(new ListViewItem(new string[] { "Всего", count.ToString() }));
            for (int i = 0; i < mas.Length; i++)
            {
                ListViewItem item = new ListViewItem(new string[] { mas[i].ToString(), ((float)(amount[i])/(float)(count)).ToString() });
                listView1.Items.Add(item);
            }
            for (int i = 0; i < mas.Length; i++)
            {
                ListViewItem item = new ListViewItem(new string[] { mas[i].ToString(), ((float)(amountBasic[i]) / (float)(count)).ToString() });
                listView2.Items.Add(item);
            }
        }

        private void Finish()
        {
            label1.Text = inputFile;
            label2.Text = outputFile;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            MessageBox.Show("Успех!");
        }

        private void Working(int what)
        {
            if(radioButton1.Checked)
            {
                if ((label1.Text == inputFile) || (label2.Text == outputFile))
                {
                    MessageBox.Show("Не выбраны пути к файлам!");
                }
                else
                {
                    int shift = 0;
                    int r = 2;
                    if (what == 1)
                        r = 1;
                    if (!ShiftChecking(ref shift, r))
                    {
                        return;
                    }
                    if ((what == 2) || (what == 3))
                    {
                        shift = Math.Abs(shift);
                        if (!CheckKey(shift))
                        {
                            MessageBox.Show("Ошибка! Сообщение с таким ключом расшифровать будет невозможно!");
                            return;
                        }
                    }
                    if (!CheckingFile())
                    {
                        string path = outputDirectory + "\\ИСХОДНЫЙ ФАЙЛ.txt";
                        CopyFile(inputFileName, path);
                        if (what == 1)
                            path = outputDirectory + "\\РЕЗУЛЬТИРУЮЩИЙ ФАЙЛ - АДДИТИВНЫЙ ШИФР.txt";
                        else
                            path = outputDirectory + "\\РЕЗУЛЬТИРУЮЩИЙ ФАЙЛ - МУЛЬТИПЛИКАТИВНЫЙ ШИФР.txt";
                        if ((!CodeOffset) && (what == 1))
                            shift = -shift;
                        using (StreamReader fin = new StreamReader(inputFileName))
                        {
                            FileStream fs = new FileStream(path, FileMode.Create);
                            using (StreamWriter fout = new StreamWriter(fs, Encoding.UTF8))
                            {
                                string str;
                                while ((str = fin.ReadLine()) != null)
                                {
                                    str = str.ToLower();
                                    string outStr = "";
                                    for (int i = 0; i < str.Length; i++)
                                    {
                                        int index = FindSymbolIndex(str[i]);
                                        switch (what)
                                        {
                                            case 1:
                                                {
                                                    index += shift;
                                                    index %= mas.Length;
                                                    if (index < 0)
                                                    {
                                                        index = mas.Length + index;
                                                    }
                                                    break;
                                                }
                                            case 2:
                                                {
                                                    index *= shift;
                                                    index %= mas.Length;
                                                    break;
                                                }
                                            case 3:
                                                {
                                                    index *= FindKReverse(shift);
                                                    index %= mas.Length;
                                                    break;
                                                }
                                        }
                                        outStr += mas[index];
                                    }
                                    fout.WriteLine(outStr);
                                }
                                fout.Close();
                            }
                            fs.Close();
                            fin.Close();
                        }
                        Finish();
                        FrequencyAnalizing(path, radioButton1.Checked);
                    }
                }
            }
            else
            {
                int shift = 0;
                int r = 2;
                if (what == 1)
                    r = 1;
                if (!ShiftChecking(ref shift, r))
                {
                    return;
                }
                if ((what == 2) || (what == 3))
                {
                    shift = Math.Abs(shift);
                    if (!CheckKey(shift))
                    {
                        MessageBox.Show("Ошибка! Сообщение с таким ключом расшифровать будет невозможно!");
                        return;
                    }
                }
                String[] s = textBox4.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                for(int i=0; i<s.Length; i++)
                {
                    if (!CheckAlphabet(s[i]))
                    {
                        MessageBox.Show("Ошибка! Присутствуют недопустимые символы!");
                        return;
                    }
                }
                textBox5.Text = "";
                if ((!CodeOffset) && (what == 1))
                    shift = -shift;
                for (int j = 0; j < s.Length; j++)
                {
                    string outStr = "";
                    string str = s[j];
                    for (int i = 0; i < str.Length; i++)
                    {
                        int index = FindSymbolIndex(str[i]);
                        switch (what)
                        {
                            case 1:
                                {
                                    index += shift;
                                    index %= mas.Length;
                                    if (index < 0)
                                    {
                                        index = mas.Length + index;
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    index *= shift;
                                    index %= mas.Length;
                                    break;
                                }
                            case 3:
                                {
                                    index *= FindKReverse(shift);
                                    index %= mas.Length;
                                    break;
                                }
                        }
                        outStr += mas[index];
                    }
                    textBox5.Text = textBox5.Text + outStr + "\r\n";
                }
                Finish();
                FrequencyAnalizing("", radioButton1.Checked);
            }
        }

        private bool CheckKey(int key)
        {
            bool check = false;
            for(int i=0; i<simple.Count; i++)
            {
                if(key == simple[i])
                {
                    check = true;
                    break;
                }
            }
            return check;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Working(2);
        }

        private int FindKReverse(int start)
        {
            for (int i = 1; ; i++)
            {
                int k = start;
                k *= i;
                if (k % mas.Length == 1)
                {
                    return (int)(k/start);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Working(3);
        }

        private bool CheckInMatrix(char sym, char[,]matrix)
        {
            for(int i=0; i<rows; i++)
            {
                for(int j=0; j<columns; j++)
                {
                    if (sym == matrix[i, j])
                        return true;
                }
            }
            return false;
        }

        private void FillMatrix(char[,] matrix, string phrase)
        {
            phrase = phrase.ToLower();
            int index = 0;
            for(int i=0; i<phrase.Length; i++)
            {
                if(!CheckInMatrix(phrase[i], matrix))
                {
                    matrix[index/rows,index % rows] = phrase[i];
                    index++;
                }
            }
            for (int i=0; i<mas.Length; i++)
            {
                if(!CheckInMatrix(mas[i],matrix))
                {
                    matrix[index / rows, index % rows] = mas[i];
                    index++;
                }
            }
        }

        private void InitializeMatrix(char[,] matrix)
        {
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    matrix[i, j] = '!';
        }

        private string GetBigrams(string str)
        {
            string strNew = "";
            int i;
            for(i=0; i+1<str.Length; i+=2)
            {
                if(str[i] != str[i+1])
                {
                    strNew = strNew + str[i] + str[i + 1];
                }
                else
                {
                    str = str.Insert(i + 1, "я");
                    strNew = strNew + str[i] + str[i + 1];
                }
            }
            if(i!=str.Length)
            {
                str += 'я';
                strNew = strNew + str[i] + str[i + 1];
            }
            return strNew;
        }

        private int GetRowIndex(char sym, char[,]matrix)
        {
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    if (sym == matrix[i, j])
                        return i;
            return -1;
        }

        private int GetColumnIndex(char sym, char[,] matrix)
        {
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    if (sym == matrix[i, j])
                        return j;
            return -1;
        }

        private void CodeBigrams(string bigrams, StreamWriter fout, char[,] matrix)
        {
            string str = "";
            for(int i=0; i<bigrams.Length-1; i+=2)
            {
                char sym1 = bigrams[i];
                char sym2 = bigrams[i + 1];
                int indexRow1 = GetRowIndex(sym1, matrix);
                int indexRow2 = GetRowIndex(sym2, matrix);
                int indexColumn1 = GetColumnIndex(sym1, matrix);
                int indexColumn2 = GetColumnIndex(sym2, matrix);
                if(indexRow1 == indexRow2)
                {
                    if (indexColumn1 == columns-1)
                        str += matrix[indexRow1, 0];
                    else
                        str += matrix[indexRow1, indexColumn1+1];
                    if (indexColumn2 == columns-1)
                        str += matrix[indexRow2, 0];
                    else
                        str += matrix[indexRow2, indexColumn2 + 1];
                }
                else
                {
                    if(indexColumn1 == indexColumn2)
                    {
                        if (indexRow1 == rows-1)
                            str += matrix[0, indexColumn1];
                        else
                            str += matrix[indexRow1+1, indexColumn1];
                        if (indexRow2 == rows-1)
                            str += matrix[0, indexColumn2];
                        else
                            str += matrix[indexRow2 + 1, indexColumn2];
                    }
                    else
                    {
                        str += matrix[indexRow1,indexColumn2];
                        str += matrix[indexRow2, indexColumn1];
                    }
                }
            }

            if (fout != null)
                fout.WriteLine(str);
            else
                textBox5.Text = textBox5.Text + str + "\r\n";
        }

        private void Coding(string str, StreamWriter fout, char[,] matrix)
        {
            string bigrams = GetBigrams(str);
            CodeBigrams(bigrams, fout, matrix);
        }

        private void Decoding(string bigrams, StreamWriter fout, char[,] matrix)
        {
            string str = "";
            for (int i = 0; i < bigrams.Length - 1; i += 2)
            {
                char sym1 = bigrams[i];
                char sym2 = bigrams[i + 1];
                int indexRow1 = GetRowIndex(sym1, matrix);
                int indexRow2 = GetRowIndex(sym2, matrix);
                int indexColumn1 = GetColumnIndex(sym1, matrix);
                int indexColumn2 = GetColumnIndex(sym2, matrix);
                if (indexRow1 == indexRow2)
                {
                    if (indexColumn1 == 0)
                        str += matrix[indexRow1, columns-1];
                    else
                        str += matrix[indexRow1, indexColumn1 - 1];
                    if (indexColumn2 == 0)
                        str += matrix[indexRow2, columns-1];
                    else
                        str += matrix[indexRow2, indexColumn2 - 1];
                }
                else
                {
                    if (indexColumn1 == indexColumn2)
                    {
                        if (indexRow1 == 0)
                            str += matrix[rows-1, indexColumn1];
                        else
                            str += matrix[indexRow1 - 1, indexColumn1];
                        if (indexRow2 == 0)
                            str += matrix[rows-1, indexColumn2];
                        else
                            str += matrix[indexRow2 - 1, indexColumn2];
                    }
                    else
                    {
                        str += matrix[indexRow1, indexColumn2];
                        str += matrix[indexRow2, indexColumn1];
                    }
                }
            }
            if(fout!=null)
                fout.WriteLine(str);
            else
                textBox5.Text = textBox5.Text + str + "\r\n";
        }

        private void button7_Click(object sender, EventArgs e)
        {

            if (radioButton1.Checked)
            {
                if ((label1.Text == inputFile) || (label2.Text == outputFile))
                {
                    MessageBox.Show("Не выбраны пути к файлам!");
                }
                else
                {
                    if ((CheckAlphabet(textBox3.Text)) && (textBox3.Text != ""))
                    {
                        if (!CheckingFile())
                        {
                            char[,] matrix = new char[rows, columns];
                            InitializeMatrix(matrix);
                            FillMatrix(matrix, textBox3.Text);
                            string path = outputDirectory + "\\ИСХОДНЫЙ ФАЙЛ.txt";
                            CopyFile(inputFileName, path);
                            path = outputDirectory + "\\РЕЗУЛЬТИРУЮЩИЙ ФАЙЛ - ШИФР ПЛЕЙФЕРА.txt";
                            using (StreamReader fin = new StreamReader(inputFileName))
                            {
                                FileStream fs = new FileStream(path, FileMode.Create);
                                using (StreamWriter fout = new StreamWriter(fs, Encoding.UTF8))
                                {
                                    string str;
                                    while ((str = fin.ReadLine()) != null)
                                    {
                                        str = str.ToLower();
                                        Decoding(str, fout, matrix);
                                    }
                                    fout.Close();
                                }
                                fin.Close();
                            }
                            Finish();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Фраза содержит недопустимые символы!");
                    }
                }
            }
            else
            {
                if ((CheckAlphabet(textBox3.Text)) && (textBox3.Text != ""))
                {
                    String[] s = textBox4.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < s.Length; i++)
                    {
                        if (!CheckAlphabet(s[i]))
                        {
                            MessageBox.Show("Ошибка! В тексте присутствуют недопустимые символы!");
                            return;
                        }
                    }
                    textBox5.Text = "";
                    char[,] matrix = new char[rows, columns];
                    InitializeMatrix(matrix);
                    FillMatrix(matrix, textBox3.Text);
                    for (int i = 0; i < s.Length; i++)
                    {
                        Decoding(s[i], null, matrix);
                    }
                    Finish();
                }
                else
                {
                    MessageBox.Show("Фраза содержит недопустимые символы!");
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            EnableFile();
            DisableText();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            EnableText();
            DisableFile();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            CodeOffset = false;
            Working(1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                if ((label1.Text == inputFile) || (label2.Text == outputFile))
                {
                    MessageBox.Show("Не выбраны пути к файлам!");
                }
                else
                {
                    if ((CheckAlphabet(textBox3.Text)) && (textBox3.Text != ""))
                    {
                        if (!CheckingFile())
                        {
                            char[,] matrix = new char[rows, columns];
                            InitializeMatrix(matrix);
                            FillMatrix(matrix, textBox3.Text);
                            string path = outputDirectory + "\\ИСХОДНЫЙ ФАЙЛ.txt";
                            CopyFile(inputFileName, path);
                            path = outputDirectory + "\\РЕЗУЛЬТИРУЮЩИЙ ФАЙЛ - ШИФР ПЛЕЙФЕРА.txt";
                            using (StreamReader fin = new StreamReader(inputFileName))
                            {
                                FileStream fs = new FileStream(path, FileMode.Create);
                                using (StreamWriter fout = new StreamWriter(fs, Encoding.UTF8))
                                {
                                    string str;
                                    while ((str = fin.ReadLine()) != null)
                                    {
                                        str = str.ToLower();
                                        Coding(str, fout, matrix);
                                    }
                                    fout.Close();
                                }
                                fin.Close();
                            }
                            Finish();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Фраза содержит недопустимые символы!");
                    }
                }
            }
            else
            {
                if ((CheckAlphabet(textBox3.Text)) && (textBox3.Text != ""))
                {
                    String[] s = textBox4.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < s.Length; i++)
                    {
                        if (!CheckAlphabet(s[i]))
                        {
                            MessageBox.Show("Ошибка! В тексте присутствуют недопустимые символы!");
                            return;
                        }
                    }
                    textBox5.Text = "";
                    char[,] matrix = new char[rows, columns];
                    InitializeMatrix(matrix);
                    FillMatrix(matrix, textBox3.Text);
                    for (int i = 0; i < s.Length; i++)
                    {
                        Coding(s[i], null, matrix);

                    }
                    Finish();
                }
                else
                {
                    MessageBox.Show("Фраза содержит недопустимые символы!");
                }
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            CodeOffset = true;
            Working(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                outputDirectory = FBD.SelectedPath;
                FBD.Description = "Выбрать директорию";
                label2.Text = outputDirectory;
            }
        }
    }
}
