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
                    message = "Помилка! Поля введення значення ключа повинно бути заповненим";
                }
                else
                {
                    if(int.TryParse(textBox1.Text, out int key))
                    {
                        int compCount = 0;
                        NodeData? result = tree.Search(key, ref compCount);
                        if (result != null)
                        {
                            message = $"Ключ {key} знайдено за {compCount} порівнянь! Йому відповідає значення: '{result.Value}'";
                        }
                        else
                        {
                            message = $"Ключ {key} не знайдено!";
                        }
                    }
                    else
                    {
                        message = $"Помилка! {key} - некоректний ключ. Пошук неможливий";
                    }
                }
            }
            else if(radioButton2.Checked)
            {
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
                {
                    message = "Помилка! Поля введення повинні бути заповненими. Додавання запису неможливе";
                }
                else
                {
                    if (int.TryParse(textBox1.Text, out int key))
                    {
                        if (!FileHelper.IsThereRepeatingKeys(tree, key))
                        {
                            tree.Insert(key, textBox2.Text);
                            message = "Запис успішно додано до БД!";
                        }
                        else
                        {
                            message = $"Помилка! {key} ключ уже є в БД. Додавання запису неможливе";
                        }
                    }
                    else
                    {
                        message = $"Помилка! {key} - некоректний ключ. Додавання запису неможливе";
                    }
                }
            }
            else if (radioButton3.Checked)
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    message = "Помилка! Поля введення значення ключа повинно бути заповненим. Видалення неможливе";
                }
                else
                {
                    if (int.TryParse(textBox1.Text, out int key))
                    {
                        bool success = true;
                        tree.Delete(key, ref success);
                        if (success)
                        {
                            message = $"Ключ {key} видалено успішно";
                        }
                        else
                        {
                            message = $"Ключ {key} не знайдено! Видалення неможливе";
                        }
                    }
                    else
                    {
                        message = $"Помилка! {key} - некоректний ключ. Видалення неможливе";
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
                {
                    message = "Помилка! Поля введення повинні бути заповненими. Редагування неможливе";
                }
                else
                {
                    if (int.TryParse(textBox1.Text, out int key))
                    {
                        bool success = false;
                        tree.Edit(key, textBox2.Text, ref success);
                        if (success)
                        {
                            message = $"Ключ {key} змінено успішно! Нове значення - '{textBox2.Text}'";
                        }
                        else
                        {
                            message = $"Ключ {key} не знайдено! Редагування неможливе";
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
            string message = "Файл успішно очищено!";
            textBox3.AppendText(message + Environment.NewLine);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tree.SaveData(tree.root);
            textBox3.AppendText(Environment.NewLine + "-=Дерево має вигляд=-" + Environment.NewLine);
            for (int i = 0; i < tree.fullTreeList.Count; i++)
            {
                textBox3.AppendText(tree.fullTreeList[i].ToString() + Environment.NewLine);
            }
        }
    }
}