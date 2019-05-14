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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private string fileName; //змінна, яка запам'ятовує шлях та ім'я обраного для шифрування/дешифрування файлу
        private string saveEncrypted; //змінна, запам'ятовує шлях та ім'я збереженого зашифрованого файлу
        private string saveDecoded; //змінна, запам'ятовує шлях та ім'я збереженого дешифрованого файлу
        private char[] textArray; //масив символів, для того щоб працювати з текстом посимвольно

        public Form1()
        {
            InitializeComponent();
        }

        private void openFile(object sender, EventArgs e) //кнопка, при натисненні на яку повинно з'явитися діалогове вікно для вибору файлу
        {
            openFileDialog1.Filter = "(*.txt) | *.txt"; //задаємо обмеження, щоб вибиралися файли тільки з таким розширенням
            if (openFileDialog1.ShowDialog() == DialogResult.OK) //якщо, після закриття вікна, результат повернення дорівнює OK, то ім’я вибраного файлу зберігається у властивості FileName.
            {
                fileName = openFileDialog1.FileName; //запам'ятовуємо шлях та ім'я файлу в змінній fileName
            }
        }

        private void saveEncryptedFile(object sender, EventArgs e) //функція для збереження зашифрованого файлу
        {
            if (fileName == null) //перевіряємо чи користувач обрав файл для шифрування
            {
                MessageBox.Show("Оберіть файл котрий хочете зашифрувати.");
            }
            else
            {
                saveFileDialog1.Filter = "(*.txt) | *.txt"; //задаємо обмеження, щоб зберігалися файли тільки з текстовим розширенням
                if (saveFileDialog1.ShowDialog() == DialogResult.OK) //якщо, після закриття вікна, результат повернення дорівнює OK, то ім’я вибраного файлу зберігається у властивості FileName.
                {
                    saveEncrypted = saveFileDialog1.FileName; //запам'ятовуємо шлях та ім'я файлу в змінній saveEncrypted
                    MessageBox.Show("Файл зашифровано"); //повідомляємо про те, що операція пройшла успішно
                }
                encryptionCaesar(); //функція, що виконує шифрування тексту
            }
        }

        private void encryptionCaesar() //функція шифрування тексту
        {
            string skey = numericUpDown1.Value.ToString(); //отримуємо значення ключа шифрування
            int key = Convert.ToInt32(skey); //перетворюємо отримане значення в числовий тип
            string text = "";
            StreamReader sr = new StreamReader(fileName, Encoding.Default); //потік для читання тфайлу
            while (!sr.EndOfStream) //читається весь текст до кінця
            {
                text += sr.ReadLine();
            } 
            sr.Close(); //закриваємо потік читання
            StreamWriter sw = new StreamWriter(saveEncrypted); //потік, для запису у зашифрованого тексту в новий файл
            textArray = text.ToCharArray(); //перетворюємо наш текст у масив символів
            for (int i = 0; i < textArray.Length; i++) //проходимя по усим символам
            {
                if (textArray[i] >= 'a' && textArray[i] <= 'z') //перевіряємо чи символ належить множині a-z
                {
                    char encryptedText = (char)((textArray[i] - 'a' + key) % 26 + 'a'); //зміщуємо символ на наш ключ
                    sw.Write(encryptedText); //записуємо посимвольно наший замінений символ у файл
                }
                else if (textArray[i] >= 'A' && textArray[i] <= 'Z') //перевіряємо чи символ належить множині A-Z
                {
                    char encryptedText = (char)((textArray[i] - 'A' + key) % 26 + 'A'); //также само
                    sw.Write(encryptedText);
                }
                else
                {
                    sw.Write(textArray[i]); //якщо наш символ не буква, то записуємо його як є
                }
            }
            sw.Close(); // закриваємо наш потік
        }
        
        private void saveDecryptedFile(object sender, EventArgs e) //функція для збереження розшифрованого файлу, поясненя такі ж
        {
            if (fileName == null) 
            {
                MessageBox.Show("Оберіть файл котрий хочете розшифрувати."); 
            }
            else
            {
                saveFileDialog1.Filter = "(*.txt) | *.txt"; 
                if (saveFileDialog1.ShowDialog() == DialogResult.OK) 
                {
                    saveDecoded = saveFileDialog1.FileName; 
                    MessageBox.Show("Файл розшифровано"); 
                }
                decryptionOfCaesar(); //функція, що виконує дешифровку тексту
            }
        }

        private void decryptionOfCaesar() //функція дешифрування тексту, такий же алгоритм як і при шифруванні
        {
            string skey = numericUpDown1.Value.ToString();
            int key = Convert.ToInt32(skey);
            string text = "";
            StreamReader sr = new StreamReader(fileName, Encoding.Default);
            while (!sr.EndOfStream)
            {
                text += sr.ReadLine();
            }
            sr.Close();
            StreamWriter sw = new StreamWriter(saveDecoded);
            textArray = text.ToCharArray();
            for (int i = 0; i < textArray.Length; i++)
            {
                if (textArray[i] >= 'a' && textArray[i] <= 'z')
                {
                    char decodedText = (char)((textArray[i] - 'z' - key) % 26 + 'z');
                    sw.Write(decodedText);
                }
                else if (textArray[i] >= 'A' && textArray[i] <= 'Z')
                {
                    char decodedText = (char)((textArray[i] - 'Z' - key) % 26 + 'Z');
                    sw.Write(decodedText);
                }
                else
                {
                    sw.Write(textArray[i]);
                }
            }
            sw.Close();
        }
        
        private void Instruction(object sender, EventArgs e) //кнопка інструкції до програми
        {
            MessageBox.Show("Програма для шифрування і дешифрування тексту англійської мови шифром Цезаря.\nПринцип дії полягає в тому, щоб циклічно зсунути алфавіт, а ключ — це кількість літер, на які робиться зсув.\n" +
                "       1.Спочатку ви повинні обрати файл котрий бажаєте зашифрувати або дешифрувати.\n" +
                "       2.Потім задайте значення ключа шифрування.\n" + 
                "       3.Оберіть потрібну вам операцію.\n" +
                "       4.По бажанню можете переглянути вміст файлу який був зашифрований або розшифрований.\n" +
                "Вдалого вам користування!!!", "Інструкція");
            
        }

        private void openAnEncryptedFile(object sender, EventArgs e) //перегляд зашифрованого тексту
        {
            try //пробуємо відкрити файл 
            {
                System.Diagnostics.Process.Start(saveEncrypted); //відкриваємо наш файл
            }
            catch(Exception exp) //якщо файлу неіснує, виводимо повідомлення про це
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void openDecryptedFile(object sender, EventArgs e) //перегляд розшифрованого тексту, такий же алгоритм
        {
            try 
            {
                System.Diagnostics.Process.Start(saveDecoded);
            }
            catch (Exception exp) 
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void closeTheProgram(object sender, EventArgs e) //закриття програми
        {
            Application.Exit(); //виконує власне закриття нашої програми
        }
    }
}
