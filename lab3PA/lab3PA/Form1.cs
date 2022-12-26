namespace lab3PA
{
    public partial class Form1 : Form
    {
        static string path = "db.txt";
        const int DEGREE = 50;
        BTree? tree = FileHelper.ReadFromFile(path, DEGREE);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string message = "";
            if (radioButton1.Checked)
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    message = "�������! ���� �������� �������� ����� ������� ���� ����������";
                }
                else
                {
                    if(int.TryParse(textBox1.Text, out int key))
                    {
                        int compCount = 0;
                        NodeData? result = tree.Search(key, ref compCount);
                        if (result != null)
                        {
                            message = $"���� {key} �������� �� {compCount} ��������! ���� ������� ��������: '{result.Value}'";
                        }
                        else
                        {
                            message = $"���� {key} �� ��������!";
                        }
                    }
                    else
                    {
                        message = $"�������! {key} - ����������� ����. ����� ����������";
                    }
                }
            }
            else if(radioButton2.Checked)
            {
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
                {
                    message = "�������! ���� �������� ������ ���� �����������. ��������� ������ ���������";
                }
                else
                {
                    if (int.TryParse(textBox1.Text, out int key))
                    {
                        if (!FileHelper.IsThereRepeatingKeys(tree, key))
                        {
                            tree.Insert(key, textBox2.Text);
                            message = "����� ������ ������ �� ��!";
                        }
                        else
                        {
                            message = $"�������! {key} ���� ��� � � ��. ��������� ������ ���������";
                        }
                    }
                    else
                    {
                        message = $"�������! {key} - ����������� ����. ��������� ������ ���������";
                    }
                }
            }
            else if (radioButton3.Checked)
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    message = "�������! ���� �������� �������� ����� ������� ���� ����������. ��������� ���������";
                }
                else
                {
                    if (int.TryParse(textBox1.Text, out int key))
                    {
                        bool success = true;
                        tree.Delete(key, ref success);
                        if (success)
                        {
                            message = $"���� {key} �������� ������";
                        }
                        else
                        {
                            message = $"���� {key} �� ��������! ��������� ���������";
                        }
                    }
                    else
                    {
                        message = $"�������! {key} - ����������� ����. ��������� ���������";
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
                {
                    message = "�������! ���� �������� ������ ���� �����������. ����������� ���������";
                }
                else
                {
                    if (int.TryParse(textBox1.Text, out int key))
                    {
                        bool success = false;
                        tree.Edit(key, textBox2.Text, ref success);
                        if (success)
                        {
                            message = $"���� {key} ������ ������! ���� �������� - '{textBox2.Text}'";
                        }
                        else
                        {
                            message = $"���� {key} �� ��������! ����������� ���������";
                        }
                    }
                }
            }
            textBox3.AppendText(message + Environment.NewLine);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FileHelper.SaveFile(path, tree, out string message);
            textBox3.AppendText(message + Environment.NewLine);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            File.Delete(path);
            string message = "���� ������ �������!";
            textBox3.AppendText(message + Environment.NewLine);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tree.SaveData(tree.root);
            textBox3.AppendText(Environment.NewLine + "-=������ �� ������=-" + Environment.NewLine);
            for (int i = 0; i < tree.fullTreeList.Count; i++)
            {
                textBox3.AppendText(tree.fullTreeList[i].ToString() + Environment.NewLine);
            }
        }
    }
}