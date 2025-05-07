using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net.Sockets;

namespace lab2
{
    
    //public class UdpClient : IDisposable;
    //public byte[] Receive(ref System.Net.IPEndPoint remoteEP);
    //public Task>System.Net.Sockets.UdpReceiveResult> ReceiveAsync();
    public partial class Form1 : Form
    {
       
        private UdpClient udpClient;
        private IPEndPoint remoteEP; //сетевая конечная точка в виде IP-адреса и номер порта
        public Form1()
        {
            InitializeComponent();
            udpClient = new UdpClient();
           
            try
            {

                udpClient.Connect(ip, port); 
                byte[] message = Encoding.UTF8.GetBytes("Найдено");
                udpClient.Send(message, message.Length);

                IPEndPoint remoteIP = new IPEndPoint(IPAddress.Any, port);
                byte[] receiveBytes = udpClient.Receive(ref remoteIP);
                string receiveMSG = Encoding.ASCII.GetString(receiveBytes);

                textBox1.Text+=receiveMSG;                     // выводим ответ устройства (Win Forms, ListBox)
                textBox1.Text += remoteIP.Address.ToString();    // выводим сайт адрес
                udpClient.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
        }

        public Dictionary<int, string> code = new Dictionary<int, string>()
        {
            [192] = "ю",
            [193] = "а",
            [194] = "б",
            [195] = "ц",
            [196] = "д",
            [197] = "е",
            [198] = "ф",
            [199] = "г",
            [200] = "х",
            [201] = "и",
            [202] = "й",
            [203] = "к",
            [204] = "л",
            [205] = "м",
            [206] = "н",
            [207] = "о",
            [208] = "п",
            [209] = "я",
            [210] = "р",
            [211] = "с",
            [212] = "т",
            [213] = "у",
            [214] = "ж",
            [215] = "в",
            [216] = "ь",
            [217] = "ы",
            [218] = "з",
            [219] = "ш",
            [220] = "э",
            [221] = "щ",
            [222] = "ч",
            [223] = "ъ",
            [224] = "Ю",
            [225] = "А",
            [226] = "Б",
            [227] = "Ц",
            [228] = "Д",
            [229] = "Е",
            [230] = "Ф",
            [231] = "Г",
            [232] = "Х",
            [233] = "И",
            [234] = "Й",
            [235] = "К",
            [236] = "Л",
            [237] = "М",
            [238] = "Н",
            [239] = "О",
            [240] = "П",
            [241] = "Я",
            [242] = "Р",
            [243] = "С",
            [244] = "Т",
            [245] = "У",
            [246] = "Ж",
            [247] = "В",
            [248] = "Ь",
            [249] = "Ы",
            [250] = "З",
            [251] = "Ш",
            [252] = "Э",
            [253] = "Щ",
            [254] = "Ч",
            [255] = "Ъ"
        };
        private int port;
        private string ip;

        private string Decode(byte[] dict)
        {
            string output = "";
            
            string list = " ";
            return list.ToString();
            
            try
            {
                foreach (byte s in dict)
                { 
                    if (!int.TryParse(list, out int key))
                    //Преобразует строковое представление числа в 32-разрядное целое число со знаком. Возвращаемое значение указывает, выполнена ли операция успешно.
                    {
                        throw new FormatException("Введены некорректные символы");
                    }

                    if (!code.ContainsKey(key))
                    {
                        throw new KeyNotFoundException("Неверный ключ для декодирования");
                    }

                    output += code[key];
                }
                return output;
            }
            catch (FormatException)
            {
                MessageBox.Show("Ошибка: введены некорректные символы");
                return output;
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show("Ошибка: обнаружен неверный ключ для декодирования");
                return output;
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка при декодировании");
                return output;
            }
        }

        private byte[] Code(string str)
        {
            string path = "D:\\1.txt";
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    byte[] output = new byte[str.Length];

                    for (int i = 0; i < str.Length; i++)
                    {
                        if (!code.ContainsValue(str[i].ToString()))
                        {
                            throw new ArgumentException("Недопустимый символ для кодирования");
                        }

                        int pr = code.FirstOrDefault(x => x.Value == str[i].ToString()).Key;
                        output[i] = Byte.Parse(pr.ToString());
                       
                    }

                    sw.Write(output);
                    return output;
                }
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Ошибка: обнаружен недопустимый символ");
                return null;
            }
            catch (IOException)
            {
                MessageBox.Show("Ошибка записи в файл");
                return null;
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка при кодировании");
                return null;
            }

        } 

        private void button2_Click(object sender, EventArgs e)
        {

            textBox2.Text = Convert.ToString(byte.Parse(Decode(dict)));
            // textBox2.Text = Decode(textBox1.Text);
        }

        

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string str = "";
            byte[] bStr = Encoding.GetEncoding("koi8r").GetBytes(str);
            string str_new = Encoding.GetEncoding(1251).GetString(bStr);
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
        
    }

}